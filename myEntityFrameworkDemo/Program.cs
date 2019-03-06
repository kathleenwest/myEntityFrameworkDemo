using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEntityFrameworkDemo
{
    class Program
    {

        // Comments: Global Fields for Program Class
        // Static and Private for Program Class
        // Did not use properties per design choice

        private static NWindEntities data;          // Entity Framework Model
        private static DbSet<Category> categories;  // Entity Database Set
        private static bool play;                   // Sentinal for Program Loop
        private static StringBuilder menu;          // Holds Menu String Object
        private static string choice;               // User Menu Choice

        static void Main(string[] args)
        {
            Task runDemo = Task.Run(() => RunDemo());
            Task.WaitAll(runDemo);
        } // end of Main method


        #region RunDemo
        private static void RunDemo()
        {
            try
            {

                // Create Entity Model Object
                data = new NWindEntities();
                categories = data.Categories;

                // Generate Main Menu StringBuilder Object
                menu = CreateMenu();

                // Set the entry for the program flow
                play = true;

                #region ConsoleAttributes
                Console.Title = "Entity Framework SQL Demo";
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                #endregion

                while (play)
                {
                    choice = String.Empty;

                    // Clear the Screen
                    Console.Clear();

                    // Write Menu
                    Console.WriteLine(menu);

                    // User Input
                    #region UserInput
                    while (choice == String.Empty)
                    {
                        // Read User Input
                        choice = Console.ReadLine();

                        switch (choice.ToUpper())
                        {
                            // Read Database
                            case "A":
                                ReadDatabase();
                                break;
                            // Add to Database
                            case "B":
                                AddRecord();
                                break;
                            // LINQ Query
                            case "C":
                                EFLinq();
                                break;
                            // Exit the Program
                            case "X":
                                play = false;
                                return;
                            default:
                                choice = String.Empty;
                                break;
                        }
                    }
                    #endregion

                    Console.ReadLine();
                }
            }

            catch (Exception ex)
            {
                // Catches a General Exception
                Console.WriteLine("General Error Message. Check connection string");
                Console.WriteLine("There has been an error and exception: {0}, {1}", ex.Message, ex.StackTrace);
                Console.WriteLine("The Demo program has terminated. Press any key");
                Console.Read();
            }

            finally

            {
                // Nothing Particular I want to go here 
            }
            
        }
        #endregion

        #region CreateMainMenu
        private static StringBuilder CreateMenu()
        {
            // Creates the Main Menu
            menu = new StringBuilder();
            menu.AppendLine("|============================================|");
            menu.AppendLine("|.......Entity Framework SQL Demo............|");
            menu.AppendLine("|============================================|");
            menu.AppendLine("|..........North Winds Database..............|");
            menu.AppendLine("|____________________________________________|");
            menu.AppendLine("| A.)...Read from Database...................|");
            menu.AppendLine("| B.)...Add to Database......................|");
            menu.AppendLine("| C.)...Query Database (LINQ)................|");
            menu.AppendLine("| X.)...Exit.................................|");
            menu.AppendLine("|____________________________________________|");
            return menu;
        }
        #endregion

        #region AddRecord
        private static void AddRecord()
        {
            string categoryName = String.Empty;
            string description = String.Empty;

            try
            {
                Console.WriteLine(new string('=', 20));
                Console.WriteLine("Add New Record: Categories");
                Console.WriteLine(new string('=', 20));

                while (String.IsNullOrEmpty(categoryName))
                {
                    Console.WriteLine("Enter a Category Name:");
                    categoryName = Console.ReadLine();
                }

                while (String.IsNullOrEmpty(description))
                {
                    Console.WriteLine("Enter Category Description:");
                    description = Console.ReadLine();
                }

                //Create New Category
                Category newCat = new Category()
                {
                    CategoryName = categoryName,
                    Description = description,
                    Picture = null
                };

                //Save New Category
                data.Categories.Add(newCat);
                data.SaveChanges();

                // User validation
                Console.WriteLine("Created a new record to Categories");
            }

            catch (Exception ex)
            {
                // Catches a General Exception
                Console.WriteLine("Database Write Error Message. Check connection string, database setup.");
                Console.WriteLine("There has been an error and exception: {0}, {1}", ex.Message, ex.StackTrace);
                Console.WriteLine("The request has been terminated. Press any key");
                Console.Read();
            }

            finally
            {
                    //nothing I want to do here yet
            }

        }
        #endregion

        #region ReadDatabase
        private static void ReadDatabase()
        {
            try
            {
                // READ Database    
                Console.WriteLine(new string('=', 20));
                Console.WriteLine("READ Database: Categories");
                Console.WriteLine(new string('=', 20));

                foreach (Category cat in categories)
                {
                    Console.WriteLine("{0,2} {1,-15} {2}", cat.CategoryID, cat.CategoryName, cat.Description);
                }
            }

            catch (Exception ex)
            {
                // Catches a General Exception
                Console.WriteLine("Database Read Error Message. Check connection string");
                Console.WriteLine("There has been an error and exception: {0}, {1}", ex.Message, ex.StackTrace);
                Console.WriteLine("The request has been terminated. Press any key");
                Console.Read();

            }

            finally
            {
                // Nothing I want to add here yet
            }
            
            

        }
        #endregion

        #region LINQQuery
        private static void EFLinq()
        {
            string categoryName = String.Empty;

            try
            {
                Console.WriteLine(new string('=', 20));
                Console.WriteLine("LINQ Query: Run");
                Console.WriteLine(new string('=', 20));

                while (String.IsNullOrEmpty(categoryName))
                {
                    Console.WriteLine("Enter a Category Name:");
                    categoryName = Console.ReadLine();
                }

                var query = from cat in data.Categories
                            where cat.CategoryName.Equals(categoryName)
                            select cat;

                var c = query.FirstOrDefault();
                if (c != null)
                {
                    Console.WriteLine("Category: {0} {1}", c.CategoryID, c.CategoryName);
                }

                foreach (var prod in c.Products)
                {
                    Console.WriteLine("{0:00000} {1}", prod.ProductID, prod.ProductName);
                }

            }

            catch (Exception ex)
            {
                // Catches a General Exception
                Console.WriteLine("Database Query Error. Check connection string.");
                Console.WriteLine("There has been an error and exception: {0}, {1}", ex.Message, ex.StackTrace);
                Console.WriteLine("The request has been terminated. Press any key");
                Console.Read();
            }

            finally
            {
                // Nothing I want to do here yet
            }
        }
        #endregion


    } // end of class Program
}
