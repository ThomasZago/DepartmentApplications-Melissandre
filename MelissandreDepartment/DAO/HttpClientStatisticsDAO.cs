using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelissandreDepartment.DAO
{
    public class HttpClientStatisticsDAO
    {
        private static HttpClientStatisticsDAO _instance;
        private static readonly object _lockObject = new object();

        public static HttpClientStatisticsDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new HttpClientStatisticsDAO();
                        }
                    }
                }
                return _instance;
            }
        }

        private HttpClientStatisticsDAO()
        {

        }
    }
}
