using NetTestModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetTestClient
{
    public class AnswerClass : List<String>
    {
        public AnswerClass()
        {
            Add("A"); Add("B"); Add("C"); Add("D");
        }
    }
    /// <summary>
    /// updataTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class updataTestWindow : Window
    {
        WCF.NetTestServiceClient client;
        string url = "http://localhost:8733/NetTestService/";
        DataTable dt;
        DataView dv;
        public UserClass currentUser;
        public void showMsg(String s)
        {
            MessageBox.Show(s, "Information", MessageBoxButton.OK);
        }
        public bool confirm(String s)
        {
            return (MessageBox.Show(s, "Information", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }
        public updataTestWindow()
        {
            InitializeComponent();
            client = new WCF.NetTestServiceClient();
            client.getTestDataTableCompleted += Client_getTestDataTableCompleted;
            //client.deleteTestCompleted += Client_deleteTestCompleted;
            //client.deleteTestListCompleted += Client_deleteTestListCompleted;
            client.updateTestCompleted += Client_updateTestCompleted;
            // 设置访问的服务器地址
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(url, UriKind.Absolute));
        }
        private void Client_updateTestCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                dt.AcceptChanges();
            }
            else showMsg(e.Error.Message);
        }

        private void Client_deleteTestListCompleted(object sender, WCF.deleteTestListCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                List<int> IDS = (List<int>)e.UserState;
                foreach (int ID in IDS)
                {
                    DataRow row = dt.AsEnumerable().FirstOrDefault(x => (int)x["ID"] == ID);
                    if (row != null)
                    {
                        row.Delete();
                        row.AcceptChanges();
                    }
                }
            }
            else showMsg(e.Error.Message);
        }

        private void Client_deleteTestCompleted(object sender, WCF.deleteTestCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                DataRow row = dt.AsEnumerable().FirstOrDefault(x => (int)x["ID"] == (int)e.UserState);
                if (row != null)
                {
                    row.Delete();
                    row.AcceptChanges();
                }
            }
            else showMsg(e.Error.Message);
        }

        private void Client_getTestDataTableCompleted(object sender, WCF.getTestDataTableCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                dt = e.Result;
                dv = dt.DefaultView;
                uGrid.ItemsSource = dv;
            }
            else showMsg(e.Error.Message);
        }

        private void uGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (uGrid.SelectedIndex >= 0)
            {
                txtText.DataContext = dv[uGrid.SelectedIndex];
            }
            else txtText.DataContext = null;
        }

        //private void btUpdateTest_Click(object sender, RoutedEventArgs e)
        //{
        //    if (uGrid.SelectedIndex >= 0)
        //    {
        //        int index = uGrid.SelectedIndex;
        //        TestClass test = new TestClass
        //        {
        //            ID = (int)dv[index]["ID"],
        //            tTitle = dv[index]["tTitle"].ToString(),
        //            tAnswer = dv[index]["tAnswer"].ToString(),
        //            tText = dv[index]["tText"].ToString()
        //        };
        //        String uName = txtUser.Text.Trim();
        //        String uPass = txtPass.Password.Trim();
        //        if (uName != "" && uPass != "")
        //        {
        //            try
        //            {
        //                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
        //                {
        //                    MessageHeader user = MessageHeader.CreateHeader("uName", "MySpace", uName);
        //                    MessageHeader pass = MessageHeader.CreateHeader("uPass", "MySpace", uPass);
        //                    OperationContext.Current.OutgoingMessageHeaders.Add(user);
        //                    OperationContext.Current.OutgoingMessageHeaders.Add(pass);
        //                    client.updateTestAsync(test, test.ID);
        //                }
        //            }
        //            catch (Exception exp) { showMsg(exp.Message); }
        //        }
        //    }
        //}
        private void btUpdateTest_Click(object sender, RoutedEventArgs e)
        {
            string uName = txtUser.Text.Trim();
            string uPass = txtPass.Password.Trim();
            List<TestClass> tests = new List<TestClass>();
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    tests.Add(new TestClass
                    {
                        ID = (int)row["ID"],
                        tTitle = row["tTitle"].ToString(),
                        tAnswer = row["tAnswer"].ToString(),
                        tText = row["tText"].ToString()
                    });
                }
            }
            if (tests.Count > 0)
            {
                if (confirm("确定要更新？"))
                {
                    try
                    {
                        using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                        {
                            MessageHeader user = MessageHeader.CreateHeader("uName", "MySpace", uName);
                            MessageHeader pass = MessageHeader.CreateHeader("uPass", "MySpace", uPass);
                            OperationContext.Current.OutgoingMessageHeaders.Add(user);
                            OperationContext.Current.OutgoingMessageHeaders.Add(pass);
                            client.updateTestListAsync(tests.ToArray());
                        }
                        showMsg("更新成功！");
                    }
                    catch (Exception exp) { showMsg(exp.Message); }
                }
            }
        }

        private void btGetTest_Click(object sender, RoutedEventArgs e)
        {
            //获取试题列表
            String uName = txtUser.Text.Trim();
            String uPass = txtPass.Password.Trim();
            if (uName != "" && uPass != "")
            {
                try
                {
                    using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                    {
                        MessageHeader user = MessageHeader.CreateHeader("uName", "MySpace", uName);
                        MessageHeader pass = MessageHeader.CreateHeader("uPass", "MySpace", uPass);
                        OperationContext.Current.OutgoingMessageHeaders.Add(user);
                        OperationContext.Current.OutgoingMessageHeaders.Add(pass);
                        client.getTestDataTableAsync();
                    }
                }
                catch (Exception exp) { showMsg(exp.Message); }
            }
        }
    }
}
