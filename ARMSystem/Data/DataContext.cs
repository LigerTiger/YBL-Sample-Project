using ARMSystem.Model;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace ARMSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Employee> ActiveEmployeesView { get; set; }

        public List<Employee> GetActiveEmployees()
        {
            var result = new List<Employee>();
            var connection = Database.GetDbConnection();
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetActiveEmployees";
                command.CommandType = CommandType.StoredProcedure;

                var param = new OracleParameter();
                param.ParameterName = "result";
                param.Direction = ParameterDirection.Output;
                param.OracleDbType = OracleDbType.RefCursor;

                command.Parameters.Add(param);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var employee = new Employee
                        {
                            SrNo = reader.GetInt32(reader.GetOrdinal("SrNo")),
                            EmpAdId = reader.GetString(reader.GetOrdinal("EmpAdId")),
                            EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                            BusinessUnit = reader.IsDBNull(reader.GetOrdinal("BusinessUnit")) ? null : reader.GetString(reader.GetOrdinal("BusinessUnit")),
                            CorporateDesignation = reader.IsDBNull(reader.GetOrdinal("CorporateDesignation")) ? null : reader.GetString(reader.GetOrdinal("CorporateDesignation")),
                            FunctionalDesignation = reader.IsDBNull(reader.GetOrdinal("FunctionalDesignation")) ? null : reader.GetString(reader.GetOrdinal("FunctionalDesignation")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? null : reader.GetString(reader.GetOrdinal("Type")),
                            CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedOn")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy"))
                        };

                        result.Add(employee);
                    }
                }
            }

            connection.Close();
            return result;
        }
    }
}
