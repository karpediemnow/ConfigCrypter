using System;
using DevAttic.ConfigCrypter.ConfigProviders.Json;
using Microsoft.Extensions.Configuration;

namespace DevAttic.ConfigCrypter.Extensions
{
    public static class ConfigurationBuilderExtensions
    {

        /// <summary>
        /// Adds a provider to decrypt keys in the given json config file.
        /// </summary>
        /// <param name="builder">A ConfigurationBuilder instance.</param>
        /// <param name="configAction">An action used to configure the configuration source.</param>
        /// <returns>The current ConfigurationBuilder instance.</returns>
        public static IConfigurationBuilder AddEncryptedJsonFile(
                    this IConfigurationBuilder builder, Action<EncryptedJsonConfigSource> configAction)

        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configAction is null)
            {
                throw new ArgumentNullException(nameof(configAction), "Configuration Action cannot by null");
            }

            var configSource = new EncryptedJsonConfigSource();

            configAction(configSource);

            //if (configSource.CertificateLoader == null  && configSource.CrypterFactory ==null)
            //{
            //    throw new InvalidOperationException(
            //        "Either CertificatePath or CertificateSubjectName has to be provided if CertificateLoader has not been set manually.");
            //}

            if (string.IsNullOrEmpty(configSource.Path))
            {
                throw new InvalidOperationException(
                    "The \"Path\" property has to be set to the path of a config file.");
            }

            builder.Add(configSource);
            return builder;
        }
    }
}