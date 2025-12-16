using NetTestModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTestDAL
{
    public class UserService
    {
        public string login(UserClass user)
        {
            string s = "failed";
            using (SqlConnection con = new SqlConnection(DBHelper.conString))
            {
                DBHelper DB = new DBHelper(con);
                SqlParameter pName = new SqlParameter { ParameterName = "@uName", SqlDbType = System.Data.SqlDbType.Char, Value = user.uName };
                SqlParameter pPass = new SqlParameter { ParameterName = "@uPass", SqlDbType = System.Data.SqlDbType.Char, Value = user.uPass };
                try
                {
                    DB.executeCommand("insert into users values(@uName,@uPass)", pName, pPass);
                    s = "registered";
                }
                catch
                {
                    if (DB.getScalar("select count(*) from users where uName = @uName and uPass = @uPass", pName, pPass) > 0)
                    {
                        s = "logined";
                    }
                }
                con.Close();
            }
            return s;
        }
    }
}
