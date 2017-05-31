
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.Entities
{
    [Table("SendMessages")]
    public class SendMessage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [StringLength(40)]
        public string Recievername { get; set; }
        [StringLength(50)]
        public string Messagetitle { get; set; }
        [StringLength(500)]
        public string Message { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual KodlatvUser Owner { get; set; }

    }
}
