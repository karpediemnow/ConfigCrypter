using System;
using System.Collections.Generic;
using DevAttic.ConfigCrypter.Crypters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DevAttic.ConfigCrypter.ConfigCrypters.Json
{
    /// <summary>
    /// Config crypter that encrypts and decrypts keys in JSON config files.
    /// </summary>
    public class JsonConfigCrypter : IConfigCrypter
    {
        private readonly ICrypter _crypter;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigCrypter"/> class.
        /// Creates an instance of the JsonConfigCrypter.
        /// </summary>
        /// <param name="crypter">An ICrypter instance.</param>
        public JsonConfigCrypter(ICrypter crypter)
        {
            _crypter = crypter;
        }


        #region EncryptKeys

        /// <summary>
        /// Encrypts the key in the given content of a config file.
        /// </summary>
        /// <param name="configFileContent">String content of a config file.</param>
        /// <param name="configKey">Key of the config entry. The key has to be in JSONPath format.</param>
        /// <returns>The content of the config file where the key has been encrypted.</returns>
        public string EncryptKey(string configFileContent, string configKey)
        {
            var (parsedConfig, settingsToken) = ParseConfig(configFileContent, configKey);
            if (!settingsToken.Value<string>().StartsWith(ConfigFileCrypterOptions.Describer.ENCRYPTED, StringComparison.OrdinalIgnoreCase))
            {
                var encryptedValue = _crypter.EncryptString(settingsToken.Value<string>());
                settingsToken.Replace(encryptedValue);
            }

            var newConfigContent = parsedConfig.ToString(Formatting.Indented);

            return newConfigContent;
        }


        /// <summary>
        /// Discovery and Encrypts the keys start with [TOENCRYPT] in the given content of a config file.
        /// </summary>
        /// <param name="configFileContent">String content of a config file.</param>        
        /// <returns>The content of the config file where the key has been encrypted.</returns>
        public string DiscoveryEncryptKeys(string configFileContent)
        {
            var (parsedConfig, settingsTokens) = DiscoveryConfigKeys(configFileContent, ConfigFileCrypterOptions.Describer.TOENCRYPT);

            if (settingsTokens != null)
            {
                foreach (var item in settingsTokens)
                {
                    var encryptedValue = _crypter.EncryptString(item.Value<string>());
                    item.Replace(encryptedValue);
                }
            }
            var newConfigContent = parsedConfig.ToString(Formatting.Indented);

            return newConfigContent;

        }


        #endregion


        #region DecryptKeys

        /// <summary>
        /// Decrypts the key in the given content of a config file.
        /// </summary>
        /// <param name="configFileContent">String content of a config file.</param>
        /// <param name="configKey">Key of the config entry. The key has to be in JSONPath format.</param>
        /// <returns>The content of the config file where the key has been decrypted.</returns>
        public string DecryptKey(string configFileContent, string configKey)
        {
            var (parsedConfig, settingsToken) = ParseConfig(configFileContent, configKey);

            if (settingsToken.Value<string>().StartsWith(ConfigFileCrypterOptions.Describer.ENCRYPTED, StringComparison.OrdinalIgnoreCase))
            {
                var encryptedValue = _crypter.DecryptString(settingsToken.Value<string>());
                settingsToken.Replace(encryptedValue);
            }

            var newConfigContent = parsedConfig.ToString(Formatting.Indented);

            return newConfigContent;
        }

        /// <summary>
        /// Discovery and Decrypts the keys start with [ENCRYPTED] in the given content of a config file.
        /// </summary>
        /// <param name="configContent">String content of a config file.</param>        
        /// <returns>The content of the config file where the key has been decrypted.</returns>
        public string DiscoveryDecryptKeys(string configContent)
        {
            var (parsedConfig, settingsTokens) = DiscoveryConfigKeys(configContent, ConfigFileCrypterOptions.Describer.ENCRYPTED);

            if (settingsTokens != null)
            {
                foreach (var item in settingsTokens)
                {
                    var encryptedValue = _crypter.DecryptString(item.Value<string>());
                    item.Replace(encryptedValue);
                }

                //remove the [TOENCRYPT] from the config value, if any
                //foreach (var item in parsedConfig.DiscoveryConfigKeys(ConfigFileCrypterOptions.Describer.TOENCRYPT))
                //{
                //    item.Replace(item.Value<string>().Replace(ConfigFileCrypterOptions.Describer.TOENCRYPT, String.Empty));
                //}
            }
            var newConfigContent = parsedConfig.ToString(Formatting.Indented);

            return newConfigContent;
        }

        #endregion



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }





        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _crypter?.Dispose();
            }
        }

        private static (JObject ParsedConfig, JToken Key) ParseConfig(string json, string configKey)
        {
            var parsedJson = JObject.Parse(json);
            var keyToken = parsedJson.SelectToken(configKey);

            if (keyToken == null)
            {
                throw new InvalidOperationException($"The key {configKey} could not be found.");
            }

            return (parsedJson, keyToken);
        }

        private (JObject ParsedConfig, IEnumerable<JToken> Keys) DiscoveryConfigKeys(string json, string searchPattern)
        {
            var parsedJson = JObject.Parse(json);

            List<JToken> keyTokens = parsedJson.DiscoveryConfigKeys(searchPattern);

            return (parsedJson, keyTokens);
        }



    }

    public static class JsonExtensions
    {
        public static List<JToken> DiscoveryConfigKeys(this JToken parsedJson, string searchPattern)
        {
            List<JToken> keyTokens = parsedJson.FindTokensValueStartWith(searchPattern);

            return keyTokens;
        }


        public static List<JToken> FindTokensValueStartWith(this JToken containerToken, string startWith)
        {
            if (containerToken == null)
                throw new ArgumentNullException(nameof(containerToken), $"The value {nameof(JToken)} cannot be null");

            List<JToken> matches = new List<JToken>();
            FindTokensValueStartWith(containerToken, startWith, matches);
            return matches;
        }


        private static void FindTokensValueStartWith(JToken containerToken, string startWith, List<JToken> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Value.Type == JTokenType.String
                        && child.Value.Value<string>().StartsWith(startWith, StringComparison.OrdinalIgnoreCase))
                    {
                        matches.Add(child.Value);
                    }
                    FindTokensValueStartWith(child.Value, startWith, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokensValueStartWith(child, startWith, matches);
                }
            }
        }



        public static List<JToken> FindTokensByName(this JToken containerToken, string name)
        {
            if (containerToken == null)
                throw new ArgumentNullException(nameof(containerToken), $"The value {nameof(JToken)} cannot be null");

            List<JToken> matches = new List<JToken>();
            FindTokensByName(containerToken, name, matches);
            return matches;
        }

        private static void FindTokensByName(JToken containerToken, string name, List<JToken> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Name == name)
                    {
                        matches.Add(child.Value);
                    }
                    FindTokensByName(child.Value, name, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokensByName(child, name, matches);
                }
            }
        }
    }
}
