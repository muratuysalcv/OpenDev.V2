
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OpenDev.Data.DataModel
{
    [Table("form")]
    public class Form
    {
        [Key, Required]
        [Column("form_id")]
        public int FormId { get; set; }
        [Column("form_key")]
        public string FormKey { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("parent_form_id")]
        public int? ParentFormId { get; set; }
        [Column("html_head")]
        public string? HtmlHead { get; set; }
        [Column("html_body")]
        public string? HtmlBody { get; set; }
        [Column("html_foot")]
        public string? HtmlFoot { get; set; }
        [Column("sql_load")]
        public string? SqlLoad { get; set; }
        [Column("sql_work")]
        public string? SqlWork { get; set; }
        [Column("active")]
        public bool Active { get; set; }
        [Column("app_key")]
        public string? AppKey { get; set; }

        [Column("form_type_key")]
        public string FormTypeKey { get; set; }

        [Column("layout")]
        public string? Layout { get; set; }

    }
}
