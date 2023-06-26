using MelissandreDepartment.Tool;
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
using System.Windows.Shapes;

namespace MelissandreDepartment.View
{
    /// <summary>
    /// Logique d'interaction pour MainTechnicalWindow.xaml
    /// </summary>
    public partial class MainTechnicalWindow : Window
    {
        public MainTechnicalWindow()
        {
            InitializeComponent();
            InitializeNavigation();
        }

        private void InitializeNavigation()
        {
            Navigation.Instance.Initialize(mainFrame);
            Navigation.Instance.NavigateTo("ServiceAccountManagementView");
        }
    }
}
