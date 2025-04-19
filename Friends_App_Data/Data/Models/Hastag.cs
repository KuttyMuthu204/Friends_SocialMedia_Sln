using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friends_App_Data.Data.Models
{
    public class Hastag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated{ get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
