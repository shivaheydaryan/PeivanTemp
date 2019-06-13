using System.Data.Entity;
using ViewModel;

namespace Models
{
    public class DataContext:DbContext
    {
        public DataContext() { }



        
        public DbSet<User> Users { get; set; }

        public DbSet<Access> Accesses { get; set; }

        public DbSet<UserAccess> UsersAccess { get; set; }

        public DbSet<Institute> Institutes { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<CourseTotal> CourseTotals { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Institute_User> Institute_Users { get; set; }
        
        public DbSet<Class_User> Class_Users { get; set; }
        public DbSet<Term> Terms { get; set; }

        public DbSet<Institute_Term> Institute_Terms { get; set; }

        public DbSet<Institute_CourseTotal> Institute_CourseTotals { get; set; }

        public DbSet<Grade> Grades { get; set; }
        public DbSet<Institute_Grade> Institute_Grades { get; set; }

        public DbSet<StudentPersonalInfo> StudentPersonalInfo { get; set; }

        public DbSet<register_institute> register_institutes { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleUser> ArticleUsers { get; set; }
        //public DbSet<Institute_Manager_Teacher> Institute_Manager_Teachers { get; set; }

        //public DbSet<Institute_Manager_Class_Student> Institute_Manager_Class_Students { get; set; }




        //public DbSet<Exam> Exams { get; set; }
        //public DbSet<SPI> SPI { get; set; }

        //public DbSet<ICC> ICC { get; set; }

        //public DbSet<Course_STC> Course_STC { get; set; }
        

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Manager> Managers { get; set; }
        public DbSet<LessonPlan> LessonPlans { get; set; }

        //public DbSet<Register_Score> Register_Scores { get; set; }

        public DbSet<Type_Score> Type_Scores { get; set; }

        public DbSet<Quantitative_Score> Quantitative_Scores { get; set; }

        public DbSet<Qualitative_Score> Qualitative_Scores { get; set; }

        public DbSet<MentalState> MentalStates { get; set; }

        public DbSet<Roll> Rolls { get; set; }

        //public DbSet<Message_Content> Message_Contents { get; set; }

        //public DbSet<Message_Receiver> Message_Receivers { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Reminder> Reminders { get; set; }

        public DbSet<Rank> Ranks { get; set; }

        public DbSet<Homework> Homeworks { get; set; }

        public DbSet<Psychological_File> Psychological_Files { get; set; }

        public DbSet<Discipline_Item> Discipline_Items { get; set; }
        public DbSet<Encouragement_Punishment> Encouragement_Punishments { get; set; }

        public DbSet<Extracurricular> Extracurriculars { get; set; }

        public DbSet<Student_Extracurricular> Student_Extracurriculars { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<CityZone> CityZones { get; set; }

        public DbSet<HeaderSlider> HeaderSliders { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Note> Notes { get; set; }

        //public DbSet<Notification_Content> Notification_Contents { get; set; }

        public DbSet<Log> Logs { get; set; }

        
    }
}