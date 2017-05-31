using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities
{
    [Table("Likes")]
    public class Liked
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public virtual Note Note { get; set; }
        public virtual BlogUser LikedUser { get; set; }
    }
}
