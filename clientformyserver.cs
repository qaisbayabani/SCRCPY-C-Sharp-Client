using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.IO;
using OpenCvSharp;
using System.Linq;
using System.Text;




class Program
{


    public static void Main()
    {

        ForFun fun = new ForFun();


        Thread ti = new Thread(new ThreadStart(() => fun.Fun()));
        ti.Start();


        
    }



    class ForFun
    {
        public void Fun()
        {



            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            Console.WriteLine(hostName);
            // Get the IP
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            Console.WriteLine("My IP Address is :" + myIP);

            TcpListener server = null;
            TcpListener server2 = null;

            Int32 port = 27183;
            IPAddress localAddr = IPAddress.Parse(myIP);
            server = new TcpListener(localAddr, port);
            TcpClient client = null;
            NetworkStream stream = null;

            Int32 port2 = 27184;
            IPAddress localAddr2 = IPAddress.Parse(myIP);
            server2 = new TcpListener(localAddr2, port2);
            TcpClient client2 = null;
            NetworkStream stream2 = null;

            byte[] bb4 = new byte[12];

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



                    Console.Write("Waiting for a connection Server No.2---------- ");


                    Thread ti3 = new Thread(new ThreadStart(() => ForFun3.Fun3())); ti3.Start();

                    client2 = server2.AcceptTcpClient();
                    Console.WriteLine("Connected!========>2");
                    
                    stream2 = client2.GetStream();
                    BufferedStream bf2 = new BufferedStream(stream2);


                    while (true)
                    {
                        while (client.Connected)
                        {
                            if (bf.Read(bb4, 0, bb4.Length) > -1)
                            {
                                bf2.Write(bb4, 0, bb4.Length);
                            }
                        }



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

            if (stream != null)
            {

                stream.Flush();
                stream.Close();
                stream2.Flush();
                stream2.Close();
                client.Close();
                client2.Close();
                server.Stop();
                server2.Stop();

            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();

        }
    }

    class ForFun3
    {

        public static void Fun3()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            Console.WriteLine(hostName);
            
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            

            VideoCapture capture = new VideoCapture("tcp://"+myIP+":27184");


            Mat image = new Mat();

            Mat image2 = new Mat();

            Mat image3 = new Mat();

            while (true)
            {

                image = capture.RetrieveMat();

                Size s = new Size(image.Cols*0.650, image.Rows*0.650);
 
                //rotateImage(image, image2, 0, 1);

                //Cv2.SelectROI("RO",image);

 //                 Cv2.Flip(image, image2, FlipMode.Y);
 
                
                  Cv2.Resize(image, image2, s);
                
                try
                {

                    Cv2.ImShow("window", image2);

                    //  Thread.Sleep(1);

                }
                catch (Exception e) { }

                Cv2.WaitKey(1);


            }
        }
    }

}
