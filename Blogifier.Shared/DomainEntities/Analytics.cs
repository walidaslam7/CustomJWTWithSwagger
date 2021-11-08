using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogifier.Shared.DomainEntities
{
    public class Analytics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnalyticsId { get; set; }
        public int BlogId { get; set; }
        public string IPAddress { get; set; }
        
    }
}
