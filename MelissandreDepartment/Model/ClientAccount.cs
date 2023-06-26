using MelissandreServiceLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelissandreDepartment.Model
{
    internal class ClientAccount : Account
    {
        public ClientAccountType Role { get; set; }
    }
}
