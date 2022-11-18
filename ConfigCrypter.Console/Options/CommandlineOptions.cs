using CommandLine;

namespace ConfigCrypter.Console.Options
{

    public class CommandLineCertificateBaseInfo
    {
        [Option('p', "path", Required = true, HelpText = "Path of the certificate.", Group = "CertLocation")]
        public string CertificatePath { get; set; }

        [Option("password", HelpText = "The password certificate.")]
        public string Password { get; set; }
    }

    public class CommandLineCertificateInfo: CommandLineCertificateBaseInfo
    {   

        [Option('n', "name", Required = true, HelpText = "The subject name of the certificate (CN).", Group = "CertLocation")]
        public string CertSubjectName { get; set; }
       
    }

    public class CommandlineOptions: CommandLineCertificateInfo
    {          

        //[Option('k', "keys", Required = false, HelpText = "One or more keys to encrypt in the config file," +
        //                                                  " other than the keys with prefix value ([ENCRYPTED] or [TOENCRYPT])." +
        //                                                  " The keys must by separated by ','.")]
        public virtual string Keys { get; set; }

        [Option('f', "file", Required = true, HelpText = "The path to the config file.")]
        public string ConfigFile { get; set; }

        [Option('r', "replace", HelpText = "Replaces the original file if passed as parameter.", Default = false)]
        public bool Replace { get; set; }

        [Option("format", Default = ConfigFormat.Json, HelpText = "The format of the config file (Future implementations).")]
        public ConfigFormat ConfigFormat { get; set; }


    }


    public enum ConfigFormat
    {
        Json
    }
}
