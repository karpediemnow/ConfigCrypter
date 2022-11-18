using CommandLine;

namespace ConfigCrypter.Console.Options
{
    [Verb("create", HelpText = "Create a pfx certificate file (PKCS#12 archive).")]
    public class CreateOptions : CommandLineCertificateInfo
    {

        //[Option('c',"CreateCACertificate", Default =false, HelpText = "Create the CA Root certificate")]
        //public bool CACertificate { get; set; }

    }
}
