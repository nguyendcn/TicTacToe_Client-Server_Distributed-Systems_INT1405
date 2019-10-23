using DCN.TicTacToe.Server;
using DCN.TicTacToe.Shared.SQLServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCN.TicTacToe.UI.Server
{
    public partial class Frm_Index : Form
    {
        private DCN.TicTacToe.Server.Server server;
        private Timer updateListTimer;
        private Timer autoSendData;
        private List<int> listDataByTime;

        private SqlConnection sqlConnection;


        public Frm_Index()
        {
            server = new TicTacToe.Server.Server(9999);
            InitializeComponent();

            RegisterEvents();

            updateListTimer = new Timer();
            updateListTimer.Interval = 1000;
            updateListTimer.Tick += UpdateListTimer_Tick;
            updateListTimer.Start();

            autoSendData = new Timer();
            autoSendData.Interval = 1000;
            autoSendData.Tick += AutoSendData_Tick;
            listDataByTime = new List<int>();

            try
            {
                sqlConnection = DBUtils.GetDBConnection();
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Can not open connection.");
            }
           
        }

        private void DeleteAllRow()
        {
            try
            {
                string sql = "Delete from tbl_online";

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = sqlConnection;
                cmd.CommandText = sql;

                int rowCount = cmd.ExecuteNonQuery();

                Debug.WriteLine("Row Count delete affected = " + rowCount);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception);
                Console.WriteLine(exception.StackTrace);
            }
            finally
            {
            //    sqlConnection.Close();
            //    sqlConnection.Dispose();
            //    sqlConnection = null;
            }
        }

        private void UpdateListOnline()
        {
            if(listDataByTime.Count >= 30)
                listDataByTime.RemoveAt(0);
            listDataByTime.Add(GetQuantityConnect());
        }

        private void UpdateToDataBase()
        {
            try
            {
                SqlCommand cmd = sqlConnection.CreateCommand();

                string sql_1 = "Insert into tbl_online (Id, Quantity) values (";
                string sql_2 = ");  ";
                string sql = "";

                for (int i = 0;i < listDataByTime.Count; i++)
                {
                    sql += sql_1 + (i + 1) + ", " + listDataByTime[i] + sql_2;
                }
                // Câu lệnh Insert.
                

                cmd.CommandText = sql;

                // Thực thi Command (Dùng cho delete, insert, update).
                int rowCount = cmd.ExecuteNonQuery();

                Console.WriteLine("Row Count add affected = " + rowCount);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                //sqlConnection.Close();
                //sqlConnection.Dispose();
                //sqlConnection = null;
            }
        }

        private void AutoSendData_Tick(object sender, EventArgs e)
        {
            DeleteAllRow();

            UpdateListOnline();

            UpdateToDataBase();
        }

        private void RegisterEvents()
        {
            //this
            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;
            this.Load += ServerForm_Load;

            //Server
            server.ClientValidating += server_ClientValidating;
        }

        void ServerForm_Load(object sender, EventArgs e)
        {
            btnStart.PerformClick();
        }

        void server_ClientValidating(TicTacToe.Server.EventArguments.ClientValidatingEventArgs args)
        {
            if (!server.Receivers.Exists(x => x.Email == args.Request.Email))
            {
                args.Confirm();
            }
            else
            {
                args.Refuse();
            }
        }

        private void UpdateListTimer_Tick(object sender, EventArgs e)
        {
            UpdateClientsList();
        }

        private void InvokeUI(Action action)
        {
            this.Invoke(action);
        }

        private void UpdateClientsList()
        {
            InvokeUI(() =>
            {
                listClients.Items.Clear();

                foreach (var receiver in server.Receivers)
                {
                    String[] str = new String[5];
                    str[0] = receiver.ID.ToString();
                    str[1] = receiver.Email;
                    str[2] = receiver.Status.ToString();
                    str[3] = receiver.TotalBytesUsage.ToString();

                    if (receiver.OtherSideReceiver != null)
                    {
                        str[4] = receiver.OtherSideReceiver.Email;
                    }

                    ListViewItem item = new ListViewItem(str);
                    listClients.Items.Add(item);
                }
            });

        }

        void btnStop_Click(object sender, EventArgs e)
        {
            server.Stop();
            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }

        void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            btnStop.Enabled = true;
            btnStart.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Frm_ChartConnect chartConnect = new Frm_ChartConnect();
            chartConnect.UpdateChart += (data) => {
                if(data.Count >= 30)
                    data.RemoveAt(0);
                data.Add(GetQuantityConnect());
                chartConnect.Data = data;
            };
            chartConnect.Show();
        }

        private int GetQuantityConnect()
        {
            int count = 0;
            foreach(Receiver re in server.Receivers)
            {
                if (re.Status != Shared.Enum.StatusEnum.Disconnected)
                    count++;
            }
            return count;
        }

        private void btn_AutoSendData_Click(object sender, EventArgs e)
        {
            autoSendData.Start();
        }
    }
}
