
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDev.Data.DataModel
{
    [Table("app")]
    public class App
    {
        [Key, Required]
        [Column("app_key")]
        public string AppKey { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }


        [Column("parent_app_key")]
        public string? ParentAppKey { get; set; }

        [Column("hosts")]
        public string Hosts { get; set; }

        [Column("active")]
        public bool Active { get; set; }
        [Column("cloud_key")]
        public string CloudKey { get; set; }

        [Column("con_id")]
        public int? ConId { get; set; }
        [Column("schema_name")]
        public string SchemaName { get; set; }

    }
}
