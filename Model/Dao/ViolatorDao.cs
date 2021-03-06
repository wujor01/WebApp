﻿using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ViolatorDao
    {
        WebAppDbContext db = null;
        public ViolatorDao()
        {
            db = new WebAppDbContext();
        }

        public long Insert(Violator entity)
        {
            db.Violators.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long Update(Violator entity,string username)
        {
            var violator = db.Violators.Find(entity.ID);
            violator.Employee_ID = entity.Employee_ID;
            violator.Type_ID = entity.Type_ID;
            violator.Loan = entity.Loan;
            violator.Description = entity.Description;

            //Ngày chỉnh sửa = Now
            violator.ModifiedBy = username;
            violator.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public bool Delete(int id)
        {
            try
            {
                var violator = db.Violators.Find(id);
                db.Violators.Remove(violator);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public Violator ViewDetail(int id)
        {
            return db.Violators.Find(id);
        }

        public IEnumerable<Violator> ListAllPaging(string searchString, int page, int pageSize, int departmentId)
        {
            IQueryable<Violator> model = db.Violators;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Employee.Code.Contains(searchString) || x.ID.ToString().Contains(searchString)
                );
            }
            if (departmentId == 0)
            {
                return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            else
            {
                return model.OrderByDescending(x => x.CreatedDate).Where(x=>x.Employee.Department_ID == departmentId).ToPagedList(page, pageSize);
            }
        }

        public List<ViolatorType> ListAll()
        {
            return db.ViolatorTypes.ToList();
        }
    }
}
