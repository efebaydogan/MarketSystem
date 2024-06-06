using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string conString = "Data Source = DESKTOP-HJ43K02;Initial Catalog=MarketSystem;Integrated Security=True;TrustServerCertificate=True;Trusted_Connection=True";
            SqlConnection connect = new SqlConnection(conString);

            EmployeeData ed = new EmployeeData();
            SqlDataReader dr;

            Console.WriteLine("Hello! \nWelcome to the market system.");
            Console.WriteLine("If you are a new employee you need to sign up or you can just login.");
            Console.WriteLine("What do you want to do? \n1 - Sign up \n2 - Login ");

            string userInput = Console.ReadLine();

            if (userInput == "1")
            {
                Console.Clear();
                SignUpFunction();        
            }

            else if (userInput == "2")
            {
                Console.Clear();
                LoginFunction();
            }

            Console.ReadKey();

            //functions

            void SignUpFunction()
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }


                Console.WriteLine("Welcome to the sign up panel.");

                Console.WriteLine("First, what is your name?");
                ed.name = Console.ReadLine();
                Console.WriteLine("Specify a username.");
                ed.username = Console.ReadLine();
                Console.WriteLine("Set a password.");
                ed.password = Console.ReadLine();
                Console.WriteLine("What is your role? (Store Manager,Cashier,Janitor,Security Guard,");
                ed.role = Console.ReadLine();

                string signup = "Insert into EmployeeTable (username,name,password,role) values (@un,@n,@pw,@r)";
                SqlCommand cmdsignup = new SqlCommand(signup, connect);

                cmdsignup.Parameters.AddWithValue("@un", ed.username);
                cmdsignup.Parameters.AddWithValue("n", ed.name);
                cmdsignup.Parameters.AddWithValue("pw", ed.password);
                cmdsignup.Parameters.AddWithValue("r", ed.role);
                cmdsignup.ExecuteNonQuery();

                Console.WriteLine("Successful.");

                connect.Close();


            }
            void LoginFunction()
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                Console.WriteLine("Welcome to the login panel.");

                Console.WriteLine("What is your user name?");
                ed.username = Console.ReadLine();
                Console.WriteLine("What is your password?");
                ed.password = Console.ReadLine();

                string login = "Select * from EmployeeTable where username = @un and password = @p";
                SqlCommand cmdlogin = new SqlCommand(login, connect);

                cmdlogin.Parameters.AddWithValue("@un",ed.username);
                cmdlogin.Parameters.AddWithValue("@p", ed.password);

                dr = cmdlogin.ExecuteReader();

                if (dr.Read())
                {
                    Console.WriteLine("Your information is correct! You can log in successfuly.");
                    ed.isLogin = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Your information is incorrect! Try again..");
                    Console.ReadKey();
                    LoginFunction();
                }
            }
        }
    }
}