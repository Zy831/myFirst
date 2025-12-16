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
    /// deleteTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class deleteTestWindow : Window
    {
        WCF.NetTestServiceClient client;
        string url = "http://localhost:8733/NetTestService/";
        DataTable dt;
        DataView dv;
        public void showMsg(String s)
        {
            MessageBox.Show(s, "Information", MessageBoxButton.OK);
        }
        public deleteTestWindow()
        {
            InitializeComponent();
            client = new WCF.NetTestServiceClient();
            client.getTestDataTableCompleted += Client_getTestDataTableCompleted;
            client.deleteTestCompleted += Client_deleteTestCompleted;
            client.deleteTestListCompleted += Client_deleteTestListCompleted;
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(url));
        }
        private void Client_deleteTestCompleted(object sender, WCF.deleteTestCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                DataRow row = dt.AsEnumerable().FirstOrDefault(x => (int)x["ID"] == (int)e.UserState);
                if (row != null)
                {
                    row.Delete();//从数据集中删除这一行，但是不影响别的行的状态
                    row.AcceptChanges();
                }
            }
            else showMsg(e.Error.Message);
        }
        void Client_deleteTestListCompleted(object sender, WCF.deleteTestListCompletedEventArgs e)
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
            {
                if (e.Error == null)
                {
                    dt = e.Result;
                    dv = dt.DefaultView;
                    uGrid.ItemsSource = dv;
                }
                else showMsg(e.Error.Message);
            }
        }

        private void btGetTest_Click(object sender, RoutedEventArgs e)
        {
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
        private void btDeleteTest_Click(object sender, RoutedEventArgs e)
        {
            if (uGrid.SelectedItems.Count > 0)
            {
                String uName = txtUser.Text.Trim();
                String uPass = txtPass.Password.Trim();
                if (uName != "" && uPass != "")
                {
                    List<int> IDS = new List<int>();//获取要删除的试题的ID序列
                    for (int i = 0; i < uGrid.SelectedItems.Count; i++)
                    {
                        DataRowView row = (DataRowView)uGrid.SelectedItems[i];
                        IDS.Add((int)row["ID"]);
                    }
                    try
                    {
                        using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                        {
                            MessageHeader user = MessageHeader.CreateHeader("uName", "MySpace", uName);
                            MessageHeader pass = MessageHeader.CreateHeader("uPass", "MySpace", uPass);
                            OperationContext.Current.OutgoingMessageHeaders.Add(user);
                            OperationContext.Current.OutgoingMessageHeaders.Add(pass);
                            client.deleteTestListAsync(IDS.ToArray(), IDS);
                        }
                    }
                    catch (Exception exp) { showMsg(exp.Message); }
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
