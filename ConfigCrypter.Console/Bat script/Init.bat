
ECHO "init test-certificate-psw.pfx config file appsettings.json"
ConfigCrypter.Console.exe init --path Certificates\test-certificate-psw.pfx --password MySecret! --name TestSubjectName -k Test.ToBeEncryptedByCommandLine,Test.ToBeEncryptedByCommandLine2 --file appsettings.json
