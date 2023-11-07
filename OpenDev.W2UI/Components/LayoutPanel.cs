using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDev.W2UI.Components
{
    public class LayoutPanel : Component
    {
        private string? _loadUrl { get; set; }
        private string? _html { get; set; }

        public virtual string layoutName { get; set; }

        public LayoutPanel(string panelName = null, string layoutName = null)
        {
            this.style = "border: 1px solid #efefef; padding: 5px";
            this.layoutName = layoutName;
            this.name = panelName;
            tabs = new PanelTabContainer();
        }
        public string? type { get; set; }
        public int? size { get; set; }
        public bool? resizable { get; set; }

        public string? html { get { return _html; } set { this._html = value; } }

        public string load
        {
            set
            {
                _html = null;
                _loadUrl = value;
            }
        }
        public string? style { get; set; }

        public string PrepareOnLoadJs()
        {
            if (!string.IsNullOrEmpty(_loadUrl))
            {
                onLoadJs = " w2ui." + this.layoutName + "['load']('" + this.type + "', '" + this._loadUrl + "','blur');";
                
            }
            return onLoadJs;
        }

        public PanelTabContainer? tabs { get; set; }

        public UIToolbar? toolbar { get; set; }
    }
}
