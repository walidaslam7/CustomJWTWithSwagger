using Blogifier.Providers.Context;
using Blogifier.Shared.DomainEntities;
using Blogifier.Shared.ViewModels.AppModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogifier.Services.Services
{
    public class ExceptionLogsService
    {
        public void SaveExceptions(IServiceProvider services, ExceptionLogsModel model)
        {
            var context = services.GetRequiredService<AppDbContext>();
            context.ExceptionLogs.Add(
                new ExceptionLogs()
                {
                    Message = model.Message,
                    StackTrace = model.StackTrace,
                    ErrorCode = model.ErrorCode
                });
            context.SaveChanges();
        }
    }
}
