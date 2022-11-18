using CommandLine;

namespace ConfigCrypter.Console.Options
{
    [Verb("decrypt", HelpText = "Decrypts the key/s in the config file.")]
    public class DecryptOptions : CommandlineOptions
    {
        public DecryptOptions(string keys)
        {
            Keys = keys;
        }

        [Option('k', "keys", Required = false, HelpText = "One or more keys to encrypt in the config file," +
                                                       " other than the keys with prefix value [ENCRYPTED]." +
                                                       " The keys must by separated by ','.")]
        public override string Keys { get; set; }
    }
}
