using OpenDev.Common.Global;
using OpenDev.Data.DataModel;
using System.Net;
using static System.Net.WebRequestMethods;

namespace OpenDev.Common.ApiModel
{
    public class RenderRequest
    {
        public RenderRequest()
        {
            RequestParamList = new List<ParamData>();
        }
        public string? CloudKey { get; set; }
        public string? AppKey { get; set; }
        public string? FormKey { get; set; }
        public string? RenderMode { get; set; }
        public List<ParamData>? RequestParamList { get; set; }
    }

    public class PreRender
    {
        public List<ParamData> LoadParams { get; set; }
        public List<ParamData> WorkParams { get; set; }
        public string HtmlHead { get; set; }
        public string HtmlBody { get; set; }
        public string HtmlFoot { get; set; }
        public string? Layout { get; set; }
        public bool LayoutUsed { get; set; }
    }

    public class RenderResponse
    {
        public RenderResponse()
        {
            StatusCode = HttpStatusCode.OK;
        }
        public string AppKey { get; set; }
        public string FormKey { get; set; }
        public List<ParamData> ResponseParamList { get; set; }
        public string HTML { get; set; }
        public decimal RenderTimeSeconds { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public bool HasError
        {
            get
            {
                if (StatusCode == HttpStatusCode.OK)
                    return false;
                return true;
            }
        }
        public void SetError(string ErrorCode, string ErrorTitle, string ErrorMessage)
        {
            this.ErrorCode = ErrorCode;
            this.ErrorMessage = ErrorMessage;
            this.ErrorTitle = ErrorTitle;
            this.StatusCode = HttpStatusCode.InternalServerError;
        }

        public string ErrorHtmlTemplate(string errorHtml = "")
        {
            if (errorHtml == null)
                errorHtml = @" <!-- loader -->
                                    <div id=""loader"">
                                        <div class=""spinner-border text-primary"" role=""status""></div>
                                    </div>
                                    <!-- * loader -->

                                    <!-- App Header -->
                                    <div class=""appHeader no-border transparent position-absolute"">
                                        <div class=""left"">
                                            <a href=""#"" class=""headerButton goBack"">
                                                <ion-icon name=""chevron-back-outline""></ion-icon>
                                            </a>
                                        </div>
                                        <div class=""pageTitle"">Error</div>
                                        <div class=""right"">
                                        </div>
                                    </div>
                                    <!-- * App Header -->

                                    <!-- App Capsule -->
                                    <div id=""appCapsule"">

                                        <div class=""error-page"">
                                            <img src=""/lib/img/error.png"" alt=""alt"" class=""imaged square w200"">
                                            <h1 class=""title"">{ErrorTitle}</h1>
                                            <div class=""text mb-5"">
                                                {ErrorMessage}
                                            </div>

                                            <div class=""fixed-footer"">
                                                <div class=""row"">
                                                    <div class=""col-12"">
                                                        <button type=""button"" onclick=""history.back()"" class=""btn btn-primary btn-lg btn-block"">Go Back</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- * App Capsule -->";
            return errorHtml;
        }
        public string ErrorCode { get; set; }
        public string ErrorTitle { get; set; }
        public string ErrorMessage { get; set; }
    }

}
