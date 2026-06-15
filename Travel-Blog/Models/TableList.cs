using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel_Blog.Models
{
    public class TableList
    {
        public List<TBLADMIN> AdminList { get; set; }
        public List<TBLCATEGORY> CategoryList { get; set; }
        public List<TBLBLOGS> BlogsList { get; set; }
        public string ActiveCategoryName { get; set; }
    }
}