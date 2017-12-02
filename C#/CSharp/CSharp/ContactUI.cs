using System;
using MySql.Data.MySqlClient;

namespace CSharp
{
    class ContactUi
    {
        public static void Main(string[] args)
        {
            InitialScreen();
        }

        static void InitialScreen()
        {
            Console.WriteLine("*************************************************");
            Console.WriteLine("\tWelcome to your contacts viewer");
            Console.WriteLine("*************************************************");

            MySqlDb db = new MySqlDb();

            db.Connect();
         
            var sqlCommand = new MySqlCommand("SELECT * FROM Contact;", db.Connection);
            try
            {
                var resultsReader = sqlCommand.ExecuteReader();
                while (resultsReader.Read())
                {
                    string name = resultsReader.GetString("name");
                    Console.WriteLine(name);
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Exception caught: {0}", e);
            }
            finally
            {
                db.Close();
            }

            Console.ReadLine();
        }
    }
}
