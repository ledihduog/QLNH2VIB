using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.IRepository;
using QuanLyNhaHang.Models.Context;
using System.Collections.Generic;

namespace QuanLyNhaHang.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected QLNHContext db { get; set; }
        protected DbSet<T> table = null;

        public GenericRepository()
        {
            db = new QLNHContext();
            table = db.Set<T>();
        }

        public GenericRepository(QLNHContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetByID(int id)
        {
            return table.Find(id);
        }

        public int Create(T item)
        {
            table.Add(item);
            return db.SaveChanges();
        }

        public int Update(T item)
        {
            table.Attach(item);
            db.Entry(item).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(int id)
        {
            table.Remove(table.Find(id));
            return db.SaveChanges();
        }

        public int Delete(List<T> item)
        {
            table.RemoveRange(item);
            return db.SaveChanges();
        }


        public async Task<int> CreateAsync(T item)
        {
            table.Add(item);
            return await db.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T item)
        {
            table.Attach(item);
            db.Entry(item).State = EntityState.Modified;
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            table.Remove(table.Find(id));
            return await db.SaveChangesAsync();
        }

        public int RemoveRange(IEnumerable<T> items)
        {
            table.RemoveRange(items);
            return db.SaveChanges();
        }

        public int CreateRange(List<T> items)
        {
            table.AddRange(items);
            return db.SaveChanges();
        }
    }
}
