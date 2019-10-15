﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace University
{
    public class XmlDBProvider : IDBProvider
    {
        private XmlReader reader = new XmlReader();

        
        const string adressesFilename = "Adress.xml";
        const string accountantsFilename = "Accountants.xml";
        const string autocadesFilename = "Autocades.xml";
        const string carsFilename = "Car.xml";
        const string dekansFilename = "Dekans.xml";
        const string employeesFilename = "Employees.xml";
        const string facultiesFilename = "Faculties.xml";
        const string garagesFilename = "Garages.xml";
        const string headsFilename = "Heads.xml";
        const string institutesFilename = "Institutes.xml";
        const string managersFilename = "Managers.xml";
        const string staffesFilename = "Staffes.xml";
        const string studentsFilename = "Students.xml";
        const string universitiesFilename = "Universities.xml";
        const string filesLocationPrefix = "..\\..\\Resources\\";

        ObjectConverter converter = new ObjectConverter();

        public XmlDBProvider()
        { 
            dbSt = LoadStudents();
            dbAd = LoadAdress();
            dbDek = LoadDekans();
            dbFc = LoadFaculties();

        }

        private List<DBStudent> LoadStudents()
        {
            XDocument xdoc = XDocument.Load(filesLocationPrefix + studentsFilename);
            List<DBStudent> students = new List<DBStudent>(from stud in xdoc.Element("students").Elements("student")
                                                           select new DBStudent
                                                            {
                                                                FirstName = stud.Element("firstName").Value,
                                                                SecondName = stud.Element("secondName").Value,
                                                                Departament = stud.Element("faculty").Value,
                                                                YearOfBirth = int.Parse(stud.Element("yearOfBirth").Value),
                                                                Marks = stud.Element("marks").Value,
                                                                FacultyID = int.Parse(stud.Element("facultID").Value)


                                                            });
            return students;
        }

        List<DBStudent> dbSt = new List<DBStudent>();

        private List<DBAdress> LoadAdress()
        {
            XDocument xdoc = XDocument.Load(filesLocationPrefix + adressesFilename);
            List<DBAdress> adress = new List<DBAdress>(from adr in xdoc.Element("adresses").Elements("adress")
                                                       select new DBAdress
                                                           {
                                                               CityAdr = adr.Element("cityAdr").Value,
                                                               StreetAdr = adr.Element("streetAdr").Value,
                                                               BuildingAdr = adr.Element("buildingAdr").Value,
                                                               AdressID = int.Parse(adr.Element("adressID").Value)

                                                           });
            return adress;
        }

        List<DBAdress> dbAd = new List<DBAdress>();

        private List<DBDekan> LoadDekans()
        {
            XDocument xdoc = XDocument.Load(filesLocationPrefix + dekansFilename);
            List<DBDekan> dekans = new List<DBDekan>(from dek in xdoc.Element("dekans").Elements("dekan")
                                                       select new DBDekan
                                                           {
                                                               FirstName = dek.Element("firstName").Value,
                                                               SecondName = dek.Element("secondName").Value,
                                                               Departament = dek.Element("faculty").Value,
                                                               YearOfBirth = int.Parse(dek.Element("yearOfBirth").Value),
                                                               Degree = dek.Element("degree").Value,
                                                               FacultyID = int.Parse(dek.Element("facultID").Value)


                                                           });
            return dekans;
        }

        List<DBDekan> dbDek = new List<DBDekan>();

        private List<DBFaculty> LoadFaculties()
        {
            XDocument xdoc = XDocument.Load(filesLocationPrefix + facultiesFilename);
            List<DBFaculty> faculties = new List<DBFaculty>(from facult in xdoc.Element("faculties").Elements("faculty")
                                                           select new DBFaculty
                                                           {
                                                               NameDepartament = facult.Element("name").Value,
                                                               UniversityName = facult.Element("universityName").Value,
                                                               AdressID = int.Parse(facult.Element("adressID").Value),
                                                               FacultyID = int.Parse(facult.Element("facultID").Value),
                                                               UniversityID = int.Parse(facult.Element("unID").Value)


                                                           });
            return faculties;
        }

        List<DBFaculty> dbFc = new List<DBFaculty>();




        private List<Dictionary<string, string>> ReadFromXml(string fileName, string nodeName, List<string> fieldsToLookFor)
        {
            return reader.Load(fileName, nodeName, fieldsToLookFor);
        }

        public List<Adress> GetAdresses()
        {
            List<Adress> adresses = new List<Adress>();

            foreach (DBAdress adr in dbAd)
            {
                adresses.Add(converter.Convert(adr));
            }

            return adresses;

        }

        private DBAdress GetAdressesByID(int ID)
        {
            return (from adr in dbAd
                                  where adr.AdressID == ID
                                  select adr).First();
        }

        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            foreach (DBStudent stud in dbSt)
            {
                students.Add(converter.Convert(stud));
            }
            
            return students;
        }

        private List<DBStudent> GetStudentsByFacultyID(int ID)
        {
             return new List<DBStudent> (from stud in dbSt
                                where stud.FacultyID == ID
                                select stud);
        }



        public List<Dekan> GetDekans()
        {
            
            List<Dekan> dekans = new List<Dekan>();
            
            foreach (DBDekan dek in dbDek)
            {
                dekans.Add(converter.Convert(dek));
            }

            return dekans;
        }

        public DBDekan GetDekanByFacultyID(int ID)
        {
            
           
               return (from dek in dbDek
                                  where dek.FacultyID == ID
                                  select dek).First();
        }

        public List<Faculty> GetFaculties()
        {
           
            List<Faculty> faculties = new List<Faculty>();

            foreach (DBFaculty facult in dbFc)
            {
                Faculty faculty = new Faculty(facult.NameDepartament, converter.Convert(GetAdressesByID(facult.AdressID)),
                    facult.UniversityName, converter.Convert(GetDekanByFacultyID(facult.FacultyID)));
                
                foreach (DBStudent st in GetStudentsByFacultyID(facult.FacultyID))
                {
                    faculty.AddStudent(converter.Convert(st));
                }

                faculties.Add(faculty);
            }

            return faculties;
        }

        public DBFaculty GetFacultyByUniversityID(int ID)
        {

            return (from fac in dbFc
                    where fac.UniversityID == ID
                    select fac).First();
        }

        //public List<Garage> GetGarages()
        //{
        //    List<Dictionary<string, string>> parsedGarages = ReadFromXml(garagesFilename, "garage", new List<string> { "square", "number", "adressID" });
        //    List<Garage> garages = new List<Garage>();

        //    foreach (Dictionary<string, string> garageValues in parsedGarages)
        //    {
        //        Adress adr = this.GetAdressesByID(garageValues["adressID"]);
        //        garages.Add(new Garage(adr, float.Parse(garageValues["square"]), int.Parse(garageValues["number"])));
        //    }

        //    return garages;
        //}

        //public List<Car> GetCars()
        //{
        //    List<Dictionary<string, string>> parsedCars = ReadFromXml(carsFilename, "car", new List<string> { "name", "year", "fuel" });
        //    List<Car> cars = new List<Car>();

        //    foreach (Dictionary<string, string> carValues in parsedCars)
        //    {
        //        cars.Add(new Car(carValues["name"], int.Parse(carValues["year"]), carValues["fuel"]));
        //    }

        //    return cars;
        //}

        //public List<Head> GetHeads()
        //{
        //    List<Dictionary<string, string>> parsedHeads = ReadFromXml(headsFilename, "head", new List<string> { "firstName", "secondName", "yearOfBirth", "degree", "faculty" });
        //    List<Head> heads = new List<Head>();

        //    foreach (Dictionary<string, string> headValues in parsedHeads)
        //    {
        //        heads.Add(new Head(headValues["firstName"], headValues["secondName"], int.Parse(headValues["yearOfBirth"]), headValues["faculty"], headValues["degree"]));
        //    }

        //    return heads;
        //}

        //public Dekan GetDekanByID(string ID)
        //{
        //    List<Dictionary<string, string>> parsedDekans = ReadFromXml(dekansFilename, "dekan", new List<string> { "firstName", "secondName", "yearOfBirth", "degree", "faculty", "facultID" });

        //    foreach (Dictionary<string, string> dekanValues in parsedDekans)
        //    {
        //        if (ID.Equals(dekanValues["facultID"]))
        //            return new Dekan(dekanValues["firstName"], dekanValues["secondName"], int.Parse(dekanValues["yearOfBirth"]), dekanValues["faculty"], dekanValues["degree"]);
        //    }

        //    return null;
        //}























    }
}
