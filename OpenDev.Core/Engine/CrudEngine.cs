using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenDev.Common.ApiModel;
using OpenDev.Common.Global;
using OpenDev.Data.DataModel;
using System.Data;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OpenDev.Core.Engine
{
    public class CrudEngine : BaseEngine
    {
        public CrudRequest _crudRequestModel { get; set; }
        public App _app { get; set; }
        public Form _form { get; set; }

        private DbModel _db { get; set; }
        private string _pageHtmlTemplate
        {
            get
            {
                return @"
                            <!doctype html>
                            <html>
                            <head>
                                <title>{title}</title>
                                {RENDER_HEAD}
                            </head>
                            <body>
                                {RENDER_BODY}
                                {RENDER_FOOT}
                            </body>
                         </html>";
            }
        }
        public CrudEngine(CrudRequest model, DbModel db = null)
        {
            if (db == null)
                _db = new DbModel();
            else
                _db = db;

            _crudRequestModel = model;
            this._app = _db.AppList.FirstOrDefault(x => x.AppKey == model.AppKey);
            this._form = _db.FormList.FirstOrDefault(x => x.FormKey == model.FormKey && x.AppKey == model.AppKey);
        }

        public CrudResponse Render()
        {
            var response = new CrudResponse();
            var dataResponse = new CrudResponse()
            {
                TableName = _crudRequestModel.TableName
            };
            DbModel _db = new DbModel();
            var app = _db.AppList.FirstOrDefault();
            var columList = _db.V_ColumnInfoList.Where(x => x.TableName == _crudRequestModel.TableName);
            dataResponse.Data = columList.Select(x => new
            {
                ClumnName = x.ColumnName,
                UniversalDataType = x.UdtName,
                DataType = x.DataType,
                MaxLength = x.CharacterMaximumLength
            });

            var renderEngine = new RenderEngine(new RenderRequest()
            {
                AppKey = _crudRequestModel.AppKey,
                FormKey = _crudRequestModel.FormKey,
                RequestParamList = _crudRequestModel.RequestParamList,
                RenderMode = _crudRequestModel.RenderMode
            });
            var renderResponse = renderEngine.Render();
            response.HTML = renderResponse.HTML;

            return response;
        }

    }
}
