using OpenDev.Common.Global;
using OpenDev.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDev.Core.Helper
{
    public class StringHelper
    {
        public static AccessModel ConvertStringToAccessModel(string appDotPath, DbModel _db = null)
        {
            if (_db == null) _db = new DbModel();

            var model = new AccessModel() { };
            var values = appDotPath.Split(".");
            model.CloudKey = values[0];
            if (values.Length > 1)
                model.AppKey = values[1];
            if (values.Length > 2)
                model.FormKey = values[2];
            return model;
        }
        public static string GetAccessString(Cloud cloud = null, App app = null, Form form = null, DbModel _db = null)
        {
            var key = "";
            if (_db == null)
            {
                _db = new DbModel();
            }
            if (form != null)
            {
                app = _db.AppList.FirstOrDefault(x => x.AppKey == form.AppKey);
                cloud = _db.CloudList.FirstOrDefault(x => x.CloudKey == app.CloudKey);
                key = cloud.CloudKey + "." + app.AppKey + "." + form.AppKey;
            }
            else if (app != null)
            {
                cloud = _db.CloudList.FirstOrDefault(x => x.CloudKey == app.CloudKey);
                key = cloud.CloudKey + "." + app.AppKey;
            }
            else if (cloud != null)
            {
                cloud = _db.CloudList.FirstOrDefault(x => x.CloudKey == app.CloudKey);
                key = cloud.CloudKey;
            }
            return key;
        }
    }
}
