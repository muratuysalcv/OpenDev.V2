using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDev.W2UI.Components
{
    public class W2Button : Component
    {
        public W2Button(string text, string scriptInline)
        {
            this.Class = "w2ui-btn";
            this.text = text;
            this.onclick = new ComponentEvent()
            {
                ScriptInline = scriptInline
            };
        }
        public W2Button(string text, string functionName, List<NameValue> functionParams, string functionScript = null)
        {

            this.Class = "w2ui-btn";
            this.text = text;
            this.onclick = new ComponentEvent()
            {
                FunctionName = functionName,
                FunctionParams = functionParams,
                FunctionScript = functionScript
            };
        }

        [DisplayName("class")]
        public string Class { get; set; }
        public ComponentEvent onclick { get; set; }
        public string onclickInline { get; set; }

        public string text { get; set; }

        public string Render()
        {
            string result = "<button class='" + this.Class + "' {onclick}  >" + text + "</button>";
            if (onclick == null && !string.IsNullOrEmpty(this.onclickInline))
            {
                result = result.Replace("{onclick}", @"onclick=""" + onclickInline + @"""");
            }
            else
            {
                result = result.Replace("{onclick}", @"onclick=""" + this.onclick.ScriptInline + @"""");
            }
            return result;
        }
    }
}
