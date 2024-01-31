using Lucene.Net.Util;

namespace Lucene.Net.Analysis.OpenNlp.TokenAttributes
{
    /// <summary>
    /// Inteface to use to get/add the <see cref="NamedEntityTypeAttribute"/> concrete type.
    /// </summary>
    public interface INamedEntityTypeAttribute : IAttribute
    {
        string NamedEntityType { get; set; }
    }
}
