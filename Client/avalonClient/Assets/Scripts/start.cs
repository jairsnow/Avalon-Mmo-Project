using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.Security.Cryptography;
using LitJson;
using UnityEngine.UI;

public class start : MonoBehaviour {

    enum tcpConnectionStatus
    {
        none,
        tryConnect,
        connected,
        failToConnect,
        tryLogin,
        logged,
        failLogin
    };

    public string serverIpAddress;
	private Socket _clientSocket;
	private bool connectToserver = false;
    tcpConnectionStatus connctionStatus = tcpConnectionStatus.none;

    private class loginRequest {
		public string action = "loginRequest";
        public string username;
        public string password;
    }
    
    public void loginButtonClick() {

        connctionStatus = tcpConnectionStatus.tryConnect;
        try
        {
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientSocket.BeginConnect(new IPEndPoint(IPAddress.Loopback, 3333), new AsyncCallback(ConnectCallback), null);
        }
        catch (Exception ex)
        {
            connctionStatus = tcpConnectionStatus.failToConnect;
            Debug.Log(ex.Message);
        }

    }

    void Update()
    {

        Text connectionInformer = GameObject.Find("connectionInformer").GetComponent<Text>();

        if (connctionStatus == tcpConnectionStatus.tryConnect) { connectionInformer.text = "Try to connect to the server"; }
        else if (connctionStatus == tcpConnectionStatus.connected) { connectionInformer.text = "Connected to {IP}"; }
        else if (connctionStatus == tcpConnectionStatus.failToConnect) { connectionInformer.text = "Fail to connect to the server: {IP}"; }
        else if (connctionStatus == tcpConnectionStatus.tryLogin) { connectionInformer.text = "Try to log into the server"; }
        else if (connctionStatus == tcpConnectionStatus.logged) { connectionInformer.text = "Login Succeful"; }
        else if (connctionStatus == tcpConnectionStatus.failLogin) { connectionInformer.text = "Login fail. Username or Password incorrect"; }

    }



    private void ConnectCallback(IAsyncResult AR)
    {
        try {
            connctionStatus = tcpConnectionStatus.connected;
            _clientSocket.EndConnect(AR);
			connectToserver = true;
            sendMessage();
        }
		catch (Exception ex)
        {
            connctionStatus = tcpConnectionStatus.failToConnect;
            Debug.Log(ex.Message);
		}
	}

	private void sendMessage()
	{

		try
		{
            loginRequest loginRequest = new loginRequest();

            loginRequest.username = GameObject.Find("usernameValue").GetComponent<InputField>().ToString();
            loginRequest.password = GetCrypt(GameObject.Find("passwordValue").GetComponent<InputField>().ToString());

            string json = JsonMapper.ToJson(loginRequest);
            Debug.Log(json);

            byte[] _buffer = Encoding.ASCII.GetBytes(json);
			_clientSocket.BeginSend(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
		} 
		catch (SocketException) { } // Server Close
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	private void SendCallback(IAsyncResult AR)
	{
		try {
			_clientSocket.EndReceive(AR);
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	public static string GetCrypt(string text)
	{

		SHA512 shaM = new SHA512Managed();
		byte[] hash = shaM.ComputeHash(Encoding.ASCII.GetBytes(text));
		
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte b in hash)
		{
			stringBuilder.AppendFormat("{0:x2}", b);
		}
		return stringBuilder.ToString();
	
	}
}
