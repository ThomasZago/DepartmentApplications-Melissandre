using MelissandreDepartment.Tool;
using MelissandreDepartment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MelissandreDepartment.View
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            InitializeNavigation();
            SignInViewModel.Instance.LoginSuccess += SignInViewModel_LoginSuccess;
        }

        private void InitializeNavigation()
        {
            Navigation.Instance.Initialize(mainFrame);
            Navigation.Instance.NavigateTo("SignInPage");
        }
        private void SignInViewModel_LoginSuccess(object sender, string role)
        {
            if (role == "Technical")
            {
                MainTechnicalWindow.Instance.Show();
                Close();
            }
        }
    }
}

