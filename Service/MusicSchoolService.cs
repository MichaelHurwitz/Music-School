using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Music_School_Sql.Model;
using static Music_School_Sql.Configuration.MusicSchoolConfiguration;

namespace Music_School_Sql.Service
{
    internal static class MusicSchoolService
    {
        public static void mmmm() =>
            Console.WriteLine("mmmmm");

        public static void CreateXmlIfNotExists()
        {
            if (!File.Exists(musicSchoolPath))
            {
                // create new document (xml)
                XDocument document = new();
                // create an element
                XElement musicSchool = new("music-school");
                // document add element
                document.Add(musicSchool);
                // document save changes to provided path
                document.Save(musicSchoolPath);
            }
        }
        //פונקציה שמקבלת שם של סטודנט וכלי נגינה -> ומעדכנת את כלי הנגינה של הסטודנט FirstOrDefalt 
        // פונקציה שמקבלת שם של מורה ושם של מורה חדש -> ומעדכנת את שם המורה
        // פונקציה שמקבלת סטודנט ומחליפה אותו בסטודנט אחר לפי השם

        public static void UpdateInsrument(string studentName, Instrument newInstrument)
        {
            XDocument document = XDocument.Load(musicSchoolPath);
            XElement? student = document.Descendants("teacher")
                .FirstOrDefault(s => s.Attribute("name")?.Value == studentName);

            if (student != null)
            {
                student.Element("instrument")?.SetValue(newInstrument.Name);
                document.Save(musicSchoolPath);
            }
        }
        
        public static void UpdateTeacherName(string currentTeacherName, string newTeacherName)
        {
            XDocument document = XDocument.Load(musicSchoolPath);
            XElement? teacher = document.Descendants("teacher")
                .FirstOrDefault(t => t.Attribute("name")?.Value == currentTeacherName);

            if (teacher != null)
            {
                teacher.SetAttributeValue("teacher", newTeacherName);
                document.Save(musicSchoolPath);
            }
        }

        public static void ReplaceStudent(string classRoomName, Student oldStudent, Student newStudent)
        {
            XDocument document = XDocument.Load(musicSchoolPath);
            XElement? classRoom = document.Descendants("class-room")
                .FirstOrDefault(room => room.Attribute("name")?.Value == classRoomName);

            if (classRoom == null) return;

            XElement student = classRoom.Descendants("student")
                .FirstOrDefault(st => st.Attribute("name")?.Value == oldStudent.Name);

            if (student != null)
            {
                student.SetAttributeValue("name", newStudent.Name);
                student.Element("instrument")?.SetValue(newStudent.Instrument.Name);
                document.Save(musicSchoolPath);
            }
        }

        private static XElement ConvertStudentToElement(Student student) =>
            new XElement("teacher",
                new XAttribute("name", student.Name),
                new XElement("instrument", student.Instrument.Name)
            );

        public static void AddManyStudents(string classRoomName, params Student[] students)
        {
            XDocument document = XDocument.Load(musicSchoolPath);
            XElement? classRoom = document.Descendants("class-room")
                .FirstOrDefault(room => room.Attribute("name")?.Value == classRoomName);

            if (classRoom == null) return;

            List<XElement> studentList = students
                .Select(student => ConvertStudentToElement(student))
                .ToList();

            classRoom.Add(studentList);
            document.Save(musicSchoolPath);
        }

        public static void AddStudent(string classRoomName, string studentName, string instrumentName)
        {
            // Load document
            XDocument document = XDocument.Load(musicSchoolPath);
            // Find the specific class-room by attribute name
            XElement? classRoom = document.Descendants("class-room")
                .FirstOrDefault(room => room.Attribute("name")?.Value == classRoomName);

            if (classRoom == null) return;

            // Create XElement of instrument
            XElement instrument = new XElement("instrument", instrumentName);
            // Create XElement of teacher with attribute name
            XElement student = new XElement("teacher",
                new XAttribute("name", studentName),
                instrument);

            // Add teacher to the class-room
            classRoom.Add(student);
            // Save document
            document.Save(musicSchoolPath);
        }

        public static void AddTeacher(string classRoomName, string teacherName)
        {
            // Load document
            XDocument document = XDocument.Load(musicSchoolPath);

            // Find the specific class-room by classRoomName
            XElement? classRoom = document.Descendants("class-room")
                .FirstOrDefault(room => room.Attribute("name")?.Value == classRoomName);

            if (classRoom == null)
            {
                // Classroom not found, insert the classroom first
                InsertClassroom(classRoomName);
                // Reload the document to get the newly added classroom
                document = XDocument.Load(musicSchoolPath);
                classRoom = document.Descendants("class-room")
                    .FirstOrDefault(room => room.Attribute("name")?.Value == classRoomName);
            }

            if (classRoom != null)
            {
                // Check if teacher already exists in the class-room
                XElement? existingTeacher = classRoom.Descendants("teacher")
                    .FirstOrDefault(teacher => teacher.Attribute("name")?.Value == teacherName);
                if (existingTeacher != null)
                {
                    return;
                }

                // Create new XElement teacher with attribute name
                XElement teacher = new XElement("teacher",
                    new XAttribute("name", teacherName));

                // Add teacher to the class-room
                classRoom.Add(teacher);
                // Save document
                document.Save(musicSchoolPath);
            }
        }

        public static void InsertClassroom(string classRoomName)
        {
            // Load document
            XDocument document = XDocument.Load(musicSchoolPath);

            // Find music-school (root)
            XElement? musicSchool = document.Root;

            // Validate music-school exists
            if (musicSchool == null) return;

            // Create new XElement (class-room)
            XElement classRoom = new XElement("class-room",
                new XAttribute("name", classRoomName));

            // Add to music-school
            musicSchool.Add(classRoom);
            document.Save(musicSchoolPath);
        }
    }
}
