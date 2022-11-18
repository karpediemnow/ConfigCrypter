ECHO "decrypt test-certificate-psw.pfx config file appsettings_encrypted.json"
ConfigCrypter.Console.exe decrypt --path Certificates\test-certificate-psw.pfx --password MySecret! --file appsettings_encrypted.json -k Test.ToBeEncryptedByCommandLine,Test.ToBeEncryptedByCommandLine2


ECHO "decrypt test-certificate-no-psw.pfx config file config_encrypted.json"
ConfigCrypter.Console.exe decrypt --path Certificates\test-certificate-no-psw.pfx --file config_encrypted.json -k ToBeEncryptedByCommandLine,ToBeEncryptedByCommandLine2