using MelissandreServiceLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelissandreDepartment.Model
{
    public abstract class Account
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public AccountStatus Status { get; set; }
        public bool IsSelected { get; set; }
    }
}
