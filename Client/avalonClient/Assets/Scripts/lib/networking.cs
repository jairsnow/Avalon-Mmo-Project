using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Assets.Scripts.lib {

    class Net {

        private string _ip;
        private bool _connected;
        private Socket _clientSocket;


        #region base functions

        public Net() {
            _connected = false;
        }

        public string ip { get { return _ip; } }
        #endregion

        public bool connect(string ip) {

            // Set the local variabile
            if (_ip != null && _ip.Length != 0 && _connected == false) {
                _ip = ip;
            }
            
            if(_connected) {
                MessageBox.Show("You are already connected to " + _ip);
                return false;
            }
            
            return bindServer(ip);
            
		}

		public bool bindServer(string ip) {
            
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//             IAsyncResult result = _clientSocket.BeginConnect(new IPEndPoint(IPAddress.Loopback, 3333), new AsyncCallback(ConnectCallback), null);
            IAsyncResult result = _clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), 3333), null, null);
            
            bool success = result.AsyncWaitHandle.WaitOne(5000, true);

            if (!success) {
                return false;
            }
            else {
                if (_clientSocket.Connected) {
                    _connected = true;
                    return true;
                }
                return false;
            }

        }

        public bool send(string json) {

            UnityEngine.Debug.Log("send --> "+json);

            byte[] jsonByte = Encoding.ASCII.GetBytes(json);
            _clientSocket.BeginSend(jsonByte, 0, jsonByte.Length, 0, null, _clientSocket);

            return false;

        }

		private void ConnectCallback(IAsyncResult AR) {
			if (_clientSocket.Connected) {
				_connected = true;
			}
		}
        
    }
    
}