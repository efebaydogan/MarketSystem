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
            ExpenseData exd = new ExpenseData();
            SalesData sd = new SalesData();
            SqlDataReader dr;

            Console.WriteLine("Hello! \nWelcome to the market system.");
            Console.WriteLine("If you are a new employee you need to take your username and password from store manager.Or you can just login.");
            Console.WriteLine("İf you know username and password , press 1.");

            string userInput = Console.ReadLine();

            if (userInput == "1")
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
                        Console.WriteLine("As a Store Manager,what do you want to do?");
                        Console.WriteLine("1 - Product Management\n2 - Employee Management\n3 - Income and Expense");
                        int smInput = Convert.ToInt16(Console.ReadLine());

                        switch (smInput)
                        {
                            case 1:
                                ProductManagement();
                                break;
                            case 2:
                                EmployeeManagement();
                                break;
                            case 3:
                                IncomeExpense();
                                break;
                            default:
                                Console.WriteLine("İnvalid Value");
                                break;

                        }
                    }

                    if (ed.role == "Cashier")
                    {
                        Console.Clear();
                        Console.WriteLine("As a Cashier,what do you want to do?");
                        Console.WriteLine("1 - Start the day\n2 - Show the ID of the products.");

                        int cInput = Convert.ToInt16(Console.ReadLine());

                        switch (cInput)
                        {
                            case 1:
                                Sales();
                                break;

                            default:
                                Console.WriteLine("İnvalid Value");
                                break;
                        }
                    }
                }
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

                    else if (updateInput == 2)
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
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

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
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

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

            void EmployeeManagement()
            {
                Console.Clear();

                string employeeInput;

                Console.WriteLine("Welcome to the employee management panel.What do you want to do?");
                Console.WriteLine("1 - Add Employee\n2 - Update Employee\n3 - Delete Employee\n4 - Show Employees");
                employeeInput = Console.ReadLine();

                //Add Employee 
                if (employeeInput == "1")
                {
                    Console.Clear();

                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

                    Console.WriteLine("First, what is your new employee's name?");
                    ed.name = Console.ReadLine();
                    Console.WriteLine("Specify a username for your new employee.");
                    ed.username = Console.ReadLine();
                    Console.WriteLine("Set a password for new employee.");
                    ed.password = Console.ReadLine();
                    Console.WriteLine("What is your employee's role? (Store Manager,Cashier,Janitor,Security Guard,");
                    ed.role = Console.ReadLine();

                    string signup = "Insert into EmployeeTable (username,name,password,role) values (@un,@n,@pw,@r)";
                    SqlCommand cmdsignup = new SqlCommand(signup, connect);

                    cmdsignup.Parameters.AddWithValue("@un", ed.username);
                    cmdsignup.Parameters.AddWithValue("n", ed.name);
                    cmdsignup.Parameters.AddWithValue("pw", ed.password);
                    cmdsignup.Parameters.AddWithValue("r", ed.role);
                    cmdsignup.ExecuteNonQuery();

                    connect.Close();
                    Console.WriteLine("Successful.");
                    Console.ReadKey();
                    MainMenu();
                }

                //Update Employee
                else if (employeeInput == "2")
                {
                    Console.Clear();

                    int updateInput;

                    Console.WriteLine("Which employee's information do you want to change? Type the ID.");
                    ed.ID = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("What changes do you want to make about your employee?\n1 - Password 2 - Role");
                    updateInput = Convert.ToInt16(Console.ReadLine());

                    if (updateInput == 1)
                    {
                        if (connect.State == ConnectionState.Closed)
                        {
                            connect.Open();
                        }

                        Console.Clear();

                        Console.WriteLine("Type the new password.");
                        ed.password = Console.ReadLine();

                        string updatepEmployee = "Update EmployeeTable set password = @pw where ID = @id";
                        SqlCommand cmduEmployee = new SqlCommand(updatepEmployee, connect);

                        cmduEmployee.Parameters.AddWithValue("pw", ed.password);
                        cmduEmployee.Parameters.AddWithValue("id", ed.ID);

                        cmduEmployee.ExecuteNonQuery();

                        connect.Close();
                        Console.WriteLine("Successful.");
                        Console.ReadKey();
                        MainMenu();
                    }

                    else if (updateInput == 2)
                    {
                        if (connect.State == ConnectionState.Closed)
                        {
                            connect.Open();
                        }

                        Console.WriteLine("Type the new role.");
                        ed.role = Console.ReadLine();

                        Console.Clear();
                        string updaterEmployee = "Update EmployeeTable set role = @role where ID = @id";
                        SqlCommand cmduEmployee = new SqlCommand(updaterEmployee, connect);

                        cmduEmployee.Parameters.AddWithValue("@role", ed.role);
                        cmduEmployee.Parameters.AddWithValue("id", ed.ID);

                        cmduEmployee.ExecuteNonQuery();

                        connect.Close();
                        Console.WriteLine("Successful.");
                        Console.ReadKey();
                        MainMenu();

                    }
                }

                //Delete Employee
                else if (employeeInput == "3")
                {
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

                    Console.Clear();
                    Console.WriteLine("Which employee do want to delete ? Type the id.");
                    ed.ID = Convert.ToInt32(Console.ReadLine());

                    string deleteEmployee = "Delete from EmployeeTable where ID = @id";
                    SqlCommand cmddEmployee = new SqlCommand(deleteEmployee, connect);

                    cmddEmployee.Parameters.AddWithValue("id", ed.ID);

                    cmddEmployee.ExecuteNonQuery();

                    Console.WriteLine("Successful.");
                    Console.ReadKey();
                    MainMenu();
                }

                //Show Employee
                else if (employeeInput == "4")
                {
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

                    Console.Clear();

                    string showEmployee = "Select * from EmployeeTable";
                    SqlCommand cmduEmployee = new SqlCommand(showEmployee, connect);

                    dr = cmduEmployee.ExecuteReader();

                    Console.WriteLine("{0,-10} | {1,-20} | {2,-20} | {3,-20}", "ID", "Username", "Name", "Role");
                    Console.WriteLine(new String('-', 75));

                    while (dr.Read())
                    {
                        Console.WriteLine("{0,-10} | {1,-20} | {2,-20} | {3,-20}", dr["ID"], dr["username"], dr["name"], dr["role"]);
                    }

                    Console.WriteLine("\nSuccessful.");
                    Console.ReadKey();
                    MainMenu();

                }
            }

            void IncomeExpense()
            {
                Console.Clear();

                int ieInput;
                Console.WriteLine("Which financial function do you want to see?\n1 - Income   2 - Expense");
                ieInput = Convert.ToInt16(Console.ReadLine());

                if (ieInput == 1)
                {
                    Console.Clear();

                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

                    string showIncome = "Select ID , name , stock , price from ProductTable";
                    string sumIncome = "Select sum(price) from ProductTable";
                    SqlCommand cmdsIncome = new SqlCommand(showIncome, connect);
                    SqlCommand cmdsumIncome = new SqlCommand(sumIncome, connect);

                    int total = Convert.ToInt32(cmdsumIncome.ExecuteScalar());
                    dr = cmdsIncome.ExecuteReader();

                    Console.WriteLine("{0, -10} | {1, -25} | {2, -10} | {3, -10}", "ID", "Name", "Stock", "Price");
                    Console.WriteLine(new string('-', 60));

                    while (dr.Read())
                    {
                        Console.WriteLine("{0, -10} | {1, -25} | {2, -10} | {3, -10}",
                        dr["ID"], dr["name"], dr["stock"], dr["price"]);
                    }

                    Console.WriteLine("\n" + "Summary : " + total);

                    Console.WriteLine("\nPress any key to go main menu.");
                    Console.ReadKey();
                    MainMenu();
                }

                else if (ieInput == 2)
                {
                    Console.Clear();
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }

                    int expenseInput;
                    Console.WriteLine("What do you want to do abount your expenses?");
                    Console.WriteLine("1 - Add Expense\n2 - Update expense\n3 - Delete Expense\n4 - Show expense");
                    expenseInput = Convert.ToInt16(Console.ReadLine());

                    //Add Expense
                    if (expenseInput == 1)
                    {
                        Console.Clear();

                        Console.WriteLine("Type your expense's name.");
                        exd.name = Console.ReadLine();
                        Console.WriteLine("Type your expense's type.(bill,salary,taxes,repairs)");
                        exd.type = Console.ReadLine();
                        Console.WriteLine("Type your expense's amount.");
                        exd.amount = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Type your expense's date.(YYYY-MM-DD)");
                        exd.date = Console.ReadLine();

                        string addExpense = "Insert into ExpenseTable (name,type,amount,date) values (@name,@type,@amount,@date)";
                        SqlCommand cmdaExpense = new SqlCommand(addExpense, connect);

                        cmdaExpense.Parameters.AddWithValue("@name", exd.name);
                        cmdaExpense.Parameters.AddWithValue("@type", exd.type);
                        cmdaExpense.Parameters.AddWithValue("@amount", exd.amount);
                        cmdaExpense.Parameters.AddWithValue("@date", exd.date);

                        cmdaExpense.ExecuteNonQuery();

                        Console.WriteLine("Successful!");
                        Console.ReadKey();
                        MainMenu();
                    }

                    //Update Expense
                    else if (expenseInput == 2)
                    {

                        Console.Clear();
                        string updateInput;
                        Console.WriteLine("Which expense do you want to change? Type the ID.");
                        exd.ID = Convert.ToInt16(Console.ReadLine());
                        Console.WriteLine("What do you want to change about this expense?\n1 - Amount 2 - Date");
                        updateInput = Console.ReadLine();

                        if (updateInput == "1")
                        {
                            Console.Clear();
                            Console.WriteLine("Write the new amount.");
                            exd.amount = Convert.ToInt32(Console.ReadLine());
                            string updateaExpense = "Update ExpenseTable set amount = @amount where ID = @id";
                            SqlCommand cmduExpense1 = new SqlCommand(updateaExpense, connect);

                            cmduExpense1.Parameters.AddWithValue("@amount", exd.amount);
                            cmduExpense1.Parameters.AddWithValue("@id", exd.ID);

                            cmduExpense1.ExecuteNonQuery();

                            Console.WriteLine("Successful.");
                            Console.WriteLine("\nPress any key to go main menu.");
                            Console.ReadKey();
                            MainMenu();

                        }

                        else if (updateInput == "2")
                        {
                            Console.Clear();
                            Console.WriteLine("Write the new date.");
                            exd.date = Console.ReadLine();

                            string updatedExpense = "Update ExpenseTable set date = @date where ID = @id";
                            SqlCommand cmduExpense2 = new SqlCommand(updatedExpense, connect);

                            cmduExpense2.Parameters.AddWithValue("@date", exd.date);
                            cmduExpense2.Parameters.AddWithValue("@id", exd.ID);

                            cmduExpense2.ExecuteNonQuery();

                            Console.WriteLine("Successful.");
                            Console.WriteLine("\nPress any key to go main menu.");
                            Console.ReadKey();
                            MainMenu();

                        }
                    }

                    //Delete Expense
                    else if (expenseInput == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Which expense do you want to delete? Type the ID.");
                        exd.ID = Convert.ToInt32(Console.ReadLine());

                        string deleteExpense = "Delete from ExpenseTable where ID = @id";
                        SqlCommand cmddExpense = new SqlCommand(deleteExpense, connect);

                        cmddExpense.Parameters.AddWithValue("@id", exd.ID);

                        cmddExpense.ExecuteNonQuery();

                        Console.WriteLine("Successful.");
                        Console.WriteLine("\nPress any key to go main menu.");
                        Console.ReadKey();
                        MainMenu();

                    }

                    //Show Expense
                    else if (expenseInput == 4)
                    {
                        Console.Clear();
                        string showExpense = "Select ID,name,type,amount,date from ExpenseTable";
                        string sumExpense = "Select sum(amount) from ExpenseTable";
                        SqlCommand cmdsExpense = new SqlCommand(showExpense, connect);
                        SqlCommand cmdsumExpense = new SqlCommand(sumExpense, connect);

                        dr = cmdsExpense.ExecuteReader();
                        int total = Convert.ToInt32(cmdsumExpense.ExecuteScalar());

                        Console.WriteLine("{0,-10} | {1,-15} | {2,-15} | {3,-15} | ", "ID", "Name", "Type", "Amount");
                        Console.WriteLine(new string('-', 65));

                        while (dr.Read())
                        {
                            Console.WriteLine("{0,-10} | {1,-15} | {2,-15} | {3,-15} | ", dr["ID"], dr["name"], dr["type"], dr["amount"]);
                        }

                        Console.WriteLine("\nSummary : " + total);

                        Console.WriteLine("\nSuccessful.");
                        Console.ReadKey();
                        MainMenu();

                    }
                }
            }

            //Cashier Functions

            void Sales()
            {
                Console.Clear();

                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                //We will create a Table named SalesTable.We create this for calculate the total easily.
                //In this function,first,we will take data from ProductTable.Then we will insert datas to new table.Then We will update new stocks(-1).Then we will calculate the total.Then we will delete the datas in the SalesTable.And we repeat this process.

                while (true)
                {
                    Console.WriteLine("Enter the product's ID.");
                    pd.ID = Convert.ToInt32(Console.ReadLine());

                    if (pd.ID == 101)
                    {
                        break;
                    }

                    string selectProduct = "Select name,price,stock from ProductTable where ID = @id";
                    string insertsTable = "Insert into SalesTable (ID,name,price) values (@id,@name,@price)";
                    string updateStock = "Update ProductTable set stock = @stock where ID = @id";
                    string sumPrice = "Select sum(price) from SalesTable";
                    string deleteSales = "Delete from SalesTable";

                    SqlCommand cmdsProduct = new SqlCommand(selectProduct, connect);
                    SqlCommand cmdiTable = new SqlCommand(insertsTable, connect);
                    SqlCommand cmduStock = new SqlCommand(updateStock, connect);
                    SqlCommand cmdsPrice = new SqlCommand(sumPrice, connect);
                    SqlCommand cmddSales = new SqlCommand(deleteSales, connect);

                    cmdsProduct.Parameters.AddWithValue("@id", pd.ID);
                    dr = cmdsProduct.ExecuteReader();

                    if (dr.Read())
                    {
                        pd.name = Convert.ToString(dr["name"]);
                        pd.price = Convert.ToInt32(dr["price"]);
                        pd.stock = Convert.ToInt32(dr["stock"]);
                    }

                    pd.stock--;
                    dr.Close();

                    if (pd.ID != 00)
                    {
                        cmdiTable.Parameters.AddWithValue("@id", pd.ID);
                        cmdiTable.Parameters.AddWithValue("@name", pd.name);
                        cmdiTable.Parameters.AddWithValue("@price", pd.price);

                        cmduStock.Parameters.AddWithValue("@stock", pd.stock);
                        cmduStock.Parameters.AddWithValue("@id", pd.ID);

                        cmdiTable.ExecuteNonQuery();
                        cmduStock.ExecuteNonQuery();

                        pd.total = Convert.ToInt32(cmdsPrice.ExecuteScalar());
                    }

                    else if (pd.ID == 0)
                    {
                        Console.WriteLine("Total : " + pd.total);

                        cmddSales.ExecuteNonQuery();

                        Console.WriteLine("Finished! Press enter to go to new customer ");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                }

                MainMenu();

            }
        }
    }
}
