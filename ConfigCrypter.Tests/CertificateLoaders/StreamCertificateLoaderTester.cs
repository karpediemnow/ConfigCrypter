using DevAttic.ConfigCrypter.CertificateLoaders;
using Xunit;

namespace DevAttic.ConfigCrypter.Tests.CertificateLoaders
{
    //StreamCertificateLoader
    public static class StreamCertificateLoaderTester
    {
        [Fact]
        public static void Load_Certificate_no_psw()
        {
            var loader = new StreamCertificateLoader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(TestConstants.CertificateResourceName_NO_PSW));

            Assert.NotNull(loader.LoadCertificate());
        }


        [Fact]
        public static void Load_Certificate_With_null_no_psw()
        {
            var loader = new StreamCertificateLoader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(TestConstants.CertificateResourceName_NO_PSW), null);

            Assert.NotNull(loader.LoadCertificate());
        }

        [Fact]
        public static void Load_Certificate_psw()
        {
            var loader = new StreamCertificateLoader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(TestConstants.CertificateResourceName_PSW), TestConstants.Certificate_PSW);

            Assert.NotNull(loader.LoadCertificate());
        }

    }
}
