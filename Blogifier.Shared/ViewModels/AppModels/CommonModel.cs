using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blogifier.Shared.ViewModels.AppModels
{
    public class Principal
    {
        public string email { get; set; }
        public List<Claim> claims { get; set; }
    }
}
