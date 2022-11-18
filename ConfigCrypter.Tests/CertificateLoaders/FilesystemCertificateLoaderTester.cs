using DevAttic.ConfigCrypter.CertificateLoaders;
using Xunit;

namespace DevAttic.ConfigCrypter.Tests.CertificateLoaders
{
    //FilesystemCertificateLoader
    public static class FilesystemCertificateLoaderTester
    { 
        [Fact]
        public static void Load_Certificate_no_psw()
        {
            var loader = new FilesystemCertificateLoader(@"Certificates\"+ TestConstants.CertificateName_NO_PSW);

            Assert.NotNull(loader.LoadCertificate());
        }


        [Fact]
        public static void Load_Certificate_With_null_no_psw()
        {
            var loader = new FilesystemCertificateLoader(@"Certificates\" + TestConstants.CertificateName_NO_PSW, (string)null);

            Assert.NotNull(loader.LoadCertificate());
        }

        [Fact]
        public static void Load_Certificate_psw()
        {
            var loader = new FilesystemCertificateLoader(@"Certificates\" + TestConstants.CertificateName_PSW, TestConstants.Certificate_PSW);

            Assert.NotNull(loader.LoadCertificate());
        }

    }
}
