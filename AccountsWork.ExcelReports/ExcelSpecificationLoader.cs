using AccountsWork.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Data.OleDb;
using System.ComponentModel.Composition;

namespace AccountsWork.ExcelReports
{
    public interface IExcelSpecificationLoader
    {
        Task<IList<ServiceZipDetailsSet>> GetServiceZips(string company, string filename, string month, int year);
    }
    [Export(typeof(IExcelSpecificationLoader))]
    public class ExcelSpecificationLoader : IExcelSpecificationLoader
    {
        public async Task<IList<ServiceZipDetailsSet>> GetServiceZips(string company, string filename, string month, int year)
        {
            var serviceZips = new List<ServiceZipDetailsSet>();
            var conn = new OleDbConnection(string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"", filename));
            await conn.OpenAsync();
            var cmd = new OleDbCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM [Лист1$]";
            var reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
            {
                int q = 0;
                if (reader["Quantity"] != DBNull.Value)
                    int.TryParse(reader["Quantity"].ToString(), out q);
                if (reader["BlankNumber"].ToString().Trim() == "tr")
                {
                    //here
                }
                serviceZips.Add(new ServiceZipDetailsSet
                {
                    WorkDate = Convert.ToDateTime(reader["Date"]),
                    BlankNumber = reader["BlankNumber"].ToString().Trim(),
                    ZipName = reader["Work"].ToString().Trim(),
                    StoreNumber = Convert.ToInt16(reader["StoreNumber"]),
                    ZipPrice = Convert.ToDecimal(reader["Price"]),
                    Company = company,
                    ServiceMonth = month,
                    ServiceYear = year,
                    ZipQuantity = q
                });               
            }
            reader.Close();
            conn.Close();
            return serviceZips;
        }
    }
}
