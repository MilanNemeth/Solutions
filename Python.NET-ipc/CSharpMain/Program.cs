using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Linq;

namespace CSharpMain
{
    static class Program
    {
        const string localHost = "127.0.0.1";
        const int port = 50500;
        const ushort maxIteration = 1000;

        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.White;
            UdpClient RemoteUdpClient = new UdpClient(port);
            var RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(localHost), port);
            ushort counter = 0;
            List<long> SwTimes = new List<long>();

            Console.WriteLine($"Stopwatch frequency = {Stopwatch.Frequency}");
            var sw = new Stopwatch();
            sw.Start();

            long previous = sw.ElapsedMilliseconds;
            Console.WriteLine(previous + "\n");

            while (++counter < maxIteration)
            {
                try
                {
                    Byte[] receiveBytes = RemoteUdpClient.Receive(ref RemoteIpEndPoint);
                    string returnData = Encoding.UTF8.GetString(receiveBytes);
                    var information = JsonSerializer.Deserialize<float[]>(returnData);

                    Console.WriteLine(  "received message: \n" +
                                        "json: " + returnData.ToString() + "\n" +
                                        "after processing: ");
                    foreach (var item in information)
                    {
                        Console.Write(item+"\t");
                    }
                    Console.WriteLine("\n" +
                                        "From -> " +
                                                RemoteIpEndPoint.Address.ToString() +
                                                ":" +
                                                RemoteIpEndPoint.Port.ToString());
                    long ellapsed = sw.ElapsedMilliseconds;
                    Console.WriteLine(ellapsed);
                    var ell_prev = (previous == 0 ? 0 : ellapsed - previous);
                    Console.WriteLine("ellapsed till previous = " + ell_prev);
                    SwTimes.Add(ell_prev); 
                    Console.WriteLine();
                    if (ell_prev > 200)
                    {
                        Console.ForegroundColor = (Console.ForegroundColor == ConsoleColor.White ? ConsoleColor.Red : ConsoleColor.White);
                    }
                    previous = ellapsed;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    throw;
                }
            }
            Console.WriteLine($"Av:{SwTimes.Average()}, Min:{SwTimes.Min()}, Max:{SwTimes.Max()}");
        }
    }
}
