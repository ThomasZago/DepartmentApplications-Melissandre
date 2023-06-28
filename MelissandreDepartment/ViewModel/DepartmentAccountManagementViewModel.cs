using MelissandreDepartment.DAO;
using MelissandreDepartment.Model;
using MelissandreDepartment.Tool;
using MelissandreServiceLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MelissandreDepartment.ViewModel
{
    public class DepartmentAccountManagementViewModel : AccountManagementViewModel
    {
        public RelayCommand GetClientCommand { get; set; }
        public RelayCommand AddAccountCommand { get; set; }
        public RelayCommand DeleteAccountCommand { get; set; }

        private DepartmentAccountType? roleFormParameter;
        public DepartmentAccountType? RoleFormParameter
        {
            get { return roleFormParameter; }
            set
            {
                roleFormParameter = value;
                OnPropertyChanged(nameof(RoleFormParameter));
            }
        }

        private static DepartmentAccountManagementViewModel instance;
        public static DepartmentAccountManagementViewModel Instance
        {
            get
            {
                if (instance == null)
                    instance = new DepartmentAccountManagementViewModel();
                return instance;
            }
        }

        private DepartmentAccountManagementViewModel()
        {
            AddAccountCommand = new RelayCommand((o) => AddAccount(), (o) => CanAddAccount());
            GetClientCommand = new RelayCommand((o) => GetClient());
            DeleteAccountCommand = new RelayCommand((o) => DeleteAccount(o));
            GetClientCommand.Execute(this);
        }


        private async void AddAccount()
        {
            DepartmentAccountType accountType = RoleFormParameter.Value;
            string generatedPassword = GeneratePassword();

            try
            {
                (bool success, string message) = await userDAO.RegisterAccount(FullNameFormParameter, EmailFormParameter, accountType.ToString(), generatedPassword);
                Message = message;

                if (success)
                {
                    DepartmentAccount account = new DepartmentAccount
                    {
                        FullName = FullNameFormParameter,
                        Email = EmailFormParameter,
                        Role = accountType,
                        Status = AccountStatus.Active
                    };
                    Accounts.Add(account);
                }
            }

            catch (Exception ex)
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
            DepartmentAccount account = parameter as DepartmentAccount;
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

        private async Task GetClient()
        {
            try
            {
                List<string> enumStringValues = new List<string>();

                foreach (object value in Enum.GetValues(typeof(DepartmentAccountType)))
                {
                    enumStringValues.Add(value.ToString());
                }
                (bool success, string message, List<Account> clients) = await userDAO.GetClients(enumStringValues);

                if (success)
                {
                    // Handle the retrieved clients as needed
                    foreach (DepartmentAccount client in clients)
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
    }
}
