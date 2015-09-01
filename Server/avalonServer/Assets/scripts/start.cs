using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using LitJson;

class Sql
{

    private MySqlConnection connection;
    private string server;
    private string database;
    private string uid;
    private string password;

    public Sql()
    {
        Initialize();
    }

    private void Initialize()
    {
		server = "192.168.1.4";
        database = "avalon";
        uid = "root";
        password = "";
        
		string connectionString;
        
		connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);
    }
	
    private bool OpenConnection()
    {
        try
        {
            connection.Open();
            return true;
        }
        catch (MySqlException ex)
        {
            switch (ex.Number)
            {
                case 0:
                    Debug.Log("Cannot connect to server.  Contact administrator");
                    break;

                case 1045:
                    Debug.Log("Invalid username/password, please try again");
                    break;
            }
			Debug.Log(ex.Number);
            return false;
        }
    }

    private bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.Log(ex.Message);
            return false;
        }
    }
    public void Insert()
    {
        string query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')";

        //open connection
        if (this.OpenConnection() == true)
        {
            //create command and assign the query and connection from the constructor
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //Execute command
            cmd.ExecuteNonQuery();

            //close connection
            this.CloseConnection();
        }
    }

    public void Update()
    {
        string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

        //Open connection
        if (this.OpenConnection() == true)
        {
            //create mysql command
            MySqlCommand cmd = new MySqlCommand();
            //Assign the query using CommandText
            cmd.CommandText = query;
            //Assign the connection using Connection
            cmd.Connection = connection;

            //Execute query
            cmd.ExecuteNonQuery();

            //close connection
            this.CloseConnection();
        }
    }

    public void Delete()
    {
        string query = "DELETE FROM tableinfo WHERE name='John Smith'";

        if (this.OpenConnection() == true)
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            this.CloseConnection();
        }
    }

    public void Select()
    {
        string query = "SELECT * FROM login";

        //Open connection
        if (this.OpenConnection() == true)
        {
            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
				Debug.Log (dataReader["username"]);
            }

            //close Data Reader
            dataReader.Close();

            //close Connection
            this.CloseConnection();
        }
        else
        {
			Debug.Log ("You are not connected to the Databases!");
        }
    }

    public int Count()
    {
        string query = "SELECT Count(*) FROM tableinfo";
        int Count = -1;

        //Open Connection
        if (this.OpenConnection() == true)
        {
            //Create Mysql Command
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //ExecuteScalar will return one value
            Count = int.Parse(cmd.ExecuteScalar() + "");

            //close Connection
            this.CloseConnection();

            return Count;
        }
        else
        {
            return Count;
        }
    }


    public MySqlDataReader query(string sql)
    {

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader result = cmd.ExecuteReader();
        this.CloseConnection();

        return result;
    }

}


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

    /*
	void OnGUI() {
		if (autoStart == false) {
			if (GUI.Button (new Rect (100, 100, 100, 25), "Start Server")) {
				autoStart = true;
				startServer ();
			}
		} else {
			GUI.Label (new Rect (200, 100, 100, 20), "Server Started!");
		}
	}
    */

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
			AppendTextBox(" \r\n <" + _multiClientSocket.Count.ToString() + ">");
			
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
            Debug.Log("Mmh.. start debug");
            
			byte[] copy = _buffer;
            Array.Resize(ref copy, received);
            revideloginResposne response = JsonMapper.ToObject<revideloginResposne>(Encoding.ASCII.GetString(copy));

            Debug.Log(response.username +" "+ response.password);
            /*
            Sql sql = new Sql();
            MySqlDataReader result = sql.query("SELECT id FROM login WHERE username='"+ response.username + "' AND password='" + response.password + "'");
            
            //Read the data and store them in the list
            while (result.Read())
            {
                Debug.Log(result["id"]);
            }

            Debug.Log("Dopo query");
            
            //close Data Reader
            // result.Close();

            // AppendTextBox(socket.GetHashCode() + " says: " + text);
            */
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
			
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

    public class revideloginResposne
    {
        public string action;
        public string username;
        public string password;
    }

	private void AppendTextBox(string text)
	{
		// MUST ADD INVOKE METHOD FOR MULTITREADING
		Debug.Log (text + "\r\n");
		
	}

}
