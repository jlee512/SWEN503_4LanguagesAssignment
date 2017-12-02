using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CSharp
{
    class AbstractMySqlDbClass
    {
        protected readonly string Host;
        protected readonly string Database;
        protected readonly int Port;
        protected readonly string Username;
        protected readonly string Password;
        //Difference in construction between C# and Java
        private MySqlConnection connection;
        public MySqlConnection Connection { get { return connection; } }

        public AbstractMySqlDbClass(string host, string database, int port, string username, string password)
        {
            Host = host;
            Database = database;
            Port = port;
            Username = username;
            Password = password;
        }

        public string ConnectionString()
        {
            return string.Format("Server={0}; database={1}; UID={2}; password={3}", Host, Database, Username, Password);
        }

        public bool Connect()
        {
            if (Connection == null)
            {
                connection = new MySqlConnection(ConnectionString());
                connection.Open();
            }
            return true;
        }

        public void Close()
        {
            connection.Close();
        }

    }
}
