using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DevAttic.ConfigCrypter.ConfigProviders.Json.Parser;
using DevAttic.ConfigCrypter.Crypters;
using DevAttic.ConfigCrypter.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace DevAttic.ConfigCrypter.ConfigProviders.Json
{
    /// <summary>
    ///  JSON configuration provider that uses the underlying crypter to decrypt the given keys.
    /// </summary>
    public class EncryptedJsonConfigProvider : JsonConfigurationProvider
    {

        private readonly EncryptedJsonConfigSource _jsonConfigSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedJsonConfigProvider"/> class.
        /// Creates an instance of the EncryptedJsonConfigProvider.
        /// </summary>
        /// <param name="source">EncryptedJsonConfigSource that is used to configure the provider.</param>
        public EncryptedJsonConfigProvider(EncryptedJsonConfigSource source) : base(source)
        {
            _jsonConfigSource = source;
        }

        public override void Load(Stream stream)
        {   
            try
            {
                using var crypter = _jsonConfigSource.CrypterFactory(_jsonConfigSource);
                Data = EncryptedJsonConfigurationFileParser.Parse(stream, crypter);
            }
            catch (JsonException e)
            {
                throw new FormatException($"Error JSONParseError {e.Message}");
            }
            catch (Exception ex)
            {
                throw new DecryptKeyException("Unable to decrypt key, check if are using the correct decrypter keys", ex);
            }            
        }
    }

}