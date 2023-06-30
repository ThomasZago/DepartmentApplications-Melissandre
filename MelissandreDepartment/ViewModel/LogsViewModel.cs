using MelissandreDepartment.DAO;
using MelissandreDepartment.Model;
using MelissandreDepartment.Tool;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MelissandreDepartment.ViewModel
{
    public class LogsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<DockerContainerModel> logs;
        public ObservableCollection<DockerContainerModel> Logs
        {
            get { return logs; }
            set
            {
                if (logs != value)
                {
                    logs = value;
                    OnPropertyChanged(nameof(Logs));
                }
            }
        }
        public DockerContainerModel SelectedLog { get; set; }
        public RelayCommand GetLogCommand { get; private set; }

        private readonly PowershellDockerDAO _dockerDAO;

        private static LogsViewModel _instance;
        private static readonly object _lockObject = new object();

        public static LogsViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new LogsViewModel();
                        }
                    }
                }
                return _instance;
            }
        }

        private LogsViewModel()
        {
            Logs = new ObservableCollection<DockerContainerModel>();
            _dockerDAO = PowershellDockerDAO.Instance;
            GetLogCommand = new RelayCommand(async (param) => await GetLog(param), (param) => CanGetLog(param));
            Logs.Add(new DockerContainerModel { ContainerName = "backend-melissandre-grafana-1" });
            Logs.Add(new DockerContainerModel { ContainerName = "backend-melissandre-order-tracking-1" });
            Logs.Add(new DockerContainerModel { ContainerName = "backend-melissandre-order-1" });
            Logs.Add(new DockerContainerModel { ContainerName = "backend-melissandre-traefik-1" });
            Logs.Add(new DockerContainerModel { ContainerName = "backend-melissandre-prometheus-1" });
            Logs.Add(new DockerContainerModel { ContainerName = "backend-melissandre-authentication-1" });
            // Call GetLog method for each DockerContainerModel in the Logs collection
            foreach (var dockerContainerModel in Logs)
            {
                GetLogCommand.Execute(dockerContainerModel);
            }
        }

        private bool CanGetLog(object param)
        {
            return param is DockerContainerModel selectedLog && !string.IsNullOrEmpty(selectedLog.ContainerName);
        }

        private async Task GetLog(object param)
        {
            if (param is DockerContainerModel selectedLog)
            {
                string containerName = selectedLog.ContainerName;
                string logContent = _dockerDAO.GetContainerLog(containerName);

                selectedLog.LogContent = logContent;
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
