using FluorineFx.IO;
using FluorineFx.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace MSPCreator.MSP
{
    class AMF
    {
        public static HttpClient client = new HttpClient();
        private static string generateSID()
        {
            var rng = new Random();
            string text = "";

            
            while (text.Length < 48)
                text += BitConverter.ToString(Encoding.ASCII.GetBytes((rng.NextDouble() * int.MaxValue).ToString())).Replace("-", "");
            text = text.Substring(0, 46);
          
            text = Convert.ToBase64String(Encoding.ASCII.GetBytes(text));
            
            return text;
        }
        public static object Call(string Method, object[] Content, bool UseSecurity = true)
        {

            client.DefaultRequestHeaders.Referrer = new Uri("app:/cache/t1.bin/[[DYNAMIC]]/2");
            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("(Mozilla/5.0 (Windows; U; en) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/32.0)"));
            AMFMessage message = new AMFMessage(3);
            if (UseSecurity)
            {
                message.AddHeader(new AMFHeader("sessionID", false, generateSID()));
                message.AddHeader(new AMFHeader("id", false, ChecksumCalculator.createChecksum(Content)));
            }
            message.AddHeader(new AMFHeader("needClassName", false, false));
            message.AddBody(new AMFBody(Method, "/1", Content));
            MemoryStream memoryStream = new MemoryStream();
            AMFSerializer serializer = new AMFSerializer(memoryStream);
            serializer.WriteMessage(message);
            serializer.Flush();
            serializer.Dispose();
            byte[] requestData = memoryStream.ToArray();
            memoryStream.Dispose();
            var content = new ByteArrayContent(requestData);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-amf");
        
            var resp = client.PostAsync("https://ws-pl.mspapis.com/msp/100.10.14/Gateway.aspx?method=" + Method, content).GetAwaiter().GetResult();
            return DecodeAMF(resp.Content.ReadAsStreamAsync().GetAwaiter().GetResult());
        }
        private static object DecodeAMF(Stream b)
        {
            AMFDeserializer deserializer = new AMFDeserializer(b);
            object o = deserializer.ReadAMFMessage().Bodies[0].Content;
            deserializer.Dispose();
            b.Dispose();
            return o;
        }
    }
}
