using CommandLine;

namespace ConfigCrypter.Console.Options
{
    [Verb("init", HelpText = "Create a certificate file and encrypt the key/s in the configuration file.")]
    public class InitOptions : CommandlineOptions
    {
        [Option('k', "keys", Required = false, HelpText = "One or more keys to encrypt in the config file," +
                                                       " other than the keys with prefix value [TOENCRYPT]." +
                                                       " The keys must by separated by ','.")]
        public override string Keys { get; set; }
    }
}
