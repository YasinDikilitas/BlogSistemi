using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.Entities
{
    [Table("Videos")]
    public class Video:MyEntityBase
    {
        [StringLength(150)]
        public string Videoinfo { get; set; }
        [StringLength(500),Required]
        public string Youtubeurl { get; set; }
        [StringLength(50)]
        public string Levelofvideo { get; set; }
        public int Watchnumber { get; set; }

        public virtual Channel Channel { get; set; }
        public virtual Category Category { get; set; }
    }
}
