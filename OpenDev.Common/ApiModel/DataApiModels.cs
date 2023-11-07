using OpenDev.Common.Global;

namespace OpenDev.Common.ApiModel
{
    public class DataRequest
    {
        public string AppKey { get; set; }
        public string TableName { get; set; }

        public string ConnectionKey { get; set; }
        public List<DataFilter> DataFilterList { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalDataCount { get; set; }
        public object Data { get; set; }

        public List<ParamData>? RequestParamList { get; set; }

    }
    public class DataReponse
    {
        public string AppKey { get; set; }
        public string TableName { get; set; }
        public List<DataFilter> DataFilteredList { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalDataCount { get; set; }
        public object Data { get; set; }

    }

    public class DataFilter
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public object ColumnValue { get; set; }
        public string SearchType { get; set; }
    }

    public class ColumnsInfo
    {
        public string title { get; set; }
        public string field { get; set; }
        public string sorter { get; set; }
        public int width { get; set; }
    }

    public class GridDataRow
    {

    }
}
