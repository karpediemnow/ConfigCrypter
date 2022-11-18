ECHO "Creating test-certificate-psw.pfx"
ConfigCrypter.Console.exe create --path Certificates\test-certificate-psw.pfx --password MySecret! --name TestSubjectName

ECHO "Creating test-certificate-fake.pfx"
ConfigCrypter.Console.exe create --path Certificates\test-certificate-fake.pfx  --name TestSubjectName

ECHO "Creating test-certificate-no-psw.pfx"
ConfigCrypter.Console.exe create --path Certificates\test-certificate-no-psw.pfx --name TestSubjectName


