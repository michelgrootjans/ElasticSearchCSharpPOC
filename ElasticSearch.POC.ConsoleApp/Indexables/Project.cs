using PlainElastic.Net;
using PlainElastic.Net.Mappings;

namespace ElasticSearch.POC.ConsoleApp
{
    internal class Project : IIndexable
    {
        public string _id { get; set; }
        public string identificatie { get; set; }
        public string project_type { get; set; }
        public string omschrijving { get; set; }
        public string status { get; set; }
        public string programmatie { get; set; }
        public string gemeente { get; set; }
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
                                .String(p => p.status, f => f.Analyzer(DefaultAnalyzers.keyword))
                                .String(p => p.programmatie, f => f.Analyzer(DefaultAnalyzers.keyword))
                                .String(p => p.gemeente, f => f.Analyzer(DefaultAnalyzers.keyword))
                            )
                    )
                    .BuildBeautified();
            }
        }
    }
}