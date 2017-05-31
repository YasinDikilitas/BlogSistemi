using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities
{
   public class MyEntitiyBase
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime ModifyDate { get; set; }
        [Required,StringLength(30)]
        public string ModifiedUser { get; set; }
    }
}
