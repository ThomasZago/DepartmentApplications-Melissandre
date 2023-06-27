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
        private void AddAccount()
        {
            DepartmentAccount account = new DepartmentAccount
            {
                Email = EmailFormParameter,
                Role = (DepartmentAccountType)RoleFormParameter,
                Status = AccountStatus.Active
            };
            // implement dao logic
            Accounts.Add(account);
        }

        private bool CanAddAccount()
        {
            return RoleFormParameter.HasValue && IsValidEmail(EmailFormParameter);
        }
    }
}
