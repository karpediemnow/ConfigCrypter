
ECHO "init test-certificate-psw.pfx config file appsettings.json"
ConfigCrypter.Console.exe certInfo --path Certificates\test-certificate-psw.pfx --password MySecret! -v
