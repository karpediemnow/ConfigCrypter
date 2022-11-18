using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DevAttic.ConfigCrypter.CertificateLoaders;
using DevAttic.ConfigCrypter.Exceptions;

namespace DevAttic.ConfigCrypter.Crypters
{


    /// <summary>
    /// RSA based crypter that uses the public and private key of a certificate to encrypt and decrypt strings.
    /// </summary>
    public class RSACrypter : ICrypter
    {
        
        private readonly ICertificateLoader _certificateLoader;
        private RSA _privateKey;
        private RSA _publicKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="RSACrypter"/> class.
        ///  Creates an instance of the RSACrypter.
        /// </summary>
        /// <param name="certificateLoader">A certificate loader instance.</param>
        public RSACrypter(ICertificateLoader certificateLoader)
        {
            _certificateLoader = certificateLoader;
            InitKeys();
        }

        /// <summary>
        /// Encrypts the given string with the private key of the loaded certificate.
        /// </summary>
        /// <param name="value">String to decrypt.</param>
        /// <returns>Encrypted string.</returns>
        public string DecryptString(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "The value to Decrypt cannot be null");

            var decodedBase64 = Convert.FromBase64String(value.Replace(ConfigFileCrypterOptions.Describer.ENCRYPTED, string.Empty));
            var decryptedValue = _privateKey.Decrypt(decodedBase64, RSAEncryptionPadding.OaepSHA256);

            return Encoding.UTF8.GetString(decryptedValue);
        }


        #region Dispose
        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~RSACrypter()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        /// <summary>
        /// Disposes the underlying keys.
        /// </summary>
        /// <param name="disposing">True if called from user code, false if called by finalizer.</param>

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources

                _privateKey?.Dispose();
                _publicKey?.Dispose();
            }
            // free native resources here if there are any
        }

        #endregion





        /// <summary>
        /// Decrypts the given string with the public key of the loaded certificate.
        /// </summary>
        /// <param name="value">String to encrypt.</param>
        /// <returns>Encrypted string.</returns>
        public string EncryptString(string value)
        {
            var encryptedValue = _publicKey.Encrypt(Encoding.UTF8.GetBytes(value), RSAEncryptionPadding.OaepSHA256);

            return ConfigFileCrypterOptions.Describer.ENCRYPTED + Convert.ToBase64String(encryptedValue);
        }

        private void InitKeys()
        {
            try
            {
                using var certificate = _certificateLoader.LoadCertificate();
                _privateKey = certificate.GetRSAPrivateKey();
                _publicKey = certificate.GetRSAPublicKey();

                if (_privateKey == null)
                    throw new InvalidOperationException("Unable to Get the RSA Private Key. Needed to Decrypt items.");

                if (_publicKey == null)
                    throw new InvalidOperationException("Unable to Get the RSA Public Key. Needed to encrypt items.");

            }
            catch (Exception ex)
            {
                throw new LoadCertificateException($"Unable to load the certificate using the loader: {_certificateLoader.GetType().Name}", ex);
            }
            //catch (CryptographicException ex)
            //{
            //    //password usata per il certificato non valida
            //    //ex.Message == "Password di rete specificata non corretta."
            //    System.Diagnostics.Debug.WriteLine(ex.Message);
            //    throw;
            //}

        }
    }
}