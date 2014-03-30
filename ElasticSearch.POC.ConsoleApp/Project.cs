using System;
using PlainElastic.Net;
using PlainElastic.Net.Mappings;

namespace ElasticSearch.POC.ConsoleApp
{
    internal class Project : IIndexable
    {
        public string _id { get; set; }
        public string Identificatie { get; set; }
        public string ProjectType { get; set; }
        public string Omschrijving { get; set; }
        public string Status { get; set; }
        public string Programmatie { get; set; }
    }

    class ProjectMapper
    {
        public string Mapping()
        {
            return new MapBuilder<Project>()
                .RootObject(
                    typeName: "project",
                    map: r => r
                        .Properties(prop => prop
                            .String(p => p.Status, f => f.Analyzer(DefaultAnalyzers.keyword))
                            .String(p => p.Programmatie, f => f.Analyzer(DefaultAnalyzers.keyword))
                        )
                )
                .BuildBeautified();
        }
    }
}