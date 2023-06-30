using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows.Data;
using MelissandreDepartment.Model; // Assuming the Account class is in the MelissandreDepartment.Model namespace
using MelissandreDepartment.Tool;
using MelissandreServiceLibrary.Enum;

namespace MelissandreDepartment.ViewModel
{
    public class ServiceAccountManagementViewModel : INotifyPropertyChanged
    {
        public RelayCommand AddAccountCommand { get; set; }
        public RelayCommand DeleteAccountCommand { get; set; }
        public RelayCommand SendNewPasswordCommand { get; set; }
        public RelayCommand ActivateAccountsCommand { get; set; }
        public RelayCommand DeactivateAccountsCommand { get; set; }

        private static ServiceAccountManagementViewModel instance;
        public static ServiceAccountManagementViewModel Instance
        {
            get
            {
                if (instance == null)
                    instance = new ServiceAccountManagementViewModel();
                return instance;
            }
        }

        private String emailFormParameter;
        public String EmailFormParameter
        {
            get { return String.IsNullOrEmpty(emailFormParameter)? String.Empty : emailFormParameter; }
            set
            {
                emailFormParameter = value;
                OnPropertyChanged(nameof(EmailFormParameter));
            }
        }

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

        private ObservableCollection<DepartmentAccount> accounts;
        public ObservableCollection<DepartmentAccount> Accounts
        {
            get { return accounts; }
            set
            {
                accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        private ICollectionView accountsView;
        public ICollectionView AccountsView
        {
            get { return accountsView; }
            set
            {
                accountsView = value;
                OnPropertyChanged(nameof(AccountsView));
            }
        }

        private ServiceAccountManagementViewModel()
        {
            // Initialize the accounts collection and view
            Accounts = new ObservableCollection<DepartmentAccount>();
            AccountsView = CollectionViewSource.GetDefaultView(Accounts);
            AddAccountCommand = new RelayCommand(o => AddAccount(), o => CanAddAccount());
            SendNewPasswordCommand = new RelayCommand((o) => SendNewPassword(o));
            DeleteAccountCommand = new RelayCommand((o) => DeleteAccount(o)); 
            ActivateAccountsCommand = new RelayCommand(o => ActivateAccounts(), o => CanActivateOrDeactivateAccounts());
            DeactivateAccountsCommand = new RelayCommand(o => DeactivateAccounts(), o => CanActivateOrDeactivateAccounts());

            //Only for test purpose
            DepartmentAccount account = new DepartmentAccount
            {
                Email = "thom.zago@gmail.com",
                Id= 1,
                Role = DepartmentAccountType.technical,
                Status = AccountStatus.Active
            };
            Accounts.Add(account);
            account = new DepartmentAccount
            {
                Email = "thom.zago2@gmail.com",
                Id = 1,
                Role = DepartmentAccountType.commercial,
                Status = AccountStatus.Active
            };
            Accounts.Add(account);
        }
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
        private bool IsValidEmail(string email)
        {
            String pattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
            return Regex.IsMatch(email, pattern);
        }

        private void SendNewPassword(object parameter)
        {
            DepartmentAccount account = parameter as DepartmentAccount;
            if (account != null)
            {
                //Implement DAO logic
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void DeleteAccount(object parameter)
        {
            DepartmentAccount account = parameter as DepartmentAccount;
            if (account != null)
            {
                //Implement DAO logic
                Accounts.Remove(account);
            }
            else
            { 
                throw new NotImplementedException();
            }
        }

        private void ActivateAccounts()
        {
            List<DepartmentAccount> selectedAccounts = GetSelectedAccounts();
            //Implement DAO logic
            foreach (DepartmentAccount account in selectedAccounts)
            {
                account.Status = AccountStatus.Active;
            }
            AccountsView.Refresh();
        }

        private void DeactivateAccounts()
        {
            List<DepartmentAccount> selectedAccounts = GetSelectedAccounts();
            //Implement DAO logic
            foreach (DepartmentAccount account in selectedAccounts)
            {
                account.Status = AccountStatus.Inactive;
            }
            AccountsView.Refresh();
        }

        private bool CanActivateOrDeactivateAccounts()
        {
            return GetSelectedAccounts().Any();
        }

        private List<DepartmentAccount> GetSelectedAccounts()
        {
            List<DepartmentAccount> selectedAccounts = new List<DepartmentAccount>();

            foreach (DepartmentAccount account in Accounts)
            {
                if (account.IsSelected)
                {
                    selectedAccounts.Add(account);
                }
            }

            return selectedAccounts;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
