using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedZipCodePattern.Models
{
    public class NamedZipCodePattern
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pattern { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
