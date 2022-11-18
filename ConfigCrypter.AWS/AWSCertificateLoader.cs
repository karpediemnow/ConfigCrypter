using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using DevAttic.ConfigCrypter;
using DevAttic.ConfigCrypter.CertificateLoaders;
using DevAttic.ConfigCrypter.Helpers;

namespace ConfigCrypter.AWS
{
    /// <summary>
    /// https://aws.amazon.com/blogs/security/how-to-use-aws-secrets-manager-client-side-caching-in-dotnet/.
    /// </summary>
    public class AWSCertificateLoader : IDisposable, ICertificateLoader, ICertificateLoaderAsync
    {
        private readonly IAmazonSecretsManager secretsManager;
        private readonly SecretsManagerCache cache;

        public AWSCertificateLoader(string certificateSecretId, string passwordSecretId = null)
        {
            this.secretsManager = new AmazonSecretsManagerClient();
            this.cache = new SecretsManagerCache(this.secretsManager);
            CertificateSecretId = certificateSecretId;
            PasswordSecretId = passwordSecretId;
        }

        public AWSCertificateLoader(IAmazonSecretsManager secretsManager, SecretsManagerCache cache, string certificateSecretId, string passwordSecretId)
        {
            this.secretsManager = secretsManager;
            this.cache = cache;
            CertificateSecretId = certificateSecretId;
            PasswordSecretId = passwordSecretId;
        }

        public string CertificateSecretId { get; }
        public string PasswordSecretId { get; }

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
        ~AWSCertificateLoader()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources

                secretsManager.Dispose();
                cache.Dispose();

            }
            // free native resources here if there are any
        }

        #endregion

        public async Task<X509Certificate2> LoadCertificateAsync()
        {
            string certString = await cache.GetSecretString(CertificateSecretId).ConfigureAwait(false);
            string password =null;
            
            if (PasswordSecretId !=null)
             password = await cache.GetSecretString(PasswordSecretId).ConfigureAwait(false);

            return DevAttic.ConfigCrypter.CertUtils.Manage.LoadCertificateFromString(certString, password.ToSecureString());
            
        }

        public X509Certificate2 LoadCertificate()
        {
            return  AsyncHelpers.RunSync<X509Certificate2>(() => LoadCertificateAsync());
        }

    }

}
