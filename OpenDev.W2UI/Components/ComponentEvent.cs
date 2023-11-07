using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDev.W2UI.Components
{
    public class ComponentEvent
    {
        public string name { get; set; }
        public string ScriptInline { get; set; }

        public string FunctionScript { get; set; }

        public string FunctionName { get; set; }

        public List<NameValue> FunctionParams { get; set; }

        public ComponentEvent()
        {

        }
        public string FunctionDefineScript()
        {
            if (string.IsNullOrEmpty(FunctionName) && string.IsNullOrEmpty(this.FunctionScript))
            {
                return "alert('"+name+" must have function name and function script to render FunctionDefineScript().')";
            }

            string scriptText = "";
            if (!string.IsNullOrEmpty(FunctionName))
            {
                scriptText = @"<script>function " + this.FunctionName + "(" + string.Join(",", this.FunctionParams.Select(x => x.Name)) + "){"+this.FunctionScript+"}</script>";
            }
            return scriptText;
        }
    }
}
