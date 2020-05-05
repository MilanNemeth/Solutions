using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace ObjectBinaryStream
{
    class Person
    {
        public static string description = "Person class: use to create general person objects.";
        static public List<Person> People = new List<Person>();

        public string name;
        public DateTime dayOfBirth;
        public double weight;

        public int Age
        {
            get
            {
                TimeSpan age = DateTime.Now.Subtract(dayOfBirth);
                return (int)(age.TotalDays / 365.25);
            }
        }

        public Person(string name, DateTime dayOfBirth, double weight)
        {
            this.name = name;
            this.dayOfBirth = dayOfBirth;
            this.weight = weight;
        }

        public override string ToString()
        {
            return  $"Name:\t\t{this.name} \n" +
                    $"Date of Birth:\t{this.dayOfBirth.ToShortDateString()} \n" +
                    $"Weight:\t\t{this.weight} \n" +
                    $"Age:\t\t{this.Age} \n";
        }

        public void SavePerson(string fileName)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Append)))
            {
                writer.Write(this.name);
                writer.Write(this.dayOfBirth.ToBinary());
                writer.Write(this.weight);
            }
        }
        public static void LoadPeople(string fileName)
        {
            string name;
            DateTime dayOfBirth;
            double weight;
            if (File.Exists(fileName))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        name = reader.ReadString();
                        dayOfBirth = DateTime.FromBinary(reader.ReadInt64());
                        weight = reader.ReadDouble();
                        People.Add(new Person(name, dayOfBirth, weight));
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string dataBasePath = "PersonDB.bin";

            Person P1 = new Person("John Doe", new DateTime(2000, 1, 21), 81.5);
            Person P2 = new Person("Jane Doe", new DateTime(1985, 2, 13), 66.3);
            Person P3 = new Person("Pista", new DateTime(2000, 12, 30), 100.1);
            P1.SavePerson(dataBasePath);
            P2.SavePerson(dataBasePath);
            P3.SavePerson(dataBasePath);

            Person.LoadPeople(dataBasePath);
            Person.People.ForEach(i => Console.WriteLine(i));

            Console.ReadKey();
        }
    }
}
