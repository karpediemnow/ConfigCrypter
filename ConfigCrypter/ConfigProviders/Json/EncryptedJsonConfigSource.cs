using System;
using DevAttic.ConfigCrypter.CertificateLoaders;
using DevAttic.ConfigCrypter.Crypters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace DevAttic.ConfigCrypter.ConfigProviders.Json
{
    /// <summary>
    /// ConfigurationSource for encrypted JSON config files.
    /// </summary>
    public class EncryptedJsonConfigSource : JsonConfigurationSource
    {
        /// <summary>
        /// Gets or sets a certificate loader instance. Custom loaders can be used.
        /// </summary>
        public ICertificateLoader CertificateLoader { get; set; }

        /// <summary>
        ///  Use the default file system certificate loader.
        /// </summary>
        /// <param name="certificatePath">The fully qualified path of the certificate.</param>
        /// <param name="certificatePassword">The password for loding the certificate (if any).</param>
        public void FileSystemCertificateLoader(string certificatePath, string certificatePassword = null)
        {
            CertificateLoader = new FilesystemCertificateLoader(certificatePath, certificatePassword);
        }

        /// <summary>
        /// Use the default local machine store loader by subject (windows only).
        /// </summary>
        /// <param name="certificateSubjectName">The subject name of the certificate (Issued for).</param>
        public void StoreCertificateLoader(string certificateSubjectName)
        {
            CertificateLoader = new StoreCertificateLoader(certificateSubjectName);
        }


        ///// <summary>
        ///// The fully qualified path of the certificate.
        ///// </summary>
        //public string CertificatePath { get; set; }

        ///// <summary>
        ///// The password for loding the certificate
        ///// </summary>
        //public string CertificatePassword { get; set; }

        ///// <summary>
        ///// The subject name of the certificate (Issued for).
        ///// </summary>
        //public string CertificateSubjectName { get; set; }

        /// <summary>
        /// Gets or sets factory function that is used to create an instance of the crypter.
        /// The default factory uses the RSACrypter and passes it the given certificate loader.
        /// </summary>
        public Func<EncryptedJsonConfigSource, ICrypter> CrypterFactory { get; set; } = cfg => new RSACrypter(cfg.CertificateLoader);


        ///// <summary>
        ///// List of keys that should be decrypted. Hierarchical keys need to be separated by colon.
        ///// <code>Example: "Nested:Key"</code>
        ///// </summary>
        //public List<string> KeysToDecrypt { get; set; } = new List<string>();


        /// <summary>
        /// Creates an instance of the EncryptedJsonConfigProvider.
        /// </summary>
        /// <param name="builder">IConfigurationBuilder instance.</param>
        /// <returns>An EncryptedJsonConfigProvider instance.</returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            base.Build(builder);
            return new EncryptedJsonConfigProvider(this);
        }
    }
}