using Lucene.Net.Util;

namespace Lucene.Net.Analysis.OpenNlp.StanfordCoreNlp.TokenAttributes
{
    /// <summary>
    /// Inteface to use to get/add the <see cref="SentimentTypeAttribute"/> concrete type.
    /// </summary>
    public interface ISentimentTypeAttribute : IAttribute
    {
        string SentimentType { get; set; }
    }
}
