using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace DevAttic.ConfigCrypter.CertificateLoaders
{
    public class EmbeddedResourcesCertificateLoader : ICertificateLoader
    {
        private readonly string manifestcertificateName;
        private string password;

        public EmbeddedResourcesCertificateLoader(Assembly assembly, string manifestcertificateName, string password = null)
        {
            this.manifestcertificateName = manifestcertificateName;
            Assembly = assembly;
            this.password = password;
        }

        public Assembly Assembly { get; }

        public X509Certificate2 LoadCertificate()
        {
            using var certStream = Assembly.GetManifestResourceStream(manifestcertificateName);
            using var ms = new MemoryStream();
            certStream!.CopyTo(ms);
                return new X509Certificate2(ms.ToArray(), password);
        }
    }
}




/*
 if (string.IsNullOrWhiteSpace(password))
     return new X509Certificate2(ms.ToArray());
 else
     return new X509Certificate2(ms.ToArray(), password);
 */