﻿using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class DayOffDao
    {
        WebAppDbContext db = null;
        public DayOffDao()
        {
            db = new WebAppDbContext();
        }

        public long Insert(DayOff entity)
        {       
                entity.Date = DateTime.Now;
                db.DayOffs.Add(entity);
                db.SaveChanges(); 
            return entity.ID;
        }

        public long Update(DayOff entity)
        {

            var dayOff = db.DayOffs.Find(entity.ID);
            dayOff.Description = entity.Description;
            dayOff.Status = entity.Status;
            dayOff.Date = entity.Date;

            //Ngày chỉnh sửa = Now
            dayOff.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public bool Delete(int id)
        {
            try
            {
                var dayOff = db.DayOffs.Find(id);
                db.DayOffs.Remove(dayOff);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public DayOff ViewDetail(int id)
        {
            return db.DayOffs.Find(id);
        }

        public IEnumerable<DayOff> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<DayOff> model = db.DayOffs;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Employee.Code.Contains(searchString) || x.Date.ToString().Contains(searchString)
                );
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}