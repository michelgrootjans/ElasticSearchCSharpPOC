namespace ElasticSearch.ConsoleApp.EsAccess
{
    internal interface ITypeMapper
    {
        string TypeName { get; }
        string Mapping { get; }
    }
}