using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    class MySqlDb : AbstractMySqlDbClass
    {
        public MySqlDb() : base(DbConfiguration.MySql_Hostname, DbConfiguration.MySql_DbName, DbConfiguration.MySql_Port, DbConfiguration.MySql_Username, DbConfiguration.MySql_Password)
        {
        }
    }
}
