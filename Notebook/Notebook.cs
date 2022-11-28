using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;



namespace Notebook
{
    class Notebook
    {
 
        public int IdDownloaded { get; set; }

        readonly SqlConnection sqlConnect = new SqlConnection("Server = DESKTOP-OOU4EPJ; Database=Notebook;Trusted_Connection=True;");

        public void Start()
        {
            while (true)
            {

                var Note = new Note();
                Console.WriteLine("Choose operation:\n[1] - Add user\n[2] - Login\n[3] - Exit\n");
                var choose = Console.ReadLine();
                switch (choose)
                {
                    case "1":
                        AddUser();
                        break;
                    case "2":
                        Login();
                        while (true)
                        {
                            LoginMenu();
                        }
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("ERROR! Enter the correct option");
                        break;
                }
            }
        }
        public void AddUser()
        {
            
            SqlCommand cmd = new SqlCommand();
            var User = new User();
            Console.WriteLine("Enter your name:");
            User.Name = Console.ReadLine();
            Console.WriteLine("Enter your surname:");
            User.Surname = Console.ReadLine();
            Console.WriteLine("Enter your login:");
            User.Login = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            User.Password = Console.ReadLine();
            
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"INSERT into users (login, password, name, surname) VALUES (\'{User.Login}\', \'{HashString(User.Password)}\', \'{User.Name}\', \'{User.Surname}\')";
            cmd.Connection = sqlConnect;
            sqlConnect.Open();
            if (cmd.ExecuteNonQuery() != 0) Console.WriteLine("User added");
            sqlConnect.Close();

          
        }
        public void AddNote(int id)
        {
            SqlCommand cmd = new SqlCommand();
            var Note = new Note();
            Console.WriteLine("Enter the title of the note: ");
            Note.Title = Console.ReadLine();
            Console.WriteLine("Enter the content:");
            Note.Content = Console.ReadLine();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"INSERT into notes (userid,title, content) VALUES (\'{id}\',\'{Note.Title}\', \'{Note.Content}\')";
            cmd.Connection = sqlConnect;
            sqlConnect.Open();
            if (cmd.ExecuteNonQuery() != 0) Console.WriteLine("Note added\n");
            sqlConnect.Close();

        }

        public void ShowNotes(int id)
        {
            var Note = new Note();
            SqlCommand cmd = new SqlCommand();
            Console.WriteLine("Your notes: \n");

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"SELECT * From notes WHERE  userid=\'{id}\'";
            cmd.Connection = sqlConnect;
            sqlConnect.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("\n");
                Note.NoteID = reader.GetInt32(3);
                Console.WriteLine("NoteID: " + Note.NoteID);
                Note.Title = reader.GetString(0);
                Console.WriteLine("Title: " + Note.Title);
                Note.Content = reader.GetString(1);
                Console.WriteLine("Content: " + Note.Content);
                Console.WriteLine("\n");
            }
  
            sqlConnect.Close();

        }
        

        public void DeleteNote(int id)
        {
            string tmp;
            SqlCommand cmd = new SqlCommand();
            var Note = new Note();
            Console.WriteLine("Enter noteid: ");
            tmp = Console.ReadLine();
            Note.NoteID = Convert.ToInt32(tmp);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"DELETE FROM notes " +
                              $"WHERE  noteid = \'{Note.NoteID}\' and userid = \'{id}\'";

            cmd.Connection = sqlConnect;
            sqlConnect.Open();
            if (cmd.ExecuteNonQuery() != 0) Console.WriteLine("Note deleted\n");
            sqlConnect.Close();
        }

        public int Login()
        {
            
            SqlCommand cmd = new SqlCommand();   
            var User = new User();
            Console.WriteLine("Enter your login:");
            User.Login = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            User.Password = Console.ReadLine(); 
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"SELECT userid From users " +
                              $"WHERE  password=\'{HashString(User.Password)}\' and login=\'{User.Login}\'";
            cmd.Connection = sqlConnect;
            sqlConnect.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                IdDownloaded = reader.GetInt32(0);
            }
            else
            { 
                Console.WriteLine("Incorrect password or login!");
                Environment.Exit(0);
            }
            
            sqlConnect.Close();
            return IdDownloaded;
        }
        public void EditNote(int id)
        {
            string tmp;
            SqlCommand cmd = new SqlCommand();
            var Note = new Note();
            Console.WriteLine("Choose noteid of note which you want edit: ");
            tmp = Console.ReadLine();
            Console.WriteLine("Set a new title: ");
            Note.Title = Console.ReadLine();
            Console.WriteLine("Write a new content of note: ");
            Note.Content = Console.ReadLine();
            Note.NoteID = Convert.ToInt32(tmp);

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"UPDATE notes " +
                              $"SET title = \'{Note.Title}\', content = \'{Note.Content}\' " +
                              $"WHERE  noteid = \'{Note.NoteID}\' and userid = \'{id}\'";
           
            cmd.Connection = sqlConnect;
            sqlConnect.Open();
            if (cmd.ExecuteNonQuery() != 0) Console.WriteLine("Note edited");
            sqlConnect.Close();
        }
        public void LoginMenu()
        {
            Console.WriteLine("Choose operation:\n[1] - Add Note\n[2] - Show notes\n[3] - Edit Note\n[4] - Delete Note\n[5] - Exit Program");
            string choose = Console.ReadLine();
            switch (choose)
            {
                case "1":
                    AddNote(IdDownloaded);
                    break;
                case "2":
                    ShowNotes(IdDownloaded);
                    break;
                case "3":
                    EditNote(IdDownloaded);
                    break;
                case "4":
                    DeleteNote(IdDownloaded);
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("ERROR! Bad option.");
                    break;

            }
        }

        public static string HashString(string input)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.Default.GetBytes(input);
            var hashedPassword = sha256.ComputeHash(bytes);
            var hashedPasswordInString = Convert.ToBase64String(hashedPassword);

            return hashedPasswordInString;
        }


    }
}


      

 