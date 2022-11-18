using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using DevAttic.ConfigCrypter.CertificateLoaders;
using DevAttic.ConfigCrypter.Crypters;
using Moq;

namespace DevAttic.ConfigCrypter.Tests
{
    public static class Mocks
    {
        public static Mock<ICertificateLoader> CertificateLoaderFAKE
        {
            get
            {
                var certLoaderMock = new Mock<ICertificateLoader>();
                certLoaderMock.Setup(loader => loader.LoadCertificate()).Returns(() =>
                {
                    return ResourceCertificateLoader(TestConstants.CertificateResourceName_FAKE);

                });

                return certLoaderMock;
            }
        }


        public static Mock<ICertificateLoader> CertificateLoaderNO_PSW
        {
            get
            {
                var certLoaderMock = new Mock<ICertificateLoader>();
                certLoaderMock.Setup(loader => loader.LoadCertificate()).Returns(() =>
                {
                    return ResourceCertificateLoader(TestConstants.CertificateResourceName_NO_PSW);

                });

                return certLoaderMock;
            }
        }

        public static Mock<ICertificateLoader> CertificateLoaderPSW
        {
            get
            {
                var certLoaderMock = new Mock<ICertificateLoader>();
                certLoaderMock.Setup(loader => loader.LoadCertificate()).Returns(() =>
                {
                    return ResourceCertificateLoader(TestConstants.CertificateResourceName_PSW, TestConstants.Certificate_PSW);

                });

                return certLoaderMock;
            }
        }

        public static Mock<ICertificateLoader> InMemoryCertificateLoader
        {
            get
            {
                var certLoaderMock = new Mock<ICertificateLoader>();
                certLoaderMock.Setup(loader => loader.LoadCertificate()).Returns(() =>
                {
                    return CreateCertificate;

                });

                return certLoaderMock;
            }
        }

        public static X509Certificate2 CreateCertificate
        {
            get
            {
                return DevAttic.ConfigCrypter.CertUtils.Create.SelfSignedCertificate("In Memory SelfSigned Ceriticate");

            }
        }


        //public static Mock<ICertificateLoader> TxtCertificateLoaderPSW
        //{
        //    get
        //    {
        //        var certLoaderMock = new Mock<ICertificateLoader>();
        //        certLoaderMock.Setup(loader => loader.LoadCertificate()).Returns(() =>
        //        {
        //            return TxtResourceCertificateLoader(TestConstants.CertificateResourceName_PSW, TestConstants.Certificate_PSW);

        //        });

        //        return certLoaderMock;
        //    }
        //}


        public static Mock<ICertificateLoader> CertificateLoaderPSW_WithNoPSW
        {
            get
            {
                var certLoaderMock = new Mock<ICertificateLoader>();
                certLoaderMock.Setup(loader => loader.LoadCertificate()).Returns(() =>
                {
                    return ResourceCertificateLoader(TestConstants.CertificateResourceName_PSW);

                });

                return certLoaderMock;
            }
        }

        //public static X509Certificate2 TxtResourceCertificateLoader(string manifestcertificateName, string password = null)
        //{
        //    using var certStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(manifestcertificateName);
        //    using var ms = new MemoryStream();
        //    certStream!.CopyTo(ms);

        //    if (string.IsNullOrWhiteSpace(password))
        //        return new X509Certificate2(ms.ToArray());
        //    else
        //        return new X509Certificate2(ms.ToArray(), password);

        //}

        public static X509Certificate2 ResourceCertificateLoader(string manifestcertificateName, string password = null)
        {
            using var certStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(manifestcertificateName);
            using var ms = new MemoryStream();
            certStream!.CopyTo(ms);

            //if (string.IsNullOrWhiteSpace(password))
            //    return new X509Certificate2(ms.ToArray());
            //else
            return new X509Certificate2(ms.ToArray(), password, X509KeyStorageFlags.Exportable);

        }

        public static Mock<ICrypter> Crypter
        {
            get
            {
                var crypterMock = new Mock<ICrypter>();

                crypterMock.Setup(crypter => crypter.EncryptString(It.IsAny<string>()))
                    .Returns<string>(input => $"{ConfigFileCrypterOptions.Describer.ENCRYPTED}{input}_encrypted");


                crypterMock.Setup(crypter => crypter.DecryptString(It.IsAny<string>()))
                    .Returns<string>(input =>
                    {
                        input = input.Replace(ConfigFileCrypterOptions.Describer.ENCRYPTED, string.Empty);
                        var encryptedIndex = input.LastIndexOf("_encrypted", StringComparison.Ordinal);

                        return encryptedIndex > -1
                            ? input.Substring(0, encryptedIndex)
                            : input.Substring(0, input.Length);
                    });

                return crypterMock;
            }
        }
    }
}
