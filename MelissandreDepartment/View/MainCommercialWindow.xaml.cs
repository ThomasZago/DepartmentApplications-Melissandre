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
    /// Logique d'interaction pour MainCommercialWindow.xaml
    /// </summary>
    public partial class MainCommercialWindow : Window
    {
        private static MainCommercialWindow instance;
        private static readonly object lockObject = new object();

        private MainCommercialWindow()
        {
            InitializeComponent();
            InitializeNavigation();
        }

        public static MainCommercialWindow Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new MainCommercialWindow();
                        }
                    }
                }
                return instance;
            }
        }

        private void InitializeNavigation()
        {
            Navigation.Instance.Initialize(mainFrame);
            Navigation.Instance.NavigateTo("SignInPage");
        }
    }
}
