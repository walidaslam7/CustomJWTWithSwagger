using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogifier.Shared.ViewModels.RequestModels
{
    public class SignInRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class RefreshRequest
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
    }
}
