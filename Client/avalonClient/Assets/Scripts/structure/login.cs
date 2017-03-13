using System;

namespace client {

    public class login : baseClass {
        
        public string username;
        public string password;

        public bool result = false;

        public login() {
            className = "server.login";
        }

    }

}
