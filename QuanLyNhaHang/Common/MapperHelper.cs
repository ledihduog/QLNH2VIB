using AutoMapper;
using System.Data;

namespace QuanLyNhaHang.Common
{
    public static class MapperHelper
    {
        /// <summary>
        /// Map voi object moi
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T2 Map<T1, T2>(T1 obj)
        {
            var mapperCongfig = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<T1, T2>().ReverseMap());
            var autoMapper = mapperCongfig.CreateMapper();
            return autoMapper.Map<T1, T2>(obj);
        }

        public static List<T2> MapList<T1, T2>(List<T1> obj)
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<T1, T2>().ReverseMap());
            var autoMapper = mapperConfig.CreateMapper();
            return autoMapper.Map<List<T1>, List<T2>>(obj);
        }

        /// <summary>
        /// Map voi object co san
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="obj"></param>
        /// <param name="desObj"></param>

        public static void Map<T1, T2>(T1 obj, T2 desObj)
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<T1, T2>().ReverseMap());
            var autoMapper = mapperConfig.CreateMapper();
            autoMapper.Map<T1, T2>(obj, desObj);
        }

        /// <summary>
        /// map với dataTable chuyển về Đối tượng
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTableToList<T>(DataTable dataTable) where T : new()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, T>();
            });

            var mapper = config.CreateMapper();

            // Chuyển DataTable thành danh sách Dictionary<string, object>
            var dataList = dataTable.AsEnumerable()
                .Select(row =>
                {
                    var dict = new Dictionary<string, object>();
                    foreach (var col in dataTable.Columns.Cast<DataColumn>())
                    {
                        dict[col.ColumnName] = row[col] != DBNull.Value ? row[col] : null; // Xử lý NULL
                    }
                    return dict;
                }).ToList();

            return mapper.Map<List<T>>(dataList);
        }
    }
}
