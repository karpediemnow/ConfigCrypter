using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DevAttic.ConfigCrypter
{
    public static class CertUtils
    {
#pragma warning disable CA1034 // Nested types should not be visible
        public static class Create
#pragma warning restore CA1034 // Nested types should not be visible
        {
            //https://www.asptricks.net/2016/08/create-self-signed-certificate.html

            #region Create Certificate bouncycastle Implementation sometime get bad data error
            /*
            public static void CertificateFileV1(string certificatePath, string certSubjectName, string password)
            {
                CertificateFileV1(certificatePath, certSubjectName, password.ToSecureString());
            }
            public static void CertificateFileV1(string certificatePath, string certSubjectName, SecureString password)
            {

                string dirName = Path.GetDirectoryName(certificatePath);
                if (!string.IsNullOrWhiteSpace(dirName) && !Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                byte[] certData = CertificateV1(certSubjectName, password);

                Console.WriteLine("Creating Certificate");

                File.WriteAllBytes(certificatePath, certData);

                Console.WriteLine("Certificate:" + certSubjectName);

            }

            public static byte[] CertificateV1(string certSubjectName, string password)
            {
                return CertificateV1(certSubjectName, password.ToSecureString());
            }
            public static byte[] CertificateV1(string certSubjectName, SecureString password)
            {

                byte[] certData;

                AsymmetricKeyParameter myCAprivateKey = null;
                //Console.WriteLine("Creating Certificate");
                X509Certificate2 certificate = SelfSignedCertificateV1(certSubjectName, certSubjectName, out myCAprivateKey, 2, true);


                //Export as pfx with privatekey
                certData = certificate.Export(X509ContentType.Pfx, password);

                return certData;
            }

            public static X509Certificate2 SelfSignedCertificateV1(string subjectName, string issuerName, out AsymmetricKeyParameter PrivKey, int validForYears = 2, bool includePrivatekey = false)
            {
                const int keyStrength = 2048;

                // Generating Random Numbers
                SecureRandom random = new SecureRandom(new CryptoApiRandomGenerator());

                // The Certificate Generator
                X509V3CertificateGenerator certificateGenerator = new X509V3CertificateGenerator();
                certificateGenerator.AddExtension(X509Extensions.ExtendedKeyUsage.Id, true, new ExtendedKeyUsage(KeyPurposeID.IdKPServerAuth));

                // Serial Number
                BigInteger serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
                certificateGenerator.SetSerialNumber(serialNumber);

                // Issuer and Subject Name
                X509Name subjectDN = new X509Name("CN=" + subjectName);
                X509Name issuerDN = new X509Name("CN=" + issuerName);
                certificateGenerator.SetIssuerDN(issuerDN);
                certificateGenerator.SetSubjectDN(subjectDN);

                // Valid For
                DateTime notBefore = DateTime.UtcNow.Date;
                DateTime notAfter = notBefore.AddYears(validForYears);
                certificateGenerator.SetNotBefore(notBefore);
                certificateGenerator.SetNotAfter(notAfter);

                // Subject Public Key
                AsymmetricCipherKeyPair subjectKeyPair;
                var keyGenerationParameters = new KeyGenerationParameters(random, keyStrength);
                var keyPairGenerator = new RsaKeyPairGenerator();
                keyPairGenerator.Init(keyGenerationParameters);
                subjectKeyPair = keyPairGenerator.GenerateKeyPair();

                PrivKey = subjectKeyPair.Private;

                certificateGenerator.SetPublicKey(subjectKeyPair.Public);

                // selfsign certificate
                ISignatureFactory signatureFactory = new Asn1SignatureFactory("SHA512WITHRSA", PrivKey, random);
                Org.BouncyCastle.X509.X509Certificate certificate = certificateGenerator.Generate(signatureFactory);
                AsymmetricAlgorithm dotNetPrivateKey = ((RsaPrivateCrtKeyParameters)subjectKeyPair.Private).ToDotNetKey();


                X509Certificate2 x509 = new X509Certificate2(DotNetUtilities.ToX509Certificate(certificate));

                if (includePrivatekey)
                {
                    // merge into X509Certificate2
                    //https://forums.xamarin.com/discussion/164550/how-to-set-x509certificate2-privatekey
                    //https://github.com/NuGet/Home/issues/8626
                    //https://github.com/dotnet/runtime/issues/19581
                    X509Certificate2 certWithPrivateKey = x509.CopyWithPrivateKey((RSA)dotNetPrivateKey);
                    certWithPrivateKey.FriendlyName = subjectName;

                    return certWithPrivateKey;
                }
                else
                {
                    return x509;
                }
            }
            /**/
            #endregion

            #region net core primitive
            public static void CertificateFile(string certificatePath, string certSubjectName, string password)
            {
                using SecureString secure = password.ToSecureString();
                CertificateFile(certificatePath, certSubjectName, secure);
            }

            public static string CertificateFile(string certificatePath, string certSubjectName, SecureString password)
            {


                if (certSubjectName == null)
                    certSubjectName = "TS Self Signed Certificate";


                var cert = SelfSignedCertificate(certSubjectName);

                string dirName = Path.GetDirectoryName(certificatePath);
                if (!string.IsNullOrWhiteSpace(dirName) && !Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                if (!Path.HasExtension(certificatePath))
                {
                    certificatePath += ".pfx";
                }

#pragma warning disable CA1303 // Do not pass literals as localized parameters
                Console.WriteLine("Creating Certificate...");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

                // Create PFX (PKCS #12) with private key
                File.WriteAllBytes(certificatePath, cert.Export(X509ContentType.Pfx, password));

                Console.WriteLine("Certificate Path: " + certificatePath);
                Console.WriteLine("Subject Name: " + certSubjectName);


                //// Create Base 64 encoded CER (public key only)
                //File.WriteAllText("c:\\temp\\mycert.cer",
                //    "-----BEGIN CERTIFICATE-----\r\n"
                //    + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                //    + "\r\n-----END CERTIFICATE-----");

                return certificatePath;
            }

            public static X509Certificate2 SelfSignedCertificate(string subjectName, out RSA keys, int validForYears = 2)
            {

                keys = RSA.Create(); // generate asymmetric key pair
                var req = new CertificateRequest($"cn={subjectName}", keys, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(validForYears));

                return cert;
            }

            public static X509Certificate2 SelfSignedCertificate(string subjectName, int validForYears = 2)
            {
                using var rsa = RSA.Create();
                var req = new CertificateRequest($"cn={subjectName}", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(validForYears));

                return cert;
            }

            #endregion
        }

        public static IEnumerable<string> CertInfo(string certificatePath, SecureString secureString, bool verbose = false)
        {
            if (!File.Exists(certificatePath))
                throw new FileNotFoundException($"Unable fo find the certificate {certificatePath}");

            using var x509 = new X509Certificate2(certificatePath, secureString, X509KeyStorageFlags.Exportable);

            var infos = new List<string>
            {
                $"Subject: {x509.Subject}{0}",
                $"Issuer: {x509.Issuer}{0}",
                $"Version: {x509.Version}{0}",
                $"Valid Date: {x509.NotBefore}{0}",
                $"Expiry Date: {x509.NotAfter}{0}",
                $"Thumbprint: {x509.Thumbprint}{0}",
                $"Serial Number: {x509.SerialNumber}",
                $"Friendly Name: {x509.PublicKey.Oid.FriendlyName}",
                $"HasPrivateKey: { x509.HasPrivateKey}"
            };

            if (verbose)
            {
                // infos.Add($"Public Key Format: { x509.PublicKey.EncodedKeyValue.Format(true)}");
                infos.Add($"Raw Data Length: {x509.RawData.Length}");
                try
                {
                    infos.Add($"PublicKey Certificate XML String: {Environment.NewLine}{x509.PrivateKey?.ToXmlString(false)}");
                    infos.Add($"PrivateKey Certificate XML String: {Environment.NewLine}{x509.PrivateKey?.ToXmlString(true)}");
                }
                catch
                {
                    // Certificate XML export can thow an exception. But don't care ;)
                }

                try
                {
                    infos.Add($"PublicKey Certificate PEM String: {Environment.NewLine}{x509.ExportWithPublicKeyToString()}");
                    infos.Add($"PrivateKey Certificate PEM String: {Environment.NewLine}{x509.ExportWithPrivateKeyToString(secureString)}");
                }
                catch (Exception ex)
                {
                    infos.Add($"Error: {Environment.NewLine}{ex.Message}");
                }
            }
            infos.Add($"Certificate to string:{Environment.NewLine}{x509.ToString(verbose)}");

            return infos;
        }

#pragma warning disable CA1034 // Nested types should not be visible
        public static class Manage
#pragma warning restore CA1034 // Nested types should not be visible
        {
            #region Import Export Cerificate
            public static string ExportCertiticateToStringFile(string certificatePath, string outputPath, bool publicKeyOnly = false, SecureString password = null)
            {
                string certdirName = Path.GetDirectoryName(certificatePath);

                if (!File.Exists(certificatePath))
                    throw new FileNotFoundException($"Unable fo find the certificate {certificatePath}");

                if (!string.IsNullOrWhiteSpace(certdirName) && !Directory.Exists(certdirName))
                    Directory.CreateDirectory(certdirName);

                if (string.IsNullOrWhiteSpace(Path.GetFileName(outputPath)))
                {
                    if (publicKeyOnly)
                        outputPath = Path.GetFileNameWithoutExtension(certificatePath) + ".public.key";
                    else
                        outputPath = Path.GetFileNameWithoutExtension(certificatePath) + ".private.key";
                }

                if (!Path.HasExtension(outputPath))
                {
                    outputPath += ".export.key";
                }


                string outDirName = Path.GetDirectoryName(outputPath);

                if (!string.IsNullOrWhiteSpace(outDirName) && !Directory.Exists(outDirName))
                    Directory.CreateDirectory(outDirName);

                if (File.Exists(outputPath))
                    File.Delete(outputPath);

                using X509Certificate2 cert = new X509Certificate2(certificatePath, password, X509KeyStorageFlags.Exportable);

                string certStr;
                if (publicKeyOnly)
                    certStr = cert.ExportWithPublicKeyToString();
                else
                    certStr = cert.ExportWithPrivateKeyToString(password);

                File.WriteAllText(outputPath, certStr);

                return outputPath;
            }

            //public static (string privKeyPem, string publKeyPem ) ExportCertificateKeysToString(this X509Certificate2 cert, string password = null)
            //{

            //    byte[] certificateBytes = cert.RawData;
            //    char[] certificatePem = PemEncoding.Write("CERTIFICATE", certificateBytes);

            //    AsymmetricAlgorithm key = cert.GetRSAPrivateKey() ?? cert.GetECDsaPrivateKey();
            //    byte[] pubKeyBytes = key.ExportSubjectPublicKeyInfo();
            //    byte[] privKeyBytes = key.ExportPkcs8PrivateKey();
            //    char[] pubKeyPem = PemEncoding.Write("PUBLIC KEY", pubKeyBytes);
            //    char[] privKeyPem = PemEncoding.Write("PRIVATE KEY", privKeyBytes);

            //    return (null, null);
            //}


            public static string ConvertExportedStringToCertificateFile(string privateKeyFilePath, string outputPath, SecureString password = null)
            {
                if (!File.Exists(privateKeyFilePath))
                    throw new FileNotFoundException($"Unable fo find the PrivateKey File {privateKeyFilePath}");

                var privKeyString = File.ReadAllText(privateKeyFilePath);

                using X509Certificate2 cert = LoadCertificateFromString(privKeyString, password);

                if (string.IsNullOrWhiteSpace(Path.GetFileName(outputPath)))
                    outputPath = Path.GetFileNameWithoutExtension(privateKeyFilePath) + ".pfx";
                string outDirName = Path.GetDirectoryName(outputPath);

                if (!string.IsNullOrWhiteSpace(outDirName) && !Directory.Exists(outDirName))
                    Directory.CreateDirectory(outDirName);

                if (File.Exists(outputPath))
                    File.Delete(outputPath);


                var certData = cert.Export(X509ContentType.Pfx, password);

                File.WriteAllBytes(outputPath, certData);

                return outputPath;

            }

            public static X509Certificate2 LoadCertificateFromStringFile(string certificateStringPath, SecureString password = null)
            {
                if (!File.Exists(certificateStringPath))
                    throw new FileNotFoundException($"Unable fo find the string certificate {certificateStringPath}");

                string certString = File.ReadAllText(certificateStringPath);

                return LoadCertificateFromString(certString, password);

            }


            public static X509Certificate2 LoadCertificateFromString(string certString, SecureString password = null)
            {
                if (certString == null)
                    throw new ArgumentNullException(nameof(certString), "Certitifiate key cannot be null.");

                certString = certString
                    .Replace("-----BEGIN CERTIFICATE-----", string.Empty)
                    .Replace("-----END CERTIFICATE-----", string.Empty)
                    .Replace("-----BEGIN PRIVATE KEY-----", string.Empty)
                    .Replace("-----END PRIVATE KEY-----", string.Empty)
                    .Replace("-----BEGIN PUBLIC KEY-----", string.Empty)
                    .Replace("-----END PUBLIC KEY-----", string.Empty)
                             .Replace("\r", string.Empty)
                             .Replace("\n", string.Empty);

                return new X509Certificate2(Convert.FromBase64String(certString), password, X509KeyStorageFlags.Exportable);
            }


            public static X509Certificate2 LoadCertificateFromFile(string certificatePath, SecureString password = null)
            {
                if (!File.Exists(certificatePath))
                    throw new FileNotFoundException($"Unable fo find the certificate {certificatePath}");

                return new X509Certificate2(certificatePath, password, X509KeyStorageFlags.Exportable);
            }
            #endregion
        }

    }

    public static class CertificateExtMethods
    {
        /*
        internal static AsymmetricAlgorithm ToDotNetKey(this RsaPrivateCrtKeyParameters privateKey)
        {

            var cspParams = new CspParameters()
            {
                KeyContainerName = Guid.NewGuid().ToString(),
                KeyNumber = (int)KeyNumber.Exchange,
                Flags = CspProviderFlags.UseMachineKeyStore
            };

            var rsaProvider = new RSACryptoServiceProvider(cspParams);

            //var rsaProvider = new RSACryptoServiceProvider();
            var parameters = new RSAParameters()
            {
                Modulus = privateKey.Modulus.ToByteArrayUnsigned(),
                P = privateKey.P.ToByteArrayUnsigned(),
                Q = privateKey.Q.ToByteArrayUnsigned(),
                DP = privateKey.DP.ToByteArrayUnsigned(),
                DQ = privateKey.DQ.ToByteArrayUnsigned(),
                InverseQ = privateKey.QInv.ToByteArrayUnsigned(),
                D = privateKey.Exponent.ToByteArrayUnsigned(),
                Exponent = privateKey.PublicExponent.ToByteArrayUnsigned()
            };

            rsaProvider.ImportParameters(parameters);

            return rsaProvider;
        }
        /**/

        #region Import Export Cerificate

        public static string ExportWithPrivateKeyToString(this X509Certificate2 cert, SecureString password = null)
        {
            cert.NullCheck(nameof(cert));

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("-----BEGIN CERTIFICATE-----");

            byte[] certData = cert.Export(X509ContentType.Pfx, password);

            builder.AppendLine(
                Convert.ToBase64String(certData, Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }

        public static string ExportWithPublicKeyToString(this X509Certificate2 cert)
        {
            cert.NullCheck(nameof(cert));

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("-----BEGIN CERTIFICATE-----");

            byte[] certData = cert.Export(X509ContentType.Cert);

            builder.AppendLine(
                Convert.ToBase64String(certData, Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }

        #endregion
    }

    public static class StoreManageCertificateExtMethods
    {
        #region  Windows Store Managment

        public static bool AddPersonalCertToStore(this X509Certificate2 cert)
        {
            return AddCertToStore(cert, StoreName.My, StoreLocation.LocalMachine);
        }

        public static bool AddRootCertToStore(this X509Certificate2 cert)
        {
            return AddCertToStore(cert, StoreName.Root, StoreLocation.LocalMachine);
        }

        public static bool AddCertToStore(this X509Certificate2 cert, StoreName st, StoreLocation sl)
        {
            bool bRet = false;

            try
            {
                X509Store store = new X509Store(st, sl);
                store.Open(OpenFlags.ReadWrite);
                store.Add(cert);

                store.Close();

                bRet = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                bRet = false;
            }

            return bRet;
        }


        public static bool RemovePersonalCertToStore(this X509Certificate2 cert)
        {
            return RemoveCertToStore(cert, StoreName.My, StoreLocation.LocalMachine);
        }

        public static bool RemoveRootCertToStore(this X509Certificate2 cert)
        {
            return RemoveCertToStore(cert, StoreName.Root, StoreLocation.LocalMachine);
        }

        public static bool RemoveCertToStore(this X509Certificate2 cert, StoreName st, StoreLocation sl)
        {
            bool bRet = false;

            try
            {
                X509Store store = new X509Store(st, sl);
                store.Open(OpenFlags.ReadWrite);
                store.Remove(cert);

                store.Close();

                bRet = true;
            }
            catch
            {
                bRet = false;
            }

            return bRet;
        }
        #endregion
    }
}
