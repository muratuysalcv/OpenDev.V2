using Newtonsoft.Json;
using NUglify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenDev.W2UI.Components
{
    public class Layout : Component
    {

        #region Constructor
        public Layout(string name, int? height, int? width)
        {
            this.height = height;
            this.width = width;
            this.name = name;
            box = "#" + name;
            padding = 4;
            panel_template = new object();
            panels = new List<LayoutPanel>();
            resizer = 4;
            JsInstanceFunction = "w2layout";
        }

        #endregion

        #region Properties


        public int? width { get; set; }

        public int? height { get; set; }
        /// <summary>
        /// Object, default = {}
        /// Object with temporary variables for internal use
        /// </summary>
        public object? last { get; set; }

        /// <summary>
        /// Integer, default = 1 Padding between panels in px.
        /// </summary>
        public int padding { get; set; }//1

        /// <summary>
        /// Object, default = {...} // see below Object that is used to init panels.
        /// </summary>
        public object panel_template { get; set; }


        /// <summary>
        /// Array, default = []
        /// Array of panel objects.
        /// </summary>
        public List<LayoutPanel> panels { get; set; }

        /// <summary>
        ///  Integer, default = 4
        //   The size of draggable resizer between panels.
        /// </summary>
        public int resizer { get; set; }//4



        /// <summary>
        /// String, default = '' Unique name for the object.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// onChange - function (event)
        //Called when a panel content is set or changed.
        /// </summary>
        public ComponentEvent? onChange { get; set; }

        /// <summary>
        /// onHide - function (event)
        /// Called when a panel is hidden.
        /// </summary>
        public ComponentEvent? onHide { get; set; }

        /// <summary>
        /// Called when resizer is clicked.
        /// </summary>
        public ComponentEvent? onResizerClick { get; set; }

        /// <summary>
        /// Called when panels are being resized by the user.
        /// </summary>
        public ComponentEvent? onResizing { get; set; }

        /// <summary>
        /// Called when a panel is shown.
        /// </summary>
        public ComponentEvent? onShow { get; set; }

        /// <summary>
        /// Called when object is destroyed.
        /// </summary>
        public ComponentEvent? onDestroy { get; set; }


        /// <summary>
        /// Called when object is refreshed.
        /// </summary>
        public ComponentEvent? onRefresh { get; set; }


        /// <summary>
        /// Called when object is rendered.
        /// </summary>
        public ComponentEvent? onRender { get; set; }


        /// <summary>
        /// Called when object is resized.
        /// </summary>
        public ComponentEvent? onResize { get; set; }

        #endregion

        public void AddPanel(LayoutPanel panel)
        {
            panel.layoutName = this.name;
            this.panels.Add(panel);
        }

        public string Render()
        {
            var renderResult = "";
            try
            {
                if (this.panels.Count(x => x.type == LayoutPanelType.main) != 1)
                {
                    throw new Exception("Layout must have 1 main panel.");
                }
                renderResult = "<div id='" + this.name + "' style='height:" + this.height + "px;width:" + this.width + "px;'></div>";
                renderResult += @"<script>
                            window.onload = (event) => {
                                           $('#" + this.name + @"')." + this.JsInstanceFunction + @"(
                                                {json_object}
                                            );
                                         {additional_js}

                                    };
                                   
                            </script>";
                var jsonText = "";
                try
                {
                    jsonText = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        ConstructorHandling=ConstructorHandling.AllowNonPublicDefaultConstructor
                    });;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
                Uglify.Js(jsonText, new NUglify.JavaScript.CodeSettings() { MinifyCode = false });
                renderResult = renderResult.Replace("{json_object}", jsonText);
                string additional_js = "";
                foreach (var item in this.panels)
                {
                    // todo : later will be add to footer script
                    if (!string.IsNullOrEmpty(item.PrepareOnLoadJs()))
                    {
                        additional_js += item.onLoadJs;
                    }
                }
                renderResult = renderResult.Replace("{additional_js}", additional_js);
                //todo: kodu düzgünce temizlemek lazım.
                renderResult = renderResult.Replace("\"function (event) {this.owner.html('main', event.target);}\"", "function (event) {this.owner.html('main', event.target);}");
                renderResult = renderResult.Replace("\"function(event){this.owner.html('main', `EVENT: ${event.type}<br>TARGET: ${event.target}`)}\"", "function(event){this.owner.html('main', `EVENT: ${event.type}<br>TARGET: ${event.target}`)}");
            }
            catch (Exception ex)
            {
                renderResult = "<div class='m-2 alert alert-danger'><b>" + this.name + "</b> : " + ex.Message + "<br><br>" + ex.StackTrace + "</div>";
            }

            return renderResult;
        }

        /// <summary>
        /// right,left,main,bottom
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        public string ToggleJS(string panelName)
        {
            return "w2ui."+this.name+".toggle('" + panelName + "', true)";
        }
        public string ToggleButton(string buttonText, string panelName)
        {
            var button = new W2Button(buttonText, "w2ui." + this.name+@".toggle('" + panelName + @"', true)");
            return button.Render();
        }


    }
}
