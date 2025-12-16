using NetTestBLL;
using NetTestModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NetTestServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“NetTestService”。
    public class NetTestService : INetTestService
    {
        UserClass getUser()
        {
            String uName = OperationContext.Current.IncomingMessageHeaders.GetHeader<String>("uName", "MySpace");
            String uPass = OperationContext.Current.IncomingMessageHeaders.GetHeader<String>("uPass", "MySpace");
            return new UserClass { uName = uName, uPass = uPass };
        }
        public String login(UserClass user)
        {
            UserManager manager = new UserManager();
            return manager.login(user);
        }
        public TestClass addTest(TestClass test)
        {
            TestManager manager = new TestManager();
            manager.addTest(getUser(), ref test);
            return test;
        }
        public DataTable getTestDataTable()
        {
            TestManager manager = new TestManager();
            return manager.getTestDataTable(getUser());
        }
        public bool deleteTest(TestClass test)
        {
            TestManager manager = new TestManager();
            return manager.deleteTest(getUser(), test);
        }
        public bool deleteTestList(List<int> IDS)
        {
            TestManager manager = new TestManager();
            return manager.deleteTestList(getUser(), IDS);
        }
        public void updateTest(TestClass test)
        {
            TestManager manager = new TestManager();
            manager.updateTest(getUser(), test);
        }
        public void updateTestList(List<TestClass> tests)
        {
            TestManager manager = new TestManager();
            manager.updateTestList(getUser(), tests);
        }
        public DataTable getUserTestDataTable()
        {
            TestManager manager = new TestManager();
            return manager.getUserTestDataTable(getUser());
        }
        public int setUserMark(int mValue)
        {
            TestManager manager = new TestManager();
            return manager.setUserMark(getUser(), mValue);
        }
        public DataTable getMarkDataTable()
        {
            TestManager manager = new TestManager();
            return manager.getMarkDataTable(getUser());
        }
        public void deleteMarkList(List<int> mID)
        {
            TestManager manager = new TestManager();
            manager.deleteMarkList(getUser(), mID);
        }
    }
}
