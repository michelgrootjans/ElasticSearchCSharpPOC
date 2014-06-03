using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ElasticSearch.ConsoleApp.Models;

namespace ElasticSearch.ConsoleApp.DbAccess
{
    internal class DataAccessLayer
    {

        public IEnumerable<Project> GetProjecten()
        {
            var projects = new List<Project>();
            using (var connection = CreateDbConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = getProjectsQuery;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        projects.Add(MapToProject(reader));
                    }
                }
            }
            return projects;
        }

        private const string getProjectsQuery = @"
                select pj.Id,
	                   pj.Identificatie,
	                   pj.Omschrijving,
	                   pj.projectType,
	                   st.Naam as 'status',
	                   g.Naam as 'gemeente'
                from Projecten pj
                left outer join Statussen st on st.Id = pj.StatusID
                left outer join Gemeentes g on pj.GemeenteID = g.ID
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

        private static IDbConnection CreateDbConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
            return new SqlConnection(connectionString);
        }
    }
}