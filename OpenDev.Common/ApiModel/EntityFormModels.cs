namespace OpenDev.Common.ApiModel
{
    public class EntityFormRequest
    {
        public string AppKey { get; set; }
        public string EntityFormKey { get; set; }

    }
    public class EntityFormReponse
    {
        public string AppKey { get; set; }
        public string EntityFormKey { get; set; }
        public object Data { get; set; }
    }
}
