using System.Net.Sockets;
using System.IO;
using System;
using System.Net;
using System.Threading;

namespace Network
{

    public enum NetworkReason
    {
        None = 0,
        ConnectTimeout,
        InvalidPacketHeader,
        SendError,
        ManualClose,
        RecvError,
        NoValidIPInHost,
        InvalidAddress,
        ResoveHost,
    }

    /// <summary>
    /// 消息头
    /// </summary>
    public struct PacketHeader
    {
        //消息ID
        public UInt32 MsgID;
        //Tag
        public UInt16 Tag;
        //消息长度 （包含消息头）
        public UInt16 Size;

        public bool Valid(UInt16 expect)
        {
            return expect == Tag;
        }
    }

    public class ClientSocket
    {
        /// <summary>
        /// socket
        /// </summary>
        Socket _socket;

        /// <summary>
        /// 包验证
        /// </summary>
        UInt16 _sendTag = 1;
        UInt16 _recvTag = 1;
        object _sendTagGuard = new object();

        

        /// <summary>
        /// 消息头长度
        /// </summary>
        const Int32 PacketHeaderSize = 4 + 2 + 2;

        enum Stage
        {
            None,
            GettingHostEntry,
            BeginConnect,
            Ready,
        }

        Stage _stage = Stage.None;
        object _stageGuard = new object();

        #region Events

        /// <summary>
        /// 收到消息
        /// </summary>
        public Action<UInt32, MemoryStream> EventRecvPacket;

        /// <summary>
        /// socket 关闭
        /// </summary>
        public Action<NetworkReason> EventClosed;

        /// <summary>
        /// 连接成功/失败的回调
        /// </summary>
        public Action EventConnected;

        #endregion

        /// <summary>
        /// 超时时长
        /// </summary>
        public int _timeoutSecond = 5000;

        string _address;

        public ClientSocket()
        {            
            _sendTag = 1;
            _recvTag = 1;
        }

        public bool ResolveHost
        {
            get;
            set;
        }

        #region Connect


        void SetStage( Stage s )
        {
            lock( _stageGuard )
            {
                _stage = s;
            }
        }

        Stage GetStage()
        {
            lock (_stageGuard)
            {
                return _stage;
            }
        }

        static bool SpliteAddress( string address, out string host, out int port )
        {
            String[] arr = address.Split(':');

            if (arr.Length < 2)
            {
                host = "";
                port = 0;

                return false;
            }

            host = arr[0];
            port = (int)Int32.Parse(arr[1]);

            return true;
        }

        int _port;

        public void Connect(string address )
        {            
            if (IsConnected  )
                return;


            if (GetStage() != Stage.None)
                return;

            string host;            
            if (!SpliteAddress(address, out host, out _port))
            {
                Close(NetworkReason.InvalidAddress);
                return;
            }

            _address = address;

            _sendTag = 1;
            _recvTag = 1;

            if (ResolveHost )
            {
                BeginGetHostEntry(host);  
            }
            else
            {
                var addr = IPAddress.Parse(host);
                BeginConnect(new IPAddress[] { addr }, _port);
            }
                      
        }

        void BeginGetHostEntry(string host)
        {      
            SetStage(Stage.GettingHostEntry);

            Dns.BeginGetHostEntry(host, new AsyncCallback(HandleBeginGetHostEntry), null);
        }

        void HandleBeginGetHostEntry( IAsyncResult result )
        {
            var ips = Dns.EndGetHostEntry(result);

            BeginConnect(ips.AddressList, _port);
        }

        void BeginConnect(IPAddress[] host, int port)
        { 
            SetStage(Stage.BeginConnect);

            if (_socket == null)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            try
            {
                _socket.BeginConnect(host, port, new AsyncCallback(HandleConnected), _socket);
            }
            catch (Exception)
            {
                Close(NetworkReason.None);
            }
        }

        private void HandleConnected(IAsyncResult result)
        {

            // 当多个连接过来时, 只有第一个
            if (GetStage() != Stage.BeginConnect)
                return;

            SetStage(Stage.Ready);            

            try
            {
                _socket.EndConnect(result);
            }
            catch(Exception )
            {
               
            }

            Thread thread = new Thread(new ThreadStart(IOThreadRecv));
            thread.IsBackground = true;
            SetThreadRunning(true);
            thread.Start();


            if (EventConnected != null)
            {
                EventConnected();
            }

        }

        #endregion

        #region Recv Packet


        object _threadRunningGurad = new object();

        bool _threadRunning;
        bool IsThreadRunning()
        {
            lock (_threadRunningGurad)
            {
                return _threadRunning;
            }
        }

        void SetThreadRunning(bool running)
        {
            lock (_threadRunningGurad)
            {
                _threadRunning = running;
            }
        }

        void IOThreadRecv()
        {
            while (IsThreadRunning())
            {
                if (_socket == null)
                    break;

                if (!_socket.Connected)
                {
    
                    Close(NetworkReason.None);
                    break;
                }

                ReadHeader();
            }
        }



        void ReadHeader( )
        {
            StartReadPacket(PacketHeaderSize,  HandleReadHeader, null );
        }        

        /// <summary>
        /// 读取包头
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        void HandleReadHeader( MemoryStream stream, object tag )
        {            
            PacketHeader header;

            using (BinaryReader reader = new BinaryReader(stream))
            {
                header.MsgID = reader.ReadUInt32();
                header.Tag = reader.ReadUInt16();
                header.Size = reader.ReadUInt16();


                // 检查包头
                if (!header.Valid(_recvTag))
                {
                    Close(NetworkReason.InvalidPacketHeader);
                    return;
                }

                var dataSize = (Int32)(header.Size - PacketHeaderSize);
               

                if ( dataSize > 0 )
                {
                    StartReadPacket(dataSize, HandleReadBody, header);
                }
                else
                {
                    DoRecvPacket(header, new MemoryStream());
                }

                
            }            
        }

        void DoRecvPacket( PacketHeader header, MemoryStream stream )
        {            
            // 派发消息
            if (EventRecvPacket != null)
            {
                EventRecvPacket(header.MsgID, stream);
            }

            _recvTag++;          
        }

        void HandleReadBody(MemoryStream stream, object tag)
        {            

            var header = (PacketHeader)tag;

            DoRecvPacket(header, stream);
        }


        struct AsyncData
        {
            public int Size;
            public byte[] Data;
            public MemoryStream Stream;
            public Action<MemoryStream, object> Callback;
            public object Tag;
        }


        void StartReadPacket(Int32 size, Action<MemoryStream, object> callback, object tag )
        {
            // TODO bytebuffer 优化
            AsyncData ad;
            ad.Data = new byte[size];
            ad.Stream = new MemoryStream();
            ad.Callback = callback;
            ad.Size = size;
            ad.Tag = tag;
            AsyncReadPacket(ad);
        }

        
        void AsyncReadPacket( AsyncData ad )
        {
            while( true )
            {
                if (_socket == null)
                    break;

                int recvLen;

                try
                {

                    recvLen = _socket.Receive(ad.Data);
                }
                catch( Exception )
                {
                    Close(NetworkReason.None);
                    break;
                }


                if (recvLen <= 0)
                {

                    Close(NetworkReason.None);
                    break;
                }

                ad.Size -= recvLen;

                ad.Stream.Write(ad.Data, 0, recvLen);


                if (ad.Size == 0)
                {
                    ad.Stream.Seek(0, SeekOrigin.Begin);
                    ad.Callback(ad.Stream, ad.Tag);
                    break;
                }
            }

        }
        #endregion

        #region SendMessage

        public void SendPacket(UInt32 msgID, MemoryStream stream)
        {
            if (_socket == null || !_socket.Connected || stream  == null )
            {                
                return;
            }

            MemoryStream pktStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(pktStream);            
            writer.Write(msgID);
            writer.Write(GenSendTag());
            writer.Write((UInt16)(PacketHeaderSize + stream.Length));
            writer.Write(stream.GetBuffer(), 0, (int)stream.Length);
            writer.Flush();

            try
            {
                if ( _socket != null && _socket.Connected )
                {                    
                    _socket.Send(pktStream.GetBuffer(), 0, (int)pktStream.Length, SocketFlags.None );
                }


            }
            catch (Exception )
            {
                Close(NetworkReason.SendError);
            }

        }


        UInt16 GenSendTag( )
        {
            lock( _sendTagGuard )
            {             
                var tag = _sendTag;
                _sendTag++;
                return tag;
            }
        }

        #endregion

        public void Stop( )
        {
            Close(NetworkReason.ManualClose);
        }

        object _closeGuard = new object();

        void Close(NetworkReason reason)
        {
            lock ( _closeGuard)
            {
                //关闭socket
                if (_socket == null)
                    return;

                SetStage(Stage.None);                

                if (_socket.Connected)
                {
                    try
                    {

                        _socket.Shutdown(SocketShutdown.Both);
                        _socket.Close();
                    }
                    catch
                    {

                    }
                }

                // 关闭线程
                SetThreadRunning(false);

                _socket = null;


                //通知外界socket已经关闭
                if (EventClosed != null)
                {
                    EventClosed(reason);
                }
            }
           
        }
         
        #region Property

        /// <summary>
        /// 是否连接上
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (_socket == null)
                    return false;

                return _socket.Connected;
            }
        }

        /// <summary>
        /// 获取被解析好的地址
        /// </summary>
        public string Address
        {
            get {
                return _address; 
            }
        }

        #endregion

    }

}

