using Npgsql;
using OpenDev.Common.ApiModel;
using OpenDev.Data.DataModel;
using System.Data;

namespace OpenDev.Core.Engine
{
    public class DataEngine
    {
        public App _app { get; set; }
        public DbModel _db { get; set; }
        public DataRequest _dataRequestModel { get; set; }

        public DataEngine(DataRequest dataRequestModel)
        {
            _dataRequestModel = dataRequestModel;
            var _db = new DbModel();
            this._app = _db.AppList.FirstOrDefault(x => x.AppKey == _dataRequestModel.AppKey);

        }

        public List<DataTable> SelectDataQuery(string sql)
        {
            var db = new DbModel();
            var con = db.ConList.FirstOrDefault(x => x.ConId == _app.ConId);
            var connectionString = "Server={server};Port={port};Database={db};User Id={user};Password={pass};";
            connectionString = connectionString.Replace("{server}", con.DbServer)
                .Replace("{user}", con.DbUser).Replace("{pass}", con.DbPassword).Replace("{db}", con.DbName)
                .Replace("{port}", con.Port + "");
            var dataSource = NpgsqlDataSource.Create(connectionString);
            var command = dataSource.CreateCommand(sql);
            var list = new List<DataTable>();
            NpgsqlDataReader reader = null;
            
            reader = command.ExecuteReader();


            try
            {
                while (true)
                {
                    var r = reader.HasRows;
                    var dt = new DataTable();
                    dt.Load(reader);
                    list.Add(dt);
                }
            }
            catch (Exception ex)
            {
                if (!
                    (
                        ex.Message != "Invalid attempt to call HasRows when reader is closed." ||
                        ex.Message != "Okuyucu kapalıyken HasRows öğesini çağırma girişimi geçersiz.")
                    )
                {
                    throw ex;
                }
            }
            return list;
        }


    }
}
