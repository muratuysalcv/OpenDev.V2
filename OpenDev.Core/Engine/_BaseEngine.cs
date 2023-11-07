using OpenDev.Data.DataModel;

namespace OpenDev.Core.Engine
{
    public class BaseEngine
    {
        public DbModel _db { get; set; }
        public BaseEngine() {
            _db = new DbModel();
        } 

    }
}
