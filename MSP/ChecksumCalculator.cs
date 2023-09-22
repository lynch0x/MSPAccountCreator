using FluorineFx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSPCreator.MSP
{
    class ChecksumCalculator
    {
        static SHA1 sHA = SHA1.Create();
        static MD5 md = MD5.Create();
        public static string createChecksum(object[] arguments)
        {
         //   return "";
            return BitConverter.ToString(sHA.ComputeHash(Encoding.UTF8.GetBytes(fromArray(arguments) + "2zKzokBI4^26#oiP"+ getTicketValue(arguments) ))).Replace("-", "").ToLower();
        }
        public static string createChecksum(string username, string password, byte[] face, byte[] body)
        {
            
            return Convert.ToBase64String(md.ComputeHash(Encoding.UTF8.GetBytes(username + password  + fromByteArray(face)+ fromByteArray(body) + "cEKezTJx%S31T$bg")),0,16);
        }
        private static string fromArray(Array arguments)
        {
            
            string text = "";
            foreach (object obj in arguments)
            {
                if(!(obj is null || obj is TicketHeader))
                text += fromObjectInner(obj);
            }
            return text;
        }
        private static string getTicketValue(object[] o)
        {
            for (int i = 0; i < o.Length; i++)
            {
                if (o[i] is TicketHeader )
                {
                    
                        var podzial = ((TicketHeader)o[i]).Ticket.Split(

                             ','
                        );
                        var serv = podzial[0];
                        var koncowka = podzial.Last();
                        var znaczki = koncowka.Substring(koncowka.Length - 5);
                        return serv+znaczki;

                }
            }
            return "XSV7%!5!AX2L8@vn";
        }
        private static string fromObjectInner(object obyek)
        {
            if (obyek is byte[])
            {
                return fromByteArray((byte[])obyek);
            }
            if(obyek is Array)
            {
                return fromArray((Array)obyek);
            }
            if (obyek is ASObject)
            {
                return fromArray(new SortedDictionary<string, object>((ASObject)obyek).Values.ToArray());
            }
            
            return obyek.ToString();
        }
       
        private static string fromByteArray(byte[] a)
        {
            if (a.Length <= 20)
            {
                return BitConverter.ToString(a).Replace("-", "").ToLower();
            }
            byte[] bytes = new byte[20];
            for (int i = 0; i < 20; i++)
            {
                int index = a.Length / 20 * i;
                bytes[i] = a[index];
            }
            return BitConverter.ToString(bytes.ToArray()).Replace("-", "").ToLower();
        }
    }
}
