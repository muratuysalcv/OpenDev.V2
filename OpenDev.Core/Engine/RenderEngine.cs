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

namespace OpenDev.Core.Engine
{
    public class RenderEngine : BaseEngine
    {
        public RenderRequest _renderRequestApiModel { get; set; }
        public App _app { get; set; }
        public Form _form { get; set; }
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
        public RenderEngine(RenderRequest model)
        {
            _db = new DbModel();
            _renderRequestApiModel = model;
            this._app = _db.AppList.FirstOrDefault(x => x.AppKey == model.AppKey);
            this._form = _db.FormList.FirstOrDefault(x => x.FormKey == model.FormKey && x.AppKey == model.AppKey);
        }

        /// <summary>
        /// Configures for master page renders. Final render not completed in this function.
        /// Last Render() function completes rendering from returned PreRenderApiModel objects.
        /// </summary>
        /// <param name="form"></param>
        /// <returns name="PreRenderApiModel"></returns>
        private PreRender PreRender(Form form, Form requestedBy = null)
        {
            var preRender = new PreRender();
            if (form != null)
            {
                bool hasAccess = true;
                if (requestedBy != null)
                {
                    var accessModel = new AccessEngine(fromForm: requestedBy, toForm: form);
                    hasAccess = accessModel.HasAccess();
                }
                // no access
                if (!hasAccess)
                {
                    preRender.HtmlBody = "ERROR:Access denied. form : " + form.FormKey;
                }
                // not active
                else if (!form.Active)
                {
                    preRender.HtmlBody = "ERROR:Form not exist or not active. form : " + form.FormKey;
                }
                // valid to render
                else
                {
                    if (form.ParentFormId.HasValue)
                    {
                        var subForm = _db.FormList.FirstOrDefault(x => x.AppKey == _app.AppKey && x.FormId == form.ParentFormId.Value && x.Active);
                        var subPreRender = this.PreRender(subForm);
                        preRender.HtmlHead = subPreRender.HtmlHead + form.HtmlHead;
                        preRender.HtmlFoot = subPreRender.HtmlFoot + form.HtmlFoot;
                        preRender.HtmlBody = subPreRender.HtmlBody.Replace("{RENDER_BODY}", form.HtmlBody);
                        if (!string.IsNullOrEmpty(subPreRender.Layout))
                        {
                            preRender.Layout = subForm.Layout;
                        }
                    }
                    else
                    {
                        preRender.HtmlHead = form.HtmlHead;
                        preRender.HtmlBody = form.HtmlBody;
                        preRender.HtmlFoot = form.HtmlFoot;
                        preRender.Layout = form.Layout;
                    }
                }
            }
            else
                preRender.HtmlBody = "ERROR:404";
            return preRender;
        }

        public RenderResponse Render(bool mainForm = true)
        {
            // işlem sonucunu tutup return edilecek model.
            var renderRes = new RenderResponse() { };

            // render için kullanılacak template
            var pageTemplate = "";

            // replacement yapılacak değerlerin listesini tutacak obje.
            var replacementData = new List<Common.Global.ParamData>();

            // eğer main form ise kalan {*} tüm parametrelri temizlemesine izin verilir.
            var clearEmptyParameters = mainForm;

            // return edilecek model
            RenderResponse parentFormRenderResponse = null;

            #region VALIDASYONLAR
            // eğer form null ise bir sorun vardır ve hata kodu set edilir.
            if (this._form == null)
            {
                renderRes.SetError("E404", "Page Not Found", "Form not exist or not active.");
            }
            #endregion

            // eğer hata varsa zaten parent ne olduğu önemli değildir.
            if (!renderRes.HasError && this._renderRequestApiModel.RenderMode!="raw")
            {
                #region PARENT FORMUN RENDER EDİLMESİ
                if (this._form != null)
                    if (this._form.ParentFormId.HasValue)
                    {
                        // TODO: performans iyileştirimesi için cache kullanılabilir.
                        Form parentForm = _db.FormList.FirstOrDefault(x => x.FormId == this._form.ParentFormId.Value);
                        var parentRenderEngine = new RenderEngine(new RenderRequest()
                        {
                            AppKey = _app.AppKey,
                            FormKey = parentForm.FormKey,
                            RequestParamList = this._renderRequestApiModel.RequestParamList
                        });
                        parentFormRenderResponse = parentRenderEngine.Render(false);
                    }

                #endregion
            }

            pageTemplate = this._pageHtmlTemplate;
            #region PAGE TEMPLATE kararı verilir.

            // eğer hata varsa hiçbirşey görmemesi için 
            // layout head ve foot alınır. 
            // render kısmına da hata kodu eklenir.
            if (renderRes.HasError)
            {
                pageTemplate = this._pageHtmlTemplate;
                var customErrorForm = _db.FormList.Where(x => x.FormKey == renderRes.ErrorCode && x.FormTypeKey == "error");

                // eğer özel hata sayfası yoksa default 'error' sayfası render edilir ve parametreler eklenir.
                if (customErrorForm != null)
                {
                    pageTemplate = (new RenderEngine(new RenderRequest()
                    {
                        AppKey = _renderRequestApiModel.AppKey,
                        FormKey = "error",
                        RequestParamList = _renderRequestApiModel.RequestParamList
                    })).Render().HTML;
                    replacementData.Add(new ParamData()
                    {
                        Name = "{error.title}",
                        Value = renderRes.ErrorTitle
                    });
                    replacementData.Add(new ParamData()
                    {
                        Name = "{error.message}",
                        Value = renderRes.ErrorMessage
                    });
                    replacementData.Add(new ParamData()
                    {
                        Name = "{error.code}",
                        Value = renderRes.ErrorCode
                    });
                }
                pageTemplate = pageTemplate.Replace("{RENDER_HEAD}", "<render key='layout.head'></render>");
                pageTemplate = pageTemplate.Replace("{RENDER_FOOT}", "<render key='layout.foot'></render>");
                pageTemplate = pageTemplate.Replace("{RENDER_BODY}", renderRes.ErrorHtmlTemplate());
            }
            // eğer hata yok ve parent form varsa template doğrudan oradan gelen html dir.
            else if (parentFormRenderResponse != null)
            {
                pageTemplate = parentFormRenderResponse.HTML;
            }
            // eğer parent form yok ama formun kendisine ait bir layout u var ise devreye girer.
            else if (!string.IsNullOrEmpty(this._form.Layout))
            {
                pageTemplate = this._form.Layout;
            }
            #endregion

            #region TEMPLATE e göre replacement tamamlanır ve sayfa hazır hale getirilir.
            // gelen template replace edilir.
            var head = "";
            var foot = "";
            var body = "";
            if (this._form != null)
            {
                head = _form.HtmlHead;
                foot = _form.HtmlFoot;
                body = _form.HtmlBody;
            }
            if (!mainForm)
            {
                head += "{RENDER_HEAD}";
                foot += "{RENDER_FOOT}";
            }

            // eğer bir hata var ise ana body değeri error html olarak değiştirilir. 
            // kullanıcının hatasını görmesi sağlanır.
            if (renderRes.HasError)
            {
                body = renderRes.ErrorHtmlTemplate();
            }

            pageTemplate = pageTemplate.Replace("{RENDER_HEAD}", head);
            pageTemplate = pageTemplate.Replace("{RENDER_FOOT}", foot);
            pageTemplate = pageTemplate.Replace("{RENDER_BODY}", body);
            renderRes.HTML = pageTemplate;
            #endregion


            #region REPLACEMENT PREPERATION - FORM VALUES

            //formdan gelen parametreler replacement a eklenir.
            foreach (var item in _renderRequestApiModel.RequestParamList)
            {
                replacementData.Add(new ParamData()
                {
                    Name = "{" + item.Name + "}",
                    Value = item.Value
                });
                if (!item.Name.StartsWith("P_"))
                    replacementData.Add(new ParamData()
                    {
                        Name = "{P_" + item.Name + "}",
                        Value = item.Value
                    });
            }

            #endregion


            #region REPLACEMENT PREPERATION - RENDERS
            //datarender starts
            //render 1 : <form key="codemirror.head" /> -> codemirror isimli formun head kısmını implement et.
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(renderRes.HTML);
            var renders = htmlDocument.DocumentNode.SelectNodes("//render");
            if (renders != null)
            {
                foreach (var render in renders)
                {
                    string replaceValue = "";
                    string replaceText = "";
                    var key = render.Attributes["key"].Value;
                    bool isRootAttr = key.StartsWith("@");
                    if (isRootAttr)
                        key = key.Replace("@", "");
                    var values = key.Split(".");

                    var cloudKey = "";
                    var appKey = "";
                    var formKey = "";
                    var columnKey = "";

                    if (isRootAttr)
                    {
                        cloudKey = values[0];
                        if (values.Length > 1)
                            appKey = values[1];
                        if (values.Length > 2)
                            formKey = values[2];
                        if (values.Length > 3)
                            columnKey = values[3];
                    }
                    else
                    {
                        cloudKey = _app.CloudKey;
                        appKey = _app.AppKey;
                        if (values.Length > 0)
                            formKey = values[0];
                        if (values.Length > 1)
                            columnKey = values[1];
                    }

                    // sample : @cloud.app.form or codemirror.head etc. in same app 
                    if (string.IsNullOrEmpty(columnKey))
                    {
                        var sunRenderModel = new RenderEngine(new RenderRequest()
                        {
                            AppKey = appKey,
                            FormKey = formKey,
                            CloudKey = cloudKey,
                            RequestParamList = _renderRequestApiModel.RequestParamList
                        });
                        var subRenderResponse = sunRenderModel.Render();
                        replacementData.Add(new Common.Global.ParamData()
                        {
                            Name = render.OuterHtml,
                            Value = subRenderResponse.HTML
                        });
                    }
                    // sample : codemirror.head, localhost.core.codemirror.head->diğer cloud  
                    else
                    {
                        var subForm = _db.FormList.FirstOrDefault(x => x.FormKey == formKey && x.AppKey == appKey);
                        var preRender = PreRender(subForm);
                        if (columnKey == "head")
                            replaceText = preRender.HtmlHead;
                        else if (columnKey == "body")
                            replaceText = preRender.HtmlBody;
                        else if (columnKey == "foot")
                            replaceText = preRender.HtmlFoot;
                        replacementData.Add(new Common.Global.ParamData()
                        {
                            Name = render.OuterHtml,
                            Value = replaceText
                        });
                    }
                }
            }

            #endregion


            #region REPLACEMENT PREPERATION - SQL LOAD PARAMETERS


            // sql load parameters will load to html
            var dataEngine = new DataEngine(new DataRequest()
            {
                AppKey = this._app.AppKey
            });
            if (this._form != null)
                if (!string.IsNullOrEmpty(_form.SqlLoad))
                {
                    var sql = _form.SqlLoad;
                    // sql de parametreler hazırlansın diye replacement yapılır.
                    foreach (var item in replacementData)
                    {
                        sql = sql.Replace(item.Name, item.Value);

                    }
                    // boş parametreler NULL v.b değerler ile değişip {*} temizleyip kodu hatasız duruma getirir.
                    sql = ClearEmptyParameters(sql);

                    // sorgu içerisinde kaç adet SELECT var ise o kadar adette dataTable döndüdür.
                    List<DataTable> dtList = null;
                    try
                    {
                        dtList = dataEngine.SelectDataQuery(sql);
                    }
                    catch (Exception ex)
                    {
                        renderRes.HTML += "<div class='alert alert-danger'><b>" + this._app.AppKey + "." + this._form.FormKey + "</b> sql_load code has error. Please check your query." + ex.Message + "</div>" + renderRes.HTML;
                        throw;
                    }

                    //gelen datatable listesine göre de replacement listesini hazırlar.
                    var loadReplaceList = GetParamDataByDT(renderRes.HTML, dtList);

                    // replacement listesi ana replacement listesine eklenir.
                    replacementData.AddRange(loadReplaceList);
                }
            #endregion


            #region REPLACEMENT

            // tüm işlemler tamamdır ve replacement yapılır.
            renderRes.HTML = ReplaceWithParams(renderRes.HTML, replacementData);

            // eğer parametreler ile işimiz bittiyse ve parent render değilse temizlik çalışması içindir.
            if (clearEmptyParameters)
                renderRes.HTML = ClearEmptyParameters(renderRes.HTML, true);
            #endregion

            return renderRes;
        }

        public string ClearEmptyParameters(string text, bool removeAll = false)
        {

            List<SelectListItem> clearValues = new List<SelectListItem>();
            var regex = new Regex("{(.*?)}");
            var col = regex.Matches(text + "");
            foreach (var regVal in col)
            {
                var regValClear = (regVal + "").Replace("{", "").Replace("}", "");
                if (regValClear.StartsWith("P_") || regValClear == "returnUrl")
                {
                    if (removeAll)
                    {
                        clearValues.Add(new SelectListItem()
                        {
                            Text = "{" + regValClear + "}",
                            Value = ""
                        });
                    }
                    else
                    {
                        clearValues.Add(new SelectListItem()
                        {
                            Text = "N'{" + regValClear + "}'",
                            Value = "NULL"
                        });
                        clearValues.Add(new SelectListItem()
                        {
                            Text = "'{" + regValClear + "}'",
                            Value = "NULL"
                        });
                        clearValues.Add(new SelectListItem()
                        {
                            Text = "{{" + regValClear + "}}",
                            Value = "NULL"
                        });
                        clearValues.Add(new SelectListItem()
                        {
                            Text = "{" + regValClear + "}",
                            Value = "NULL"
                        });
                    }
                }
                else if (regValClear.StartsWith("G_"))
                {
                    clearValues.Add(new SelectListItem()
                    {
                        Text = "BIND({" + regValClear + "})",
                        Value = "-- commented by opendev -- BIND({" + regValClear + "})"
                    });
                }
            }
            // CLEANING EMPTY {} PARAMETERS
            foreach (var item in clearValues)
            {
                text = text.Replace(item.Text, item.Value);
            }
            return text;
        }

        public List<ParamData> GetParamDataByDT(string HTML, List<DataTable> dtList)
        {
            List<ParamData> result = new List<ParamData>();

            //REPLACEMENT BY MAIN MODELS
            var mainModelReplacements = GetParamsByModels(dtList);

            // REPLACEMENT BY <data></data> nodes.
            var nodeReplacements = GetParamsByDataList(HTML, dtList, mainModelReplacements);

            // add list
            result.AddRange(mainModelReplacements);
            result.AddRange(nodeReplacements);

            // return ready result
            return result;
        }

        public string ReplaceWithParams(string HTML, List<ParamData> paramDatas, int level = 0)
        {
            var errors = paramDatas.Where(x => x.DataType == "ERROR").DistinctBy(x => x.Value).ToList();
            var errorText = "";
            for (int i = 0; i < errors.Count(); i++)
            {
                errorText += (i + 1) + ". " + errors[i].Value + "<br>";
            }
            if (errors.Count > 0 && level == 0)
                HTML = "<div class='alert alert-danger'>" + errorText + "</div>" + HTML;

            foreach (var paramData in paramDatas.Where(x => x.DataType != "ERROR").ToList())
            {
                HTML = HTML.Replace(paramData.Name, paramData.Value);
            }
            return HTML;
        }

        /// <summary>
        /// Prepare by main models which is not ended with []
        /// </summary>
        /// <param name="dtList">data source list</param>
        /// <returns></returns>
        public List<ParamData> GetParamsByModels(List<DataTable> dtList)
        {
            List<ParamData> result = new List<ParamData>();
            // nesne olanlar
            var modelDtList = dtList.Where(x => !x.Columns[0].ColumnName.EndsWith("[]"));
            foreach (var dt in modelDtList)
            {
                var modelName = dt.Columns[0].ColumnName;
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 1; i < dt.Columns.Count; i++)
                    {
                        result.Add(new ParamData()
                        {
                            Value = dr[i] + "",
                            Name = "{" + modelName + "." + dt.Columns[i].ColumnName + "}",
                            DataType = dt.Columns[i].DataType + ""
                        });
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Prepare parameters by node from source dtList. Also replaces main models replacements.  
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dtList"></param>
        /// <returns></returns>
        public List<ParamData> GetParamsByDataList(string HTML, List<DataTable> dtList, List<ParamData> mainModelReplacements, int level = 0)
        {
            List<ParamData> result = new List<ParamData>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HTML);
            HtmlNodeCollection dataNodes = null;
            try
            {
                dataNodes = htmlDoc.DocumentNode.SelectNodes("//data");
            }
            catch (Exception ex)
            {
            }
            var mainHtmlNodes = new List<HtmlNode>();
            if (dataNodes != null)
            {
                foreach (var node in dataNodes)
                {
                    var sourceKeyNode = node.Attributes.FirstOrDefault(x => x.Name == "source");

                    if (sourceKeyNode != null)
                    {
                        if (sourceKeyNode.Value.ToCharArray().Count(x => x == '.') == level)// hangi seviyede ise o kadr nokta olabilir.
                        {
                            mainHtmlNodes.Add(node);
                        }
                    }
                    else
                    {
                        result.Add(new ParamData()
                        {
                            Name = node.OuterHtml,
                            DataType="ERROR",
                            Value = "Required attributes not exist. Please check your attributes."
                        });
                    }
                }
            }

            foreach (var node in mainHtmlNodes)
            {
                var nodeResult = "";
                if (node.Attributes != null)
                {
                    var attributes = node.Attributes.ToList();
                    var sourceAttr = attributes.FirstOrDefault(x => x.Name == "source");
                    var IdAttr = attributes.FirstOrDefault(x => x.Name == "id");
                    var whereAttr = attributes.FirstOrDefault(x => x.Name == "where");
                    if (sourceAttr != null && IdAttr != null)
                    {
                        try
                        {
                            var source = sourceAttr.Value + "";
                            var nodeId = IdAttr.Value + "";
                            var where = "";
                            if (whereAttr != null)
                                where = whereAttr.Value;

                            // liste olanlar
                            var dtRaw = dtList.FirstOrDefault(x => x.Columns[0].ColumnName == source + "[]");
                            if (dtRaw != null)
                            {
                                var dt = dtRaw.Clone();
                                if (!string.IsNullOrEmpty(where))
                                {
                                    List<DataRow> filteredRows = new List<DataRow>();
                                    try
                                    {
                                        filteredRows = dtRaw.Select(where).ToList();
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Please check where condition. Where query : " + where + " " + ex.Message);
                                    }
                                    foreach (var dr in filteredRows)
                                    {
                                        var row = dt.NewRow();
                                        foreach (DataColumn dc in dtRaw.Columns)
                                        {
                                            row[dc.ColumnName] = dr[dc.ColumnName];
                                        }
                                        dt.Rows.Add(row);
                                    }
                                    dtList.Remove(dtRaw);
                                    dtList.Add(dt);
                                }
                                else
                                {
                                    dt = dtRaw.Clone();
                                }
                                dtRaw.Dispose();
                                var type = node.Attributes["type"].Value;
                                if (type == "loop" || type == "iterator" || type == "for")
                                {
                                    var iterationTemplate = node.InnerHtml;
                                    var content = "";
                                    int iteration = 0;
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        iteration++;
                                        var subContent = iterationTemplate;
                                        foreach (DataColumn column in dt.Columns)
                                        {
                                            subContent = subContent.Replace("{" + column.ColumnName + "}", dr[column.ColumnName] + "");
                                            subContent = subContent.Replace("{" + source + "." + column.ColumnName + "}", dr[column.ColumnName] + "");
                                            subContent = subContent.Replace("{" + nodeId + "." + source + "." + column.ColumnName + "}", dr[column.ColumnName] + "");
                                        }
                                        content += subContent;
                                    }

                                    content = ReplaceWithParams(content, result, level + 1);
                                    //replace edilmiş halini alt kısımlar da replace edilsin diye tekrar çağırıyoruz.
                                    result.AddRange(GetParamsByDataList(content, dtList, mainModelReplacements, level + 1));

                                    content = ReplaceWithParams(content, result, level+1);

                                    result.Add(new ParamData()
                                    {
                                        Name = node.OuterHtml,
                                        Value = content
                                    });
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            result.Add(new ParamData()
                            {
                                Name = node.OuterHtml + "ERROR",
                                DataType = "ERROR",
                                Value = "Html <data> node occured an error. Error info : " + ex.Message
                            });
                        }
                    }
                    else

                        result.Add(new ParamData()
                        {
                            Name = node.OuterHtml + "ERROR",
                            DataType = "ERROR",
                            Value = "Required attributes not exist. Please check your attributes."
                        });
                }
                else
                {
                    result.Add(new ParamData()
                    {
                        Name = node.OuterHtml + "ERROR",
                        DataType = "ERROR",
                        Value = "Required attributes not exist. Please check your attributes."
                    });
                }
            }
            return result;
        }
    }
}
