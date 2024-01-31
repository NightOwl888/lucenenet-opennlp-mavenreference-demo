using Lucene.Net.Util;
using System;
using Attribute = Lucene.Net.Util.Attribute;

namespace Lucene.Net.Analysis.OpenNlp.StanfordCoreNlp.TokenAttributes
{
    /// <summary>
    /// A custom attribute to store the sentiment type for a given token.
    /// </summary>
    public class SentimentTypeAttribute : Attribute, ISentimentTypeAttribute
    {
        public string SentimentType { get; set; }

        public override void Clear()
        {
            SentimentType = null;
        }

        public override void CopyTo(IAttribute target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (target is not ISentimentTypeAttribute t)
                throw new ArgumentException($"Argument type {target.GetType().FullName} must implement {nameof(ISentimentTypeAttribute)}", nameof(target));
            t.SentimentType = SentimentType;
        }

        public override void ReflectWith(IAttributeReflector reflector)
        {
            if (reflector is null)
                throw new ArgumentNullException(nameof(reflector));

            reflector.Reflect(typeof(SentimentTypeAttribute), "sentimentType", SentimentType);
        }
    }
}
