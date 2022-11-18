using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DevAttic.ConfigCrypter;
using DevAttic.ConfigCrypter.CertificateLoaders;
using DevAttic.ConfigCrypter.Helpers;

namespace ConfigCrypter.Azure
{
    public class AzureCertificateLoader : ICertificateLoader, ICertificateLoaderAsync
    {
        private SecretClient client;

        public AzureCertificateLoader(string keyVaultName, string certiticateSecretId, string passwordSecretId = null)
        {
            KeyVaultName = keyVaultName;
            CertificateSecretId = certiticateSecretId;
            PasswordSecretId = passwordSecretId;

            var kvUri = "https://" + KeyVaultName + ".vault.azure.net";


            client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

        }

        public string CertificateSecretId { get; }
        public string PasswordSecretId { get; }
        public string KeyVaultName { get; }

        public async Task<X509Certificate2> LoadCertificateAsync()
        {

            var pkRes = await client.GetSecretAsync(CertificateSecretId).ConfigureAwait(false);

            string pk = pkRes.Value.Value;
            string psw = null;

            if (PasswordSecretId == null)
            {
                var pswRes = await client.GetSecretAsync(PasswordSecretId).ConfigureAwait(false);
                psw = pswRes.Value.Value;
            }

            return CertUtils.Manage.LoadCertificateFromString(pk, psw.ToSecureString());

        }

        public X509Certificate2 LoadCertificate()
        {
            return AsyncHelpers.RunSync<X509Certificate2>(() => LoadCertificateAsync());
        }
    }
}
