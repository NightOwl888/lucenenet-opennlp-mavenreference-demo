using edu.stanford.nlp.pipeline;
using Lucene.Net.Analysis.OpenNlp.Tools;
using Lucene.Net.Analysis.Util;
using System.Collections.Generic;
using System.IO;
using AttributeFactory = Lucene.Net.Util.AttributeSource.AttributeFactory;

namespace Lucene.Net.Analysis.OpenNlp.StanfordCoreNlp
{
    /// <summary>
    /// An analyzer that retrieves the chunker, NER, and Sentiment values and
    /// provides them to the analysis pipeline.
    /// </summary>
    public sealed class SentimentNERAnalyzer : Analyzer
    {
        private readonly IDictionary<string, string> tokenizerArgs;
        private readonly IDictionary<string, string> posFilterArgs;
        private readonly IDictionary<string, string> chunkerFilterArgs;
        private readonly string nerModelFile;
        private readonly IResourceLoader loader;
        private StanfordCoreNLP pipeline;

        public SentimentNERAnalyzer(OpenNLPConfiguration openNLPConfiguration, StanfordCoreNLPConfiguration stanfordCoreNLPConfiguration)
        {
            loader = openNLPConfiguration.ResourceLoader;

            tokenizerArgs = new Dictionary<string, string>
            {
                ["tokenizerModel"] = openNLPConfiguration.TokenizerModel,
                ["sentenceModel"] = openNLPConfiguration.SentenceModel,
            };
            posFilterArgs = new Dictionary<string, string>
            {
                ["posTaggerModel"] = openNLPConfiguration.POSModel,
            };
            chunkerFilterArgs = new Dictionary<string, string>
            {
                ["chunkerModel"] = openNLPConfiguration.ChunkerModel,
            };
            nerModelFile = openNLPConfiguration.NERModel;

            // Initialize Stanford CoreNLP for sentiment analysis.
            // Note this takes a long time to load, but it can be reused in CreateComponents().
            pipeline = new StanfordCoreNLP(stanfordCoreNLPConfiguration.ToProperties());
        }

        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            var opennlpFactory = new OpenNLPTokenizerFactory(tokenizerArgs);
            opennlpFactory.Inform(loader);
            Tokenizer source = opennlpFactory.Create(AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY, reader);

            // OpenNLP for NER
            var opennlpPOSFilterFactory = new OpenNLPPOSFilterFactory(posFilterArgs);
            opennlpPOSFilterFactory.Inform(loader);
            TokenStream tokenStream = opennlpPOSFilterFactory.Create(source);

            var opennlpChunkerFilterFactory = new OpenNLPChunkerFilterFactory(chunkerFilterArgs);
            opennlpChunkerFilterFactory.Inform(loader);
            tokenStream = opennlpChunkerFilterFactory.Create(tokenStream);

            OpenNLPOpsFactory.GetNERTaggerModel(nerModelFile, loader);
            var nerTagger = OpenNLPOpsFactory.GetNERTagger(nerModelFile);
            tokenStream = new OpenNLPNERFilter(tokenStream, nerTagger);

            // Add Stanford CoreNLP for sentiment analysis
            tokenStream = new StanfordSentimentFilter(tokenStream, pipeline);

            return new TokenStreamComponents(source, tokenStream);
        }
    }
}
