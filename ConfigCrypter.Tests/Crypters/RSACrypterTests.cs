using DevAttic.ConfigCrypter.Crypters;
using DevAttic.ConfigCrypter.Exceptions;
using Xunit;

namespace DevAttic.ConfigCrypter.Tests.Crypters
{
    public class RSACrypterTests
    {
        [Fact]
        public void Constructor_CallsLoadFilesystemCertificateLoaderFake()
        {
            var fsPswCert = Mocks.CertificateLoaderFAKE;

            using var rsaCrypter = new RSACrypter(fsPswCert.Object);

            fsPswCert.Verify(loader => loader.LoadCertificate());
        }


        [Fact]
        public void Constructor_CallsLoadFilesystemCertificateLoaderPsw()
        {
            var fsPswCert = Mocks.CertificateLoaderPSW;

            using var rsaCrypter = new RSACrypter(fsPswCert.Object);

            fsPswCert.Verify(loader => loader.LoadCertificate());
        }

        [Fact]
        public void Constructor_CallsInMemoryCertificateLoader()
        {
            var fsNoPswCert = Mocks.InMemoryCertificateLoader;

            using var rsaCrypter = new RSACrypter(fsNoPswCert.Object);

            fsNoPswCert.Verify(loader => loader.LoadCertificate());
        }


        [Fact]
        public void Constructor_CallsLoadFilesystemCertificateLoaderNoPsw()
        {
            var fsNoPswCert = Mocks.CertificateLoaderNO_PSW;

            using var rsaCrypter = new RSACrypter(fsNoPswCert.Object);

            fsNoPswCert.Verify(loader => loader.LoadCertificate());
        }


        [Fact]
        public void Constructor_CallsLoadFilesystemCertificateLoaderNoPsw_WithNoPassword()
        {
            var fsNoPswCert = Mocks.CertificateLoaderPSW_WithNoPSW;


            Assert.Throws<LoadCertificateException>(() => new RSACrypter(fsNoPswCert.Object));

        }

    }
}
