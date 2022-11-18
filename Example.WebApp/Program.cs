using DevAttic.ConfigCrypter.CertificateLoaders;
using DevAttic.ConfigCrypter.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Example.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration(cfg =>
                {
                    //Standard config File
                    //cfg.AddJsonFile("appsettings.json", false, true);
                    //Replace with                    
                    cfg.AddEncryptedJsonFile(crypter =>
                    {
                        crypter.Path = "appsettings.json";
                        crypter.Optional = false;
                        crypter.ReloadOnChange = true;

                        crypter.CertificateLoader = new EmbeddedResourcesCertificateLoader(System.Reflection.Assembly.GetExecutingAssembly(), "Example.WebApp.test-certificate.pfx");

                        //or loaded from path:                       
                        //string certificatePath = Environment.GetEnvironmentVariable("CertificatePath");
                        //if (string.IsNullOrWhiteSpace(certificatePath))
                        //    crypter.CrypterFactory = cfg => new DummyCrypter();
                        //else
                        //    crypter.FileSystemCertificateLoader(certificatePath);


                    });
                });
    }
}
