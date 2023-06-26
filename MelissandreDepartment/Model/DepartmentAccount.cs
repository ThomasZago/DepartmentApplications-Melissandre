using MelissandreServiceLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelissandreDepartment.Model
{
    public class DepartmentAccount : Account
    {
        public DepartmentAccountType Role { get; set; }
    }
}
