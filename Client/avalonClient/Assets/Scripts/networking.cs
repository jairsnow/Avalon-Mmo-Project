using LitJson;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace socketNetworking
{

    class Networking
    {

        private Socket _clientSocket;
        public Action connectFail;
        public Action connectSucceful;
        
        public bool connect()
        {
            try
            {
                
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _clientSocket.BeginConnect(new IPEndPoint(IPAddress.Loopback, 3333), new AsyncCallback(ConnectCallback), null);
            }
            catch (Exception ex)
            {
                connectFail();
                Debug.Log(ex.Message);
            }
          
            return false;
        }

        private void ConnectCallback(IAsyncResult AR)
        {
            try
            {
                _clientSocket.EndConnect(AR);
                connectSucceful();
            }
            catch (Exception ex)
            {
                connectFail();
                Debug.Log(ex.Message);
            }
        }

        public singleQueque send(genericRequest json)
        {
            Action noFeedback = () => { };
            return this.send(json, noFeedback);
        }

        public singleQueque send(genericRequest json, Action feedback)
        {
            singleQueque singleQueque = new singleQueque();

            try
            {

                byte[] _buffer = Encoding.ASCII.GetBytes(JsonMapper.ToJson(json));
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                
                singleQueque.startMessageTimestamp = unixTimestamp;
                singleQueque.action = json.action;
                singleQueque.status = false;
                singleQueque.feedback = feedback;
                
                globalqueque.queque.Add(singleQueque);
                _clientSocket.BeginSend(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(recieveCallback), singleQueque);
            }
            catch (SocketException) { } // Server Close
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            return singleQueque;
        }

        private void recieveCallback(IAsyncResult AR)
        {

            singleQueque singleQueque = (singleQueque) AR.AsyncState;
            singleQueque.status = true;

            try
            {
                _clientSocket.EndReceive(AR);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        
    }
}