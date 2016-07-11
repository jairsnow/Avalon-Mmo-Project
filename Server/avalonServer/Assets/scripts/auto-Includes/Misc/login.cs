using LitJson;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class login : baseClass {
    
    public override string init(string json)
    {   
		string loggedUserID;
        recieveloginRquest request = JsonMapper.ToObject<recieveloginRquest>(json);

        mysql sql = new mysql();

        MySqlDataReader result = sql.query("SELECT id FROM login WHERE username='" + request.username + "' AND password='" + request.password + "'");

        int findOneResult = 0;
        while (result.Read())
        {
            findOneResult++;
			loggedUserID = result["id"].ToString();
        }

        recieveLoginResponse response = new recieveLoginResponse();

        if (findOneResult == 1)
        {
            response.response = 1;
            response.message = "Succeful login attempt by '" + request.username + "'";
            // selectCharacterList(1);
        }
		else if(findOneResult > 1)
        {
            response.response = 0;
            response.message = "More than 1 account are found with this credential. Contact the admin to assistence.";
        }
        else
        {
            response.response = 0;
            response.message = "Failed login attempt by ==> '" + request.username + "'";
        }

        //close Data Reader
        sql.CloseConnection();

        return JsonMapper.ToJson(response);
    }
    
	/*
	private charcterStructure selectCharacterList(int idUser) {

		List<charcterStructure> characterList = new List<charcterStructure>();
		mysql sql = new mysql();

		MySqlDataReader characterListQuery = sql.query("SELECT id, name, classIS, sex, skinColor FROM characters WHERE loginID='"+ idUser +"'");
		MySqlDataReader maxCharacterAllow = sql.query("SELECT value FROM generalParameters WHERE varName='maxCharacterAllow'");

		return characterList;

	}
	*/ 

    private class recieveloginRquest : baseRequest
    {
        public string username;
        public string password;
    }

    private class recieveLoginResponse {
        public int response { get; set; }
        public string message { get; set; }
    }

	private class characterList
    {
        int response;
        List<charcterStructure> charcter = new List<charcterStructure>();
    }

	private class charcterStructure {
        int id;
        string name;
        int classID;
        byte sex;

        string skinColor;
    }

}
