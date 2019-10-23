using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.SQLServer
{
    public class DBSQLServerUtils
    {
        public static SqlConnection
                GetDBConnection(String connectionString)
        {
            //
            // Data Source=TRAN-VMWARE\SQLEXPRESS;Initial Catalog=simplehr;Persist Security Info=True;User ID=sa;Password=12345
            //

            SqlConnection conn = new SqlConnection(connectionString);

            return conn;
        }
    }
}
