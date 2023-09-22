using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MSPCreator.MSP
{
    internal class TicketGenerator
    {
        static MD5 md = MD5.Create();
        static int markingID = Program.rng.Next(1000);
        public static string generateTicketEnding(string ticket)
        {
            markingID++;
            byte[] bytes = Encoding.ASCII.GetBytes(markingID.ToString());
            return ticket + BitConverter.ToString(md.ComputeHash(bytes)).Replace("-", "").ToLower() + BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}
