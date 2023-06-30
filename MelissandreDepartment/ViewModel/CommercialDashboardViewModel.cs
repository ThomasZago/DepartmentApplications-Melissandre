using MelissandreDepartment.DAO;
using MelissandreDepartment.Tool;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MelissandreDepartment.ViewModel
{
    public class CommercialDashboardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private static CommercialDashboardViewModel _instance;
        private static readonly object _lockObject = new object();
        private double waitingPercentage;
        private double acceptedPercentage;
        private double onWayPercentage;
        private int transactionCount;
        private readonly HttpClientStatisticsDAO dao;

        public double WaitingPercentage
        {
            get { return waitingPercentage; }
            set
            {
                waitingPercentage = value;
                OnPropertyChanged(nameof(WaitingPercentage));
            }
        }

        public double AcceptedPercentage
        {
            get { return acceptedPercentage; }
            set
            {
                acceptedPercentage = value;
                OnPropertyChanged(nameof(AcceptedPercentage));
            }
        }

        public double OnWayPercentage
        {
            get { return onWayPercentage; }
            set
            {
                onWayPercentage = value;
                OnPropertyChanged(nameof(OnWayPercentage));
            }
        }

        public int TransactionCount
        {
            get { return transactionCount; }
            set
            {
                transactionCount = value;
                OnPropertyChanged(nameof(TransactionCount));
            }
        }

        public ICommand GetDataCommand { get; }


        public static CommercialDashboardViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new CommercialDashboardViewModel();
                        }
                    }
                }
                return _instance;
            }
        }

        private CommercialDashboardViewModel()
        {
            dao = HttpClientStatisticsDAO.Instance; // Replace YourDAO with the actual implementation of your DAO class

            GetDataCommand = new RelayCommand((o) => GetData());

            // Start the timer to fetch data every 30 seconds
            /*var timer = new System.Timers.Timer(30000);
            timer.Elapsed += async (sender, e) => GetDataCommand.Execute;
            timer.Start();*/
        }

        private async void GetData()
        {
            /*try
            {
                // Get the data from your DAO
                var data = await dao.GetDashboardData();

                WaitingPercentage = data.WaitingPercentage;
                AcceptedPercentage = data.AcceptedPercentage;
                OnWayPercentage = data.OnWayPercentage;
                TransactionCount = data.TransactionCount;

                TransactionData.Add(data.TransactionAmount);
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine($"An error occurred while retrieving data: {ex.Message}");
            }*/
        }

        /// <summary>
        /// Raises OnPropertychangedEvent when property changes
        /// </summary>
        /// <param name="name">String representing the property name</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

