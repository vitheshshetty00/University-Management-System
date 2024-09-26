using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace University_Management_System.Entities
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }

        public override string ToString()
        {
            return $"{Street}, {City}, {State}, {PinCode}";
        }
    }
}
