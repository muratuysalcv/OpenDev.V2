
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDev.Data.DataModel
{
    [Table("access")]
    public class Access
    {
        [Key, Required]
        [Column("access_id")]
        public int AccessId { get; set; }

        [Column("from")]
        public string From { get; set; }

        [Column("to")]
        public string To { get; set; }
        [Column("has_access")]
        public bool HasAccess { get; set; }
       
    }
}
