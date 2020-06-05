using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolftechApi.Models
{
    public class LoadingClass
    {
        public int OID { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public int? DepartmentParent_OID { get; set; }
    }
}
