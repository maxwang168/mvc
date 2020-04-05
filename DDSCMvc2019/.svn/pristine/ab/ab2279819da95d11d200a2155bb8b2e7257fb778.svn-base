using CommonLibrary.DES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStrGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string m_errorMessage = string.Empty;
            string a = "data source=172.16.18.26;initial catalog=DDSC_DB;User ID=sa;Password=P@ssw0rd;timeOut=20";
            string b = DESCode.desEncryptBase64(a, ref m_errorMessage);
            Console.WriteLine("Encode Connection String:" + b);
        }
    }
}
