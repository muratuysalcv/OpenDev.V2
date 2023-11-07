using OpenDev.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDev.Common.Global
{
    public class AccessModel
    {
        public string CloudKey { get; set; }
        public string AppKey { get; set; }
        public string FormKey { get; set; }
        public Access HasAccess { get; set; }
    }
}
