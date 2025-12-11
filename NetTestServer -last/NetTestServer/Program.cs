using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetTestServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                String url = "http://localhost:8733/NetTestService/";
                ServiceHost host = new ServiceHost(typeof(NetTestService), new Uri(url));
                host.Open();
                Console.WriteLine(url + "正在监听......");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            Console.ReadKey();
        }
    }
}
