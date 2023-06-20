using MelissandreDepartment.Model;
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
    /// Logique d'interaction pour SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        private static SignInPage _instance;
        private static readonly object _lockObject = new object();

        private SignInPage()
        {
            InitializeComponent();
        }

        public static SignInPage Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new SignInPage();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
