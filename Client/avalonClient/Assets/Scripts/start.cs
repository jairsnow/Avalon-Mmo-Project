using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.Security.Cryptography;
using LitJson;

public class start : MonoBehaviour {
    
	public string serverIpAddress;
	private Socket _clientSocket;
	private string stringToEdit = "";
	private bool connectToserver = false;

	private class test {
		public string action;
		public string data;
	}

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            /*
            Selectable next = system.currentSelectedObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {

                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            //else Debug.Log("next nagivation element not found");
            */
        }
    }

    void OnGUI() {
        /*
        if (connectToserver) {
            
			if (GUI.Button (new Rect (10, 100, 190, 25), "Disconnect")) {

			}

			stringToEdit = GUI.TextArea(new Rect(10, 10, 300, 80), stringToEdit, 200);
			
			if (GUI.Button (new Rect (210, 100, 100, 25), "Send message")) {
				sendMessage();
				// Debug.Log (stringToEdit);
			}

		} else {

			if (GUI.Button (new Rect (10, 100, 190, 25), "Connect to: " + serverIpAddress)) {
                
                test test = new test();

                test.action = "aaa";
                test.data = "bbb";

                Debug.Log(JsonMapper.ToJson(test));
                test thomas = JsonMapper.ToObject<test>(JsonMapper.ToJson(test));

                Debug.Log(thomas.data);


                try
                {
					_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					_clientSocket.BeginConnect(new IPEndPoint(IPAddress.Loopback, 3333), new AsyncCallback(ConnectCallback), null);
				}
				catch (Exception ex)
				{
					Debug.Log(ex.Message);
				}
			}

		}
        */

	}

	private void ConnectCallback(IAsyncResult AR)
	{
		try {
			_clientSocket.EndConnect(AR);
			connectToserver = true;
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	private void sendMessage()
	{

		try
		{
			byte[] _buffer = Encoding.ASCII.GetBytes(stringToEdit);
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
