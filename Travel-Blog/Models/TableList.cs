using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel_Blog.Models
{
    public class TableList
    {
        public List<TBLBLOGS> SliderBlogs { get; set; }

        public TBLBLOGS TrendingMainBlog { get; set; }
        public List<TBLBLOGS> TrendingCol1Blogs { get; set; }
        public List<TBLBLOGS> TrendingCol2Blogs { get; set; }
        public List<TBLBLOGS> TrendingListBlogs { get; set; }

        public TBLBLOGS LifestyleMainBlog { get; set; }
        public List<TBLBLOGS> LifestyleLeftSmallBlogs { get; set; }
        public List<TBLBLOGS> LifestyleCol1Blogs { get; set; }
        public List<TBLBLOGS> LifestyleCol2Blogs { get; set; }
        public List<TBLBLOGS> LifestyleRightListBlogs { get; set; }

        public TBLBLOGS SportMainBlog { get; set; }
        public List<TBLBLOGS> SportBottomLeftBlogs { get; set; }
        public TBLBLOGS SportBottomRightBlog { get; set; }
        public List<TBLBLOGS> SportSidebarBlogs { get; set; }

        public List<TBLADMIN> AdminList { get; set; }
        public List<TBLCATEGORY> CategoryList { get; set; }
        public List<TBLBLOGS> BlogsList { get; set; }
        public IPagedList<TBLBLOGS> PagedBlogsList { get; set; }
        public string ActiveCategoryName { get; set; }
    }
}