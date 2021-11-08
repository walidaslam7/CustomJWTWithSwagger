using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogifier.Shared.DomainEntities
{
    class UserProfiles
    {
        public int ProfileId { get; set; }
        public string Bio { get; set; }
        public string WorkExperience { get; set; }

        [ForeignKey("UserId")]
        public Accounts User { get; set; }
    }
}
