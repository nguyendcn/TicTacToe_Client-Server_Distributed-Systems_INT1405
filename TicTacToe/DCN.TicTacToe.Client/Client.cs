using DCN.TicTacToe.Shared.Enum;
using DCN.TicTacToe.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DCN.TicTacToe.Shared.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace DCN.TicTacToe.Client
{
    public class Client
    {
        private Thread receivingThread;
        private Thread sendingThread;
        private List<ResponseCallbackObject> callBacks;

        #region Properties

        /// <summary>
        /// The TcpClient that is encapsulated by this client instance.
        /// </summary>
        public TcpClient TcpClient { get; set; }
        /// <summary>
        /// The ip/domain address of the remote server.
        /// </summary>
        public String Address { get; private set; }
        /// <summary>
        /// The Port that is used to connect to the remote server.
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// The status of the client.
        /// </summary>
        public StatusEnum Status { get; set; }
        /// <summary>
        /// List containing all messages that is waiting to be delivered to the remote client/server
        /// </summary>
        public List<MessageBase> MessageQueue { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Raises when a new session is requested by a remote client.
        /// </summary>
        public event Delegates.SessionRequestDelegate SessionRequest;
        /// <summary>
        /// Raises when a new text message was received by the remote session client.
        /// </summary>
        public event Action<Client, String> TextMessageReceived;
        /// <summary>
        /// Raises when a new file upload request was received by the remote session client.
        /// </summary>
        public event Delegates.FileUploadRequestDelegate FileUploadRequest;
        /// <summary>
        /// Raises when a progress was made when a remote session client is uploading a file to this client instance.
        /// </summary>
        public event Action<Client, EventArguments.FileUploadProgressEventArguments> FileUploadProgress;
        /// <summary>
        /// Raises when the client was disconnected;
        /// </summary>
        public event Action<Client> ClientDisconnected;
        /// <summary>
        /// Raises when the remote session client was disconnected;
        /// </summary>
        public event Action<Client> SessionClientDisconnected;
        /// <summary>
        /// Raises when a new unhandled message is received.
        /// </summary>
        public event Action<Client, GenericRequest> GenericRequestReceived;
        /// <summary>
        /// Raises when the current session was ended by the remote client.
        /// </summary>
        public event Action<Client> SessionEndedByTheRemoteClient;
        /// <summary>
        /// 
        /// </summary>
        public event Action<UpdateTablesInProcessRequest> UpdateTablesInProcess;
        /// <summary>
        /// 
        /// </summary>
        public event Action<AcceptPlayRequest> EnablePlayRequest;
        /// <summary>
        /// 
        /// </summary>
        public event Action<UpdateCountDownRequest> UpdateCountDown;
        /// <summary>
        /// 
        /// </summary>
        public event Action<InitGame> InitGame;
        /// <summary>
        /// 
        /// </summary>
        public event Action<GameRequest> GameRequest;
        /// <summary>
        /// 
        /// </summary>
        public event Action<TimeOutRequest> TimeOutRequest;
        /// <summary>
        /// 
        /// </summary>
        public event Action<GameResponse> GameResponse;
        /// <summary>
        /// 
        /// </summary>
        public event Action<Client> AutoAccepInvite;


        #endregion

        #region Constructors

        /// <summary>
        /// Inisializes a new Client instance.
        /// </summary>
        public Client()
        {
            callBacks = new List<ResponseCallbackObject>();
            MessageQueue = new List<MessageBase>();
            Status = StatusEnum.Disconnected;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Connect to a remote server.
        /// (The client will not be able to perform any operations until it is loged in and validated).
        /// </summary>
        /// <param name="address">The server ip/domain address.</param>
        /// <param name="port">The server port.</param>
        public void Connect(String address, int port)
        {
            Address = address;
            Port = port;
            TcpClient = new TcpClient();
            TcpClient.Connect(Address, Port);
            Status = StatusEnum.Connected;
            TcpClient.ReceiveBufferSize = 1024;
            TcpClient.SendBufferSize = 1024;

            receivingThread = new Thread(ReceivingMethod);
            receivingThread.IsBackground = true;
            receivingThread.Start();

            sendingThread = new Thread(SendingMethod);
            sendingThread.IsBackground = true;
            sendingThread.Start();
        }

        /// <summary>
        /// Disconnect from the remote server.
        /// </summary>
        public void Disconnect()
        {
            MessageQueue.Clear();
            callBacks.Clear();
            try
            {
                SendMessage(new DisconnectRequest());
            }
            catch { }
            Thread.Sleep(1000);
            Status = StatusEnum.Disconnected;
            TcpClient.Client.Disconnect(false);
            TcpClient.Close();
            OnClientDisconnected();
        }

        /// <summary>
        /// Log in to the remote server.
        /// </summary>
        /// <param name="email">The email address that will be used to identify this client instance.</param>
        /// <param name="callback">Will be invoked when a Validation Response was received from remote server.</param>
        public void Login(String email, Action<Client, ValidationResponse> callback)
        {
            //Create a new validation request message
            ValidationRequest request = new ValidationRequest();
            request.Email = email;

            //Add a callback before we send the message
            AddCallback(callback, request);

            //Send the message (Add it to the message queue)
            SendMessage(request);
        }

        /// <summary>
        /// Request session from a remote client.
        /// </summary>
        /// <param name="email">The remote client email address (Case sensitive).</param>
        /// <param name="callback">Will be invoked when a Session Response was received from the remote client.</param>
        public void RequestSession(String email, Action<Client, SessionResponse> callback)
        {
            SessionRequest request = new SessionRequest();
            request.Email = email;
            AddCallback(callback, request);
            SendMessage(request);
        }

        /// <summary>
        /// Ends the current session with the remote user.
        /// </summary>
        /// <param name="callback">Will be invoked when an EndSession response was received from the server.</param>
        public void EndCurrentSession(Action<Client, EndSessionResponse> callback)
        {
            EndSessionRequest request = new EndSessionRequest();
            AddCallback(callback, request);
            SendMessage(request);
        }

        /// <summary>
        /// Watch the remote client's desktop.
        /// </summary>
        /// <param name="callback">Will be invoked when a RemoteDesktop Response was received.</param>
        public void RequestDesktop(Action<Client, RemoteDesktopResponse> callback)
        {
            RemoteDesktopRequest request = new RemoteDesktopRequest();
            AddCallback(callback, request);
            SendMessage(request);
        }

        /// <summary>
        /// Send a text message to the remote client.
        /// </summary>
        /// <param name="message"></param>
        public void SendTextMessage(String message)
        {
            TextMessageRequest request = new TextMessageRequest();
            request.Message = message;
            SendMessage(request);
        }

        /// <summary>
        /// Upload a file to the remote client.
        /// </summary>
        /// <param name="fileName">The full file path to the file.</param>
        /// <param name="callback">Will be invoked when a progress is made in uploading the file</param>
        public void UploadFile(String fileName, Action<Client, FileUploadResponse> callback)
        {
            FileUploadRequest request = new FileUploadRequest();
            request.SourceFilePath = fileName;
            request.FileName = System.IO.Path.GetFileName(fileName);
            //request.TotalBytes = Helpers.FileHelper.GetFileLength(fileName);
            AddCallback(callback, request);
            SendMessage(request);
        }

        /// <summary>
        /// Send a message of type MessageBase to the remote client.
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(MessageBase message)
        {
            MessageQueue.Add(message);
        }

        /// <summary>
        /// Send a message of type GenericRequest to the remote session client.
        /// </summary>
        /// <typeparam name="T">Type of response callback delegate.</typeparam>
        /// <param name="request">A message of type GenericRequest.</param>
        /// <param name="callBack">Callback method for the response.</param>
        public void SendGenericRequest<T>(GenericRequest request, T callBack)
        {
            Guid guid = Guid.NewGuid();
            request.CallbackID = guid;
            GenericRequest genericRequest = new GenericRequest(request);
            genericRequest.CallbackID = guid;
            if (callBack != null) callBacks.Add(new ResponseCallbackObject() { CallBack = callBack as Delegate, ID = guid });
            SendMessage(genericRequest);
        }

        /// <summary>
        /// Send a message of type GenericResponse to the remote session client.
        /// </summary>
        /// <param name="response">A message of type GenericResponse.</param>
        public void SendGenericResponse(GenericResponse response)
        {
            GenericResponse genericResponse = new GenericResponse(response);
            SendMessage(genericResponse);
        }

        public void RequestCreateTable(bool createTable, int tableNumber, Action<Client, CreateTableResponse> callback)
        {
            CreateTableRequest request = new CreateTableRequest();
            request.IsCreate = createTable;
            request.TableNumber = tableNumber;
            AddCallback(callback, request);
            SendMessage(request);
        }

        public void RequestClientsInProcess(Action<Client, TablesInProcessResponse> callback)
        {
            TablesInProcessRequest request = new TablesInProcessRequest();
            AddCallback(callback, request);
            SendMessage(request);
        }

        public void RequestAlreadyPlayGame()
        {
            AcceptPlayRequest request = new AcceptPlayRequest();
            request.IsAlready = true;
            SendMessage(request);
        }

        public void RequestGame(int [,] gameBoard)
        {
            GameRequest request = new GameRequest();
            request.BoardGame = gameBoard;

            SendMessage(request);
        }

        public void RequestUpdateScore(Action<Client, UpdateScoreResponse> callback)
        {
            UpdateScoreRequest request = new UpdateScoreRequest();
            AddCallback(callback, request);
            SendMessage(request);
        }

        #endregion

        #region Threads Methods

        private void SendingMethod(object obj)
        {
            while (Status != StatusEnum.Disconnected)
            {
                if (MessageQueue.Count > 0)
                {
                    MessageBase m = MessageQueue[0];

                    BinaryFormatter f = new BinaryFormatter();
                    try
                    {
                        f.Serialize(TcpClient.GetStream(), m);
                    }
                    catch
                    {
                        Disconnect();
                    }

                    MessageQueue.Remove(m);
                }

                Thread.Sleep(30);
            }
        }

        private void ReceivingMethod(object obj)
        {
            while (Status != StatusEnum.Disconnected)
            {
                if (TcpClient.Available > 0)
                {
                    //try
                    //{
                    BinaryFormatter f = new BinaryFormatter();
                    f.Binder = new Shared.AllowAllAssemblyVersionsDeserializationBinder();
                    MessageBase msg = f.Deserialize(TcpClient.GetStream()) as MessageBase;
                    OnMessageReceived(msg);
                    //}
                    //catch (Exception e)
                    //{
                    //    Exception ex = new Exception("Unknown message recieved. Could not deserialize the stream.", e);
                    //    //OnClientError(this, ex);
                    //    Debug.WriteLine(ex.Message);
                    //}
                }

                Thread.Sleep(30);
            }
        }

        #endregion

        #region Message Handlers

        protected virtual void OnMessageReceived(MessageBase msg)
        {
            Type type = msg.GetType();

            if (msg is ResponseMessageBase)
            {
                if (type == typeof(GenericResponse))
                {
                    msg = (msg as GenericResponse).ExtractInnerMessage();
                }

                InvokeMessageCallback(msg, (msg as ResponseMessageBase).DeleteCallbackAfterInvoke);

                if (type == typeof(RemoteDesktopResponse))
                {
                    RemoteDesktopResponseHandler(msg as RemoteDesktopResponse);
                }
                else if (type == typeof(FileUploadResponse))
                {
                    FileUploadResponseHandler(msg as FileUploadResponse);
                }
                else if(type == typeof(GameResponse))
                {
                    OnGameResponse(msg as GameResponse);
                }
            }
            else
            {
                if (type == typeof(SessionRequest))
                {
                    SessionRequestHandler(msg as SessionRequest);
                }
                else if (type == typeof(EndSessionRequest))
                {
                    OnSessionEndedByTheRemoteClient();
                }
                else if (type == typeof(RemoteDesktopRequest))
                {
                    RemoteDesktopRequestHandler(msg as RemoteDesktopRequest);
                }
                else if (type == typeof(TextMessageRequest))
                {
                    TextMessageRequestHandler(msg as TextMessageRequest);
                }
                else if (type == typeof(FileUploadRequest))
                {
                    FileUploadRequestHandler(msg as FileUploadRequest);
                }
                else if (type == typeof(DisconnectRequest))
                {
                    OnSessionClientDisconnected();
                }
                else if (type == typeof(UpdateTablesInProcessRequest))
                {
                    UpdateTablesInProcessRequestHandler(msg as UpdateTablesInProcessRequest);
                }
                else if (type == typeof(AcceptPlayRequest))
                {
                    OnAcceptPlayRequest(msg as AcceptPlayRequest);
                }
                else if(type == typeof(UpdateCountDownRequest))
                {
                    OnUpdateCountDownRequest(msg as UpdateCountDownRequest);
                }
                else if(type == typeof(InitGame))
                {
                    OnInitGame(msg as InitGame);
                }
                else if(type == typeof(GameRequest))
                {
                    OnGameRequest(msg as GameRequest);
                }
                else if(type == typeof(TimeOutRequest))
                {
                    OnTimeOutRequest(msg as TimeOutRequest);
                }
                else if (type == typeof(GenericRequest))
                {
                    OnGenericRequestReceived(msg as GenericRequest);
                }
            }
        }

        private void RemoteDesktopResponseHandler(RemoteDesktopResponse response)
        {
            if (!response.Cancel)
            {
                RemoteDesktopRequest request = new RemoteDesktopRequest();
                request.CallbackID = response.CallbackID;
                SendMessage(request);
            }
            else
            {
                callBacks.RemoveAll(x => x.ID == response.CallbackID);
            }
        }

        private void FileUploadResponseHandler(FileUploadResponse response)
        {
            FileUploadRequest request = new FileUploadRequest(response);

            if (!response.HasError)
            {
                if (request.CurrentPosition < request.TotalBytes)
                {
                    // request.BytesToWrite = Helpers.FileHelper.SampleBytesFromFile(request.SourceFilePath, request.CurrentPosition, request.BufferSize);
                    request.CurrentPosition += request.BufferSize;
                    SendMessage(request);
                }
            }
            else
            {

            }
        }

        private void FileUploadRequestHandler(FileUploadRequest request)
        {
            FileUploadResponse response = new FileUploadResponse(request);

            if (request.CurrentPosition == 0)
            {
                EventArguments.FileUploadRequestEventArguments args = new EventArguments.FileUploadRequestEventArguments(() =>
                {
                    //Confirm File Upload
                    response.DestinationFilePath = request.DestinationFilePath;
                    SendMessage(response);
                },
                () =>
                {
                    //Refuse File Upload
                    response.HasError = true;
                    response.Exception = new Exception("The file upload request was refused by the user!");
                    SendMessage(response);
                });

                args.Request = request;
                OnFileUploadRequest(args);
            }
            else
            {
                //Helpers.FileHelper.AppendAllBytes(request.DestinationFilePath, request.BytesToWrite);
                SendMessage(response);
                OnUploadFileProgress(new EventArguments.FileUploadProgressEventArguments() { CurrentPosition = request.CurrentPosition, FileName = request.FileName, TotalBytes = request.TotalBytes, DestinationPath = request.DestinationFilePath });
            }
        }

        private void TextMessageRequestHandler(TextMessageRequest request)
        {
            OnTextMessageReceived(request.Message);
        }

        private void RemoteDesktopRequestHandler(RemoteDesktopRequest request)
        {
            RemoteDesktopResponse response = new RemoteDesktopResponse(request);
            try
            {
                //response.FrameBytes = Helpers.RemoteDesktop.CaptureScreenToMemoryStream(request.Quality);
            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Exception = e;
            }

            SendMessage(response);
        }

        private void SessionRequestHandler(SessionRequest request)
        {
            SessionResponse response = new SessionResponse(request);

            if (this.Status == StatusEnum.Validated)
            {
                EventArguments.SessionRequestEventArguments args = new EventArguments.SessionRequestEventArguments(() =>
                {
                    //Confirm Session
                    response.IsConfirmed = true;
                    response.Email = request.Email;
                    SendMessage(response);
                },
                () =>
                {
                    //Refuse Session
                    response.IsConfirmed = false;
                    response.Email = request.Email;
                    SendMessage(response);
                });

                args.Request = request;
                OnSessionRequest(args);
            }
            else
            {
                response.IsConfirmed = true;
                response.Email = request.Email;
                SendMessage(response);
                OnAutoAcceptInvite(this);
            }

        }

        public void UpdateTablesInProcessRequestHandler(UpdateTablesInProcessRequest request)
        {
            OnUpdateTablesInProcessRequest(request);
        }

        #endregion

        #region Callback Methods

        private void AddCallback(Delegate callBack, MessageBase msg)
        {
            if (callBack != null)
            {
                Guid callbackID = Guid.NewGuid();
                ResponseCallbackObject responseCallback = new ResponseCallbackObject()
                {
                    ID = callbackID,
                    CallBack = callBack
                };

                msg.CallbackID = callbackID;
                callBacks.Add(responseCallback);
            }
        }

        private void InvokeMessageCallback(MessageBase msg, bool deleteCallback)
        {
            var callBackObject = callBacks.SingleOrDefault(x => x.ID == msg.CallbackID);

            if (callBackObject != null)
            {
                if (deleteCallback)
                {
                    callBacks.Remove(callBackObject);
                }
                callBackObject.CallBack.DynamicInvoke(this, msg);
            }
        }

        #endregion

        #region Virtuals

        public virtual void OnAutoAcceptInvite(Client client)
        {
            if (AutoAccepInvite != null) AutoAccepInvite(client);
        }

        public virtual void OnGameResponse(GameResponse args)
        {
            if (GameResponse != null) GameResponse(args);
        }

        public virtual void OnTimeOutRequest(TimeOutRequest args)
        {
            if (TimeOutRequest != null) TimeOutRequest(args);
        }

        public virtual void OnGameRequest(GameRequest args)
        {
            if (GameRequest != null) GameRequest(args);
        }

        public virtual void OnInitGame(InitGame args)
        {
            if (InitGame != null) InitGame(args);
        }

        public virtual void OnUpdateCountDownRequest(UpdateCountDownRequest args)
        {
            if (UpdateCountDown != null) UpdateCountDown(args);
        }

        protected virtual void OnAcceptPlayRequest(AcceptPlayRequest args)
        {
            if (EnablePlayRequest != null) EnablePlayRequest(args);
        }

        protected virtual void OnUpdateTablesInProcessRequest(UpdateTablesInProcessRequest args)
        {
            if (UpdateTablesInProcess != null) UpdateTablesInProcess(args);
        }

        protected virtual void OnSessionRequest(EventArguments.SessionRequestEventArguments args)
        {
            if (SessionRequest != null) SessionRequest(this, args);
        }

        protected virtual void OnFileUploadRequest(EventArguments.FileUploadRequestEventArguments args)
        {
            if (FileUploadRequest != null) FileUploadRequest(this, args);
        }

        protected virtual void OnTextMessageReceived(String txt)
        {
            if (TextMessageReceived != null) TextMessageReceived(this, txt);
        }

        protected virtual void OnUploadFileProgress(EventArguments.FileUploadProgressEventArguments args)
        {
            if (FileUploadProgress != null) FileUploadProgress(this, args);
        }

        protected virtual void OnClientDisconnected()
        {
            if (ClientDisconnected != null) ClientDisconnected(this);
        }

        protected virtual void OnSessionClientDisconnected()
        {
            if (SessionClientDisconnected != null) SessionClientDisconnected(this);
        }

        protected virtual void OnGenericRequestReceived(GenericRequest request)
        {
            if (GenericRequestReceived != null) GenericRequestReceived(this, request.ExtractInnerMessage());
        }

        protected virtual void OnSessionEndedByTheRemoteClient()
        {
            if (SessionEndedByTheRemoteClient != null) SessionEndedByTheRemoteClient(this);
        }
        #endregion
    }
}
