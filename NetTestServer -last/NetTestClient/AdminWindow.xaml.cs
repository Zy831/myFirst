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
    /// <summary>
    /// AdminWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AdminWindow : Window
    {
        public WCF.NetTestServiceClient client;
        string url = "http://localhost:8733/NetTestService/";
        public MainWindow mainWnd;
        DataTable dt;
        DataView dv;
        public UserClass currentUser = new UserClass();
        public void showMsg(String s)
        {
            MessageBox.Show(s, "Information", MessageBoxButton.OK);
        }
        public bool confirm(String s)
        {
            return (MessageBox.Show(s, "Information", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }
        public AdminWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client.getTestDataTableCompleted += Client_getTestDataTableCompleted;
            //client.deleteTestCompleted += Client_deleteTestCompleted;
            client.deleteTestListCompleted += Client_deleteTestListCompleted;
            client.updateTestCompleted += Client_updateTestCompleted;
            if (currentUser != null)
            {
                try
                {
                    using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                    {
                        MessageHeader user = MessageHeader.CreateHeader("uName", "MySpace", currentUser.uName);
                        MessageHeader pass = MessageHeader.CreateHeader("uPass", "MySpace", currentUser.uPass);
                        OperationContext.Current.OutgoingMessageHeaders.Add(user);
                        OperationContext.Current.OutgoingMessageHeaders.Add(pass);
                        client.getTestDataTableAsync();
                    }
                }
                catch (Exception exp) { showMsg(exp.Message); }
            }
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //mainWnd.Show();
        }

        private void btDeleteTest_Click(object sender, RoutedEventArgs e)
        {
            if (uGrid.SelectedItems.Count > 0)
            {
                if (currentUser != null)
                {
                    //获取要删除的试题的ID序列
                    List<int> IDS = new List<int>();
                    //WCF.TestClass test = new WCF.TestClass {};
                    for (int i = 0; i < uGrid.SelectedItems.Count; i++)
                    {
                        DataRowView row = (DataRowView)uGrid.SelectedItems[i];
                        IDS.Add((int)row["ID"]);
                        //test .ID = (int)row["ID"] ;
                    }
                    if (confirm("删除选中行？"))
                    {
                        try
                        {

                            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                            {
                                MessageHeader user = MessageHeader.CreateHeader("uName", "MySpace", currentUser.uName);
                                MessageHeader pass = MessageHeader.CreateHeader("uPass", "MySpace", currentUser.uPass);
                                OperationContext.Current.OutgoingMessageHeaders.Add(user);
                                OperationContext.Current.OutgoingMessageHeaders.Add(pass);
                                client.deleteTestListAsync(IDS.ToArray(), IDS);
                            }
                            showMsg("删除成功！");
                        }
                        catch (Exception exp) { showMsg(exp.Message); }
                    }
                }
                else showMsg("请重新登录！");
            }
            else showMsg("请选择！");
        }

        private void btAddTest_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser.uName != "" && currentUser.uPass != "")
            {
                addTestWindow dlg = new addTestWindow();
                dlg.url = url;
                dlg.client = client;
                dlg.currentUser = currentUser;
                this.Hide();
                dlg.ShowDialog();
                this.Show();
                mainWnd.Hide();
            }
            else showMsg("用户或密码不能为空！");
        }

        private void btUpdateTest_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser != null)
            {
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
                                MessageHeader user = MessageHeader.CreateHeader("uName", "MySpace", currentUser.uName);
                                MessageHeader pass = MessageHeader.CreateHeader("uPass", "MySpace", currentUser.uPass);
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
        }

        private void uGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (uGrid.SelectedIndex >= 0)
            {
                txtText.DataContext = dv[uGrid.SelectedIndex];
            }
            else txtText.DataContext = null;
        }
    }
}
