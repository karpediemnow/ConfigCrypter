using System.Security.Cryptography.X509Certificates;
using DevAttic.ConfigCrypter.CertificateLoaders;
using Xunit;

namespace DevAttic.ConfigCrypter.Tests.CertificateLoaders
{
    //StoreCertificateLoader    
    public static class StoreCertificateLoaderTester
    {

        //[Fact]
        //public static void Load_Certificate_no_psw()
        //{
        //    //If null check if have installed the certificate in the certificate store (StoreName: My Store, Store Location: LocalMachine )
        //    Assert.NotNull(new StoreCertificateLoader(TestConstants.Certificate_SubjectName).LoadCertificate());
        //}

        [Fact]
        public static void Addcertiticate_and_Load_Certificate_no_psw()
        {
            using X509Certificate2 x509Certificate = Mocks.ResourceCertificateLoader(TestConstants.CertificateResourceName_NO_PSW);
            Assert.NotNull(x509Certificate);

            var insert = x509Certificate.AddCertToStore(StoreName.My, StoreLocation.CurrentUser);
            Assert.True(insert);

            Assert.NotNull(new StoreCertificateLoader(TestConstants.Certificate_SubjectName, StoreName.My, StoreLocation.CurrentUser).LoadCertificate());

            var removed = x509Certificate.RemoveCertToStore(StoreName.My, StoreLocation.CurrentUser);
            Assert.True(removed);
        }

        [Fact]
        public static void Addcertiticate_and_Load_Certificate_psw()
        {
          using  X509Certificate2 x509Certificate = Mocks.ResourceCertificateLoader(TestConstants.CertificateResourceName_PSW, TestConstants.Certificate_PSW);
            Assert.NotNull(x509Certificate);
            
            var insert = x509Certificate.AddCertToStore( StoreName.My, StoreLocation.CurrentUser);
            Assert.True(insert);

            Assert.NotNull(new StoreCertificateLoader(TestConstants.Certificate_SubjectName, StoreName.My, StoreLocation.CurrentUser).LoadCertificate());

            var removed = x509Certificate.RemoveCertToStore(StoreName.My, StoreLocation.CurrentUser);
            Assert.True(removed);
        }
    }
}
