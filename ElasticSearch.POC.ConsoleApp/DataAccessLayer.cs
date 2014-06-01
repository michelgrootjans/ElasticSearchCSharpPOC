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
                command.CommandText = getProjectsQuery;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var resultList = new List<Project>();
                    while (reader.Read())
                    {
                        resultList.Add(MapToProject(reader));
                    }
                    return resultList;
                }
            }
        }

        private const string getProjectsQuery = @"
                select pj.Id,
	                   pj.Identificatie,
	                   pj.Omschrijving,
	                   pj.projectType,
	                   ps.Naam as 'status',
	                   g.Naam as 'gemeente'
                from VMSW_PO_Projecten pj
                left outer join VMSW_PO_ProjectStatussen ps on ps.Id = pj.StatusID
                left outer join VMSW_GIPR_Gemeentes g on pj.GemeenteID = g.ID
                ";
        private static Project MapToProject(IDataReader reader)
        {
            var project = new Project
            {
                _id = reader.GetGuidValue("Id").ToString(),
                identificatie = reader.GetStringValue("identificatie"),
                omschrijving = reader.GetStringValue("omschrijving"),
                project_type = MapProjectType(reader),
                status = reader.GetStringValue("status", "geen status"),
                gemeente = reader.GetStringValue("gemeente")
            };
            return project;
        }

        private static string MapProjectType(IDataReader reader)
        {
            var value = reader.GetStringValue("projecttype");
            switch (value)
            {
                case "B": return "bouw";
                case "I": return "infrastructuur";
                case "G": return "grond";
                default:  return value;
            }
        }

        private IDbConnection CreatePrismaConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["PrismaConnectionString"].ConnectionString;
            return new SqlConnection(connectionString);
        }
    }
}