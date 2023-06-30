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
using MelissandreDepartment.Model;
using MelissandreDepartment.ViewModel;

namespace MelissandreDepartment.View
{
    /// <summary>
    /// Logique d'interaction pour LogsView.xaml
    /// </summary>
    public partial class LogsView : Page
    {
        private static LogsView _instance;
        private static readonly object _lockObject = new object();

        private LogsView()
        {
            InitializeComponent();
        }

        public static LogsView Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new LogsView();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
