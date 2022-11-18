using System;

namespace DevAttic.ConfigCrypter.Exceptions
{
    public class DecryptKeyException : Exception
    {
        public DecryptKeyException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
