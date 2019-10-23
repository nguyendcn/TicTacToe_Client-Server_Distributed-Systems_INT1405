using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.SQLServer
{
    public class DBUtils
    {
        public static SqlConnection GetDBConnection()
        {

            return DBSQLServerUtils.GetDBConnection("Data Source=dcn98.database.windows.net;Initial Catalog=Tesmp;User ID=nguyenne; Password=Bogia1998");
        }
    }
}
