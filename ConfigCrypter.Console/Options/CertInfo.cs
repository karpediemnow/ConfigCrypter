using CommandLine;

namespace ConfigCrypter.Console.Options
{
    [Verb("certInfo", HelpText = "Print to console information contained in the certificate.")]
    public class CertInfo : CommandLineCertificateBaseInfo
    {

        [Option('v', "verbose", HelpText = "Print verbose infos.", Default = false)]
        public bool Verbose { get; set; }
    }
}
