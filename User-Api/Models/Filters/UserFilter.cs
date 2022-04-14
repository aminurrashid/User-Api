using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Api.Models.Filters
{
    public class UserFilter
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string SortingField { get; set; }
        public int SortOrder { get; set; }
    }
}
