using System;

namespace DevAttic.ConfigCrypter.Crypters
{
    public class DummyCrypter : ICrypter
    {
        public string DecryptString(string value)
        {
            return value;
        }
        public string EncryptString(string value)
        {
            return value;
        }

        #region Dispose
        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~DummyCrypter()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources here if there are any
        }

        #endregion

    }
}
