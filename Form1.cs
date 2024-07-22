using System;
using System.Windows.Forms;
using static Music_School_Sql.Service.MusicSchoolService;
using Music_School_Sql.Model;

namespace Music_School_Xml
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreateXmlIfNotExists();
            // InsertClassroom("guitar jazz");
            // AddTeacher("guitar jazz", "yosi levi");
            // AddStudent("guitar jazz", "ofer badash", new Instrument("Guitar"));
            AddManyStudents("guitar jazz",
                new Student("itzik", new Instrument("guitar")),
                new Student("david", new Instrument("guitar")));
        }
    }
}
