using CommandLine;

namespace ConfigCrypter.Console.Options
{
    [Verb("import", HelpText = "Import Certificate File to PFX certificate.")]
    public class ImportOptions : CommandLineCertificateBaseInfo
    {
        [Option('o', "output", Required = true, HelpText = "Path of the private key file.", Group = "CertFileLocation")]
        public string CertificateOutuputPath { get; set; }
    }
}
