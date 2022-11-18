using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace DevAttic.ConfigCrypter
{
    public static class Utils
    {
        public static SecureString ToSecureString(this string password, bool makeReadOnly = true)
        {
            if (password != null)
            {
               var secure = new SecureString();

                //Array.ForEach(password.ToCharArray(), secure.AppendChar);

                password?.ToCharArray().ToList().ForEach(p => secure.AppendChar(p));

                //foreach (char c in password)
                //{
                //    secure.AppendChar(c);
                //}
                if(makeReadOnly)
                    secure.MakeReadOnly();
                return secure;
            }
            else
            {
                return null;
            }
        }

        public static string ToPlainString(this SecureString data)
        {
            var pointer = IntPtr.Zero;
            try
            {
                pointer = Marshal.SecureStringToGlobalAllocUnicode(data);
                return Marshal.PtrToStringUni(pointer);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(pointer);
            }
        }

        public static void NullCheck(this object o,
     string additionalInfo = null,
[System.Runtime.CompilerServices.CallerMemberName] string callerName = null,
     [System.Runtime.CompilerServices.CallerLineNumber] int line = -1,
     [System.Runtime.CompilerServices.CallerFilePath] string path = null)
        {
            if (o is null)
            {
                throw new ArgumentNullException($"{additionalInfo} CallerName: {callerName}."
#if DEBUG
                    +$"{Environment.NewLine}Caller FilePath: {path}." +
                    $"{Environment.NewLine}Caller LineNumber: {line}."

#endif
                    );
            }
        }
    }
}
