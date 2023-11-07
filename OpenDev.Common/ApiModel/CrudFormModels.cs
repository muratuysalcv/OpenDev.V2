using OpenDev.Common.Global;

namespace OpenDev.Common.ApiModel
{
    public class CrudRequest
    {
        public string AppKey { get; set; }
        public string TableName { get; set; }
        public string? FormKey { get; set; }
        public string? RenderMode { get; set; }
        public List<ParamData>? RequestParamList { get; set; }

    }
    public class CrudResponse
    {
        public string AppKey { get; set; }
        public string TableName { get; set; }
        public object Data { get; set; }
        public string? HTML { get; set; }

        public List<ParamData>? RequestParamList { get; set; }
    }
}
