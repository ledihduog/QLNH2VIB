namespace QuanLyNhaHang.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetByID(int id);
        int Create(T item);
        int Update(T item);
        int Delete(int id);
        int CreateRange(List<T> items);
        int RemoveRange(IEnumerable<T> items);
    }
}
