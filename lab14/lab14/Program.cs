using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;


namespace lab14
{
    [Serializable]
    public abstract partial class Movement
    {
        public string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int age;
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        public Movement(string Name, int Age)
        {
            this.Name = Name;
            this.Age = Age;
        }
        public Movement()
        {

        }
    }
    [Serializable]
    public class Person : Movement
    {
        public string Surname { get; set; }
        public Person()
        {

        }
        public Person(string Name, int Age, string Surname) : base(Name, Age)
        {
            this.Surname = Surname;
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            Person pers1 = new Person("Valera", 30, "Kachko");
            Person pers2 = new Person("Alexei", 20, "Ivanov");
            Person pers3 = new Person("Petr", 34, "Petrov");
            Person[] people = new Person[] { pers1, pers2, pers3 };


            BinaryFormatter formatter = new BinaryFormatter();//бинарная сериализация
            using (FileStream fs = new FileStream("person.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, pers1);
                Console.WriteLine("Сериализовали объект");
            }
            using (FileStream fs = new FileStream("person.dat", FileMode.OpenOrCreate))
            {
                Person newpers1 = (Person)formatter.Deserialize(fs);
                Console.WriteLine("Объект десериализован");

                Console.WriteLine($"{newpers1.ToString()} ");
            }

            SoapFormatter soap = new SoapFormatter();//Soap сериализация
            using (FileStream fs = new FileStream("person.soap", FileMode.OpenOrCreate))
            {
                soap.Serialize(fs, pers2);
                Console.WriteLine("Объект сериализован (SOAP)");
            }
            using (FileStream fs = new FileStream("person.soap", FileMode.OpenOrCreate))
            {
                Person newpers2 = (Person)soap.Deserialize(fs);

                Console.WriteLine("Объект десериализован(SOAP)");
                Console.WriteLine($"{newpers2.ToString()} ");
            }


            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Person));//Json сериализация
            using (FileStream fs = new FileStream("person.json", FileMode.OpenOrCreate))
            {
                js.WriteObject(fs, pers3);
                Console.WriteLine("Объект сериализован (JSON)");
            }
            using (FileStream fs = new FileStream("person.json", FileMode.OpenOrCreate))
            {
                Person newpers3 = (Person)js.ReadObject(fs);

                Console.WriteLine("Объект десериализован(Json)");
                Console.WriteLine($"{newpers3.ToString()} ");
            }



            XmlSerializer xml = new XmlSerializer(typeof(Person));//XML сериализация

            using (FileStream fs = new FileStream("person.xml", FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, pers3);
                Console.WriteLine("Объект сериализован (XML)");
            }

            using (FileStream fs = new FileStream("person.xml", FileMode.OpenOrCreate))
            {
                Person newpers4 = (Person)xml.Deserialize(fs);

                Console.WriteLine("Объект десериализован(XML)");
                Console.WriteLine($"{newpers4.ToString()} ");
            }



            XmlSerializer xmlarr = new XmlSerializer(typeof(Person[]));//сериализация массива
            using (FileStream fs = new FileStream("personArr.xml", FileMode.OpenOrCreate))
            {
                xmlarr.Serialize(fs, people);
                Console.WriteLine("Объект сериализован (XML)");
            }

            using (FileStream fs = new FileStream("personArr.xml", FileMode.OpenOrCreate))
            {
                Person[] newpeople = (Person[])xmlarr.Deserialize(fs);
                Console.WriteLine("Объект десериализован(XML)");
                Console.WriteLine($"{newpeople.ToString()} ");

            }


            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("14lab.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            XmlNodeList childnodes = xRoot.SelectNodes("*");//1 селектор
            foreach (XmlNode n in childnodes)
                Console.WriteLine(n.OuterXml);

            XmlNodeList students = xRoot.SelectNodes("//student/university");//2 селектор
            foreach (XmlNode s in students)
                Console.WriteLine(s.OuterXml);


            XDocument xdoc = new XDocument();//новый xml и запросы
            XElement model1 = new XElement("model");
            XAttribute modelName1 = new XAttribute("name", "Katya");
            XElement weight1 = new XElement("weight", "48");
            XElement height1 = new XElement("height", "180");
            model1.Add(modelName1);
            model1.Add(weight1);
            model1.Add(height1);
            XElement model2 = new XElement("model");
            XAttribute modelName2 = new XAttribute("name", "Masha");
            XElement weight2 = new XElement("weight", "50");
            XElement height2 = new XElement("height", "185");
            model2.Add(modelName2);
            model2.Add(weight2);
            model2.Add(height2);
            XElement models = new XElement("models");
            models.Add(model1);
            models.Add(model2);
            xdoc.Add(models);
            xdoc.Save("models.xml");
            var mdls = from xe in xdoc.Element("models").Elements("model")//1 запрос
                       where Convert.ToInt32(xe.Element("weight").Value) <= 50
                       select xe;
            foreach(var s in mdls)
                Console.WriteLine($"{s}");

            var items = from xs in xdoc.Element("models").Elements("model")//2 запрос
                        where xs.Attribute("name").Value == "Katya"
                        select xs;
            foreach (var n in items)
                Console.WriteLine($"{n}");
        }
    }
}
