using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OpenDev.Common.ApiModel;
using OpenDev.Data.DataModel;
using OpenDev.Core.Engine;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Data;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Cors;
using OpenDev.Common.Global;

namespace OpenDev.Core.Controllers
{
    public class OpenDevApi : ControllerBase
    {
        [HttpPost(Name = "RenderWithPost")]
        public RenderResponse RenderWithPost([FromBody] RenderRequest model)
        {
            model.RequestParamList = GetParamList(this.HttpContext);
            var renderEngine = new RenderEngine(model);
            var renderResponse = renderEngine.Render();

            return renderResponse;
        }
        [HttpGet(Name = "RenderWithGet")]
        public RenderResponse RenderWithGet([FromQuery] RenderRequest model)
        {
            model.RequestParamList = GetParamList(this.HttpContext);
            var renderEngine = new RenderEngine(model);
            var renderResponse = renderEngine.Render();

            return renderResponse;
        }

        [HttpPost(Name = "DataWithPost")]
        public DataReponse DataWithPost([FromBody] DataRequest model)
        {
            model.RequestParamList = GetParamList(this.HttpContext);
            var dataResponse = new DataReponse();
            DbModel _db = new DbModel();
            var app = _db.AppList.FirstOrDefault();
            var columnInfoListAll = _db.V_ColumnInfoList.ToList();
            var pKeyColumnInfo = _db.V_PrimaryColumnInfoList.FirstOrDefault(x => x.TableName == model.TableName && x.AppKey == model.AppKey);

            return dataResponse;
        }
        [HttpGet(Name = "DataWithGet")]
        public DataReponse DataWithGet([FromQuery] DataRequest model)
        {
            model.RequestParamList = GetParamList(this.HttpContext);
            var dataResponse = new DataReponse();
            DbModel _db = new DbModel();
            var app = _db.AppList.FirstOrDefault();
            var columsList = _db.V_ColumnInfoList.ToList();
            return dataResponse;
        }

        [HttpPost(Name = "SchemaWithPost")]
        public CrudResponse SchemaWithPost([FromBody] CrudRequest model)
        {
            model.RequestParamList = GetParamList(this.HttpContext);
            var renderEngine = new CrudEngine(model);
            var renderResponse = renderEngine.Render();
            return renderResponse;
        }
        [HttpGet(Name = "SchemaWithGet")]
        public CrudResponse SchemaWithGet([FromQuery] CrudRequest model)
        {
            model.RequestParamList = GetParamList(this.HttpContext);
            var engine = new CrudEngine(model);
            var response = engine.Render();
            return response;
        }
        public List<ParamData> GetParamList(HttpContext httpContext)
        {
            var paramList = new List<ParamData>();
            var queryKeys = httpContext.Request.Query.Keys;

            foreach (var key in queryKeys)
            {
                paramList.Add(new ParamData()
                {
                    Name = key,
                    Value = httpContext.Request.Query[key]
                });
            }

            try
            {
                var formKeys = httpContext.Request.Form.Keys;
                foreach (var key in formKeys)
                {
                    paramList.Add(new ParamData()
                    {
                        Name = key,
                        Value = httpContext.Request.Form[key]
                    });
                }

            }
            catch (Exception)
            {

            }
           
            return paramList;
        }

    }
}

