﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace University
{
    class Program
    {
        static void Main(string[] args)
        {
            UniversityCreator creator = new UniversityCreator();
            University university = creator.GetUniversity();
            IDBProvider provider = new XmlDBProvider();



            //foreach (Faculty facult in provider.GetFaculties())
            //    Console.WriteLine(facult);


            //foreach (Student stud in provider.GetStudents())
            //    Console.WriteLine(stud);

            // Console.WriteLine(new StreamReader("..\\..\\Resources\\Students.xml", Encoding.Unicode).ReadToEnd());
            provider.SaveStudent(new Student("tgrfw", "tegtgwfgh", 852, "qwertyuiop", new List<int>()),"1");
          
            Console.WriteLine();




        }
    }
}
