using System;

namespace MelissandreDepartment.Model
{
    public class ConnectedUser
    {
        private static ConnectedUser _instance;
        private static readonly object _lockObject = new object();

        public String Email { get; set; }
        public String Password { get; set; }
        public String Role { get; set; }
        public String SessionToken { get; set; }

        private ConnectedUser()
        {

        }

        public static ConnectedUser Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConnectedUser();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
