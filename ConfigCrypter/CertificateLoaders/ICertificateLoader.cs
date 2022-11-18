﻿using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DevAttic.ConfigCrypter.CertificateLoaders
{
    /// <summary>
    /// Responsible for loading a certificate.
    /// </summary>
    /// <remarks>Custom certificate loaders can be implemented by implementing this interface.</remarks>
    public interface ICertificateLoader
    {
        /// <summary>
        /// Loads a certificate.
        /// </summary>
        /// <returns>A X509Certificate2 instance.</returns>
        X509Certificate2 LoadCertificate();
    }


    public interface ICertificateLoaderAsync
    {
        Task<X509Certificate2> LoadCertificateAsync();
    }
}