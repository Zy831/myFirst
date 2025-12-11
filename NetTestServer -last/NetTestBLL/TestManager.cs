using NetTestDAL;
using NetTestModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTestBLL
{
    public class TestManager
    {
        public int addTest(UserClass user, ref TestClass test)
        {
            TestService service = new TestService();
            return service.addTest(user, ref test);
        }
        public DataTable getTestDataTable(UserClass user)
        {
            TestService service = new TestService();
            return service.getTestDataTable(user);
        }
        public bool deleteTest(UserClass user, TestClass test)
        {
            TestService service = new TestService();
            return service.deleteTest(user, test);
        }
        public bool deleteTestList(UserClass user, List<int> IDS)
        {
            TestService service = new TestService();
            return service.deleteTestList(user, IDS);
        }
        public void updateTest(UserClass user, TestClass test)
        {
            TestService service = new TestService();
            service.updateTest(user, test);
        }
        public void updateTestList(UserClass user, List<TestClass> tests)
        {
            TestService service = new TestService();
            service.updateTestList(user, tests);
        }
        public DataTable getUserTestDataTable(UserClass user)
        {
            TestService service = new TestService();
            return service.getUserTestDataTable(user);
        }
        public int setUserMark(UserClass user, int mValue)
        {
            TestService service = new TestService();
            return service.setUserMark(user, mValue);
        }
        public DataTable getMarkDataTable(UserClass user)
        {
            TestService service = new TestService();
            return service.getMarkDataTable(user);
        }
        public void deleteMarkList(UserClass user, List<int> mID)
        {
            TestService service = new TestService();
            service.deleteMarkList(user, mID);
        }
    }
}
