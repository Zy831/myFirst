using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTestDAL
{
    public class DBHelper
    {
        public static String conString = "Data Source = localhost\\SQLEXPRESS;Initial Catalog = NetTest;Integrated Security = SSPI;MultipleActiveResultSets = true";//windows身份验证
        //public static String conString = "Server=localhost;initial catalog = NetTest;user id = sa; password=@lyh123456;";
        //public static String conString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        public SqlConnection con;
        public SqlCommand cmd;
        public DBHelper(SqlConnection con)
        {
            con.Open();
            this.con = con;
            cmd = new SqlCommand();
            cmd.Connection = con;
        }
        public int executeCommand(String sql)
        {
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            return cmd.ExecuteNonQuery();
        }
        public int executeCommand(String sql, params SqlParameter[] values)
        {
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            cmd.Parameters.AddRange(values);
            return cmd.ExecuteNonQuery();
        }
        public int getScalar(String sql)
        {
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            return (int)cmd.ExecuteScalar();
        }
        public int getScalar(String sql, params SqlParameter[] values)
        {
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            cmd.Parameters.AddRange(values);
            return (int)cmd.ExecuteScalar();
        }
        public SqlDataReader getReader(String sql)
        {
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        public SqlDataReader getReader(String sql, params SqlParameter[] values)
        {
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            cmd.Parameters.AddRange(values);
            return cmd.ExecuteReader();
        }
        public SqlDataAdapter getAdapter(String sql)
        {
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            return adapter;
        }
        public SqlDataAdapter getAdapter(String sql, params SqlParameter[] values)
        {
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            cmd.Parameters.AddRange(values);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            return adapter;
        }
    }
}
