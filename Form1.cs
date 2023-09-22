using FluorineFx;
using FluorineFx.Json;
using MSPCreator.MSP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MSPCreator
{
    public partial class Form1 : Form
    {
        public static string URLEndpoint = "http://ws-pl.mspapis.com/";
        private byte[] Face,Body;
        private string lastTicket;
        public Form1()
        {
            InitializeComponent();
            //MessageBox.Show(MessyChecksum.logData());
            var procesy = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            foreach (var item in procesy)
            {
                if (item.Id == Process.GetCurrentProcess().Id) continue;
                item.Kill();
            }
      
            //AMF.Call("MovieStarPlanet.WebService.Permission.AMFPermission.GetPermissionScheme", new object[]
            //{
            //   new TicketHeader
            //   {
            //       Ticket = "8f14e45fceea167a5a36dedd4bea254337"
            //   },
            //   "PL"
            //});
            //DecryptSessionIdFile();
            //GetSessionId();
            // Body = new WebClient().DownloadData("https://snapshots.mspcdns.com/v1/MSP/au/snapshot/fullsizemoviestar/16258000.jpg");
            Body = new byte[1];
            Face = new byte[1];
        }
        //static void GetSessionId()
        //{
        //    dynamic val = AMF.Call(URLSessionEndpoint, "MovieStarPlanet.WebService.Os.AMFOs.CreateOsRef", new object[0], false);
        //    if (val.ToString().Contains("ERROR"))
        //    {
        //        GetSessionId();
        //        return;
        //    }
        //    if (!Histograms.ContainsKey(val["TjData"]))
        //    {
        //        GetSessionId();
        //        return;
        //    }
        //    AMF.SessionID = AMF.Call(URLSessionEndpoint, "MovieStarPlanet.WebService.Os.AMFOs.RunOsCheck", new object[2] { val["RefId"], Histograms[val["TjData"]] }, false).ToString();
        //}
        //string GetSessionIdFile()
        //{
        //    string text = "";
        //    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MSPCreator.histograms"))
        //    {
        //        byte[] byteArray = new byte[stream.Length];
        //        stream.Read(byteArray, 0, byteArray.Length);
        //        text = Encoding.ASCII.GetString(byteArray);
        //    }
        //    return text;
        //}
        //void DecryptSessionIdFile()
        //{
        //    byte[] text = File.ReadAllBytes("config");
        //    byte[] key = new byte[8] { 120, 99, 16, 12, 51, 41, 50, 81 };
        //    byte[] xor = new byte[text.Length];
        //    for (int i = 0; i < text.Length; i++)
        //    {
        //        xor[i] = (byte)(text[i] ^ key[i % key.Length]);
        //    }
        //    string[] array = Encoding.ASCII.GetString(xor).Split('\n');
        //    RijndaelManaged rijndaelManaged = new RijndaelManaged();
        //    rijndaelManaged.KeySize = 256;
        //    rijndaelManaged.BlockSize = 256;
        //    rijndaelManaged.Key = Convert.FromBase64String(array[0]);
        //    rijndaelManaged.IV = Convert.FromBase64String(array[1]);
        //    MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(GetSessionIdFile()));
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Read);
        //    Histograms = JavaScriptConvert.DeserializeObject<Dictionary<string, string>>(new StreamReader(cryptoStream).ReadToEnd());
        //    memoryStream.Dispose();
        //    rijndaelManaged.Dispose();
        //} 
        bool CheckUsername(string username)
        {
           // bool val = (bool)AMF.Call("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.IsNameBlocked", new object[1] { username });
            dynamic val2 = AMF.Call( "MovieStarPlanet.WebService.Moderation.AMFModeration.CheckNewUsername", new
                 string[2] { username, "MSP.PL" });
            bool val3 = (bool)AMF.Call("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.IsActorNameUsed", new string[1] { username });
            return val2["Response"] &&!val3;
        }
        string CreateAccount(string username,string password)
        {
            object[] clothes = new object[4] 
            {
                new ASObject(new Dictionary<string, object>
                {
                 //   {"ActorId",-1 },
                    {"ClothesId",21301 },
                  //  {"IsWearing",1 },
                  //  {"x",0 },
                  //  {"y",0 },
                    {"Color","0xFFE2C6,0x000000,0x000000,0xffffff" },
                    {"ActorClothesRelId",-8 }
                }),
                      new ASObject(new Dictionary<string, object>
                {
                  //  {"ActorId",-1 },
                    {"ClothesId",19609 },
                 //   {"IsWearing",1 },
                  //  {"x",0 },
                 //   {"y",0 },
                    {"Color","0x20002" },
                    {"ActorClothesRelId",-4 }
                }),
                            new ASObject(new Dictionary<string, object>
                {
                   // {"ActorId",-1 },
                    {"ClothesId",19709 },
                  //  {"IsWearing",1 },
                  //  {"x",0 },
                 //   {"y",0 },
                    {"Color","0x7CB4E5" },
                    {"ActorClothesRelId",-6 }
                }),
                                  new ASObject(new Dictionary<string, object>
                {
                   // {"ActorId",-1 },
                    {"ClothesId",19702 },
                 //   {"IsWearing",1 },
                 //   {"x",0 },
                 //   {"y",0 },
                    {"Color","0xF0253B,0xFFF4E7" },
                    {"ActorClothesRelId",-7 }
                }),
            };
            dynamic val = AMF.Call("MovieStarPlanet.WebService.User.AMFUserServiceWeb.CreateNewUserWithSecureSnapshot", new object[5]
            {
                new ASObject(new Dictionary<string, object>
                {
                    {"NoseId",38 },
                    {"InvitedByActorId",-1 },
                    {"ChosenActorName",username },
                    {"ChosenPassword",password },
                    {"SkinIsMale",false },
                    {"Clothes",clothes },
                    {"EyeColors","skincolor" },
                    {"EyeId",1},
                    {"SkinColor","15971965" },
                    {"MouthColors", "skincolor,0x9F0000" },
                    {"MouthId",24}
                }),
                ChecksumCalculator.createChecksum(username,password,Face,Body),
                null,
                Face,
                Body
            });
            if(val is null)
            {
                return null;
            }
            return val["ticket"];
        }
        void RenderCaptchaByUsername(string username)
        {
            pictureBox2.Load(URLEndpoint + "/Utils/Captcha/CaptchaGenerator.ashx?username=" + username);
        }
        bool ValidateCaptcha(string username,string captcha)
        {
            return (bool)AMF.Call("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.ValidateCaptcha", new string [2]
            {
                captcha,username
            });
        }



        private void button1_Click(object sender, EventArgs e)
        {

            //    if (true)
            //    {
            //        MessageBox.Show("You can't use this username!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            if (!CheckUsername(textBox1.Text))
            {
                MessageBox.Show("Nazwa jest niewłaściwa lub zajęta!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                ValidateCaptcha(textBox1.Text, textBox3.Text);
            }
            string ticket = CreateAccount(textBox1.Text, textBox2.Text);
            if (ticket == null)
            {
                RenderCaptchaByUsername(textBox1.Text);
                MessageBox.Show("Nie udało się stworzyć konta, może to być spowodowane przez captcha. Proszę wpisać captcha w wyznaczonym polu.", "WARN", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            lastTicket = ticket;
            //var lines = File.ReadAllLines("WygenerowanePL.txt").ToList();
            //lines.Add($"{textBox1.Text}:{textBox2.Text}");
            //File.WriteAllLines("WygenerowanePL.txt", lines);
            MessageBox.Show("Utworzono konto!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                Face = ResizeImageToPFP(File.ReadAllBytes(ofd.FileName));
                MessageBox.Show("Załadowano zdjęcie profilowe!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AMF.Call("MovieStarPlanet.WebService.Friendships.AMFFriendshipService.ApproveDefaultAnchorFriendship", new object[2]
            {
                new TicketHeader
                {
                    Ticket = TicketGenerator.generateTicketEnding(lastTicket)
                },
                Convert.ToInt32(lastTicket.Split(',')[1])
            });
            AMF.Call("MovieStarPlanet.WebService.Permission.AMFPermission.SubmitDateOfBirth", new object[4]
            {
                new TicketHeader
                {
                    Ticket = TicketGenerator.generateTicketEnding(lastTicket)
                },
                -1,
                4,
                2006
            });
            AMF.Call("MovieStarPlanet.WebService.Permission.AMFPermission.AcceptTerms", new object[1]
            {
                new TicketHeader
                {
                    Ticket = TicketGenerator.generateTicketEnding(lastTicket)
                }
            });
            MessageBox.Show("Konto zostało zweryfikowane i jest gotowe do użytkowania!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        byte[] ResizeImageToPFP(byte[] b)
        {
            byte[] a = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (var image = Bitmap.FromStream(new MemoryStream(b)))
                {
                    using (Bitmap bitmap = new Bitmap(image, new Size(100, 100)))
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                }
                a= stream.ToArray();
            }
            return a;
        }
    }
}
