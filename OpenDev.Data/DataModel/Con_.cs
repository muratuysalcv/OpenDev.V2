
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDev.Data.DataModel
{
    [Table("con")]
    public class Con
    {
        [Key, Required]
        [Column("con_id")]
        public int ConId { get; set; }

        [Column("con_type_key")]
        public string ConTypeKey { get; set; }

        [Column("app_key")]
        public string AppKey { get; set; }


        //[Column("description")]
        //public string? Description { get; set; }

        [Column("db_server")]
        public string? DbServer { get; set; }


        [Column("db_name")]
        public string? DbName { get; set; }


        [Column("db_user")]
        public string? DbUser { get; set; }


        [Column("db_password")]
        public string? DbPassword { get; set; }

        [Column("port")]
        public int? Port { get; set; }

        [Column("active")]
        public bool Active { get; set; }

    }
}
