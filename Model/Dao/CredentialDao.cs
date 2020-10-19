using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CredentialDao
    {
        WebAppDbContext db = null;
        public CredentialDao()
        {
            db = new WebAppDbContext();
        }

        public long Insert(Credential entity)
        {
            int a = 0;
            string[] arrEmpId = string.Join(",", entity.SelectedIDRole).Replace(" ", "").Split(',');
            for (int i = 0; i < arrEmpId.Length; i++)
            {
                entity.RoleID = arrEmpId[i];
                db.Credentials.Add(entity);
                db.SaveChanges();
                a = 1;
            }
            return a;
        }

        public List<Role> ListAll()
        {
            return db.Roles.ToList();
        }
        public List<UserGroup> ListAllGroup()
        {
            return db.UserGroups.ToList();
        }
        public IEnumerable<Credential> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Credential> model = db.Credentials;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.UserGroupID.Contains(searchString) || x.RoleID.ToString().Contains(searchString)
                );
            }
            return model.OrderBy(x => x.UserGroupID).ToPagedList(page, pageSize);
        }
    }
}
