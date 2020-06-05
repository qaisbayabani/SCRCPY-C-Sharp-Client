using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.IO;
using OpenCvSharp;
//using LibVLCSharp.Shared;
using System.Linq;
using System.Text;




class Program
{


    public static void Main()
    {

        ForFun fun = new ForFun();

        ForFun2 fun2 = new ForFun2();

                Thread ti = new Thread(new ThreadStart(() => fun.Fun()));
                ti.Start();

                Thread ti2 = new Thread(new ThreadStart(() => fun2.Fun2()));
                ti2.Start();

    //    Thread ti3 = new Thread(new ThreadStart(() => ForFun3.Fun3())); ti3.Start();


        //Console.WriteLine("Press ESC to stop");
        do
        {
            while (!Console.KeyAvailable)
            {
                // Do something

                Window.DestroyAllWindows();            
            
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);



    }


    public class ForFun2
    {

        public Process process = new Process();
        public ProcessStartInfo ps = new ProcessStartInfo();

        public void Fun2()
        {

            Console.WriteLine("Entered in 2nd thread");

            ps.WindowStyle = ProcessWindowStyle.Normal;
            ps.CreateNoWindow = true;
            ps.UseShellExecute = false;
            ps.RedirectStandardOutput = true;
            ps.RedirectStandardError = true;

            process = new Process();
            ps.FileName = "adb.exe ";
            ps.Arguments = "connect " + "192.168.0.2";
            process.StartInfo = ps;
            process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine(process.StandardError.ReadToEnd());
            process.Close(); process.Dispose();

            process = new Process();
            ps.FileName = "adb.exe ";
            ps.Arguments = "reverse " + "--remove-all";
            process.StartInfo = ps;
            process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine(process.StandardError.ReadToEnd());
            process.Close(); process.Dispose();

            process = new Process();
            ps.FileName = "adb.exe ";
            ps.Arguments = "forward " + "--remove-all";
            process.StartInfo = ps;
            process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine(process.StandardError.ReadToEnd());
            process.Close(); process.Dispose();

            process = new Process();
            ps.FileName = "adb.exe ";
            ps.Arguments = "push " + "scrcpy-server " + "/data/local/tmp/scrcpy-server.jar";
            process.StartInfo = ps; process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine(process.StandardError.ReadToEnd());
            process.Close(); process.Dispose();


            process = new Process();
            ps.FileName = "adb.exe ";
            ps.Arguments = "reverse " + "localabstract:scrcpy " + "tcp:27183";
            process.StartInfo = ps; process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine(process.StandardError.ReadToEnd());
            process.Close(); process.Dispose();

            process = new Process();
            ps.FileName = "adb.exe ";
            ps.Arguments = "shell " + "CLASSPATH=/data/local/tmp/scrcpy-server.jar " +
         "app_process " + "/ " + "com.genymobile.scrcpy.Server " +
        "1.12.1 " + "1024 " + "8000000 " + "0 " + "false " + "- " + "false " + "false";
            process.StartInfo = ps; process.Start();
            Console.WriteLine(process.StandardError.ReadToEnd());
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit(); process.Close(); process.Dispose();



        }

    }

    class ForFun
    {

        public void Fun()
        {

            TcpListener server = null;
            TcpListener server2 = null;

            Int32 port = 27183;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);
            TcpClient client = null;
            NetworkStream stream = null;

            Int32 port2 = 27184;
            IPAddress localAddr2 = IPAddress.Parse("127.0.0.1");
            server2 = new TcpListener(localAddr2, port2);
            TcpClient client2 = null;
            NetworkStream stream2 = null;

            //byte[] bb1 = new byte[4];

            byte[] bb2 = new byte[64];

            byte[] bb3 = new byte[4];

            //byte[] bb5 = new byte[40];

            byte[] bb4 = new byte[12];
            byte[] bb5 = new byte[6];



            try
            {

                server.Start();
                server2.Start();
                
                
                while (true)
                {

                    Console.WriteLine("Waiting for a connection Server No.1---------- ");
                    client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!========>1");
                    stream = client.GetStream();

                    BufferedStream bf = new BufferedStream(stream);

                    bf.Read(bb2, 0, bb2.Length);

                    Console.WriteLine(System.Text.Encoding.ASCII.GetString(bb2, 0, bb2.Length));


                    bf.Read(bb3, 0, bb3.Length);
                    Console.WriteLine(System.Text.Encoding.ASCII.GetString(bb3, 0, bb3.Length));

                    //stream.Read(bb4, 0, bb4.Length);
                    //Console.WriteLine(System.Text.Encoding.ASCII.GetString(bb4, 0, bb4.Length));


                    Console.Write("Waiting for a connection Server No.2---------- ");


             //       Thread ti3 = new Thread(new ThreadStart(() => ForFun3.Fun3()));ti3.Start();

                    //Thread ti3 = new Thread(new ThreadStart(() => Vlci.Vl()));ti3.Start();


                    //Thread.Sleep(2000);

                    client2 = server2.AcceptTcpClient();
                    Console.WriteLine("Connected!========>2");

                    stream2 = client2.GetStream();

                    
                    //Par pp = new Par();
                    BufferedStream bf2 = new BufferedStream(stream2);

                    byte[] messageSent = Encoding.ASCII.GetBytes
("HTTP / 1.0 200 OK Content - type: application / octet - stream Cache - Control: no - cache Connection: close");

                    bf.Read(bb4, 0, bb4.Length);

                    //bf2.Write(messageSent, 0, messageSent.Length);


                    while (bf.Read(bb4, 0, bb4.Length)>0)
                    {
                    
                        bf.Flush();
                        bf2.Flush();

                        bf2.Write(bb4, 0, bb4.Length);
                                        

                    }

                }

            }
            catch (SocketException e)
            {
                Console.WriteLine("One Number Exception on Socket", e);

            }

            catch (ArgumentNullException ane)
            {

                Console.WriteLine("Two Number on Augument Null", ane.ToString());

            }

            catch (Exception e)
            {

                Console.WriteLine("Third is Gneeal", e.ToString());

            }

            finally
            {
                // Stop listening for new clients.
                server.Stop();
                server2.Stop();
            }


            stream.Flush();
            stream.Close();
            stream2.Flush();
            stream2.Close();
            client.Close();
            client2.Close();
            server.Stop();
            server2.Stop();





            Process process = new Process();
            ProcessStartInfo ps = new ProcessStartInfo();




            ps.WindowStyle = ProcessWindowStyle.Normal;
            ps.CreateNoWindow = true;
            ps.UseShellExecute = false;
            ps.RedirectStandardOutput = true;
            ps.RedirectStandardOutput = true;
            ps.RedirectStandardError = true;


            ps.FileName = "adb.exe ";
            ps.Arguments = "reverse " + "--remove-all";
            process.StartInfo = ps;
            process.Start();

            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine(process.StandardError.ReadToEnd());

            process.Close(); process.Dispose();


            process = new Process();
            ps.FileName = "adb.exe ";
            ps.Arguments = "forward " + "--remove-all";
            process.StartInfo = ps;
            process.Start();

            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine(process.StandardError.ReadToEnd());

            process.Close(); process.Dispose();

            process = new Process();
            ps.FileName = "adb.exe ";
            ps.Arguments = "kill-server";
            process.StartInfo = ps;
            process.Start();

            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.WriteLine(process.StandardError.ReadToEnd());

            process.Close();
            process.Dispose();

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();

        }
    }

    class ForFun3{

    public static void Fun3() {


        VideoCapture capture = null;

        capture = new VideoCapture("http://127.0.0.1:27184");

        int sleepTime = (int)Math.Round(1000/ capture.Fps);

        using (var window = new Window("capture", WindowMode.KeepRatio))
        {
            // Frame image buffer
            Mat image = new Mat();
               // Cv2.CreateFrameSource_Video_CUDA("http://127.0.0.1:27184");
            
                while (true)
            {


                Found:


                    capture.Read(image); // same as cvQueryFrame
                    //capture.Fps = 25;
                    //image = capture.RetrieveMat();
                    //Console.WriteLine("======================================>"+capture.Fps);
                    
                
                if (image.Empty())
                {
                    Console.WriteLine("Empty");
                    
                     //Console.ReadKey();
                    
                        goto Found;

                }

                    try
                    {
                    
                        window.ShowImage(image);
                        
                   
                    }
                    catch (Exception e) { }

                Cv2.WaitKey(sleepTime);

            }
        }
    }

}
class Par
    {

        public void Par1(byte[] bbx) {


            for (int x = 0; x < bbx.Length; x++)
            {
                Console.WriteLine("Values in BB3====>" + bbx[x]);
            }


        }




    }

/*
    class Vlci
    {
        public static void Vl()
        {
            Core.Initialize();

            var libVLC = new LibVLC();

            //var media = new Media(libVLC, "https://r6---sn-2uja-3ipd.googlevideo.com/videoplayback?expire=1589114677&ei=1aK3Xq7yHu_XxN8PxKKxkA4&ip=102.130.114.107&id=o-AFYMWLGwQ3RvqsytDt4xC1vtFbuQYrWCp0vkkgHhvV2Z&itag=18&source=youtube&requiressl=yes&vprv=1&mime=video%2Fmp4&gir=yes&clen=91067750&ratebypass=yes&dur=1419.110&lmt=1541022281176968&fvip=4&fexp=23882514&c=WEB&txp=5431432&sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cgir%2Cclen%2Cratebypass%2Cdur%2Clmt&sig=AJpPlLswRgIhAJnAWMH9YpEto-Wor_LHyRwxU6ZqRs7cRGbgphiDNnDoAiEAnlCBFQqHufy79Jf_rUoqJfCH8JqXZYVr0hZ2KPZyfT8%3D&video_id=Re0bmIhMg1s&title=Mullah+Nasruddin+-+Episode+4&rm=sn-31oovb-q0ge7l,sn-wocr7z&req_id=cc0028da63a2a3ee&redirect_counter=2&cms_redirect=yes&ipbypass=yes&mh=Mo&mip=39.45.38.74&mm=29&mn=sn-2uja-3ipd&ms=rdu&mt=1589093066&mv=u&mvi=5&pl=18&lsparams=ipbypass,mh,mip,mm,mn,ms,mv,mvi,pl&lsig=ALrAebAwRAIgDtHpGivvQTviLWU8hPV8b2da0397AxtebV8rEcED-eICIHYSA6vTFMrGNJIxBM0oyviVEN60ihoOQj66ncW9mUfl", FromType.FromLocation);

            //var media = new Media(libVLC, "e:\\Risen.mp4");

            var media = new Media(libVLC, "http://127.0.0.1:27184", FromType.FromLocation);

            var mp = new MediaPlayer(media);

            Console.WriteLine(media.Duration);


            mp.Play();



            Console.ReadKey();
        }

    }
*/
}
