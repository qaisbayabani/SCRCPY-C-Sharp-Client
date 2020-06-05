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
            Console.`WriteLine(process.StandardError.ReadToEnd());
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

                    
                    Console.Write("Waiting for a connection Server No.2---------- ");


                    Thread ti3 = new Thread(new ThreadStart(() => ForFun3.Fun3()));ti3.Start();

                    
                    client2 = server2.AcceptTcpClient();
                    Console.WriteLine("Connected!========>2");

                    stream2 = client2.GetStream();

                    
                    BufferedStream bf2 = new BufferedStream(stream2);


                    bf.Read(bb4, 0, bb4.Length);
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
            Mat image = new Mat();
               
                while (true)
            {


                Found:


                    capture.Read(image); 
                
                if (image.Empty())
                {
                    Console.WriteLine("Empty");
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
}
