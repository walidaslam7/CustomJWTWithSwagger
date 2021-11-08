using Blogifier.Providers.Context;
using Blogifier.Shared.DomainEntities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Blogifier.Helpers.Security;

namespace Carwash.Core.Data
{
    public class SeedDB
    {
        public static void Initialize(IServiceProvider serviceProvider, string securityKey)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
            if (!context.Accounts.Any())
            {
                Accounts adminAccount = new Accounts()
                {
                    Email = "abc@gmail.com",
                    Name = "ABC",
                    AddedOn = DateTime.UtcNow,
                    Password = SecurityHelper.EncryptString("78545@PK", securityKey),
                };
                context.Accounts.Add(adminAccount);
                context.SaveChanges();
            }
        }
    }
}
