
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDev.Data.DataModel
{
    [Table("setting")]
    public class Setting
    {
        [Key, Required]
        [Column("setting_key")]
        public string SettingKey { get; set; }

        [Column("value")]
        public string Value { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

    }
}
