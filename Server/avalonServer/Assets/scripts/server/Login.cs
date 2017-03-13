using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace server {

    public class login : client.login {

        public login() {

        }
        
        public override string init(string json) {
            
            login test = LitJson.JsonMapper.ToObject<login>(json);
            UnityEngine.Debug.Log("username: "+test.username+" password: "+test.password);
            return "";

        }
        
    }

}
