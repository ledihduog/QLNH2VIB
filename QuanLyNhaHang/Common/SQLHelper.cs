using Microsoft.Data.SqlClient;

using System.Data;

namespace QuanLyNhaHang.Common
{
    public class SQLHelper<T> where T : class, new()
    {


        public static DataTable GetDataTableFromSQL(string sqlQuery, string connectionString)
        {
            SqlConnection mySqlConnection = new SqlConnection(connectionString);
            mySqlConnection.Open();
            try
            {
                SqlCommand mySqlCommand = new SqlCommand(sqlQuery, mySqlConnection);
                mySqlCommand.CommandType = CommandType.Text;
                using (var result = mySqlCommand.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(result);
                    return dataTable;
                }
            }
            catch (SqlException e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                mySqlConnection.Close();
            }
        }


        public async static Task<List<T>> SqlToList(string sql, string connectionString)
        {
            List<T> lst = new List<T>();
            SqlConnection mySqlConnection = new SqlConnection(connectionString);
            mySqlConnection.Open();
            try
            {
                SqlCommand mySqlCommand = new SqlCommand(sql, mySqlConnection);
                mySqlCommand.CommandType = CommandType.Text;
                SqlDataReader reader = await mySqlCommand.ExecuteReaderAsync();
                lst = reader.MapToList<T>();
            }
            catch (SqlException e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                mySqlConnection.Close();
            }

            return lst;
        }

        /// <summary>
        /// Phương thức thực hiện truy vấn SQL trả về giá trị scalar (ví dụ
        /// COUNT(*)). Lưu ý: Sử dụng generic type R để có thể trả về các kiểu
        /// giá trị như int, long,...
        /// </summary>
        public async static Task<R> SqlToScalar<R>(string sql, string connectionString)
        {
            R result = default(R);
            SqlConnection mySqlConnection = new SqlConnection(connectionString);
            await mySqlConnection.OpenAsync();
            try
            {
                SqlCommand mySqlCommand = new SqlCommand(sql, mySqlConnection);
                mySqlCommand.CommandType = CommandType.Text;
                object scalar = await mySqlCommand.ExecuteScalarAsync();
                if (scalar == null || scalar == DBNull.Value)
                {
                    result = default(R);
                }
                else
                {
                    result = (R)Convert.ChangeType(scalar, typeof(R));
                }
            }
            catch (SqlException e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                mySqlConnection.Close();
            }
            return result;
        }





    }



    public class ResultQuery
    {
        public int ID { get; set; }
        public int TotalRow { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorText { get; set; }
    }
}
