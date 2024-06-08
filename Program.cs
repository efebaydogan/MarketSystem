using System;
using System.Data;
using System.Data.SqlClient;

namespace MarketSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string conString = "Your Connection String";
            SqlConnection connect = new SqlConnection(conString);

            EmployeeData ed = new EmployeeData();
            ProductData pd = new ProductData();
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
                RoleManagement();
                MainMenu();
            }



            Console.ReadKey();

            //FUNCTIONS
            void MainMenu()
            {
                if (ed.isLogin == true)
                {
                    if (ed.role == "Store Manager")
                    {
                        Console.Clear();
                        Console.WriteLine("As a store manager,what do you want to do? ");
                        Console.WriteLine("1 - Product Management\n2 - Employee Management");
                        int smInput = Convert.ToInt16(Console.ReadLine());

                        switch (smInput)
                        {
                            case 1:
                                ProductManagement();
                                break;
                            default:
                                Console.WriteLine("İnvalid Value");
                                break;

                        }
                    }
                }
            }
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

                cmdlogin.Parameters.AddWithValue("@un", ed.username);
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
                dr.Close();
            }
            void RoleManagement()
            {
                //In this function,we learning the what's the role of employee.So we can authorise them to do their jobs.
                if (ed.isLogin == true)
                {
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

                    try
                    {
                        string roleManage = "Select role from EmployeeTable where username = @username";
                        SqlCommand cmdrManage = new SqlCommand(roleManage, connect);

                        cmdrManage.Parameters.AddWithValue("@username", ed.username);

                        ed.role = Convert.ToString(cmdrManage.ExecuteScalar());

                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            Console.ReadKey();

            // Store Manager Functions

            void ProductManagement()
            {
                Console.Clear();
                string productInput;


                Console.WriteLine("Welcome to the product management panel.What do you want to do?");
                Console.WriteLine("1 - Add Product\n2 - Update Product\n3 - Delete Product\n4 - Show Products");
                productInput = Console.ReadLine();

                //Add Product
                if (productInput == "1")
                {
                    Console.Clear();
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

                    Console.WriteLine("What is your new product's name?");
                    pd.name = Console.ReadLine();
                    Console.WriteLine("What is your new product's category(Food,Cleaning,Personal Care,Baby,Home,Electronic,Clothes)");
                    pd.category = Console.ReadLine();
                    Console.WriteLine("What is the stock quantity of your new product?");
                    pd.stock = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("What is your new product's price?");
                    pd.price = Convert.ToInt32(Console.ReadLine());


                    string addProduct = "Insert into ProductTable (name,category,stock,price) values (@nm,@ct,@st,@pr)";
                    SqlCommand cmdaProduct = new SqlCommand(addProduct, connect);

                    cmdaProduct.Parameters.AddWithValue("@nm", pd.name);
                    cmdaProduct.Parameters.AddWithValue("@ct", pd.category);
                    cmdaProduct.Parameters.AddWithValue("@st", pd.stock);
                    cmdaProduct.Parameters.AddWithValue("@pr", pd.price);

                    Console.WriteLine("Successful!");
                    Console.ReadKey();
                    cmdaProduct.ExecuteNonQuery();
                    connect.Close();

                    MainMenu();
                }

                //Update Product
                else if (productInput == "2")
                {
                    int updateInput;
                    Console.Clear();

                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

                    Console.WriteLine("Which product you want to change.Type the ID.");
                    pd.ID = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("What changes do you want to make to this product?\n1 - Stock 2 - Price");
                    updateInput = Convert.ToInt32(Console.ReadLine());

                    if (updateInput == 1)
                    {
                        Console.Clear();
                        Console.WriteLine("How many do you want to make the stock");
                        pd.stock = Convert.ToInt32(Console.ReadLine());

                        string updateStock = "Update ProductTable set stock = @stock where ID = @id";
                        SqlCommand cmduStock = new SqlCommand(updateStock, connect);

                        cmduStock.Parameters.AddWithValue("@stock", pd.stock);
                        cmduStock.Parameters.AddWithValue("@id", pd.ID);


                        cmduStock.ExecuteNonQuery();
                        connect.Close();

                        Console.WriteLine("Successful.");
                        Console.ReadKey();
                        MainMenu();
                    }

                    else if (productInput == "2")
                    {
                        Console.Clear();
                        Console.WriteLine("What price do you want to update");
                        pd.price = Convert.ToInt32(Console.ReadLine());

                        string updatePrice = "Update ProductTable set price = @price where ID = @id";
                        SqlCommand cmduPrice = new SqlCommand(updatePrice, connect);

                        cmduPrice.Parameters.AddWithValue("@price", pd.price);
                        cmduPrice.Parameters.AddWithValue("@id", pd.ID);

                        cmduPrice.ExecuteNonQuery();
                        connect.Close();

                        Console.WriteLine("Successful.");
                        Console.ReadKey();
                        MainMenu();
                    }
                }

                //Delete Product
                else if (productInput == "3")
                {
                    Console.WriteLine("Which product you want to delete? Type the ID.");
                    pd.ID = Convert.ToInt32(Console.ReadLine());

                    string deleteProduct = "Delete from ProductTable where ID = @id";
                    SqlCommand cmddProduct = new SqlCommand(deleteProduct, connect);

                    cmddProduct.Parameters.AddWithValue("@id", pd.ID);

                    cmddProduct.ExecuteNonQuery();
                    connect.Close();

                    Console.WriteLine("Successful.");
                    Console.ReadKey();
                    MainMenu();
                }

                //Show Product
                else if (productInput == "4")
                {
                    Console.Clear();
                    string showProduct = "Select ID , name , stock , price from ProductTable";
                    SqlCommand cmdsProduct = new SqlCommand(showProduct, connect);

                    dr = cmdsProduct.ExecuteReader();

                    //Writing column headings
                    Console.WriteLine("{0, -10} | {1, -25} | {2, -10} | {3, -10}", "ID", "Name", "Stock", "Price");
                    Console.WriteLine(new string('-', 60));

                    while (dr.Read())
                    {
                        Console.WriteLine("{0, -10} | {1, -25} | {2, -10} | {3, -10}",
                        dr["ID"], dr["name"], dr["stock"], dr["price"]);

                        /*{0, -10}:
                        0: Specifies where dr[‘ID’] should be placed.
                        -10: Specifies that the value is left-aligned (the negative sign - indicates left alignment) and 10 characters width.
                        */
                    }

                    Console.WriteLine("\nPress any key to go main menu.");
                    Console.ReadKey();
                    MainMenu();
                }
            }
        }
    }
}
