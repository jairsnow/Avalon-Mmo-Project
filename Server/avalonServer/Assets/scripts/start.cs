using UnityEngine;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;
using System;
using System.Text;
using LitJson;

public class start : MonoBehaviour {
	
	private List<Socket> _multiClientSocket = new List<Socket>(); 
	private Socket _serverSocket;
	private byte[] _buffer;
	
	public bool autoStart;

	void Start() {
		if(autoStart) {
			startServer();
		}
	}

	private void startServer() {
		try
		{
			_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_serverSocket.Bind(new IPEndPoint(IPAddress.Any, 3333));
			_serverSocket.Listen(0);
			_buffer = new byte[1024];
			_serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
			getStartupIPMessage();
        }
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	private void getStartupIPMessage() {
		IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList)
		{
			if (ip.AddressFamily.ToString() == "InterNetwork")
			{
				//localIP = ip.ToString();
				AppendTextBox("Server start on IP: " + ip.ToString() + " \r\n ");
			}
		}
	}

	private void AcceptCallback(IAsyncResult AR)
	{
		
		try
		{
			// Keep data for current socket
			Socket socket = _serverSocket.EndAccept(AR);
			_multiClientSocket.Add(socket);

            // Reinizialize listening
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
			socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
			
		}
		catch (Exception ex)
        {
            Debug.Log(ex.Message);
		}
	}

	private void ReceiveCallback(IAsyncResult AR)
	{

        try
		{
            
            Socket socket = (Socket)AR.AsyncState;
			int received = socket.EndReceive(AR);
            
			byte[] copy = _buffer;
            Array.Resize(ref copy, received);

            baseRequest response = JsonMapper.ToObject<baseRequest>(Encoding.ASCII.GetString(copy));
            System.Object initializedClass = initializeCass(response.action, Encoding.ASCII.GetString(copy));
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
			
		}
		catch (Exception ex)
        {
            Debug.Log("User disconnected accidentally. ");
            Debug.Log(ex.Message);
        }

    }

    public System.Object initializeCass(string className, string revicedObj) {

        // System.Object retunedClass = new System.Object();
        baseClass retunedClass = new baseClass();

        try {
            Type elementType = Type.GetType(className, true);

            if (elementType != null)
            {
                retunedClass = (baseClass) (Activator.CreateInstance(elementType));
                string feedback = retunedClass.init(revicedObj);
                Debug.Log(feedback);
            }
        }   
		catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.Log("The client request this class, but dosen't exist!");
        }

        return retunedClass;    
    }

	private void AppendTextBox(string text)
	{
		// MUST ADD INVOKE METHOD FOR MULTITREADING
		Debug.Log (text + "\r\n");
		
	}

}

public class baseRequest
{
    public string action;
}