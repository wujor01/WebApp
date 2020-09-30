using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { set; get; }
        public int DepartmentID { set; get; }
        public string UserName { set; get; }
        public string GroupID { set; get; }
    }
}