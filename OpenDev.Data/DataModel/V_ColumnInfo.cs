using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OpenDev.Data.DataModel
{
    [Table("v_column_list", Schema = "core")]
    public class V_ColumnInfo
    {
        [Key, Required]
        [Column("app_column_key")]
        public string AppColumnKey { get; set; }

        [Column("app_key")]
        public string AppKey { get; set; }

        [Column("table_schema")]
        public string TableSchema { get; set; }

        [Column("table_name")]
        public string TableName { get; set; }

        [Column("column_name")]
        public string ColumnName { get; set; }

        [Column("ordinal_position")]
        public int? OrdinalPosition { get; set; }

        [Column("column_default")]
        public string? ColumnDefault { get; set; }

        [Column("data_type")]
        public string DataType { get; set; }

        [Column("character_maximum_length")]
        public int? CharacterMaximumLength { get; set; }

        [Column("udt_name")]
        public string UdtName { get; set; }

        [Column("is_identity")]
        public string? IsIdentity { get; set; }

        [Column("identity_generation")]
        public string? IdentityGeneration { get; set; }
    }
}
