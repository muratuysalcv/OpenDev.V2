namespace OpenDev.App.Lib
{
    public class MetaColumn
    {
        public bool IsRequired { get; set; }

        public bool IsString { get; set; }

        public int MaxLength { get; set; }


        public string Name { get; set; }

        public string Prompt { get; set; }

        public string RequiredErrorMessage { get; set; }

        public bool Scaffold { get; set; }

        public string ShortDisplayName { get; set; }

        public string SortExpression { get; set; }

        public TypeCode TypeCode { get; set; }

        public string UIHint { get; set; }

        public bool ApplyFormatInEditMode { get; set; }

        public bool ConvertEmptyStringToNull { get; set; }

        public string DataFormatString { get; set; }

        public bool AllowInitialValue { get; set; }

        public bool IsReadOnly { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the data field is part of the table's primary
        //     key.
        //
        // Returns:
        //     true if the data field is part of the table's primary key; otherwise, false.
        public bool IsPrimaryKey { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the data field type can contain long strings.
        //
        // Returns:
        //     true if the data field type can contain long strings; otherwise, false.
        public bool IsLongString { get; set; }
        //
        // Summary:
        //     Gets the collection of metadata attributes that apply to the data field represented
        //     by the System.Web.DynamicData.MetaColumn object.
        //
        // Returns:
        //     The collection of metadata attributes that apply to the data field represented
        //     by the System.Web.DynamicData.MetaColumn object.

        //
        // Summary:
        //     Gets the data field type.
        //
        // Returns:
        //     The data field type.
        public Type ColumnType { get; set; }
        //
        // Summary:
        //     Gets the System.ComponentModel.DataAnnotations.DataTypeAttribute attribute that
        //     is applied to the data field.
        //
        // Returns:
        //     The System.ComponentModel.DataAnnotations.DataTypeAttribute attribute that is
        //     applied to the data field.
        //public DataTypeAttribute DataTypeAttribute { get; set;}
        //
        // Summary:
        //     Gets the default value for the data field.
        //
        // Returns:
        //     The default value for the data field.
        public object DefaultValue { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether field values are HTML-encoded before they
        //     are displayed in a data-bound control.
        //
        // Returns:
        //     true in all cases.
        public bool HtmlEncode { get; set; }
        //
        // Summary:
        //     Gets the display name for the data field.
        //
        // Returns:
        //     The display name for the data field.
        public virtual string DisplayName { get; set; }
        //
        // Summary:
        //     Gets an object that contains attributes of the property that represents the data
        //     field in the entity type.
        //
        // Returns:
        //     An object that contains attributes of the property that represents the data field
        //     in the entity type.

        //
        // Summary:
        //     Gets the description for the data field.
        //
        // Returns:
        //     The description for the data field.
        public virtual string Description { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the data field contains binary data.
        //
        // Returns:
        //     true if the data field contains binary data; otherwise, false.
        public bool IsBinaryData { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the data field exists in the database.
        //
        // Returns:
        //     true if the data field does not exist in the database; otherwise, false.
        public bool IsCustomProperty { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the data field is a floating-point type.
        //
        // Returns:
        //     true if the data field is a floating-point type; otherwise, false.
        public bool IsFloatingPoint { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the data field is part of a foreign key.
        //
        // Returns:
        //     true if the data field is part of a foreign key; otherwise, false.
        public bool IsForeignKeyComponent { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the data field value is automatically generated
        //     in the database.
        //
        // Returns:
        //     true if the data field value is automatically generated in the database; otherwise,
        //     false.
        public bool IsGenerated { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the data field type is an integer type.
        //
        // Returns:
        //     true if the data field type is an integer type; otherwise, false.
        public bool IsInteger { get; set; }
        //
        // Summary:
        //     Gets the System.ComponentModel.DataAnnotations.FilterUIHintAttribute.FilterUIHint
        //     value that is used for the column.
        //
        // Returns:
        //     The System.ComponentModel.DataAnnotations.FilterUIHintAttribute.FilterUIHint
        //     value that is used for the column.
        public string FilterUIHint { get; set; }
        //
        // Summary:
        //     Gets the caption that is displayed for a field when the field's value is null.
        //
        // Returns:
        //     The caption that is displayed for a field when the field's value is null.
        public string NullDisplayText { get; set; }
    }
}
