using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using DevAttic.ConfigCrypter.CertificateLoaders;
using Xunit;

namespace DevAttic.ConfigCrypter.Tests.CertUtils
{

    public class CertUtilsTests
    {
        [Fact]
        public void Create_No_Psw_In_folder_File_Certificate()
        {
            var certificatePath = Path.Combine(Path.GetTempPath(), "Xunit_test_folder", $"Create_No_Psw_In_folder_File_Certificate.pfx");

            if (File.Exists(certificatePath))
                File.Delete(certificatePath);

            var certSubjectName = "Xunit_CertSubjectName_1";

            string password = null;

            _ = DevAttic.ConfigCrypter.CertUtils.Create.CertificateFile(certificatePath, certSubjectName, password.ToSecureString());

            Assert.True(File.Exists(certificatePath));


            if (File.Exists(certificatePath))
                File.Delete(certificatePath);

            string dirName = Path.GetDirectoryName(certificatePath);

            if (Directory.Exists(dirName))
            {
                Directory.Delete(dirName);
            }
        }

        //[Fact]
        //public void Create_No_Psw_InFile_Certificate()
        //{
        //    string certificatePath = UTestUtils.TempPathRandomName(".pfx");

        //    if (File.Exists(CertificatePath))
        //        File.Delete(CertificatePath);

        //    string CertSubjectName = "Xunit_CertSubjectName_2";

        //    string Password = null;

        //    DevAttic.ConfigCrypter.CertUtils.Create.CertificateFile(CertificatePath, CertSubjectName, Password.ToSecureString());

        //    Assert.True(File.Exists(CertificatePath));


        //    if (File.Exists(CertificatePath))
        //        File.Delete(CertificatePath);
        //}

        [Fact]
        public  void Create_No_Psw_InFile_And_Load_Certificate()
        {
            string certificatePath = UTestUtils.TempPathRandomName(".pfx");

            if (File.Exists(certificatePath))
                File.Delete(certificatePath);

            string certSubjectName = "Xunit_CertSubjectName_3";

            string password = null;

            DevAttic.ConfigCrypter.CertUtils.Create.CertificateFile(certificatePath, certSubjectName, password.ToSecureString());

            var loader = new FilesystemCertificateLoader(certificatePath, password);

            loader.LoadCertificate();


            Assert.True(File.Exists(certificatePath));


            if (File.Exists(certificatePath))
                File.Delete(certificatePath);
        }

        [Fact]
        public void Create_psw_InFile_Certificate()
        {
            string certificatePath = UTestUtils.TempPathRandomName(".pfx");

            if (File.Exists(certificatePath))
                File.Delete(certificatePath);

            string certSubjectName = "Xunit_CertSubjectName_4";

            string password = "Password";

            DevAttic.ConfigCrypter.CertUtils.Create.CertificateFile(certificatePath, certSubjectName, password.ToSecureString());

            Assert.True(File.Exists(certificatePath));


            if (File.Exists(certificatePath))
                File.Delete(certificatePath);
        }

        [Fact]
        public void Create_Psw_InFile_And_Load_Certificate()
        {
            string certificatePath = UTestUtils.TempPathRandomName(".pfx");

            if (File.Exists(certificatePath))
                File.Delete(certificatePath);

            string certSubjectName = "Xunit_CertSubjectName_5";

            string password = "Password";

            DevAttic.ConfigCrypter.CertUtils.Create.CertificateFile(certificatePath, certSubjectName, password.ToSecureString());

            var loader = new FilesystemCertificateLoader(certificatePath, password);

            loader.LoadCertificate();


            Assert.True(File.Exists(certificatePath));


            if (File.Exists(certificatePath))
                File.Delete(certificatePath);
        }

        //[Fact]
        //public void Create_Psw_InFile_And_Load_Certificate_MakeCertToFile_V1()
        //{
        //    string certificatePath = UTestUtils.TempPathRandomName(".pfx");

        //    if (File.Exists(CertificatePath))
        //        File.Delete(CertificatePath);

        //    string CertSubjectName = "Xunit_CertSubjectName_5";

        //    string Password = "Password";

        //    DevAttic.ConfigCrypter.CertUtils.Create.CertificateFileV1(CertificatePath, CertSubjectName, Password.ToSecureString());

        //    var loader = new FilesystemCertificateLoader(CertificatePath, Password);

        //    var cert = loader.LoadCertificate();


        //    Assert.True(File.Exists(CertificatePath));


        //    if (File.Exists(CertificatePath))
        //        File.Delete(CertificatePath);
        //}



        [Fact]
        public void Create_No_Psw_InFile_Certificate_And_Load_With_Psw()
        {
            string certificatePath = UTestUtils.TempPathRandomName(".pfx");

            if (File.Exists(certificatePath))
                File.Delete(certificatePath);

            string certSubjectName = "Xunit_CertSubjectName_6";

            string password = null;

            DevAttic.ConfigCrypter.CertUtils.Create.CertificateFile(certificatePath, certSubjectName, password.ToSecureString());

            var exception = Record.Exception(() => new FilesystemCertificateLoader(certificatePath, "Password").LoadCertificate());

            Assert.NotNull(exception);

            if (File.Exists(certificatePath))
                File.Delete(certificatePath);
        }

        //[Fact]
        //public void Create_No_Psw_InFile_Certificate_And_Load_With_Psw_MakeCertToFile_V1()
        //{
        //    string certificatePath = UTestUtils.TempPathRandomName(".pfx");

        //    if (File.Exists(CertificatePath))
        //        File.Delete(CertificatePath);

        //    string CertSubjectName = "Xunit_CertSubjectName_99";

        //    string Password = null;

        //    DevAttic.ConfigCrypter.CertUtils.Create.CertificateFileV1(CertificatePath, CertSubjectName, Password.ToSecureString());

        //    var exception = Record.Exception(() => new FilesystemCertificateLoader(CertificatePath, "Password").LoadCertificate());

        //    Assert.NotNull(exception);

        //    if (File.Exists(CertificatePath))
        //        File.Delete(CertificatePath);
        //}

        [Fact]
        public void Create_psw_InFile_Certificate_And_Load_With_Wrong_Psw()
        {
            string certificatePath = UTestUtils.TempPathRandomName(".pfx");

            if (File.Exists(certificatePath))
                File.Delete(certificatePath);

            string certSubjectName = "Xunit_CertSubjectName_7";

            string password = "Password";

            DevAttic.ConfigCrypter.CertUtils.Create.CertificateFile(certificatePath, certSubjectName, password.ToSecureString());

            var exception = Record.Exception(() => new FilesystemCertificateLoader(certificatePath, "WRONG_PSW").LoadCertificate());

            Assert.NotNull(exception);

            if (File.Exists(certificatePath))
                File.Delete(certificatePath);
        }

        [Fact]
        public void CheckSecureString()
        {
            string str = null;

            SecureString secString = str?.ToSecureString();

            Assert.Null(secString);

            str = string.Empty;
            secString = str.ToSecureString();

            Assert.NotNull(secString);

            Assert.Equal(str, secString.ToPlainString());
            str = "Password";
            secString = str.ToSecureString();

            Assert.NotNull(secString);
            Assert.Equal(str, secString.ToPlainString());
        }

        [Fact]
        public void Export_PrivateKey_No_Psw_To_String_And_Load()
        {
            string password = null;
            using var cert = Mocks.ResourceCertificateLoader(TestConstants.CertificateResourceName_NO_PSW);

            string certStr = cert.ExportWithPrivateKeyToString();
            Assert.NotNull(certStr);
            
            X509Certificate2 certFromStr = DevAttic.ConfigCrypter.CertUtils.Manage.LoadCertificateFromString(certStr, password.ToSecureString());

            Assert.NotNull(certFromStr);

            Assert.Equal(cert.HasPrivateKey, certFromStr.HasPrivateKey);
            Assert.Equal(cert.Thumbprint, certFromStr.Thumbprint);
        }

        [Fact]
        public void Export_PrivateKey_Psw_To_String_And_Load()
        {
            string password = TestConstants.Certificate_PSW;

            using var cert = Mocks.ResourceCertificateLoader(TestConstants.CertificateResourceName_PSW, password);

            string certStr = cert.ExportWithPrivateKeyToString(password.ToSecureString());
            Assert.NotNull(certStr);

            X509Certificate2 certFromStr = DevAttic.ConfigCrypter.CertUtils.Manage.LoadCertificateFromString(certStr, password.ToSecureString());

            Assert.NotNull(certFromStr);
            Assert.Equal(cert.HasPrivateKey, certFromStr.HasPrivateKey);
            Assert.Equal(cert.Thumbprint, certFromStr.Thumbprint);

        }

        [Fact]
        public void Export_Keys_No_Psw_To_StringFile_And_Load()
        {
            string password = null;
            string outputPrivKeyPath = UTestUtils.TempPathRandomName(".private.key");
            string outputPublicKeyPath = UTestUtils.TempPathRandomName(".public.key");

            if (File.Exists(outputPrivKeyPath))
                File.Delete(outputPrivKeyPath);

            if (File.Exists(outputPublicKeyPath))
                File.Delete(outputPublicKeyPath);

            string certificatePath = @"Certificates\" + TestConstants.CertificateName_NO_PSW;

            DevAttic.ConfigCrypter.CertUtils.Manage.ExportCertiticateToStringFile(certificatePath, outputPrivKeyPath);
            DevAttic.ConfigCrypter.CertUtils.Manage.ExportCertiticateToStringFile(certificatePath, outputPublicKeyPath, true);
            
            var loadedCert = ConfigCrypter.CertUtils.Manage.LoadCertificateFromStringFile(outputPrivKeyPath, password.ToSecureString());
            Assert.NotNull(loadedCert);

            var loadedCert_pulblicKeyOnly = ConfigCrypter.CertUtils.Manage.LoadCertificateFromStringFile(outputPublicKeyPath, password.ToSecureString());

            Assert.NotNull(loadedCert_pulblicKeyOnly);

            using var cert = Mocks.ResourceCertificateLoader(TestConstants.CertificateResourceName_NO_PSW);

            AreEgual(cert, loadedCert);
            //AreEgual(loadedCert, loadedTempCert);

            Assert.Equal(cert.Thumbprint, loadedCert_pulblicKeyOnly.Thumbprint);

            Assert.False(cert.HasPrivateKey == loadedCert_pulblicKeyOnly.HasPrivateKey);

            Assert.NotEqual(cert.PrivateKey?.ToXmlString(true), loadedCert_pulblicKeyOnly.PrivateKey?.ToXmlString(true));
            Assert.NotEqual(cert.PrivateKey?.ToXmlString(false), loadedCert_pulblicKeyOnly.PrivateKey?.ToXmlString(false));

            Assert.Equal(cert.PublicKey.EncodedKeyValue.RawData, loadedCert_pulblicKeyOnly.PublicKey.EncodedKeyValue.RawData);


            if (File.Exists(outputPrivKeyPath))
                File.Delete(outputPrivKeyPath);

            if (File.Exists(outputPublicKeyPath))
                File.Delete(outputPublicKeyPath);

        }


        private static void AreEgual(X509Certificate2 cert1, X509Certificate2 cert2)
        {
            Assert.Equal(cert1.Thumbprint, cert2.Thumbprint);

            Assert.True(cert1.HasPrivateKey == cert2.HasPrivateKey);

            Assert.Equal(cert1.PrivateKey?.ToXmlString(true), cert2.PrivateKey?.ToXmlString(true));
            Assert.Equal(cert1.PrivateKey?.ToXmlString(false), cert2.PrivateKey?.ToXmlString(false));

            Assert.True(cert1.PublicKey.EncodedKeyValue.Equals(cert1.PublicKey.EncodedKeyValue));
        }


        //ConvertPrivateKeyToCertificateFile
        [Fact]
        public void Convert_PrivateKey_To_Certificate_File_And_Load()
        {
            string outputPrivKeyPath = UTestUtils.TempPathRandomName(".private.key");


            if (File.Exists(outputPrivKeyPath))
                File.Delete(outputPrivKeyPath);

            string certificatePath = @"Certificates\" + TestConstants.CertificateName_NO_PSW;

            DevAttic.ConfigCrypter.CertUtils.Manage.ExportCertiticateToStringFile(certificatePath, outputPrivKeyPath);

            Assert.True(File.Exists(outputPrivKeyPath));

            string outputTempCert = UTestUtils.TempPathRandomName(".pfx");

            DevAttic.ConfigCrypter.CertUtils.Manage.ConvertExportedStringToCertificateFile(outputPrivKeyPath, outputTempCert);

            string password = null;
            var loadedCert = ConfigCrypter.CertUtils.Manage.LoadCertificateFromStringFile(outputPrivKeyPath, password.ToSecureString());
            Assert.NotNull(loadedCert);

            var cert = ConfigCrypter.CertUtils.Manage.LoadCertificateFromFile(certificatePath);
            Assert.NotNull(cert);


            var loadedTempCert = ConfigCrypter.CertUtils.Manage.LoadCertificateFromFile(outputTempCert);
            Assert.NotNull(loadedTempCert);

            AreEgual(cert, loadedCert);
            AreEgual(loadedCert, loadedTempCert);
            //Assert.Equal(cert.Thumbprint, loadedCert.Thumbprint);
            //Assert.Equal(loadedCert.Thumbprint, loadedTempCert.Thumbprint);               

            //Assert.True(cert.HasPrivateKey == loadedCert.HasPrivateKey == loadedTempCert.HasPrivateKey);

            //Assert.Equal(cert.PrivateKey.ToXmlString(true), loadedCert.PrivateKey.ToXmlString(true));
            //Assert.Equal(loadedCert.PrivateKey.ToXmlString(true), loadedTempCert.PrivateKey.ToXmlString(true));



            if (File.Exists(outputPrivKeyPath))
                File.Delete(outputPrivKeyPath);

        }

    }
}