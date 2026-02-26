using System;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

namespace feedback360.Backend.Repositories
{
    public abstract class BaseRepository : IDisposable
    {
        protected readonly string _connectionString;
        protected OracleConnection _connection;

        protected BaseRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MYhrps"].ConnectionString;
        }

        protected OracleConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new OracleConnection(_connectionString);
            }
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }

        protected DataTable ExecuteQuery(string sql, OracleParameter[] parameters = null)
        {
            using (var cmd = new OracleCommand(sql, GetConnection()))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (var da = new OracleDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        protected int ExecuteNonQuery(string sql, OracleParameter[] parameters = null)
        {
            using (var cmd = new OracleCommand(sql, GetConnection()))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
