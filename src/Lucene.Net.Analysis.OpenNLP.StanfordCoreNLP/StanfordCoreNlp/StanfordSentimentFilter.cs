using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.sentiment;
using Lucene.Net.Analysis.OpenNlp.StanfordCoreNlp.TokenAttributes;
using Lucene.Net.Analysis.TokenAttributes;
using System;

namespace Lucene.Net.Analysis.OpenNlp.StanfordCoreNlp
{
    /// <summary>
    /// A sentiment filter that uses the Standford CoreNLP pipeline.
    /// This class uses a custom attribute, <see cref="ISentimentTypeAttribute"/>
    /// to store the sentiment type, which can be retrieved later in the Analysis pipeline.
    /// </summary>
    public sealed class StanfordSentimentFilter : TokenFilter
    {
        private readonly StanfordCoreNLP pipeline;
        private readonly ICharTermAttribute termAttr;
        private readonly ISentimentTypeAttribute sentimentTypeAttr;
        private java.util.List stanfordTokens;
        private int currentTokenIndex = 0;

        public StanfordSentimentFilter(TokenStream input, StanfordCoreNLP pipeline)
            : base(input)
        {
            this.pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            termAttr = AddAttribute<ICharTermAttribute>();
            sentimentTypeAttr = AddAttribute<ISentimentTypeAttribute>();
        }

        public override bool IncrementToken()
        {
            if (currentTokenIndex < (stanfordTokens?.size() ?? 0))
            {
                CoreLabel stanfordToken = (CoreLabel)stanfordTokens.get(currentTokenIndex++);
                termAttr.SetEmpty().Append(stanfordToken.word());
                sentimentTypeAttr.SentimentType = stanfordToken.get(typeof(SentimentCoreAnnotations.SentimentClass)).ToString();

                return true;
            }

            // Process next batch of tokens if available
            if (!m_input.IncrementToken())
            {
                return false;
            }

            // Process the current token through the Stanford CoreNLP pipeline
            string token = termAttr.ToString();
            CoreDocument document = new CoreDocument(token);
            pipeline.annotate(document);
            stanfordTokens = document.tokens();
            currentTokenIndex = 0;

            // If there are tokens from Stanford CoreNLP, repeat the process
            return IncrementToken();
        }

        public override void Reset()
        {
            base.Reset();
            this.stanfordTokens = null;
            this.currentTokenIndex = 0;
        }
    }
}
