using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DotNetOpenMail;

using System.Net;
using System.Net.Sockets;

using System.Net.Security;

using System.Diagnostics;
using System.Threading;
using System.IO;

namespace ConsoleApplication3
{
    class Program
    {
        AutoResetEvent areConn = null;
        TcpClient tcp = null;
        StreamWriter swrite = null;
        StreamReader sread = null;
        bool tcpConnect = false;
        Thread t = null;

        ThreadStart ts = null;

        static void Main(string[] args)
        {
            Program p = new Program();
            p.init(); 
        }

        public void init()
        {
            List<mxserver> ltmx = new Program().GetMXServers("gmail.com");
            
            foreach (mxserver mx in ltmx)
            {
                if (!tcpConnect)
                    remoteconnect(mx.server, 25);


            }
            
            Console.WriteLine("hello");
            Console.ReadLine();
        }

        

        public void email()
        {
            try
            {
                EmailMessage emailMessage = new EmailMessage();

                emailMessage.FromAddress = new EmailAddress("cswann@XXXXXX.com",
                    "Charles Swann");

                emailMessage.AddToAddress(new EmailAddress("hprasad@ipmg.com.au",
                    "hari"));

                emailMessage.Subject = "Missed you";

                emailMessage.TextPart = new TextAttachment("Just checking where " +
                    "you were last night.\r\nSend me a note!\r\n\r\n-Charles");

                emailMessage.HtmlPart = new HtmlAttachment("<html><body>" +
                    "<p>Just checking up on where you were last night.</p>\r\n" +
                    "<p>Send me a note!</p>\r\n\r\n" +
                    "<p>-Charles</p></body></html>");

                emailMessage.Send(new SmtpServer("localhost"));
            }
            catch (Exception ex)
            {
            }
        }

        public bool remoteconnect(string server, int port)
        {

            try
            {
                string st = Environment.NewLine;
                if (File.Exists(@"d:\log.txt"))
                    File.Delete(@"d:\log.txt");
                areConn = new AutoResetEvent(false);
                tcp = new TcpClient();
                //IAsyncResult a = tcp.BeginConnect(server, 25, new AsyncCallback(completeAsync), tcp);

                
                tcp.Connect(server, port);

                var ns = tcp.GetStream();
                tcpConnect = true;
                swrite = new StreamWriter(ns);
                sread = new StreamReader(ns);

               

                swrite.WriteLine("EHLO");
                swrite.Flush();
                Response();

                //swrite.WriteLine("auth plain " + auth);
                //swrite.Flush();
                //Response();

                swrite.WriteLine("MAIL FROM: <prasasthp@gmail.com>");
                swrite.Flush();
                Response();

                swrite.WriteLine("RCPT TO: <prasaathp84@gmail.com>");
                swrite.Flush();
                Response();


                swrite.WriteLine("DATA");
                swrite.Flush();
                Response();

                swrite.WriteLine("Subject : test");
                swrite.Flush();
                Response();

                swrite.WriteLine("hello test");
                swrite.Flush();
                Response();

                swrite.WriteLine("what reastasdf");
                swrite.Flush();
                GetResponse();
                areConn.WaitOne(1000);
                t.Suspend();


                swrite.WriteLine(".");
                swrite.Flush();
                Response();

                swrite.WriteLine("Quit");
                swrite.Flush();
                Response();

                //writer.WriteLine("RCPT TO: <prasasthp84@gmail.com>");
                //writer.Flush();
                //GetResponse();
                //areConn.WaitOne(3000);
                //t.Suspend();
            }
            catch (Exception ex)
            {

            }
            tcp.Close();
            return false;
        }

        public bool remoteconnect(string server)
        {
           
            try
            {
                string st = Environment.NewLine;
                if (File.Exists(@"d:\log.txt"))
                    File.Delete(@"d:\log.txt");
                areConn = new AutoResetEvent(false);
                tcp = new TcpClient();
                //IAsyncResult a = tcp.BeginConnect(server, 25, new AsyncCallback(completeAsync), tcp);

                string auth = EncodeTo64("username\0prasaathp@gmail.com\0fight@06");
                tcp.Connect(server, 465);

                var ns = tcp.GetStream();
                SslStream ssl = new SslStream(ns);
                ssl.AuthenticateAsClient(server);
                swrite = new StreamWriter(ssl);
                sread = new StreamReader(ssl);
                
                ts = new ThreadStart(Response);

                t = new Thread(ts);
                t.Start();
                t.Suspend();

                swrite.WriteLine("EHLO");
                swrite.Flush();               
                GetResponse();
                areConn.WaitOne(1000);
                t.Suspend();

                swrite.WriteLine("auth plain "+auth);
                swrite.Flush();
                GetResponse();
                areConn.WaitOne(1000);
                t.Suspend();

                swrite.WriteLine("MAIL FROM: <prasasthp@gmail.com>");
                swrite.Flush();
                GetResponse();
                areConn.WaitOne(1000);
                t.Suspend();

                swrite.WriteLine("RCPT TO: <prasaathp84@gmail.com>");
                swrite.Flush();
                GetResponse();
                areConn.WaitOne(1000);
                t.Suspend();


                swrite.WriteLine("DATA");
                swrite.Flush();
                GetResponse();
                areConn.WaitOne(1000);
                t.Suspend();

                swrite.WriteLine("Subject : test");
                swrite.Flush();
                GetResponse();
                areConn.WaitOne(1000);
                t.Suspend();

                swrite.WriteLine("hello test");
                swrite.Flush();
                GetResponse();
                areConn.WaitOne(1000);
                t.Suspend();

                swrite.WriteLine("what reastasdf");
                swrite.Flush();
                GetResponse();
                areConn.WaitOne(1000);
                t.Suspend();


                swrite.WriteLine(".");
                swrite.Flush();
                GetResponse();
                areConn.WaitOne(1000);
                t.Suspend();

                swrite.WriteLine("Quit");
                swrite.Flush();
                GetResponse();
                areConn.WaitOne(1000);
                t.Suspend();

                //writer.WriteLine("RCPT TO: <prasasthp84@gmail.com>");
                //writer.Flush();
                //GetResponse();
                //areConn.WaitOne(3000);
                //t.Suspend();
            }
            catch (Exception ex)
            { 
            
            }
            tcp.Close();
            return false;
        }

        public void GetResponse()
        {
            t.Resume();
        }

        public void Response()
        {
            string line = null;

            char[] chRead = new char[1000];

            try
            {
                line = sread.ReadToEnd();
               sread.ReadBlock(chRead, 0, 1000);
                //while (sread.Peek() > 0)
                //    sread.Read(chRead, 0, chRead.Length);

               

                Console.WriteLine(line);
            }
            catch (Exception ex)
            { 
                
            }

            //while ((line = sread.ReadLine()) != "")
            //{
            //    Console.WriteLine(line);
              
            //    File.AppendAllText(@"d:\log.txt", line + "\r\n");
            //}

           
        }

      

        public bool remoteconnect1(string server)
        {
           

            var port = 25;
using (TcpClient tcp = new TcpClient())  
{
    //IAsyncResult ar = tcp.BeginConnect(server, port, null, null);
    server = "smtp.gmail.com";
    port = 465;
    tcp.Connect(server, port);

   // System.Threading.WaitHandle wh = ar.AsyncWaitHandle;
    try
    {
        //if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5), false))
        //{
        //    tcp.Close();
        //    return true;
        //}
        using (var stream = tcp.GetStream())
        using (var sslStream = new SslStream(stream))
        {


            sslStream.AuthenticateAsClient(server);

            using (StreamWriter writer = new StreamWriter(sslStream))
            using (StreamReader reader = new StreamReader(sslStream))
            {
                writer.WriteLine("EHLO " + server);
                writer.Flush();
                response(reader);
                //writer.WriteLine("MAIL FROM: <prasasthp@gmail.com>");
                //writer.Flush();
                //response(reader);
                //writer.WriteLine("RCPT TO: <prasasthp84@gmail.com>");
                //writer.Flush();
                ////Console.WriteLine(reader.ReadLine());
                ////writer.WriteLine("DATA");
                ////writer.Flush();
                //response(reader);
                //writer.WriteLine("AUTH Login");
                //writer.Flush();
                //Console.WriteLine(reader.ReadLine());
                //Console.WriteLine(reader.ReadLine());
                // GMail responds with: 220 mx.google.com ESMTP
            }
        }
        tcp.Close();
        return false;
    }
    catch (Exception ex)
    {
        tcp.Close();
        return true;
    }
    finally 
    {  
        //wh.Close();  
    }  
}
 
            
           

            
        }

        public void response(StreamReader strread )
        {
            string line = "";


           try
           {
               while (( line = strread.ReadLine() ) != null)
               {
                   Console.WriteLine(line);
                   File.AppendAllText(@"d:\log", line);
               }
           }
           catch (Exception ex)
           { 
            
           }

            
        }

        public List<mxserver> GetMXServers(string domainName)
        {
            string command = "nslookup -q=mx " + domainName;
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;

            procStartInfo.CreateNoWindow = true;

            Process proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            string result = proc.StandardOutput.ReadToEnd();

            if (!string.IsNullOrEmpty(result))
                result = result.Replace("\r\n", Environment.NewLine);

            List<mxserver> lmxs = new List<mxserver>();
            int pref = 0;
            string server = "";
            foreach (string line in result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.Contains("MX preference =") && line.Contains("mail exchanger ="))
                {
                     pref = Convert.ToInt32(line.Substring(line.IndexOf("=",1)+2, line.IndexOf( ',',1 ) - (line.IndexOf("=",1)+2)  ));
                     server = line.Substring(line.IndexOf("=",line.IndexOf(","))+2 );
                     lmxs.Add(new mxserver(pref,server));
                }
            }
            lmxs.Sort(new sortOnPreference());

            return lmxs;
           
        }

        public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }


        public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

       
    }

    //struct mxserver
    //{
    //    public int preference;
    //    public string server;

    //    public mxserver(int preference, string server)
    //    {
    //        this.preference = preference;
    //        this.server = server;
    //    }
    //}

   public class mxserver : IComparable<mxserver>
    {
        public int preference;
        public string server;

        public mxserver(int preference, string server)
        {
            this.preference = preference;
            this.server = server;
        }

        public int CompareTo(mxserver b)
        {
            // Alphabetic sort name[A to Z]
            return this.preference.CompareTo(b.preference);
        }
    }

    public class sortOnPreference : IComparer<mxserver>
    {
        public int Compare(mxserver a, mxserver b)
        {
            if (a.preference > b.preference) return 1;
            else if (a.preference < b.preference) return -1;
            else return 0;
        }
    }

   
}
