namespace DevAttic.ConfigCrypter
{
    /// <summary>
    /// Options to configure the ConfigFileCrypter.
    /// </summary>
    public class ConfigFileCrypterOptions
    {
        /// <summary>
        /// Gets or sets name of the postfix that should be appended when a file has been decrypted and "ReplaceCurrentConfig" is set to true.
        /// </summary>
        public string DecryptedConfigPostfix { get; set; } = "_decrypted";

        /// <summary>
        /// Gets or sets name of the postfix that should be appended when a file has been encrypted and "ReplaceCurrentConfig" is set to true.
        /// </summary>
        public string EncryptedConfigPostfix { get; set; } = "_encrypted";
        /// <summary>
        /// Gets or sets a value indicating whether defines if the original config file should be overriden or a new file should be created.
        /// </summary>
        public bool ReplaceCurrentConfig { get; set; }

#pragma warning disable CA1034 // Nested types should not be visible
        public static class Describer
#pragma warning restore CA1034 // Nested types should not be visible
        {
            public const string ENCRYPTED = "[ENCRYPTED]";

            public const string TOENCRYPT = "[TOENCRYPT]";
        }

    }
}