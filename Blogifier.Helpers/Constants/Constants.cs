using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogifier.Helpers.Constants
{
    public static class Constant
    {
        public enum Messages {
            SIGN_IN_SUCCESS = 1,
            SIGN_IN_FAILED,
            SIGN_OUT,
            TOKEN_REFRESHED
        }
    }
}
