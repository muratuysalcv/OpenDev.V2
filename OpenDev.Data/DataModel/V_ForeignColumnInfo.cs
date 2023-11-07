using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDev.Data.DataModel
{
    [Table("v_foreign_column", Schema = "core")]
    public class V_ForeignColumnInfo
    {
        [Key, Required]
        [Column("constraint_name")]
        public string ConstraintName { get; set; }
        [Column("to_table_name")]
        public string ToTableName { get; set; }
        [Column("to_column_name")]
        public string ToColumnName { get; set; }
        [Column("from_table_name")]
        public string FromTableName { get; set; }
        [Column("from_column_name")]
        public string FromColumnName { get; set; }
    }
}
