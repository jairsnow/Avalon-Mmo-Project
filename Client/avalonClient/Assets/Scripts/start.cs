using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using LitJson;
using UnityEngine.UI;
using utility;
using socketNetworking;

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
    tcpConnectionStatus connctionStatus = tcpConnectionStatus.none;
    Networking net = new Networking();

    private class loginRequest : genericRequest {
        public string username;
        public string password;
    }
    
    public void loginButtonClick() {
        
        loginRequest loginRequest = new loginRequest();

        loginRequest.username = GameObject.Find("usernameValue").GetComponent<InputField>().ToString();
        loginRequest.password = crypt.sha512(GameObject.Find("passwordValue").GetComponent<InputField>().ToString());
        loginRequest.action = "login";

        connctionStatus = tcpConnectionStatus.tryConnect;

        net.connectSucceful = () =>
        {
            connctionStatus = tcpConnectionStatus.connected;
            
            singleQueque queque = net.send(loginRequest);
            queque.feedback = () =>
            {
                Debug.Log("OMG i try to login trought the Networking class!! ");
            };

        };

        net.connectFail = () =>
        {
            connctionStatus = tcpConnectionStatus.failToConnect;
        };
        
        net.connect();
    }

    void Update()
    {
        globalqueque.resolve();
        
        Text connectionInformer = GameObject.Find("connectionInformer").GetComponent<Text>();

        if (connctionStatus == tcpConnectionStatus.tryConnect) { connectionInformer.text = "Try to connect to the server"; }
        else if (connctionStatus == tcpConnectionStatus.connected) { connectionInformer.text = "Connected to {IP}"; }
        else if (connctionStatus == tcpConnectionStatus.failToConnect) { connectionInformer.text = "Fail to connect to the server: {IP}"; }
        else if (connctionStatus == tcpConnectionStatus.tryLogin) { connectionInformer.text = "Try to log into the server"; }
        else if (connctionStatus == tcpConnectionStatus.logged) { connectionInformer.text = "Login Succeful"; }
        else if (connctionStatus == tcpConnectionStatus.failLogin) { connectionInformer.text = "Login fail. Username or Password incorrect"; }
        
    }

}


/*
private void sendMessage()
{

    try
    {
        loginRequest loginRequest = new loginRequest();

        loginRequest.username = GameObject.Find("usernameValue").GetComponent<InputField>().ToString();
        loginRequest.password = crypt.sha512(GameObject.Find("passwordValue").GetComponent<InputField>().ToString());

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
*/
