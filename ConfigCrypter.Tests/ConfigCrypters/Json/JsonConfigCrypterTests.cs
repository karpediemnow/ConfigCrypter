using DevAttic.ConfigCrypter.ConfigCrypters.Json;
using Newtonsoft.Json;
using Xunit;

namespace DevAttic.ConfigCrypter.Tests.ConfigCrypters.Json
{
    public class JsonConfigCrypterTests
    {
        [Fact]
        public void EncryptKey_WithValidJson_CallsEncryptStringOnCrypter()
        {
            var crypterMock = Mocks.Crypter;
            using var jsonCrypter = new JsonConfigCrypter(crypterMock.Object);
            var json = JsonConvert.SerializeObject(new TestAppSettings { Key = "ValueToEncrypt" });

            var encryptedJson = jsonCrypter.EncryptKey(json, "Key");
            var parsedJson = JsonConvert.DeserializeObject<TestAppSettings>(encryptedJson);

            crypterMock.Verify(crypter => crypter.EncryptString("ValueToEncrypt"));

            // Additionally we test if the test crypter does its job.
            Assert.Equal($"{ConfigFileCrypterOptions.Describer.ENCRYPTED}ValueToEncrypt_encrypted", parsedJson.Key);
        }

        [Fact]
        public void DecryptKey_WithValidJson_CallsDecryptStringOnCrypter()
        {
            var crypterMock = Mocks.Crypter;
            using var jsonCrypter = new JsonConfigCrypter(crypterMock.Object);
            var json = JsonConvert.SerializeObject(new { Key = ConfigFileCrypterOptions.Describer.ENCRYPTED+"ValueToEncrypt_encrypted" });

            var decryptedJson = jsonCrypter.DecryptKey(json, "Key");
            var parsedJson = JsonConvert.DeserializeObject<TestAppSettings>(decryptedJson);

            crypterMock.Verify(crypter => crypter.DecryptString(ConfigFileCrypterOptions.Describer.ENCRYPTED + "ValueToEncrypt_encrypted"));
            Assert.Equal("ValueToEncrypt", parsedJson.Key);
        }


        [Fact]
        public void Crypt_and_DecryptKey_WithValidJson_CallsDecryptStringOnCrypter()
        {
            var crypterMock = Mocks.Crypter;
            using var jsonCrypter = new JsonConfigCrypter(crypterMock.Object);
            var json = JsonConvert.SerializeObject(new TestAppSettings { Key = "ValueToEncrypt" });

            var encryptedJson = jsonCrypter.EncryptKey(json, "Key");
            var parsedEncJson = JsonConvert.DeserializeObject<TestAppSettings>(encryptedJson);

            crypterMock.Verify(crypter => crypter.EncryptString("ValueToEncrypt"));
            Assert.Equal($"{ConfigFileCrypterOptions.Describer.ENCRYPTED}ValueToEncrypt_encrypted", parsedEncJson.Key);
                    

            var decryptedJson = jsonCrypter.DecryptKey(encryptedJson, "Key");
            var parsedDecJson = JsonConvert.DeserializeObject<TestAppSettings>(decryptedJson);

            crypterMock.Verify(crypter => crypter.DecryptString(parsedEncJson.Key));
            Assert.Equal("ValueToEncrypt", parsedDecJson.Key);

        }



        [Fact]
        public void Dispose_CallsDisposeOnCrypter()
        {
            var crypterMock = Mocks.Crypter;
            var jsonCrypter = new JsonConfigCrypter(crypterMock.Object);

            jsonCrypter.Dispose();

            crypterMock.Verify(crypter => crypter.Dispose());
        }
    }
}
