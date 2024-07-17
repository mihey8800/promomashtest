using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;

namespace Promomash.Host.Extensions
{
    public static class ConfigureKestrelExtensions
    {
        public static void ConfigureKestrel(this IServiceCollection serviceCollection,
            IConfigurationRoot configuration,
            Serilog.ILogger logger)
        {
            var certPem = configuration.GetValue<string>("AuthServer:JwtPemCertificateContents");
            var rsaKeyPem = configuration.GetValue<string>("AuthServer:JwtPemRsaPrivateKeyContents");

            //if (!hostingEnvironment.IsDevelopment())
            //{
            //	PreConfigure<AbpIdentityServerBuilderOptions>(options => options.AddDeveloperSigningCredential = false);
            //}

            if (string.IsNullOrEmpty(certPem) || string.IsNullOrEmpty(rsaKeyPem))
            {
                return;
            }

            try
            {
                var cert = X509Certificate2.CreateFromPem(certPem);
                var rsa = RSA.Create();
                rsa.ImportFromPem(rsaKeyPem);
                cert = cert.CopyWithPrivateKey(rsa);

                serviceCollection.Configure<KestrelServerOptions>(options => options.ConfigureHttpsDefaults(options1 =>
                {
                    options1.ServerCertificate = cert;
                    options1.AllowAnyClientCertificate();
                    options1.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                    options1.CheckCertificateRevocation = false;
                }));
            }
            catch (Exception e)
            {
                throw new Exception("Error loading sever certificate", e);
            }
        }
    }
}