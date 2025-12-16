using NetTestModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetTestClient
{
    /// <summary>
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        WCF.NetTestServiceClient client;
        string url = "http://localhost:8733/NetTestService/";
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
        public MainWindow()
        {
            InitializeComponent();
            client = new WCF.NetTestServiceClient();
            client.loginCompleted += Client_loginCompleted;
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(url, UriKind.Absolute));
        }
        private void Client_loginCompleted(object sender, WCF.loginCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                txtMsg.Text = e.Result;
                if (currentUser.uName == "Admin")
                {
                    btMark.IsEnabled = true;
                    btManager.IsEnabled = true;
                }
                else
                {
                    btTest.IsEnabled = true;
                }
            }
            else showMsg(e.Error.Message);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btManager.IsEnabled = false;
            btMark.IsEnabled = false;
            btTest.IsEnabled = false;
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            currentUser.uName = txtUser.Text.Trim();
            currentUser.uPass = txtPass.Password.Trim();
            url = tbUrl.Text.Trim();
            client.loginAsync(new UserClass { uName = currentUser.uName, uPass = currentUser.uPass });
        }

        private void btManager_Click(object sender, RoutedEventArgs e)
        {
            currentUser.uName = txtUser.Text.Trim();
            currentUser.uPass = txtPass.Password.Trim();
            if (currentUser.uName != "" && currentUser.uPass != "")
            {
                AdminWindow dlg = new AdminWindow();
                dlg.mainWnd = this;
                dlg.client = client;
                dlg.currentUser = currentUser;
                this.Hide();
                dlg.ShowDialog();
                this.Show();

            }
            else showMsg("用户或密码不能为空！");
        }

        private void btMark_Click(object sender, RoutedEventArgs e)
        {
            currentUser.uName = txtUser.Text.Trim();
            currentUser.uPass = txtPass.Password.Trim();
            if (currentUser.uName != "" && currentUser.uPass != "")
            {
                MarkWindow dlg = new MarkWindow();
                dlg.mainWnd = this;
                dlg.client = client;
                dlg.currentUser = currentUser;
                this.Hide();
                dlg.ShowDialog();
                this.Show();
            }
            else showMsg("用户或密码不能为空！");
        }

        private void btTest_Click(object sender, RoutedEventArgs e)
        {
            currentUser.uName = txtUser.Text.Trim();
            currentUser.uPass = txtPass.Password.Trim();
            if (currentUser.uName != "" && currentUser.uPass != "")
            {
                UserTestWindow dlg = new UserTestWindow();
                dlg.mainWnd = this;
                dlg.client = client;
                dlg.currentUser = currentUser;
                this.Hide();
                dlg.ShowDialog();
                this.Show(); ;
            }
            else showMsg("用户或密码不能为空！");
        }
    }
}
