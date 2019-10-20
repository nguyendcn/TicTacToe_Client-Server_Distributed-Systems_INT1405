using DCN.TicTacToe.Shared.Enum;
using DCN.TicTacToe.Shared.ExtensionMethods;
using DCN.TicTacToe.Shared.Messages;
using DCN.TicTacToe.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCN.TicTacToe.Server
{
    public class Receiver
    {
        private Thread receivingThread;
        private Thread sendingThread;

        #region Properties

        /// <summary>
        /// The receiver unique id.
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// The reference to the parent Server.
        /// </summary>
        public Server Server { get; set; }
        /// <summary>
        /// The real TcpClient working in the background.
        /// </summary>
        public TcpClient Client { get; set; }
        /// <summary>
        /// Contains a reference to the currently in session with this receiver instance is exists.
        /// </summary>
        public Receiver OtherSideReceiver { get; set; }
        /// <summary>
        /// The current status of the reciever instance.
        /// </summary>
        public StatusEnum Status { get; set; }
        /// <summary>
        /// The message queue that contains all the messages to deliver to the remote client.
        /// </summary>
        public List<MessageBase> MessageQueue { get; private set; }
        /// <summary>
        /// The Total bytes processed by this receiver instance.
        /// </summary>
        public long TotalBytesUsage { get; set; }
        /// <summary>
        /// The Email address is used to authenticate the remote client.
        /// </summary>
        public String Email { get; set; }
        /// <summary>
        /// If true will produce and exception in some cases.
        /// </summary>
        public bool DebugMode { get; set; }
        /// <summary>
        /// The room number is used to name session with other client 
        /// </summary>
        public InGameProperties InGameProperties { get; set; }
        /// <summary>
        /// Timer to send time countdown in play game
        /// </summary>
        public CountDown CountDownInGame { get; set; }

        #endregion

        #region Constructors

        public Receiver()
        {
            ID = Guid.NewGuid();
            MessageQueue = new List<MessageBase>();
            Status = StatusEnum.Connected;

            InGameProperties = new InGameProperties();

            CountDownInGame = new CountDown();
            CountDownInGame.CoutDownEv += CountDownInGame_CoutDownEv;
        }


        /// <summary>
        /// Initializes a new reciever instance
        /// </summary>
        /// <param name="client">The TcpClient to encapsulate that was obtained by the TcpListener.</param>
        /// <param name="server">The reference to the parent server containing the receivers list.</param>
        public Receiver(TcpClient client, Server server)
            : this()
        {
            Server = server;
            Client = client;
            Client.ReceiveBufferSize = 1024;
            Client.SendBufferSize = 1024;
        }

        #endregion

        #region Event Handler

        private void CountDownInGame_CoutDownEv(int time)
        {
            if(time < 0) // no response => end game
            {
                this.CountDownInGame.Stop();

                //Time out
                if (this.InGameProperties.Status == StatusInGame.InTurn)
                {
                    this.InGameProperties.Status = StatusInGame.NotReady;
                    this.SendMessage(new TimeOutRequest());
                    this.SendMessage(new StatusGameRequest(StatusGame.Lose));
                }
                else if (this.InGameProperties.Status == StatusInGame.Ready)
                {
                    this.InGameProperties.WinGame += 1;
                    this.SendMessage(new TimeOutRequest());
                    this.SendMessage(new StatusGameRequest(StatusGame.Win));
                }
                this.SendMessage(new AcceptPlayRequest());
            }
            else
            {
                UpdateCountDownRequest request = new UpdateCountDownRequest();
                request.Time = time;
                this.SendMessage(request);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the receiver and start transmitting data
        /// </summary>
        public void Start()
        {
            receivingThread = new Thread(ReceivingMethod);
            receivingThread.IsBackground = true;
            receivingThread.Start();

            sendingThread = new Thread(SendingMethod);
            sendingThread.IsBackground = true;
            sendingThread.Start();
        }

        /// <summary>
        /// Stops all data transmition and disconnectes the TcpClient
        /// </summary>
        private void Disconnect()
        {
            if (Status == StatusEnum.Disconnected) return;

            if (OtherSideReceiver != null)
            {
                OtherSideReceiver.OtherSideReceiver = null;
                OtherSideReceiver.Status = StatusEnum.Validated;
                OtherSideReceiver = null;
            }

            Status = StatusEnum.Disconnected;
            Client.Client.Disconnect(false);
            Client.Close();
        }

        /// <summary>
        /// Add the specified message to the message sender queue
        /// </summary>
        /// <param name="message">The message of type MessageBase that should be added to the queue</param>
        public void SendMessage(MessageBase message)
        {
            MessageQueue.Add(message);
        }

        #endregion

        #region Threads Methods

        private void SendingMethod()
        {
            while (Status != StatusEnum.Disconnected)
            {
                if (MessageQueue.Count > 0)
                {
                    var message = MessageQueue[0];

                    try
                    {
                        BinaryFormatter f = new BinaryFormatter();
                        f.Serialize(Client.GetStream(), message);
                    }
                    catch
                    {
                        Disconnect();
                    }
                    finally
                    {
                        MessageQueue.Remove(message);
                    }
                }
                Thread.Sleep(30);
            }
        }

        private void ReceivingMethod()
        {
            while (Status != StatusEnum.Disconnected)
            {
                if (Client.Available > 0)
                {
                    TotalBytesUsage += Client.Available;

                    try
                    {
                        BinaryFormatter f = new BinaryFormatter();
                        MessageBase msg = f.Deserialize(Client.GetStream()) as MessageBase;
                        OnMessageReceived(msg);
                    }
                    catch (Exception e)
                    {
                        if (DebugMode) throw e;
                        Exception ex = new Exception("Unknown message recieved. Could not deserialize the stream.", e);
                        Debug.WriteLine(ex.Message);
                    }
                }

                Thread.Sleep(30);
            }

        }

        #endregion

        #region Message Handlers

        private void OnMessageReceived(MessageBase msg)
        {
            Type type = msg.GetType();

            if (type == typeof(ValidationRequest))
            {
                ValidationRequestHandler(msg as ValidationRequest);
            }
            else if (type == typeof(SessionRequest))
            {
                SessionRequestHandler(msg as SessionRequest);
            }
            else if (type == typeof(SessionResponse))
            {
                SessionResponseHandler(msg as SessionResponse);
            }
            else if (type == typeof(EndSessionRequest))
            {
                EndSessionRequestHandler(msg as EndSessionRequest);
            }
            else if (type == typeof(DisconnectRequest))
            {
                DisconnectRequestHandler(msg as DisconnectRequest);
            }else if(type == typeof(CreateTableRequest))
            {
                CreateTableHandler(msg as CreateTableRequest);
            }
            else if(type == typeof(TablesInProcessRequest))
            {
                ClientsInProcessRequestHandler(msg as TablesInProcessRequest);
            }
            else if(type == typeof(AcceptPlayRequest))
            {
                AcceptPlayRequestHandler(msg as AcceptPlayRequest);
            }
            else if (OtherSideReceiver != null)
            {
                if(type == typeof(GameRequest))
                {
                    GameRequestHandler(msg as GameRequest);
                    return;
                }
                else if(type == typeof(UpdateScoreRequest))
                {
                    UpdateScoreRequestHandler(msg as UpdateScoreRequest);
                }

                OtherSideReceiver.SendMessage(msg);
                
            }
        }

        private void SendMsgToClients(Receiver []clients, MessageBase message)
        {
            foreach(Receiver rece in clients)
            {
                rece.SendMessage(message);
            }
        }

        private void SendMsgToClientsOneByOne(Receiver[] clients, MessageBase[] messages)
        {
            if(clients.Length != messages.Length)
            {
                throw new IndexOutOfRangeException("Size of clients and messages does not match.");
            }

            int index = 0;
            foreach (Receiver rece in clients)
            {
                rece.SendMessage(messages[index]);
                index++;
            }
        }

        private void PropertiesForEndGame()
        {
            this.InGameProperties.Status = StatusInGame.NotReady;
            this.OtherSideReceiver.InGameProperties.Status = StatusInGame.NotReady;

            this.CountDownInGame.Stop();
            this.OtherSideReceiver.CountDownInGame.Stop();
        }

        private void GameRequestHandler(GameRequest request)
        {
            StatusGame sg = request.BoardGame.GetStatementGame();

            this.CountDownInGame.Reset();
            this.OtherSideReceiver.CountDownInGame.Reset();

            request.BoardGame = request.BoardGame.SwapZvO();
            this.OtherSideReceiver.SendMessage(request);

            if (sg == StatusGame.Win)
            { 
                SendMsgToClientsOneByOne(new Receiver[] { this, this.OtherSideReceiver },
                                         new MessageBase[] { new GameResponse(request, sg),
                                                             new GameResponse(request, StatusGame.Lose)
                                                           }
                                         );

                this.InGameProperties.WinGame += 1;

                PropertiesForEndGame();

                SendMsgToClients(new Receiver[] { this, this.OtherSideReceiver }, new AcceptPlayRequest());
            }
            else if(sg == StatusGame.Tie)
            {
                SendMsgToClientsOneByOne(new Receiver[] { this, this.OtherSideReceiver },
                                         new MessageBase[] { new GameResponse(request, sg),
                                                             new GameResponse(request, StatusGame.Tie)
                                                           }
                                         );

                this.InGameProperties.WinGame += 1;
                this.OtherSideReceiver.InGameProperties.WinGame += 1;

                PropertiesForEndGame();

                SendMsgToClients(new Receiver[] { this, this.OtherSideReceiver }, new AcceptPlayRequest());
            }
            
        }

        public void UpdateScoreRequestHandler(UpdateScoreRequest request)
        {
            UpdateScoreResponse response = new UpdateScoreResponse(request,
                                                    this.InGameProperties.WinGame,
                                                    this.OtherSideReceiver.InGameProperties.WinGame);
            this.SendMessage(response);
        }

        private void EndSessionRequestHandler(EndSessionRequest request)
        {
            if (OtherSideReceiver != null)
            {
                OtherSideReceiver.SendMessage(new EndSessionRequest());
                ResetPropertiesClientInProcess(OtherSideReceiver);
            }
            ResetPropertiesClient(this);
            this.SendMessage(new EndSessionResponse(request));
        }

        private void DisconnectRequestHandler(DisconnectRequest request)
        {
            if (OtherSideReceiver != null)
            {
                OtherSideReceiver.SendMessage(new DisconnectRequest());
                OtherSideReceiver.Status = StatusEnum.Validated;
            }

            Disconnect();
        }

        private void SessionResponseHandler(SessionResponse response)
        {
            foreach (var receiver in Server.Receivers.Where(x => x != this))
            {
                if (receiver.Email == response.Email)
                {
                    response.Email = this.Email;

                    if (response.IsConfirmed)
                    {
                        receiver.OtherSideReceiver = this;
                        this.OtherSideReceiver = receiver;
                        this.Status = StatusEnum.InSession;
                        receiver.Status = StatusEnum.InSession;


                        if (this.InGameProperties.Room == -1 && receiver.InGameProperties.Room == -1)
                        {
                            this.InGameProperties.Room = receiver.InGameProperties.Room = GetRandomTable(Server.Receivers);
                        }
                        else if (this.InGameProperties.Room == -1)
                        {
                            this.InGameProperties.Room = receiver.InGameProperties.Room;
                        }
                        else
                        {
                            receiver.InGameProperties.Room = this.InGameProperties.Room;
                        }

                    }
                    else
                    {
                        response.HasError = true;
                        response.Exception = new Exception("The session request was refused by " + response.Email);
                    }

                    receiver.SendMessage(response);
                    if(!response.HasError)
                    {
                        this.SendMessage(new AcceptPlayRequest());
                        receiver.SendMessage(new AcceptPlayRequest());
                    }
                    return;
                }
            }
        }

        private int GetRandomTable(List<Receiver> listReceiver)
        {
            Random random = new Random();
            int tableNumber = -1;

            do
            {
                tableNumber = random.Next(1000, 9999);
            } while (TableIsExists(listReceiver, tableNumber));

            return tableNumber;
        }

        private bool TableIsExists(List<Receiver> listReceiver, int tableNumber)
        {
            return (listReceiver.Where(x =>  x.InGameProperties.Room == tableNumber)).Count() > 0;
        }

        private bool IsAvaliable(StatusEnum status)
        {
            if (status == StatusEnum.Validated || status == StatusEnum.InProcess)
                return true;
            return false;
        }

        private void SessionRequestHandler(SessionRequest request)
        {
            SessionResponse response;

            if (!IsAvaliable(this.Status)) //Added after a code project user comment.
            {
                response = new SessionResponse(request);
                response.IsConfirmed = false;
                response.HasError = true;
                response.Exception = new Exception("Could not request a new session. The current client is already in session, or is not loged in.");
                SendMessage(response);
                return;
            }

            foreach (var receiver in Server.Receivers.Where(x => x != this))
            {
                if (receiver.Email == request.Email)
                {
                    if (IsAvaliable(receiver.Status))
                    {
                        if (receiver.Status == StatusEnum.Validated)
                        {
                            request.Email = this.Email;
                            receiver.SendMessage(request);
                            return;
                        }
                        else if(receiver.Status == StatusEnum.InProcess)
                        {
                            receiver.SendMessage(new JoinTableRequest(this.Email, request));
                            return;
                        }
                    } 
                }
            }

            response = new SessionResponse(request);
            response.IsConfirmed = false;
            response.HasError = true;
            response.Exception = new Exception(request.Email + " does not exists or not loged in or in session with another user.");
            SendMessage(response);
        }

        private void ValidationRequestHandler(ValidationRequest request)
        {
            ValidationResponse response = new ValidationResponse(request);

            EventArguments.ClientValidatingEventArgs args = new EventArguments.ClientValidatingEventArgs(() =>
            {
                //Confirm Action
                Status = StatusEnum.Validated;
                Email = request.Email;
                response.IsValid = true;
                SendMessage(response);
                Server.OnClientValidated(this);
            },
            () =>
            {
                //Refuse Action
                response.IsValid = false;
                response.HasError = true;
                response.Exception = new AuthenticationException("Login failed for user " + request.Email);
                SendMessage(response);
            });

            args.Receiver = this;
            args.Request = request;

            Server.OnClientValidating(args);
        }

        private void CreateTableHandler(CreateTableRequest request)
        {
            CreateTableResponse response = new CreateTableResponse(request);

            if (request.IsCreate)
            {
                if(request.TableNumber != -1)
                {
                    if(TableIsExists(Server.Receivers, request.TableNumber))
                    {
                        response.IsSuccess = false;
                    }
                    else
                    {
                        this.InGameProperties.Room = request.TableNumber;
                        this.Status = StatusEnum.InProcess;
                        response.IsSuccess = true;
                    }
                }
                else
                {
                    this.InGameProperties.Room = GetRandomTable(Server.Receivers);
                    this.Status = StatusEnum.InProcess;
                    response.IsSuccess = true;
                }              
            }
            else
            {
                Status = StatusEnum.Validated;
                response.IsSuccess = true;
            }
            SendMessage(response);

            UpdateTableInProcessRequestHandler();
        }

        private void UpdateTableInProcessRequestHandler()
        {
            UpdateTablesInProcessRequest processRequest = new UpdateTablesInProcessRequest();
            processRequest.ClientsInProcess = GetListTableInProcess(Server.Receivers);

            foreach (var receiver in Server.Receivers.Where(x => x != this))
            {
                if (receiver.Status == StatusEnum.Validated)
                {
                    receiver.SendMessage(processRequest);
                }
            }
        }

        private List<TablePropertiesBase> GetListTableInProcess(List<Receiver> listReceiver)
        {
            List<TablePropertiesBase> listName = new List<TablePropertiesBase>();
            foreach (var receiver in Server.Receivers)
            {
                if (receiver.Status == StatusEnum.InProcess)
                {
                    listName.Add(new TablePropertiesBase(receiver.InGameProperties.Room,
                                       receiver.Email, ""));
                }
            }
            return listName;
        }

        public void ClientsInProcessRequestHandler(TablesInProcessRequest request)
        {
            TablesInProcessResponse response = new TablesInProcessResponse(request);

            response.ClientsInProcess = new List<TablePropertiesBase>();

            foreach (var receiver in Server.Receivers.Where(x => x != this))
            {
                if(receiver.Status == StatusEnum.InProcess)
                {
                    response.ClientsInProcess.Add(new TablePropertiesBase(receiver.InGameProperties.Room, 
                        receiver.Email, ""));
                }
            }

            SendMessage(response);
        }

        public void AcceptPlayRequestHandler(AcceptPlayRequest msg)
        {
            OtherSideReceiver.SendMessage(msg);
            if (msg.IsAlready)
            {
                this.InGameProperties.Status = StatusInGame.Ready;

                if (OtherSideReceiver.InGameProperties.Status == StatusInGame.Ready)
                {
                    SetupDataAndSendToClients();
                }
            }
        }

        #endregion

        #region Method


        private void SetupDataAndSendToClients()
        {
            bool isInTurn = RandomBoolean();

            this.InGameProperties.Status = isInTurn ? StatusInGame.InTurn : StatusInGame.Ready;
            SendDataInitToClient(this, 
                                    new InitGame(this.InGameProperties, isInTurn, this.Email));

            this.OtherSideReceiver.InGameProperties.Status = !isInTurn ? StatusInGame.InTurn : StatusInGame.Ready;
            SendDataInitToClient(this.OtherSideReceiver, 
                                    new InitGame(this.InGameProperties, !isInTurn, this.Email));

            this.OtherSideReceiver.CountDownInGame.Start();
            this.CountDownInGame.Start();
        }

        private bool RandomBoolean()
        {
            Random random = new Random();

            return random.Next(2) == 0 ? false : true;

        }

        private void SendDataInitToClient(Receiver client, InitGame data)
        {
            client.SendMessage(data);
        }

        private void ResetPropertiesClient(Receiver receiver)
        {
            receiver.CountDownInGame.Stop();
            receiver.InGameProperties.Reset();
            receiver.Status = StatusEnum.Validated;
            receiver.OtherSideReceiver = null;
        }

        private void ResetPropertiesClientInProcess(Receiver receiver)
        {
            receiver.CountDownInGame.Stop();
            receiver.InGameProperties.Status = StatusInGame.NotReady;
            receiver.InGameProperties.WinGame = 0;
            receiver.Status = StatusEnum.Validated;
            receiver.OtherSideReceiver = null;
        }

        #endregion
    }
}
