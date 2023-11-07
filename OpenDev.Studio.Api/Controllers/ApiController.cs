using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OpenDev.Common.ApiModel;
using OpenDev.Data.DataModel;
using OpenDev.Core.Engine;
using OpenDev.Core.Controllers;
using Microsoft.AspNetCore.Cors;

namespace OpenDev.Studio.Api.Controllers
{
    [Route("api/[action]")]
    [EnableCors()]
    [ApiController]
    public class ApiController : OpenDevApi
    {
        //[HttpPost(Name = "RenderWithPost")]
        //public RenderResponseApiModel RenderWithPost([FromBody] RenderRequestApiModel model)
        //{
        //    var renderEngine = new RenderEngine(model);
        //    var renderResponse = renderEngine.Render();

        //    return renderResponse;
        //}
        //[HttpGet(Name = "RenderWithGet")]
        //public RenderResponseApiModel RenderWithGet([FromQuery] RenderRequestApiModel model)
        //{
        //    var renderEngine = new RenderEngine(model);
        //    var renderResponse = renderEngine.Render();

        //    return renderResponse;
        //}

        //[HttpPost(Name = "DataWithPost")]
        //public DataReponseApiModel DataWithPost([FromBody] DataRequestApiModel model)
        //{
        //    var dataResponse = new DataReponseApiModel();
        //    DbModel _db = new DbModel();
        //    var app = _db.AppList.FirstOrDefault();
        //    var columsList = _db.V_ColumnInfoList.ToList();
        //    return dataResponse;
        //}
        //[HttpGet(Name = "DataWithGet")]
        //public DataReponseApiModel DataWithGet([FromQuery] DataRequestApiModel model)
        //{
        //    var dataResponse = new DataReponseApiModel();
        //    DbModel _db = new DbModel();
        //    var app = _db.AppList.FirstOrDefault();
        //    var columsList = _db.V_ColumnInfoList.ToList();
        //    return dataResponse;
        //}
    }
}
