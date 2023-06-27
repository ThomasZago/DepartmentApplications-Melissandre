using MelissandreDepartment.DAO;
using MelissandreDepartment.Model;
using MelissandreDepartment.Tool;
using MelissandreDepartment.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace MelissandreDepartment.ViewModel
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private static SignInViewModel _instance;
        private static readonly object _lockObject = new object();
        private HttpClientUserDAO authDAO;
        public event EventHandler<string> LoginSuccess;
        public static SignInViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new SignInViewModel();
                        }
                    }
                }
                return _instance;
            }
        }
        private SignInViewModel()
        {
            authDAO = HttpClientUserDAO.Instance;
            SignInCommand = new RelayCommand((o) => SignIn(), (o) => CanSignIn());
        }

        public RelayCommand SignInCommand { get; set; }
        private string email;
        private string password;
        private string message;
        private string type;

        public string Type
        {
            get { return type; } 
            set 
            {
                if (type != value)
                {
                    type = value;
                    OnPropertyChanged(nameof(type));
                }
            }
        }

        public string Email
        {
            get { return string.IsNullOrEmpty(email)? String.Empty : email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Password
        {
            get { return string.IsNullOrEmpty(password) ? String.Empty : password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        /// <summary>
        /// Raises OnPropertychangedEvent when property changes
        /// </summary>
        /// <param name="name">String representing the property name</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private async void SignIn()
        {
            try
            {
                (bool success, string message) = await authDAO.Login(email, password, type);

                if (success)
                {
                    Message = $"Logged in successfully with role: {type}";
                    ConnectedUser.Instance.Email = email;
                    ConnectedUser.Instance.Password = password;
                    ConnectedUser.Instance.Role = type;
                    LoginSuccess?.Invoke(this, type);
                }
                else
                {
                    Message = message;
                }
            }
            catch (Exception ex)
            {
                Message = ($"An error occurred during login: {ex.Message}\nPlease send this to your administrator.");
            }
        }

        private bool CanSignIn()
        {
            return Password != null && IsValidEmail();
        }

        private bool IsValidEmail()
        {
            String pattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
            return Regex.IsMatch(Email, pattern);
        }
    }
}
