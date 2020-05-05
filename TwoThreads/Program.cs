using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwoThreads
{
    class Program
    {
        public static void ThreadLoop(ref uint counter)
        {
            counter = 0;
            while (true)
            {
                ++counter;
                Thread.Sleep(0);
            }
        }

        static void Main(string[] args)
        {
            uint counter = 0;
            Thread bkGndThread = new Thread(() => ThreadLoop(ref counter))
            {
                IsBackground = true
            };

            bkGndThread.Start();
            string word = Console.ReadLine();
            bkGndThread.Abort();
            Console.WriteLine($"The background thread iterated to \"{counter:N0}\" while you were typing \"{word}\".");
            Thread.Sleep(2500);
        }
    }
}
