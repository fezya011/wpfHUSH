using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfHUSH.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
        public byte[] Photo { get; set; }
        public int LoginPasswordId { get; set; }
        public LoginPassword LoginPassword1 { get; set; }

    }

}

       
