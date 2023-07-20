using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(ServerlessRESTAzure.Startup))]

namespace ServerlessRESTAzure
{
    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {

            var keyVaultUrl = new Uri(Environment.GetEnvironmentVariable("KeyVaultUrl"));
            
            Azure.Identity.DefaultAzureCredentialOptions options = new Azure.Identity.DefaultAzureCredentialOptions { Diagnostics = { IsLoggingContentEnabled = true } };
            var secretClient = new SecretClient(keyVaultUrl, new DefaultAzureCredential(options));

            var cs = secretClient.GetSecret("sql").Value.Value;
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(cs));
        }
    }
}
