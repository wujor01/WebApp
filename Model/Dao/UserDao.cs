using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
    public class UserDao
    {
        //Khai báo db
        WebAppDbContext db = null;
        public UserDao()
        {
            db = new WebAppDbContext();
        }
        //Thêm người dùng
        public long Insert(User entity)
        {
            db.User.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        //Cập nhật thông tin User
        public bool Update(User entity)
        {
            try
            {
                var user = db.User.Find(entity.ID);
                user.Email = entity.Email;
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.TimeStart = entity.TimeStart;
                user.TimeOut = entity.TimeOut;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }
        }
        //Phân trang quản lý user và thêm mục tìm kiếm theo username và email
        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<User> model = db.User;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Username.Contains(searchString) || x.Email.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        //Kiểm tra phiên đăng nhập có hợp lệ không
        public User GetById(string userName)
        {
            return db.User.SingleOrDefault(x => x.Username == userName);
        }

        public User ViewDetail(int id)
        {
            return db.User.Find(id);
        }

        //Kiểm tra user đang nhập có quyền thực hiện các tác vụ thêm, sửa, xóa theo phân quyền

        //public List<string> GetListCredential(string userName)
        //{
        //    var user = db.User.Single(x => x.Username == userName);
        //    var data = (from a in db.Employee
        //                join b in db.UserGroups on a.UserGroupID equals b.ID
        //                join c in db.Roles on a.RoleID equals c.ID
        //                where b.ID == user.GroupID
        //                select new
        //                {
        //                    RoleID = a.RoleID,
        //                    UserGroupID = a.UserGroupID
        //                }).AsEnumerable().Select(x => new Credential()
        //                {
        //                    RoleID = x.RoleID,
        //                    UserGroupID = x.UserGroupID
        //                });
        //    return data.Select(x => x.RoleID).ToList();

        //}

        public bool ChangeStatus(long id)
        {
            var user = db.User.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }

        public bool Delete(int id)
        {
            try
            {
                var user = db.User.Find(id);
                db.User.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool CheckUserName(string userName)
        {
            return db.User.Count(x => x.Username == userName) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.User.Count(x => x.Email == email) > 0;
        }

        public int Login(string userName, string passWord)
        {
            var result = db.User.SingleOrDefault(x => x.Username == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == passWord)
                    {
                        if (DateTime.Now.TimeOfDay > result.TimeStart && DateTime.Now.TimeOfDay < result.TimeOut)
                        {
                            return 1;
                        }
                        else
                        {
                            return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
            }
        }

    }
}
