using DevAttic.ConfigCrypter.Exceptions;
using DevAttic.ConfigCrypter.Extensions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DevAttic.ConfigCrypter.Tests.ConfigProviders
{
    public class EncryptedJsonConfigSourceTests
    {
        [Fact]
        public void AddEncryptedAppSettings_DecryptsValuesOnTheFly()
        {
            var certLoaderMock = Mocks.CertificateLoaderPSW;
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddEncryptedJsonFile(config =>
            {
                config.Path = "appsettings.json";
                //config.KeysToDecrypt = new List<string> { "Test:ToBeEncrypted" };
                config.CertificateLoader = certLoaderMock.Object;
            });
            var configuration = configBuilder.Build();

            var decryptedValue = configuration["Test:Encrypted"];

            var plainValue = configuration["Test:TextPlain"];

            var toEncryptValue = configuration["Test:ToBeEncrypted"];


            Assert.Equal("This is going to be encrypted", plainValue);

            Assert.Equal(plainValue, decryptedValue);

            Assert.Equal(plainValue, decryptedValue);

            Assert.Equal(plainValue, toEncryptValue);
        }

        //[Fact]
        //public void AddEncryptedAppSettings_DecryptsValuesOnTheFly_txtCertificate()
        //{
        //    var certLoaderMock = Mocks.TxTCertificateLoaderPSW;
        //    var configBuilder = new ConfigurationBuilder();
        //    configBuilder.AddEncryptedJsonConfig(config =>
        //    {
        //        config.Path = "appsettings.json";
        //        //config.KeysToDecrypt = new List<string> { "Test:ToBeEncrypted" };
        //        config.CertificateLoader = certLoaderMock.Object;
        //    });
        //    var configuration = configBuilder.Build();

        //    var decryptedValue = configuration["Test:Encrypted"];

        //    var plainValue = configuration["Test:TextPlain"];

        //    var toEncryptValue = configuration["Test:ToBeEncrypted"];


        //    Assert.Equal("This is going to be encrypted", plainValue);

        //    Assert.Equal(plainValue, decryptedValue);

        //    Assert.Equal(plainValue, decryptedValue);

        //    Assert.Equal(plainValue, toEncryptValue);
        //}


        [Fact]
        public void AddEncryptedJsonConfig_DecryptsValuesOnTheFly()
        {
            var certLoaderMock = Mocks.CertificateLoaderNO_PSW;
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddEncryptedJsonFile(config =>
            {
                //config.KeysToDecrypt = new List<string> { "KeyToEncrypt" };
                config.CertificateLoader = certLoaderMock.Object;
                config.Path = "config.json";
            });
            var configuration = configBuilder.Build();

            var plainValue = configuration["TextPlain"];

            var decryptedValue = configuration["KeyToDecrypt"];

            var toEncryptValue = configuration["ToBeEncrypted"];


            Assert.Equal("This will be encrypted.", plainValue);

            Assert.Equal(plainValue, decryptedValue);

            Assert.Equal(plainValue, decryptedValue);

            Assert.Equal(plainValue, toEncryptValue);

        }



        [Fact]
        public void Try_ToDecryptsValuesOnTheFly_With_Wrong_Certificate()
        {
            Assert.Throws<DecryptKeyException>(() =>
            {
                var certLoaderMock = Mocks.CertificateLoaderFAKE;
                var configBuilder = new ConfigurationBuilder();
                configBuilder.AddEncryptedJsonFile(config =>
                {
                    //config.KeysToDecrypt = new List<string> { "KeyToEncrypt" };
                    config.CertificateLoader = certLoaderMock.Object;
                    config.Path = "config.json";
                });
                var configuration = configBuilder.Build();
            });

        }
    }
}
