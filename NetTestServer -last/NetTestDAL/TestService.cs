using NetTestModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTestDAL
{
    public class TestService
    {
        bool verifyAdmin(DBHelper DB, UserClass user)
        {
            if (user.uName == "Admin")
            {
                SqlParameter pName = new SqlParameter { ParameterName = "@uName", SqlDbType = System.Data.SqlDbType.Char, Value = user.uName };
                SqlParameter pPass = new SqlParameter { ParameterName = "@uPass", SqlDbType = System.Data.SqlDbType.Char, Value = user.uPass };
                return (DB.getScalar("select count(*) from users where uName = @uName COLLATE SQL_Latin1_General_CP1_CS_AS and uPass = @uPass COLLATE SQL_Latin1_General_CP1_CS_AS", pName, pPass) > 0);
            }
            return false;
        }
        public int addTest(UserClass user, ref TestClass test)
        {
            test.ID = 0;
            using (SqlConnection con = new SqlConnection(DBHelper.conString))
            {
                DBHelper DB = new DBHelper(con);
                if (verifyAdmin(DB, user))
                {
                    SqlParameter pDate = new SqlParameter { ParameterName = "@tDate", SqlDbType = SqlDbType.Char, Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                    SqlParameter pTitle = new SqlParameter { ParameterName = "@tTitle", SqlDbType = SqlDbType.Char, Value = test.tTitle };
                    SqlParameter pText = new SqlParameter { ParameterName = "@tText", SqlDbType = SqlDbType.Text, Value = test.tText };
                    SqlParameter pAnswer = new SqlParameter
                    {
                        ParameterName = "@tAnswer",
                        SqlDbType = SqlDbType.Char,
                        Value = test.tAnswer
                    };
                    if (DB.executeCommand("insert into tests (tDate,tTitle,tText,tAnswer) values(@tDate,@tTitle,@tText,@tAnswer)", pDate, pTitle, pText, pAnswer) > 0)
                    {
                        //插入成功时获得插入记录的ID
                        test.ID = DB.getScalar("select top 1 ID from tests order by ID desc");
                    }
                }
                con.Close();
            }
            return test.ID;
        }
        public DataTable getTestDataTable(UserClass user)
        {
            DataTable dt = null;
            using (SqlConnection con = new SqlConnection(DBHelper.conString))
            {
                DBHelper DB = new DBHelper(con);
                if (verifyAdmin(DB, user))
                {
                    SqlDataAdapter adapter = DB.getAdapter("select ID,tDate,tTitle,tAnswer,tText from tests order by ID");
                    dt = new DataTable("Test");
                    adapter.Fill(dt);
                }
                con.Close();
            }
            return dt;
        }
        public bool deleteTest(UserClass user, TestClass test)
        {
            bool flag = false;
            using (SqlConnection con = new SqlConnection(DBHelper.conString))
            {
                DBHelper DB = new DBHelper(con);
                if (verifyAdmin(DB, user))
                {
                    if (DB.executeCommand("delete from tests where ID=" + test.ID.ToString()) > 0) flag = true;
                }
                con.Close();
            }
            return flag;
        }
        public bool deleteTestList(UserClass user, List<int> IDS)
        {
            bool flag = false;
            using (SqlConnection con = new SqlConnection(DBHelper.conString))
            {
                DBHelper DB = new DBHelper(con);
                if (verifyAdmin(DB, user))
                {
                    foreach (int ID in IDS)
                        if (DB.executeCommand("delete from tests where ID=" + ID.ToString()) > 0) flag = true;
                }
                con.Close();
            }
            return flag;
        }
        public void updateTest(UserClass user, TestClass test)
        {
            using (SqlConnection con = new SqlConnection(DBHelper.conString))
            {
                DBHelper DB = new DBHelper(con);
                if (verifyAdmin(DB, user))
                {
                    test.tDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    SqlParameter pDate = new SqlParameter("@tDate", SqlDbType.Char); pDate.Value = test.tDate;
                    SqlParameter pTitle = new SqlParameter("@tTitle", SqlDbType.Char); pTitle.Value = test.tTitle;
                    SqlParameter pText = new SqlParameter("@tText", SqlDbType.Text); pText.Value = test.tText;
                    SqlParameter pAnswer = new SqlParameter("@tAnswer", SqlDbType.Char); pAnswer.Value = test.tAnswer;
                    DB.executeCommand("update tests set tDate=@tDate,tTitle=@tTitle,tText=@tText," +
                        "tAnswer=@tAnswer where ID=" + test.ID.ToString(), pDate, pTitle, pText, pAnswer);
                }
                con.Close();
            }
        }
        public void updateTestList(UserClass user, List<TestClass> tests)
        {
            using (SqlConnection con = new SqlConnection(DBHelper.conString))
            {
                DBHelper DB = new DBHelper(con);
                if (verifyAdmin(DB, user))
                {
                    foreach (TestClass test in tests)
                    {
                        test.tDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        SqlParameter pDate = new SqlParameter("@tDate", SqlDbType.Char); pDate.Value = test.tDate;
                        SqlParameter pTitle = new SqlParameter("@tTitle", SqlDbType.Char); pTitle.Value = test.tTitle;
                        SqlParameter pText = new SqlParameter("@tText", SqlDbType.Text); pText.Value = test.tText;
                        SqlParameter pAnswer = new SqlParameter("@tAnswer", SqlDbType.Char); pAnswer.Value = test.tAnswer;
                        DB.executeCommand("update tests set tDate=@tDate,tTitle=@tTitle,tText=@tText," +
                            "tAnswer=@tAnswer where ID=" + test.ID.ToString(), pDate, pTitle, pText, pAnswer);
                    }
                }
                con.Close();
            }
        }
        bool verifyUser(DBHelper DB, UserClass user)
        {
            //验证用户身份
            SqlParameter pName = new SqlParameter("@uName", SqlDbType.Char); pName.Value = user.uName;
            SqlParameter pPass = new SqlParameter("@uPass", SqlDbType.Char); pPass.Value = user.uPass;
            return ((int)DB.getScalar("select count(*) from users where uName=@uName and uPass=@uPass", 
                pName, pPass) > 0);
        }
        public DataTable getUserTestDataTable(UserClass user)
        {
            Random rnd = new Random();
            DataTable dt = null;
            using (SqlConnection con = new SqlConnection(DBHelper.conString))
            {
                DBHelper DB = new DBHelper(con);
                if (verifyUser(DB, user))
                {
                    //NEWID()用来产生随机顺序
                    SqlDataAdapter adapter = DB.getAdapter("select top 10* from tests order by NEWID()");
                    dt = new DataTable("Test");
                    adapter.Fill(dt);
                }
                con.Close();
            }
            return dt;
        }
        public int setUserMark(UserClass user, int mValue)
        {
            int flag = -1;
            using (SqlConnection con = new SqlConnection(DBHelper.conString))
            {
                DBHelper DB = new DBHelper(con);
                if (verifyUser(DB, user))
                {
                    SqlParameter pName = new SqlParameter("@uName", SqlDbType.Char); pName.Value = user.uName;
                    SqlParameter pDate = new SqlParameter("@mDate", SqlDbType.Char); 
                    pDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    SqlParameter pValue = new SqlParameter("@mValue", SqlDbType.Char); pValue.Value = mValue;
                    if (DB.executeCommand("insert into marks (uName,mDate,mValue) " +
                        "values(@uName,@mDate,@mValue)", pName, pDate, pValue) > 0) flag = mValue;
                }
                con.Close();
            }
            return flag;
        }
        public DataTable getMarkDataTable(UserClass user)
        {
            DataTable dt = null;
            using (SqlConnection con = new SqlConnection(DBHelper.conString))
            {
                DBHelper DB = new DBHelper(con);
                if (verifyAdmin(DB, user))
                {
                    SqlDataAdapter adapter = DB.getAdapter("select * from " +
                        "marks order by ID");
                    dt = new DataTable("Mark");
                    adapter.Fill(dt);
                }
                con.Close();
            }
            return dt;
        }
        public void deleteMarkList(UserClass user, List<int> mID)
        {
            using (SqlConnection con = new SqlConnection(DBHelper.conString))
            {
                DBHelper DB = new DBHelper(con);
                if (verifyUser(DB, user))
                {
                    foreach (int ID in mID)
                        DB.executeCommand("delete from marks where ID=" + ID.ToString());
                }
                con.Close();
            }
        }
    }
}
