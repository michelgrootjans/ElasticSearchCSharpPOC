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
        public string Gemeente { get; set; }
    }

    class ProjectMapper : ITypeMapper
    {
        public string TypeName { get { return "project"; } }

        public string Mapping
        {
            get
            {
                return new MapBuilder<Project>()
                    .RootObject(
                        typeName: TypeName,
                        map: r => r
                            .Properties(prop => prop
                                .String(p => p.Status, f => f.Analyzer(DefaultAnalyzers.keyword))
                                .String(p => p.Programmatie, f => f.Analyzer(DefaultAnalyzers.keyword))
                                .String(p => p.Gemeente, f => f.Analyzer(DefaultAnalyzers.keyword))
                            )
                    )
                    .BuildBeautified();
            }
        }
    }
}