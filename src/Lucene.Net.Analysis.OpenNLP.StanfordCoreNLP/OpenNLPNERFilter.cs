using Lucene.Net.Analysis.OpenNlp.TokenAttributes;
using Lucene.Net.Analysis.OpenNlp.Tools;
using Lucene.Net.Analysis.TokenAttributes;
using opennlp.tools.util;
using System;

namespace Lucene.Net.Analysis.OpenNlp
{
    /// <summary>
    /// Extension to <see cref="Lucene.Net.Analysis.OpenNlp"/> to provide named entity recognition support.
    /// This class uses a custom attribute, <see cref="INamedEntityTypeAttribute"/> to store the NER type,
    /// which can be retrieved later in the Analysis pipeline.
    /// </summary>
    public sealed class OpenNLPNERFilter : TokenFilter
    {
        private readonly NLPNERTaggerOp nerTaggerOp;
        private readonly ICharTermAttribute termAtt;
        private readonly INamedEntityTypeAttribute nerTypeAtt;
        private readonly string[] tokens = new string[1]; // We handle 1 token at a time for simplicity

        public OpenNLPNERFilter(TokenStream input, NLPNERTaggerOp nerTaggerOp)
            : base(input)
        {
            this.nerTaggerOp = nerTaggerOp ?? throw new ArgumentNullException(nameof(nerTaggerOp));
            this.termAtt = AddAttribute<ICharTermAttribute>();
            this.nerTypeAtt = AddAttribute<INamedEntityTypeAttribute>();
        }

        public override bool IncrementToken()
        {
            if (!m_input.IncrementToken())
            {
                return false; // no more tokens
            }

            tokens[0] = termAtt.ToString();
            Span[] spans = nerTaggerOp.GetNames(tokens);

            if (spans.Length > 0)
            {
                // Assuming a single span for simplicity.
                // You might need to handle multiple spans differently based on your use case.
                Span span = spans[0];

                // Set the type based on the OpenNLP ner model
                string entityType = span.getType();
                nerTypeAtt.NamedEntityType = entityType;
            }

            return true;
        }

        public override void Reset()
        {
            base.Reset();
            nerTaggerOp.Reset();
        }
    }
}
