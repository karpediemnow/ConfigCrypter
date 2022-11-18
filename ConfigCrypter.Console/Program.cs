using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using ConfigCrypter.Console.Options;
using DevAttic.ConfigCrypter;
using DevAttic.ConfigCrypter.CertificateLoaders;
using DevAttic.ConfigCrypter.ConfigCrypters.Json;
using DevAttic.ConfigCrypter.Crypters;

namespace ConfigCrypter.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<
                EncryptOptions
                , DecryptOptions
                , CreateOptions
                , InitOptions
                , ExportOptions
                ,ImportOptions 
                ,CertInfo
                >(args)
                .WithParsed<CreateOptions>(opts =>
                {
                    try
                    {
                        //if (opts.CACertificate)
                        //    CertUtils.CreateCertificateFileWithCAInfile(opts.CertificatePath, opts.CertSubjectName, opts.Password);                        
                        //else
                          var file=  CertUtils.Create.CertificateFile(opts.CertificatePath, opts.CertSubjectName, opts.Password.ToSecureString());
                        ConsoleLog("Create Done.");
                        ConsoleLog($"Output cetificate file: {file}");
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog(ex);
                    }
                })
                .WithParsed<EncryptOptions>(opts =>
                {
                    var crypter = CreateCrypter(opts);

                    if(crypter== null)
                    {
                        ConsoleLog($"Invalid certificate", ConsoleColor.Red);
                        return;
                    }

                    IEnumerable<string> keys = null;
                    if (!string.IsNullOrWhiteSpace(opts.Keys))
                        keys = opts.Keys.Split(',');
                    if (!File.Exists(opts.ConfigFile))
                    {
                        ConsoleLog($"The config file: {opts.ConfigFile} doesn't exist", ConsoleColor.Red);
                        return;
                    }

                    string output = crypter.EncryptKeysInFile(opts.ConfigFile, keys);
                    ConsoleLog("Encrypt Done.");
                    ConsoleLog($"Output File: {output}");
                    })
                .WithParsed<DecryptOptions>(opts =>
                {
                    var crypter = CreateCrypter(opts);

                    if (crypter == null)
                    {
                        ConsoleLog($"Invalid certificate", ConsoleColor.Red);
                        return;
                    }

                    IEnumerable<string> keys = null;
                    if (!string.IsNullOrWhiteSpace(opts.Keys))
                        keys = opts.Keys.Split(',');
                    

                    if (!File.Exists(opts.ConfigFile))
                    {
                        ConsoleLog($"The config file: {opts.ConfigFile} doesn't exist", ConsoleColor.Red);
                        return;
                    }
                    
                    string output = crypter.DecryptKeysInFile(opts.ConfigFile, keys);

                    ConsoleLog("Encrypt Done.");
                    ConsoleLog($"Output File: {output}");
                })
                .WithParsed<InitOptions>(opts =>
                {
                    CertUtils.Create.CertificateFile(opts.CertificatePath, opts.CertSubjectName, opts.Password.ToSecureString());

                    var crypter = CreateCrypter(opts);

                    if (crypter == null)
                    {
                        ConsoleLog($"Invalid certificate", ConsoleColor.Red);
                        return;
                    }


                    IEnumerable<string> keys = null;
                    if (!string.IsNullOrWhiteSpace(opts.Keys))
                        keys = opts.Keys.Split(',');

                    string output = crypter.EncryptKeysInFile(opts.ConfigFile, keys);
                    ConsoleLog("Initilizzation Done.");
                    ConsoleLog($"Output File: {output}");                  
                })
                .WithParsed<ExportOptions>(opts =>
                {
                    try
                    {
                        var file = CertUtils.Manage.ExportCertiticateToStringFile(opts.CertificatePath, opts.CertificateOutuputPath, opts.PublicKeyOnly, opts.Password.ToSecureString());
                        ConsoleLog("Export Done.");
                        ConsoleLog($"Output cetificate string file: {file}");
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog(ex);
                    }
                    
                })
                .WithParsed<ImportOptions>(opts =>
                {
                    try
                    {
                        var file = CertUtils.Manage.ConvertExportedStringToCertificateFile(opts.CertificatePath, opts.CertificateOutuputPath, opts.Password.ToSecureString());
                        ConsoleLog("Convert Done.");
                        ConsoleLog($"Output cetificate string file: {file}");                        
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog(ex);
                    }
                })
                .WithParsed<CertInfo>(opts =>
                {
                    try
                    {
                       IEnumerable<string>  infos = CertUtils.CertInfo(opts.CertificatePath, opts.Password.ToSecureString(), opts.Verbose);

                        foreach (var item in infos)
                        {
                            System.Console.WriteLine(item);
                        }
                        ConsoleLog("Cert Infos Done.");
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog(ex);
                    }                    
                })
                ;
        }

        private static void ConsoleLog(Exception ex)
        {
            ConsoleLog(ex.Message, ConsoleColor.Red);

#if DEBUG
            ConsoleLog(ex.StackTrace, ConsoleColor.Red);
#endif

        }

        private static void ConsoleLog(string msg, ConsoleColor? color = null)
        {
            ConsoleColor currentColor = System.Console.ForegroundColor;
            if (color != null)
                System.Console.ForegroundColor = (ConsoleColor)color;

            System.Console.WriteLine(msg);

            System.Console.ForegroundColor = currentColor;
        }

        private static ConfigFileCrypter CreateCrypter(CommandlineOptions options)
        {
            ICertificateLoader certLoader = null;

            ConfigFileCrypter fileCrypter = null;

            if (!string.IsNullOrEmpty(options.CertificatePath))
            {
                certLoader = new FilesystemCertificateLoader(options.CertificatePath, options.Password);
            }
            else if (!string.IsNullOrEmpty(options.CertSubjectName))
            {
                certLoader = new StoreCertificateLoader(options.CertSubjectName);
            }
            if (certLoader != null)
            {
                using var rsaCrypter = new RSACrypter(certLoader);

                using var configCrypter = new JsonConfigCrypter(rsaCrypter);

                fileCrypter = new ConfigFileCrypter(configCrypter, new ConfigFileCrypterOptions
                {
                    ReplaceCurrentConfig = options.Replace
                });
            }
#pragma warning disable CS8603 // Possible null reference return.
            return fileCrypter;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}