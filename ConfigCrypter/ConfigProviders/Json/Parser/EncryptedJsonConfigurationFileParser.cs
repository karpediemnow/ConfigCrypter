using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DevAttic.ConfigCrypter.Crypters;
using Microsoft.Extensions.Configuration;

namespace DevAttic.ConfigCrypter.ConfigProviders.Json.Parser
{
    internal class EncryptedJsonConfigurationFileParser
    {
        private EncryptedJsonConfigurationFileParser()
        {
        }

        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _context = new Stack<string>();
        private string _currentPath;
        private ICrypter _crypter;
        public static IDictionary<string, string> Parse(Stream input, ICrypter crypter)
            => new EncryptedJsonConfigurationFileParser().ParseStream(input, crypter);

        private IDictionary<string, string> ParseStream(Stream input, ICrypter crypter)
        {
            _data.Clear();

            var jsonDocumentOptions = new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };

            using (var reader = new StreamReader(input))
            using (var doc = JsonDocument.Parse(reader.ReadToEnd(), jsonDocumentOptions))
            {
                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                {
                    throw new FormatException($"Error UnsupportedJSONToken {doc.RootElement.ValueKind}");
                }
                _crypter = crypter;
                VisitElement(doc.RootElement);
            }

            return _data;
        }

        private void VisitElement(JsonElement element)
        {
            foreach (var property in element.EnumerateObject())
            {
                EnterContext(property.Name);
                VisitValue(property.Value);
                ExitContext();
            }
        }

        private void VisitValue(JsonElement value)
        {
            switch (value.ValueKind)
            {
                case JsonValueKind.Object:
                    VisitElement(value);
                    break;

                case JsonValueKind.Array:
                    var index = 0;
                    foreach (var arrayElement in value.EnumerateArray())
                    {
#pragma warning disable CA1305 // Specify IFormatProvider
                        EnterContext(index.ToString());
#pragma warning restore CA1305 // Specify IFormatProvider
                        VisitValue(arrayElement);
                        ExitContext();
                        index++;
                    }

                    break;

                case JsonValueKind.Number:
                case JsonValueKind.String:
                case JsonValueKind.True:
                case JsonValueKind.False:
                case JsonValueKind.Null:
                    var key = _currentPath;
                    if (_data.ContainsKey(key))
                    {
                        throw new FormatException($"Error KeyIsDuplicated {key}");
                    }

                    var val = value.ToString();

                    if (val.StartsWith(ConfigFileCrypterOptions.Describer.ENCRYPTED, StringComparison.OrdinalIgnoreCase))
                    {
                        val = _crypter.DecryptString(val);
                    }

                    //remove the [TOENCRYPT] from the config value, if any
                    if (val.StartsWith(ConfigFileCrypterOptions.Describer.TOENCRYPT, StringComparison.OrdinalIgnoreCase))
                    {
                        val = val.Replace(ConfigFileCrypterOptions.Describer.TOENCRYPT, string.Empty);
                    }

                    _data[key] = val;
                    break;

                default:
                    throw new FormatException($"Error UnsupportedJSONToken {value.ValueKind}");
            }
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }

}