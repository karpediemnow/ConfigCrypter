using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DevAttic.ConfigCrypter.CertificateLoaders
{
    /// <summary>
    /// Loader that loads a certificate from the Windows certificate store.
    /// </summary>
    public class StoreCertificateLoader : ICertificateLoader
    {
        private readonly string _subjectName;

        /// <summary>
        /// Load certificate from My Store, Store Location: LocalMachine
        /// </summary>
        /// <param name="subjectName"></param>
        public StoreCertificateLoader(string subjectName) : this(subjectName, StoreName.My, StoreLocation.LocalMachine)
        {
        }

        public StoreCertificateLoader(string subjectName, StoreName storeName, StoreLocation storeLocation)
        {
            if (string.IsNullOrWhiteSpace(subjectName))
            {
                throw new InvalidOperationException("Invalid CertificateSubjectName.");
            }


            _subjectName = subjectName;
            StoreName = storeName;
            StoreLocation = storeLocation;
        }

        public StoreName StoreName { get; }
        public StoreLocation StoreLocation { get; }

        /// <summary>
        /// Loads a certificate by subject name from the store.
        /// </summary>
        /// <returns>A X509Certificate2 instance.</returns>
        /// <remarks>The loader looks for the certificate in the own certificates of the local machine store. It uses the FindBySubjectName find type.</remarks>
        public X509Certificate2 LoadCertificate()
        {
            using (var store = new X509Store(StoreName, StoreLocation))
            {
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

                var certs = store.Certificates.Find(X509FindType.FindBySubjectName, _subjectName, false);
                var cert = certs.Cast<X509Certificate2>().FirstOrDefault();
                store.Close();

                return cert;
            }
        }
    }
}