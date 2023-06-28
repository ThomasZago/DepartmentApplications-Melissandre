using MelissandreDepartment.Tool;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MelissandreDepartment.ViewModel
{
    /*public class CommercialDashboardViewModel : INotifyPropertyChanged
    {
        private double waitingPercentage;
        private double acceptedPercentage;
        private double onWayPercentage;
        private int transactionCount;
        private ObservableCollection<double> transactionData;
        private readonly IDAO dao;

        public event PropertyChangedEventHandler? PropertyChanged;

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

        public ObservableCollection<double> TransactionData
        {
            get { return transactionData; }
            set
            {
                transactionData = value;
                OnPropertyChanged(nameof(TransactionData));
            }
        }

        public ICommand GetDataCommand { get; }

        public DashboardViewModel()
        {
            dao = new YourDAO(); // Replace YourDAO with the actual implementation of your DAO class

            GetDataCommand = new RelayCommand(async () => await GetData(), () => true);

            TransactionData = new ObservableCollection<double>();

            // Start the timer to fetch data every 30 seconds
            var timer = new System.Timers.Timer(30000);
            timer.Elapsed += async (sender, e) => await GetData();
            timer.Start();
        }

        private async Task GetData()
        {
            try
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
            }
        }
    }*/
}

