using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogifier.Shared.ViewModels.ResponseModels
{
    public class AccountResponses
    {
        public int userId { get; set; }
        public string token { get; set; }
        public DateTime tokenExpirationTime { get; set; }
        public string refreshToken { get; set; }
        public DateTime refreshTokenExpirationTime { get; set; }
    }
}
