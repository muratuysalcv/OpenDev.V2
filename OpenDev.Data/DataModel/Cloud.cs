
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDev.Data.DataModel
{
    [Table("cloud")]
    public class Cloud
    {
        [Key, Required]
        [Column("cloud_key")]
        public string CloudKey { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("cloud_ip")]
        public string? CloudIP { get; set; }


        [Column("ftp_user")]
        public string? FtpUser { get; set; }


        [Column("ftp_password")]
        public string? FtpPassword { get; set; }


        [Column("cloud_host")]
        public string? CloudHost { get; set; }

        [Column("active")]
        public bool Active { get; set; }

    }
}
