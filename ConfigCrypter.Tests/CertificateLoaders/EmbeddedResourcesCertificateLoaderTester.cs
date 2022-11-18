using DevAttic.ConfigCrypter.CertificateLoaders;
using Xunit;

namespace DevAttic.ConfigCrypter.Tests.CertificateLoaders
{
    //EmbeddedResourcesCertificateLoader
    public static class EmbeddedResourcesCertificateLoaderTester
    {
        [Fact]
        public static void Load_Certificate_no_psw()
        {
            var loader = new EmbeddedResourcesCertificateLoader(System.Reflection.Assembly.GetExecutingAssembly(), TestConstants.CertificateResourceName_NO_PSW);

            Assert.NotNull(loader.LoadCertificate());
        }


        [Fact]
        public static void Load_Certificate_With_null_no_psw()
        {
            var loader = new EmbeddedResourcesCertificateLoader(System.Reflection.Assembly.GetExecutingAssembly(), TestConstants.CertificateResourceName_NO_PSW, null);

            Assert.NotNull(loader.LoadCertificate());
        }

        [Fact]
        public static void Load_Certificate_psw()
        {
            var loader = new EmbeddedResourcesCertificateLoader(System.Reflection.Assembly.GetExecutingAssembly(), TestConstants.CertificateResourceName_PSW, TestConstants.Certificate_PSW);

            Assert.NotNull(loader.LoadCertificate());
        }

    }
}
