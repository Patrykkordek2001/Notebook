using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;


namespace Notebook
{
    class Notebook
    {

        public User User;
        public Note Note;
        public List<User> Users;
        public List<string> Notes = new List<string>();
        Dictionary<string, string> UsersWithNotes = new Dictionary<string, string>();


        SqlConnection sqlConnect = new SqlConnection("Server = DESKTOP-OOU4EPJ; Database=Notebook;Trusted_Connection=True;");
        

        
        


        public void Start()
        {
            while (true)
            {

                Note = new Note();
                Console.WriteLine("Choose operation:\n[1] - Add note\n[2] - Show note\n[3] - Remove note\n[4] - Exit\n[5] - Edit note");
                var choose = Console.ReadLine();
                switch (choose)
                {
                    case "1":


                        AddUser();
                        
                        //AddNote();
                        break;
                    case "2":
                        ShowNote();
                        break;
                    case "3":
                        AddNote();
                        break;
                    case "4":
                        AddNote();
                        break;
                    case "5":
                        System.Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Error! Choose correct operation.");
                        break;
                }


            }
        }
        public void AddUser()
        {
            
            SqlCommand cmd = new SqlCommand();

            var user = new User();
            Console.WriteLine("Enter your name:");
            user.Name = Console.ReadLine();
            Console.WriteLine("Enter your surname:");
            user.Surname = Console.ReadLine();
            Console.WriteLine("Enter your login:");
            user.Login = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            user.Password = Console.ReadLine();


            
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"INSERT into users (login, password, name, surname) VALUES (\'{user.Login}\', \'{user.Password}\', \'{user.Name}\', \'{user.Surname}\')";
            cmd.Connection = sqlConnect;

            sqlConnect.Open();
            cmd.ExecuteNonQuery();
            sqlConnect.Close();

          
        }
        public void AddNote()
        {
            SqlCommand cmd = new SqlCommand();
            //var user = new User();
            //Console.WriteLine("Enter your name:");
            //user.Name = Console.ReadLine();
            //Console.WriteLine("Enter your surname:");
            //user.Surname = Console.ReadLine();
            //Console.WriteLine("Enter your login:");
            //user.Login = Console.ReadLine();
            //Console.WriteLine("Enter your password:");
            //user.Password = Console.ReadLine();


            var Note = new Note();
            Console.WriteLine("Enter the title of the not: ");
            Note.Title = Console.ReadLine();
            Console.WriteLine("Enter the note:");
            Note.Content = Console.ReadLine();

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"INSERT into users (login, password, name, surname) VALUES (\'{user.Login}\', \'{user.Password}\', \'{user.Name}\', \'{user.Surname}\')";
            cmd.Connection = sqlConnect;

            sqlConnect.Open();
            cmd.ExecuteNonQuery();
            sqlConnect.Close();




        }

        public void ShowNote()
        {
            Console.WriteLine("Enter the Title of the note:");
            var searchedTitle = Console.ReadLine();
            foreach(var note in Notes)
            {
                    Console.WriteLine("\n" + "----------------------------");
                    Console.WriteLine(note);
                    Console.WriteLine("\n" + "----------------------------");

            }
        }

        public void EditNote()
        {

        }
    }
}


      

 