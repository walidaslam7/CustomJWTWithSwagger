using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogifier.Shared.DomainEntities
{
    public class Blogs : CommonProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }

        [ForeignKey("CategoryId")]
        public Categories Categories { get; set; }

        [ForeignKey("UserId")]
        public Accounts User { get; set; }
    }
}
