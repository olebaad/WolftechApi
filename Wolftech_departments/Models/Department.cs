using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolftechApi.Models
{
    public class Department
    {
        public int Oid { get; set; }
        public string Title { get; set; }
        public int NumDecendants { get; set; }
        public string Color { get; set; }
        public List<Department> Departments { get; set; }

        public Department(LoadingClass data, List<Department> departments)
        {
            Oid = data.OID;
            Title = data.Title;
            Color = data.Color;
            Departments = departments;

            int count = 0;
            foreach(Department dpt in departments)
            {
                count = count + dpt.NumDecendants + 1;
            }

            NumDecendants = count;

        }

    }
}
