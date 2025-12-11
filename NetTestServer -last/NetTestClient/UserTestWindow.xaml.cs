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
    /// UserTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserTestWindow : Window
    {
        public WCF.NetTestServiceClient client;
        string url = "http://localhost:8733/NetTestService/";
        DataTable dt;
        DataView dv;
        public MainWindow mainWnd;
        public UserClass currentUser = new UserClass();
        public void showMsg(String s)
        {
            MessageBox.Show(s, "Information", MessageBoxButton.OK);
        }
        public UserTestWindow()
        {
            InitializeComponent();
            //client = new WCF.NetTestServiceClient();
        }
        private void btGetTest_Click(object sender, RoutedEventArgs e)
        {
            currentUser.uName = txtUser.Text.Trim();
            currentUser.uPass = txtPass.Password.Trim();
            if (currentUser != null)
            {
                try
                {
                    using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                    {
                        MessageHeader user = MessageHeader.CreateHeader("uName", "MySpace", 
                            currentUser.uName);
                        MessageHeader pass = MessageHeader.CreateHeader("uPass", "MySpace",
                            currentUser.uPass);
                        OperationContext.Current.OutgoingMessageHeaders.Add(user);
                        OperationContext.Current.OutgoingMessageHeaders.Add(pass);
                        client.getUserTestDataTableAsync();
                    }
                }
                catch (Exception exp) { showMsg(exp.Message); }
            }
        }

        private void btHandleTest_Click(object sender, RoutedEventArgs e)
        {
            int mValue = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row["tAnswer"].ToString() == row["uAnswer"].ToString()) ++mValue;
            }
            msg.Text = "成绩" + mValue.ToString();
            btHandleTest.IsEnabled = false;
            btGetTest.IsEnabled = true;
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
                        client.setUserMarkAsync(mValue);
                    }
                }
                catch (Exception exp) { showMsg(exp.Message); }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client.getUserTestDataTableCompleted += Client_getUserTestDataTableCompleted;
            client.setUserMarkCompleted += Client_setUserMarkCompleted;
            //currentUser.uName = txtUser.Text.Trim();
            //currentUser.uPass = txtPass.Password.Trim();                                                     
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
                        client.getUserTestDataTableAsync();
                    }
                }
                catch (Exception exp) { showMsg(exp.Message); }
            }
        }

        private void Client_setUserMarkCompleted(object sender, WCF.setUserMarkCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                msg.Text = "成绩" + e.Result.ToString();
                btGetTest.IsEnabled = true;
                btHandleTest.IsEnabled = false;
            }
            else showMsg(e.Error.Message);
        }

        private void Client_getUserTestDataTableCompleted(object sender, WCF.getUserTestDataTableCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                dt = e.Result;
                dt.Columns.Add("tNo");
                dt.Columns.Add("uAnswer");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["tNo"] = (i + 1).ToString();
                    dt.Rows[i]["uAnswer"] = "";
                }
                dt.AcceptChanges();
                dv = dt.DefaultView;
                uGrid.ItemsSource = dv;
                btGetTest.IsEnabled = false;
                btHandleTest.IsEnabled = true;
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
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWnd.Show();
        }
    }
}
