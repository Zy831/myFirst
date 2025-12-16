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
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“INetTestService”。
    [ServiceContract]
    public interface INetTestService
    {
        [OperationContract]
        string login(UserClass user);
        [OperationContract]
        TestClass addTest(TestClass test);
        [OperationContract]
        DataTable getTestDataTable();
        [OperationContract]
        bool deleteTest(TestClass test);
        [OperationContract]
        bool deleteTestList(List<int> IDS);
        [OperationContract]
        void updateTest(TestClass test);
        [OperationContract]
        void updateTestList(List<TestClass> tests);
        [OperationContract]
        DataTable getUserTestDataTable();
        [OperationContract]
        int setUserMark(int mValue);
        [OperationContract]
        DataTable getMarkDataTable();
        [OperationContract]
        void deleteMarkList(List<int> mID);
    }
}
