using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ElasticSearch.POC.ConsoleApp
{
    internal class DataAccessLayer
    {
        public IEnumerable<Project> GetVmswProjecten()
        {
            using (var connection = CreatePrismaConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                                        select *
                                        from VMSW_PO_Projecten pj
                                        left outer join VMSW_PO_ProjectStatussen ps 
                                                     on ps.Id = pj.StatusID
                                        ";
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var resultList = new List<Project>();
                    while (reader.Read())
                    {
                        var project = new Project
                        {
                            _id = reader.GetGuidValue("Id").ToString(),
                            Identificatie = reader.GetStringValue("identificatie"),
                            ProjectType = reader.GetStringValue("projecttype"),
                            Omschrijving = reader.GetStringValue("omschrijving"),
                            HuidigeProgrammatiefase = reader.GetIntValue("programmatiefase"),
                            Status = reader.GetStringValue("Naam")
                        };
                        resultList.Add(project);
                    }
                    return resultList;
             
                }
            }
        }

        private IDbConnection CreatePrismaConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["PrismaConnectionString"].ConnectionString;
            return new SqlConnection(connectionString);
        }
    }
}