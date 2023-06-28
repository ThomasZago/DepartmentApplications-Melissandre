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
            DeleteAccountCommand = new RelayCommand((o) => DeleteAccount(o));
            GetClientCommand.Execute(this);
        }

        private async Task GetClient()
        {
            try
            {
                List<string> enumStringValues = new List<string>();

                foreach (object value in Enum.GetValues(typeof(ClientAccountType)))
                {
                    enumStringValues.Add(value.ToString());
                }
                (bool success, string message, List<Account> clients) = await userDAO.GetClients(enumStringValues);

                if (success)
                {
                    // Handle the retrieved clients as needed
                    foreach (ClientAccount client in clients)
                    {
                        Accounts.Add(client);
                    }
                }
                Message = message;
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
                (bool success, string message) = await userDAO.RegisterAccount(FullNameFormParameter, EmailFormParameter, accountType.ToString(), generatedPassword);
                Message = message;
                if (success)
                {
                    ClientAccount account = new ClientAccount
                    {
                        FullName = FullNameFormParameter,
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
            return RoleFormParameter.HasValue && !String.IsNullOrEmpty(FullNameFormParameter) && IsValidEmail(EmailFormParameter);
        }

        protected async void DeleteAccount(object parameter)
        {
            ClientAccount account = parameter as ClientAccount;
            if (account != null)
            {
                //Implement DAO logic
                (bool success, string message) = await HttpClientUserDAO.Instance.DeleteAccount(account.Email, account.Role.ToString());

                if (success)
                {
                    Accounts.Remove(account);
                }

                Message = message;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
