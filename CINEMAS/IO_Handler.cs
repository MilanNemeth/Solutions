using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace Cinemas
{
    static class IO_Handler
    {
        public static void LogItsCaller()
        {
            StackFrame frame = new StackFrame(1);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"({frame.GetMethod().DeclaringType.Name}.{frame.GetMethod().Name}) has been called successfully!");
            Console.ResetColor();
        }
        public static string EnterString(string message = "")
        {
            string result = "";
            while (result.Length<1)
            {
                Console.Write(message);
                result = Console.ReadLine();
            }
            return result;
        }
        public static byte EnterByte(string message = "")
        {
            byte result;
            Console.Write(message);
            while (!byte.TryParse(Console.ReadLine(), out result))
            {
                ErrorMessage("Incorrect value!\nTry again in range of [0-255].");
                Console.Write(message);
            }
            return result;
        }
        public static void SaveToFile(string[] stringsToSave)
        {
            string filename = "";
            try
            {
                filename = EnterString("Saving to file, please enter the filepath/filename: ");
                FileStream fs = new FileStream(@filename, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                for (int i = 0; i < stringsToSave.Length; i++)
                {
                    sw.WriteLine(stringsToSave[i]);
                }
                sw.Flush();
                fs.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                ErrorMessage("File has NOT been saved!\n" + e.Message);
                throw e;
            }
            SuccessMessage($"File has been saved!\n({filename})");
        }
        public static void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
            System.Threading.Thread.Sleep(1500);
        }
        public static void SuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message);
            Console.ResetColor();
            System.Threading.Thread.Sleep(1500);
        }
        public static void PrintCollection<T>(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }   //Not implemented in PresentationLayer, so become obsolete:
    }
}
    

