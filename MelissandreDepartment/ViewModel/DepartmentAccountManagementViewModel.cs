using MelissandreDepartment.Model;
using MelissandreDepartment.Tool;
using MelissandreServiceLibrary.Enum;
using System;

namespace MelissandreDepartment.ViewModel
{
    public class DepartmentAccountManagementViewModel : AccountManagementViewModel
    {
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
        }

        public RelayCommand AddAccountCommand { get; set; }

        private async void AddAccount()
        {
            DepartmentAccountType accountType = RoleFormParameter.Value;
            string generatedPassword = GeneratePassword();

            try
            {
                (bool success, string message) = await userDAO.RegisterAccount(EmailFormParameter, accountType.ToString(), generatedPassword);
                Message = message;

                if (success)
                {
                    DepartmentAccount account = new DepartmentAccount
                    {
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
            return RoleFormParameter.HasValue && IsValidEmail(EmailFormParameter);
        }
    }
}
