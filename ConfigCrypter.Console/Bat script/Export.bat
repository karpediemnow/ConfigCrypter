ECHO "export test-certificate-psw.pfx to file test-certificate-psw.txt"
ConfigCrypter.Console.exe export --path Certificates\test-certificate-psw.pfx --password MySecret! --output Certificates\test-certificate-psw.txt


ECHO "export test-certificate-no-psw.pfx to file test-certificate-no-psw.txt"
ConfigCrypter.Console.exe export --path Certificates\test-certificate-no-psw.pfx --output Certificates\test-certificate-no-psw.txt
