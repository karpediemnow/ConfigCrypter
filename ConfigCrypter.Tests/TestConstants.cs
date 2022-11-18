namespace DevAttic.ConfigCrypter.Tests
{
    public static class TestConstants
    {
        public const string Certificate_SubjectName = "TestSubjectName";

        public const string Certificate_PSW = "MySecret!";

        public const string CertificateName_FAKE = "test-certificate-fake.pfx";
        public const string CertificateName_NO_PSW = "test-certificate-no-psw.pfx";
        public const string CertificateName_PSW = "test-certificate-psw.pfx";

        public const string TxtCertificateName_NO_PSW = "test-certificate-no-psw.txt";
        public const string TxtCertificateName_PSW = "test-certificate-psw.txt";


        public const string CertificateResourceName_FAKE = "DevAttic.ConfigCrypter.Tests.Certificates." + CertificateName_FAKE;

        public const string CertificateResourceName_NO_PSW = "DevAttic.ConfigCrypter.Tests.Certificates." + CertificateName_NO_PSW;

        public const string CertificateResourceName_PSW = "DevAttic.ConfigCrypter.Tests.Certificates." + CertificateName_PSW;

        public const string TxtCertificateResourceName_NO_PSW = "DevAttic.ConfigCrypter.Tests.Certificates." + TxtCertificateName_NO_PSW;
        public const string TxtCertificateResourceName_PSW = "DevAttic.ConfigCrypter.Tests.Certificates." + TxtCertificateName_PSW;
    }
}
