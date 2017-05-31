using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.Entities
{
    [Table("StreamerInfos")]
    public class StreamerInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(30)]
        public string Surname { get; set; }
        [StringLength(200)]
        public string Interest { get; set; }
        [StringLength(200)]
        public string Experince { get; set; }
        [StringLength(200)]
        public string Usingos { get; set; }
        [StringLength(200)]
        public string Hobby { get; set; }


        public virtual KodlatvUser Owner { get; set; }
    }
}
