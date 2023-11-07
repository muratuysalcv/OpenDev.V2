using OpenDev.Core.Helper;
using OpenDev.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDev.Core.Engine
{
    public class AccessEngine
    {
        public DbModel _db { get; set; }
        public Cloud _fromCloud { get; set; }
        public App _fromApp { get; set; }
        public Form _fromForm { get; set; }

        public Cloud _toCloud { get; set; }
        public App _toApp { get; set; }
        public Form _toForm { get; set; }

        public string AccessStringFrom { get; set; }
        public string AccessStringTo { get; set; }

        public string AccessTypeCode { get; set; }

        public AccessEngine(Cloud fromCloud = null, Cloud toCloud = null, App fromApp = null, App toApp = null, Form fromForm = null, Form toForm = null, DbModel db = null)
        {
            _db = db;
            if (_db == null)
                _db = new DbModel();

            _fromApp = fromApp;
            _fromCloud = fromCloud;
            _fromForm = fromForm;

            _toCloud = toCloud;
            _toApp = toApp;
            _fromForm = fromForm;

            AccessStringFrom = StringHelper.GetAccessString(fromCloud, fromApp, fromForm, db);
            AccessStringTo = StringHelper.GetAccessString(toCloud, toApp, toForm, db);
            var fromCount = AccessStringFrom.Split(".").Count();
            var toCount = AccessStringTo.Split(".").Count();
            if (fromCount == 1)
                AccessTypeCode = "Cloud";
            else if (fromCount == 2)
                AccessTypeCode = "App";
            else if (fromCount == 3)
                AccessTypeCode = "Form";

            AccessTypeCode += "To";
            if (toCount == 1)
                AccessTypeCode = "Cloud";
            else if (toCount == 2)
                AccessTypeCode = "App";
            else if (toCount == 3)
                AccessTypeCode += "Form";
        }
        public bool HasAccess()
        {
            var result = false;
            var access = _db.AccessList.FirstOrDefault(x => x.From == AccessStringFrom && x.To == AccessStringTo);
            if (access != null)
                result= access.HasAccess;
            else 
            {
                // if no permit all Forms Can Access
                if (AccessTypeCode == "FormToForm")
                    result = true;
            }
            return result;
        }
    }
}
