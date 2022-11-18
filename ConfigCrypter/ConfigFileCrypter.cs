using System.Collections.Generic;
using System.IO;
using DevAttic.ConfigCrypter.ConfigCrypters;

namespace DevAttic.ConfigCrypter
{
    /// <summary>
    /// Configuration crypter that reads the configuration file from the filesystem.
    /// </summary>
    public class ConfigFileCrypter
    {
        private readonly IConfigCrypter _configCrypter;
        private readonly ConfigFileCrypterOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigFileCrypter"/> class.
        /// Creates an instance of the ConfigFileCrypter.
        /// </summary>
        /// <param name="configCrypter">A config crypter instance.</param>
        /// <param name="options">Options used for encrypting and decrypting.</param>
        public ConfigFileCrypter(IConfigCrypter configCrypter, ConfigFileCrypterOptions options)
        {
            _configCrypter = configCrypter;
            _options = options;
        }

        public string DecryptKeysInFile(string filePath)
        {
            return DecryptKeysInFile(filePath, null);
        }

        /// <summary>
        /// <para>Decrypts the given key in the config file.</para>
        /// <para> </para>
        /// <para>If the "ReplaceCurrentConfig" setting has been set in the options the file is getting replaced.</para>
        /// <para>If the setting has not been set a new file with the "DecryptedConfigPostfix" appended to the current file name will be created.</para>
        /// </summary>
        /// <param name="filePath">Path of the configuration file.</param>
        /// <param name="configKeys">Key to decrypt, passed in a format the underlying config crypter understands.</param>
        /// <returns>the config path</returns>
        public string DecryptKeysInFile(string filePath, IEnumerable<string> configKeys)
        {
            var configContent = File.ReadAllText(filePath);

            var decryptedConfigContent = configContent;
            if (configKeys != null)
            {
                foreach (var configKey in configKeys)
                {
                    decryptedConfigContent = _configCrypter.DecryptKey(configContent, configKey);
                }
            }
            decryptedConfigContent = _configCrypter.DiscoveryDecryptKeys(decryptedConfigContent);


            var targetFilePath = GetDestinationConfigPath(filePath, _options.DecryptedConfigPostfix);
            File.WriteAllText(targetFilePath, decryptedConfigContent);
            return targetFilePath;
        }


        public string EncryptKeysInFile(string filePath)
        {
            return EncryptKeysInFile(filePath, null);
        }


        /// <summary>
        /// <para>Encrypts the given key in the config file.</para>
        /// <para> </para>
        /// <para>If the "ReplaceCurrentConfig" setting has been set in the options the file is getting replaced.</para>
        /// <para>If the setting has not been set a new file with the "EncryptedConfigPostfix" appended to the current file name will be created.</para>
        /// </summary>
        /// <param name="filePath">Path of the configuration file.</param>
        /// <param name="configKeys">Key to encrypt, passed in a format the underlying config crypter understands.</param>
        /// <returns>the config path</returns>
        public string EncryptKeysInFile(string filePath, IEnumerable<string>  configKeys)
        {
            var configContent = File.ReadAllText(filePath);

            string encryptedConfigContent = configContent;

            if (configKeys != null)
            {
                foreach (var configKey in configKeys)
                {
                    encryptedConfigContent = _configCrypter.EncryptKey(encryptedConfigContent, configKey);
                }
            }
            encryptedConfigContent = EncryptKeys(encryptedConfigContent);


            var targetFilePath = GetDestinationConfigPath(filePath, _options.EncryptedConfigPostfix);
            File.WriteAllText(targetFilePath, encryptedConfigContent);

            return targetFilePath;
        }


        public string EncryptKeys(string configString)
        {
            string encryptedConfigContent = configString;

            encryptedConfigContent = _configCrypter.DiscoveryEncryptKeys(encryptedConfigContent);

            return encryptedConfigContent;
        }

        private string GetDestinationConfigPath(string currentConfigFilePath, string postfix)
        {
            if (_options.ReplaceCurrentConfig)
            {
                return currentConfigFilePath;
            }

            var currentConfigDirectory = Path.GetDirectoryName(currentConfigFilePath);
            var newFilename =
                $"{Path.GetFileNameWithoutExtension(currentConfigFilePath)}{postfix}{Path.GetExtension(currentConfigFilePath)}";
            var targetFile = Path.Combine(currentConfigDirectory, newFilename);

            return targetFile;
        }
    }
}