using global::MelissandreDepartment.DAO;
using global::MelissandreDepartment.Model;
using global::MelissandreDepartment.Tool;
using MelissandreDepartment.DAO;
using MelissandreDepartment.Model;
using MelissandreServiceLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MelissandreDepartment.ViewModel
{
    public class ClientAccountManagementViewModel : AccountManagementViewModel
    {
        private HttpClientUserDAO userDAO;

        public RelayCommand AddAccountCommand { get; set; }

        private ClientAccountType? roleFormParameter;
        public ClientAccountType? RoleFormParameter
        {
            get { return roleFormParameter; }
            set
            {
                roleFormParameter = value;
                OnPropertyChanged(nameof(RoleFormParameter));
            }
        }

        private static ClientAccountManagementViewModel instance;
        public static ClientAccountManagementViewModel Instance
        {
            get
            {
                if (instance == null)
                    instance = new ClientAccountManagementViewModel();
                return instance;
            }
        }
        private ClientAccountManagementViewModel()
        {
            userDAO = HttpClientUserDAO.Instance;
            //GetClientCommand = new RelayCommand(async o => await GetClient());
            AddAccountCommand = new RelayCommand((o) => AddAccount(), (o) => CanAddAccount());
        }

        /*private async Task GetClient()
        {
            try
            {
                (bool success, string message, List<ClientAccount> clients) = await userDAO.GetClient();

                if (success)
                {
                    // Handle the retrieved clients as needed
                    foreach (ClientAccount client in clients)
                    {
                        // Do something with the client object
                    }
                }
                else
                {
                    Message = message;
                }
            }
            catch (Exception ex)
            {
                Message = $"An error occurred while retrieving clients: {ex.Message}\nPlease send this to your administrator.";
            }
        }*/

        protected void AddAccount()
        {
            ClientAccount account = new ClientAccount
            {
                Email = EmailFormParameter,
                Role = RoleFormParameter.Value,
                Status = AccountStatus.Active
            };
            // implement DAO logic
            Accounts.Add(account);
        }

        private bool CanAddAccount()
        {
            return RoleFormParameter.HasValue && IsValidEmail(EmailFormParameter);
        }
    }
}
