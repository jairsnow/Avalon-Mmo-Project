using UnityEngine;
using MySql.Data.MySqlClient;

public class mysql
{
    /*
    private MySqlConnection connection;
    private string server;
    private string database;
    private string username;
    private string password;

    public mysql()
    {
        Initialize();
    }

    private void Initialize()
    {
        server = mysqlPswTrue.server;
        database = mysqlPswTrue.database;
        username = mysqlPswTrue.username;
        password = mysqlPswTrue.password;

        string connectionString;

        connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + username + ";" + "PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);
        this.OpenConnection();
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
            return false;
        }
    }

    public bool CloseConnection()
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
                Debug.Log(dataReader["username"]);
            }

            //close Data Reader
            dataReader.Close();

            //close Connection
            this.CloseConnection();
        }
        else
        {
            Debug.Log("You are not connected to the Databases!");
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
        return result;
    }
    */
}
