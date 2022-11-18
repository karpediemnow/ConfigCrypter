ECHO "import test-certificate-psw.txt to file test-certificate-psw.pfx"
ConfigCrypter.Console.exe import --output Certificates\test-certificate-psw.pfx --password MySecret! --path Certificates\test-certificate-psw.txt

ECHO "import test-certificate-no-psw.txt  to file test-certificate-no-psw.pfx"
ConfigCrypter.Console.exe import --output  Certificates\test-certificate-no-psw.pfx --path Certificates\test-certificate-no-psw.txt
