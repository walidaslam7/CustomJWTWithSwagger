using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogifier.Shared.DomainEntities
{
    public class CommonProperties
    {

        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
    }
}
