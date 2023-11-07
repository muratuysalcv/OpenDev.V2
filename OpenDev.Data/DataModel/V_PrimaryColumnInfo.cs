using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDev.Data.DataModel
{
    [Table("v_primary_column", Schema = "core")]
    public class V_PrimaryColumnInfo
    {
        [Key, Required]
        [Column("app_key")]
        public string AppKey{ get; set; }
        [Column("db_name")]
        public string DbName { get; set; }
        [Column("table_name")]
        public string TableName { get; set; }
        [Column("column_name")]
        public string ColumnName { get; set; }
    }
}
