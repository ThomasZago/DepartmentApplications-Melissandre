using global::MelissandreDepartment.DAO;
using global::MelissandreDepartment.Model;
using global::MelissandreDepartment.Tool;
using MelissandreServiceLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MelissandreDepartment.ViewModel
{
    public class ClientAccountManagementViewModel : AccountManagementViewModel
    {
        
        public RelayCommand AddAccountCommand { get; set; }
        public RelayCommand GetClientCommand { get; set; }

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
            GetClientCommand = new RelayCommand(async o => await GetClient());
            AddAccountCommand = new RelayCommand((o) => AddAccount(), (o) => CanAddAccount());
            GetClientCommand.Execute(this);
        }

        private async Task GetClient()
        {
            try
            {
                (bool success, string message, List<ClientAccount> clients) = await userDAO.GetClients();

                if (success)
                {
                    // Handle the retrieved clients as needed
                    foreach (ClientAccount client in clients)
                    {
                        Accounts.Add(client);
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
        }

        protected async void AddAccount()
        {

            ClientAccountType accountType = RoleFormParameter.Value;
            string generatedPassword = GeneratePassword();
            try
            {
                (bool success, string message) = await userDAO.RegisterAccount(EmailFormParameter, accountType.ToString(), generatedPassword);
                Message = message;
                if (success)
                {
                    ClientAccount account = new ClientAccount
                    {
                        Email = EmailFormParameter,
                        Role = accountType,
                        Status = AccountStatus.Active
                    };
                    Accounts.Add(account);
                }
            }
            catch(Exception ex)
            {
                Message = ex.Message;
            }
            
        }


        private bool CanAddAccount()
        {
            return RoleFormParameter.HasValue && IsValidEmail(EmailFormParameter);
        }
    }
}
