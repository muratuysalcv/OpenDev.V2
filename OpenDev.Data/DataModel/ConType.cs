
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDev.Data.DataModel
{
    [Table("con_type")]
    public class ConType
    {
        [Key, Required]
        [Column("con_type_key")]
        public string ConTypeKey { get; set; }

        [Column("name")]
        public string Name { get; set; }


        [Column("description")]
        public string? Description { get; set; }

    }
}
