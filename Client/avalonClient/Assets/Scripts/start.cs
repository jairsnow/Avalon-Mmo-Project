using UnityEngine;
using System.Net.Sockets;
using LitJson;
using UnityEngine.UI;
using socketNetworking;
using client;
using Assets.Scripts.lib;

public class start : MonoBehaviour {
    
    public string serverIpAddress;
    public InputField usernameValue;
    public InputField passwordValue;

    private Socket _clientSocket;
	Net net = new Net();
    
    public void loginButtonClick() {

        Net net = new Net();

        updateLogStatus("Try to enstablihed a connection to " + serverIpAddress);
        if (!net.connect(serverIpAddress)) {
            updateLogStatus("Impossible to connect to " + serverIpAddress);
            return;
        }

        updateLogStatus("Connected to: " + serverIpAddress+" try to login. ");
        
        login login = new login();
        login.username = usernameValue.text;
        login.password = passwordValue.text;

        string json = JsonMapper.ToJson(login);

        if (!net.send(json)) {
            updateLogStatus("Unable to login to: " + serverIpAddress +" wrong username/password");
        }
        
        
    }

    void updateLogStatus(string statusChange) {
        Text myText = GameObject.Find("txtLogConnectionStatus").GetComponent<Text>();
        myText.text = statusChange;
    }
    
}

