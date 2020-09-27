using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class RoomDao
    {
        WebAppDbContext db = null;
        public RoomDao()
        {
            db = new WebAppDbContext();
        }

        public long Insert(Room entity)
        {
            db.Rooms.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public Room ViewDetail(int id)
        {
            return db.Rooms.Find(id);
        }

        public long Update(Room entity, string username)
        {
            var room = db.Rooms.Find(entity.ID);
            room.Department_ID = entity.Department_ID;
            room.Name = entity.Name;
            room.Description = entity.Description;

            //Ngày chỉnh sửa = Now
            room.ModifiedBy = username;
            room.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public List<Room> ListAll(int departmentId)
        {
            if (departmentId == 0)
            {
                return db.Rooms.OrderBy(x => x.ID).ToList();
            }
            else
            {
                return db.Rooms.OrderBy(x => x.ID).Where(x => x.Department_ID == departmentId).ToList();
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var room = db.Rooms.Find(id);
                db.Rooms.Remove(room);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<Room> ListAllPaging(string searchString, int page, int pageSize, int departmentId)
        {
            IQueryable<Room> model = db.Rooms;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Name.Contains(searchString) || x.Department.Name.ToString().Contains(searchString)
                );
            }
            if (departmentId == 0)
            {
                return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

            }
            else
            {
                return model.OrderByDescending(x => x.CreatedDate).Where(x => x.Department_ID == departmentId).ToPagedList(page, pageSize);
            }
        }
    }
}
