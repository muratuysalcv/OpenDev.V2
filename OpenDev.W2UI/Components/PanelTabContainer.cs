using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDev.W2UI.Components
{
    public class PanelTabContainer
    {
        public PanelTabContainer()
        {
            tabs = new List<PanelTab>();
        }
        private string? _onClick { get; set; }
        private string? _active { get; set; }
        public string? active
        {
            get
            {
                if (string.IsNullOrEmpty(_active))
                {
                    var firstPanel = this.tabs.OrderBy(x => x.priority).FirstOrDefault();
                    if (firstPanel != null)
                        this._active = firstPanel.id;
                }
                return _active;
            }
            set
            {
                this._active = value;
            }
        }

        public List<PanelTab> tabs { get; set; }
        public string OnLoadScripts() {
            return "";
        }
        public string onClick
        {
            get
            {
                if (string.IsNullOrEmpty(this._onClick) && this.tabs.Count > 0)
                {
                    _onClick = "function (event) {this.owner.html('main', event.target);}";
                }
                return _onClick;
            }
            set
            {
                _onClick = value;
            }
        }
        public string Render()
        {
            var renderResult = "";
            try
            {
                renderResult = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                }); ;
            }
            catch (Exception ex)
            {

                renderResult = "<div class='m-2 alert alert-danger'><b>" +""+ "</b> : " + ex.Message + "<br><br>" + ex.StackTrace + "</div>";
            }
            return renderResult;
        }
    }
}
