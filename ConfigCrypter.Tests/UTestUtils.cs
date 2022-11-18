using System;
using System.IO;

namespace DevAttic.ConfigCrypter.Tests
{
    public static class UTestUtils
    {
        private static readonly Random _random = new Random();
        public static string TempPathRandomName(string extension)
        {
            return Path.Combine(System.IO.Path.GetTempPath(),$"{Guid.NewGuid()}{_random.Next(0, 1000000)}{extension}");
        }
    }
}
