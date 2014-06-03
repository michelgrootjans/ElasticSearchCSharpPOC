using ElasticSearch.ConsoleApp.Models;
using PlainElastic.Net;
using PlainElastic.Net.Mappings;

namespace ElasticSearch.ConsoleApp.EsAccess
{
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
                                .String(p => p.gemeente, f => f.Analyzer(DefaultAnalyzers.keyword))
                            )
                    )
                    .BuildBeautified();
            }
        }
    }
}