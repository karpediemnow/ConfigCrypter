using System;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace DevAttic.ConfigCrypter.CertificateLoaders
{
    /// <summary>
    /// Loader that loads a certificate from the filesystem.
    /// </summary>
    public class FilesystemCertificateLoader : ICertificateLoader
    {
        private readonly string _certificatePath;
        private readonly SecureString _password;


        /// <summary>
        /// Creates an instance of the certificate loader.
        /// </summary>
        /// <param name="certificatePath">Fully qualified path to the certificate (.pfx file).</param>
        public FilesystemCertificateLoader(string certificatePath, string password = null) : this(certificatePath, password.ToSecureString())
        {

        }


        public FilesystemCertificateLoader(string certificatePath, SecureString password)
        {
            if (string.IsNullOrWhiteSpace(certificatePath))
            {
                throw new InvalidOperationException("Invalid Certificate Path.");
            }

            if (File.Exists(certificatePath))
                _certificatePath = certificatePath;
            else
                throw new FileNotFoundException($"Certificate Not Found from {Directory.GetCurrentDirectory()}", Path.GetFileName(certificatePath));

            this._password = password;
        }

        /// <summary>
        /// Loads a certificate from the given location on the filesystem.
        /// </summary>
        /// <returns>A X509Certificate2 instance.</returns>
        public X509Certificate2 LoadCertificate()
        {
            X509Certificate2 certificate2;
            //if (string.IsNullOrWhiteSpace(_password))
            //    certificate2 = new X509Certificate2(_certificatePath);            
            //else            
            certificate2 = ConfigCrypter.CertUtils.Manage.LoadCertificateFromFile(_certificatePath, _password);

            return certificate2;
        }
    }
}