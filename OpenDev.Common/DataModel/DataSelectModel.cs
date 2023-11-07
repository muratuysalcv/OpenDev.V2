using OpenDev.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDev.Common.DataModel
{
    public class DataSelectModel
    {
        public App App { get; set; }
        public string SQL { get; set; }
        public string ConnecttionKey { get; set; }


    }
}
