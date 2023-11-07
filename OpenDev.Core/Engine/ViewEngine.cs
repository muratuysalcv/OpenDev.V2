using Newtonsoft.Json;
using OpenDev.Common.ApiModel;
using OpenDev.Common.Global;
using System.Globalization;
using System.Text;

namespace OpenDev.Core.Engine
{
    public class ViewEngine : BaseEngine
    {
        private RenderRequest _requestModel { get; set; }
        public ViewEngine(RenderRequest requestModel) {
            _requestModel = requestModel;
        }
        public async Task<RenderResponse> View()
        {
            RenderRequest requestModel = _requestModel;
            var responseModel = new RenderResponse();
            if (string.IsNullOrEmpty(requestModel.FormKey))
                requestModel.FormKey = "index";

            if (string.IsNullOrEmpty(requestModel.AppKey))
                requestModel.AppKey = "core";

            if (requestModel.RequestParamList == null)
                requestModel.RequestParamList = new List<ParamData>();

            var app = _db.AppList.FirstOrDefault(x => x.AppKey == requestModel.AppKey && x.Active);

            if (app != null)
            {
                var cloud = _db.CloudList.FirstOrDefault(x => x.CloudKey == app.CloudKey && app.Active);
                if (cloud != null)
                {
                    var apiRenderUrl = cloud.CloudHost + "/api/RenderWithPost";
                    try
                    {
                        using (var httpClient = new HttpClient())
                        {
                            StringContent content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                            using (var response = await httpClient.PostAsync(apiRenderUrl, content))
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                responseModel = JsonConvert.DeserializeObject<RenderResponse>
                                    (apiResponse);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        responseModel.HTML = "ERROR:"+ex.Message;
                    }
                }
                else
                {
                    responseModel.HTML = "ERROR:Cloud is not active or not exist. Cloud : " + app.CloudKey;
                }

            }
            else
            {
                responseModel.HTML = "ERROR:App is not active or not exist. App : '" + requestModel.AppKey + "'.";
            }
            return responseModel;
        }
    }
}
