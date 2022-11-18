ECHO "encrypt test-certificate-psw.pfx config file appsettings.json"
ConfigCrypter.Console.exe encrypt --path Certificates\test-certificate-psw.pfx --password MySecret! --file appsettings.json --keys Test.ToBeEncryptedByCommandLine,Test.ToBeEncryptedByCommandLine2 


ECHO "encrypt test-certificate-no-psw.pfx config file config.json"
ConfigCrypter.Console.exe encrypt --path Certificates\test-certificate-no-psw.pfx --file config.json --keys ToBeEncryptedByCommandLine,ToBeEncryptedByCommandLine2 
