using CommandLine;

namespace ConfigCrypter.Console.Options
{
    [Verb("encrypt", HelpText = "Encrypts the key/s in the config file.")]
    public class EncryptOptions : CommandlineOptions {

        [Option('k', "keys", Required = false, HelpText = "One or more keys to encrypt in the config file," +
                                                           " other than the keys with prefix value [TOENCRYPT]." +
                                                           " The keys must by separated by ','.")]
        public override string Keys { get; set; }
    
    }
}
