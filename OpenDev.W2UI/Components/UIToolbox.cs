using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDev.W2UI.Components
{
    public class UIToolbar
    {
        private string? _onClick { get; set; }
        public UIToolbar()
        {
            Items = new List<UIToolbarItem>();
        }
        public List<UIToolbarItem> Items { get; set; }
        public string? onClick
        {
            get
            {
                if (_onClick == null)
                    _onClick = "function(event){this.owner.html('main', `EVENT: ${event.type}<br>TARGET: ${event.target}`)}";

                return _onClick;
            }
            set
            {

            }
        }
    }
    public class UIToolbarMenuItem
    {
        public string? text { get; set; }
        public string? icon { get; set; }
        public string? value { get; set; }

    }
    public class UIToolbarItem
    {
        public string id { get; set; }
        public string? type { get; set; }

        public string? text { get; set; }

        public string? img { get; set; }

        public string? icon { get; set; }

        [DisplayName("checked")]
        public bool? Checked { get; set; }

        public List<UIToolbarMenuItem> items { get; set; }

        public string? tooltip { get; set; }
        public string? group { get; set; }
    }

    public static class UIToolbarItemType
    {
        public static string checkType = "check";
        public static string breakType = "break";
        public static string menuType = "menu";
        public static string radioType = "radio";
        public static string spacerType = "spacer";
        public static string buttonType = "button";
    }

}
