using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDev.W2UI.Components
{
    public abstract class Component
    {
        protected string JsInstanceFunction { get; set; }

        public virtual string? onLoadJs { get; set; }

        public bool? hidden { get; set; }

        /// <summary>
        /// name of element.
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// DOM Element, default = null
        /// The DOM element where to render the object.
        /// </summary>
        public object? box { get; set; }

        /// <summary>
        ///  Array, default = [] Array of event handlers.
        /// </summary>
        public List<object>? handlers { get; set; }

        /// <summary>
        ///  String, default = '' Additional style for the .box where the object is rendered.
        /// </summary>
        public string? style { get; set; }

        public string Render()
        {
            throw new NotImplementedException();
        }
    }
}
