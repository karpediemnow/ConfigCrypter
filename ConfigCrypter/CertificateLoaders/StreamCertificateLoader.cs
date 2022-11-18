using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace DevAttic.ConfigCrypter.CertificateLoaders
{
    public class StreamCertificateLoader : ICertificateLoader
    {
        private readonly string password;

        public StreamCertificateLoader(Stream stream, string password = null)
        {
            Stream = stream;
            this.password = password;
        }

        public Stream Stream { get; }

        public X509Certificate2 LoadCertificate()
        {
            using var certStream = Stream;
            using var ms = new MemoryStream();
            certStream!.CopyTo(ms);

            if (string.IsNullOrWhiteSpace(password))
                return new X509Certificate2(ms.ToArray());
            else
                return new X509Certificate2(ms.ToArray(), password);
        }
    }
}
