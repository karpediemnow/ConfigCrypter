using System;

namespace DevAttic.ConfigCrypter.Exceptions
{
    public class LoadCertificateException : Exception
    {
        public LoadCertificateException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
