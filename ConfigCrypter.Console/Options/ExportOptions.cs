using CommandLine;

namespace ConfigCrypter.Console.Options
{
    [Verb("export", HelpText = "Export Certificate to file.")]
    public class ExportOptions : CommandLineCertificateBaseInfo
    {
        [Option('o', "output", Required = true, HelpText = "Path of the private key file.", Group = "CertFileLocation")]
        public string CertificateOutuputPath { get; set; }


        [Option("publicKeyOnly", HelpText = "Export only the private key (.Cert).", Default = false)]
        public bool PublicKeyOnly { get; set; }
    }
}
