using NetTestModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace NetTestClient
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        WCF.NetTestServiceClient client;
        string url = "http://localhost:8733/NetTestService/";
        UserClass currentUser = new UserClass();
        public LoginWindow()
        {
            InitializeComponent();
            client = new WCF.NetTestServiceClient();
            client.loginCompleted += Client_loginCompleted;
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(url, UriKind.Absolute));
        }
        public void showMsg(String s)
        {
            MessageBox.Show(s, "Information", MessageBoxButton.OK);
        }
        private void Client_loginCompleted(object sender, WCF.loginCompletedEventArgs e)
        {
            if (e.Error == null) { showMsg(e.Result); return; }
            else { showMsg(e.Error.Message); }
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            currentUser.uName = txtUser.Text.Trim();
            currentUser.uPass = txtPass.Password.Trim();
            try
            {
                client.loginAsync(new UserClass { uName = currentUser.uName, uPass = currentUser.uPass });
            }
            catch (Exception ex) { showMsg(ex.Message); }
        }
    }
}
