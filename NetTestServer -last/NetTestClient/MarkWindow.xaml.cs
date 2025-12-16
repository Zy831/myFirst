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
    /// MarkWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MarkWindow : Window
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
        public MarkWindow()
        {
            InitializeComponent();
        }
        private void Client_deleteMarkListCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                List<int> mID = (List<int>)e.UserState;
                foreach (int ID in mID)
                {
                    DataRow row = dt.AsEnumerable().FirstOrDefault(x => (int)x["ID"] == ID);
                    if (row != null)
                    {
                        row.Delete();
                        row.AcceptChanges();
                    }
                }
                dt.AcceptChanges();
            }
            else mainWnd.showMsg(e.Error.Message);
        }

        private void Client_getMarkDataTableCompleted(object sender, WCF.getMarkDataTableCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    dt = e.Result;
                    dv = dt.DefaultView;
                    mGrid.ItemsSource = dv;
                }
                else mGrid.ItemsSource = null;
            }
            else mainWnd.showMsg(e.Error.Message);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client.getMarkDataTableCompleted += Client_getMarkDataTableCompleted;
            client.deleteMarkListCompleted += Client_deleteMarkListCompleted;
            try
            {
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    MessageHeader user = MessageHeader.CreateHeader("uName", "MySpace", currentUser.uName);
                    MessageHeader pass = MessageHeader.CreateHeader("uPass", "MySpace", currentUser.uPass);
                    OperationContext.Current.OutgoingMessageHeaders.Add(user);
                    OperationContext.Current.OutgoingMessageHeaders.Add(pass);
                    client.getMarkDataTableAsync();
                }
            }
            catch (Exception exp) { mainWnd.showMsg(exp.Message); }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWnd.Show();
        }

        private void deleteMark_Click(object sender, RoutedEventArgs e)
        {
            if (mGrid.SelectedItems.Count > 0)
            {
                List<int> mID = new List<int>();
                for (int i = 0; i < mGrid.SelectedItems.Count; i++)
                {
                    DataRowView row = (DataRowView)mGrid.SelectedItems[i];
                    mID.Add((int)row["ID"]);
                }
                if (mID.Count > 0)
                {
                    if (mainWnd.confirm("确实要删除选择成绩记录？"))
                    {
                        try
                        {
                            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                            {
                                MessageHeader user = MessageHeader.CreateHeader("uName", "MySpace", currentUser.uName);
                                MessageHeader pass = MessageHeader.CreateHeader("uPass", "MySpace", currentUser.uPass);
                                OperationContext.Current.OutgoingMessageHeaders.Add(user);
                                OperationContext.Current.OutgoingMessageHeaders.Add(pass);
                                client.deleteMarkListAsync(mID.ToArray(), mID);
                            }
                        }
                        catch (Exception exp) { mainWnd.showMsg(exp.Message); }
                    }
                }
            }
            else mainWnd.showMsg("请选择要删除的记录！");
        }
    }
}
