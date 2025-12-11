using NetTestModel;
using System;
using System.Collections.Generic;
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
    /// addTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class addTestWindow : Window
    {
        public WCF.NetTestServiceClient client;
        public MainWindow mainWnd;
        public UserClass currentUser = new UserClass();
        public string url = "http://localhost:8733/NetTestService/";
        public addTestWindow()
        {
            InitializeComponent();
            client = new WCF.NetTestServiceClient();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client.addTestCompleted += Client_addTestCompleted;
            //client.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(url));
            cbAnswer.Items.Add("A");
            cbAnswer.Items.Add("B");
            cbAnswer.Items.Add("C");
            cbAnswer.Items.Add("D");
            cbAnswer.SelectedIndex = 0;
        }
        public void showMsg(String s)
        {
            MessageBox.Show(s, "Information", MessageBoxButton.OK);
        }

        private void Client_addTestCompleted(object sender, WCF.addTestCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                TestClass test = e.Result;
                if (test.ID > 0)
                {
                    txtMsg.Text = "上传成功ID=" + test.ID.ToString();
                }
                else
                {
                    showMsg(e.Error.Message);
                }
            }
        }

        private void btUpload_Click(object sender, RoutedEventArgs e)
        {
            string uName = txtUser.Text.Trim();
            string uPass = txtPass.Password.Trim();
            string tTitle = txtTitle.Text.Trim();
            string text = txtText.Text.Trim();
            if (uName != "" && uPass != "" && tTitle != "" && text != "")
            {
                try
                {
                    using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                    {
                        MessageHeader user = MessageHeader.CreateHeader("uName", "MySpace", uName);
                        MessageHeader pass = MessageHeader.CreateHeader("uPass", "MySpace", uPass);
                        OperationContext.Current.OutgoingMessageHeaders.Add(user);
                        OperationContext.Current.OutgoingMessageHeaders.Add(pass);
                        TestClass test = new TestClass { ID = 0, tAnswer = cbAnswer.SelectedItem.ToString(), tTitle = tTitle, tText = text };
                        client.addTestAsync(test);
                    }
                }
                catch (Exception ex) { showMsg(ex.Message); }
            }
        }
    }
}
