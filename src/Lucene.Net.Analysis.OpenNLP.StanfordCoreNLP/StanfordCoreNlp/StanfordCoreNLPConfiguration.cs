using System.Collections.Generic;

namespace Lucene.Net.Analysis.OpenNlp.StanfordCoreNlp
{
    using Path = System.IO.Path;

    /// <summary>
    /// A configuration class to provide properties to the CoreNLP pipeline.
    /// All paths set here must be absolute paths.
    /// </summary>
    public class StanfordCoreNLPConfiguration
    {
        private static class Properties
        {
            internal const string Annotators = "annotators";
            internal const string POSModel = "pos.model";
            internal const string NERModel = "ner.model";
            internal const string NERUseSUTime = "ner.useSUTime";
            internal const string SUTimeRules = "sutime.rules";
            internal const string NERFineRegexNERMapping = "ner.fine.regexner.mapping";
            internal const string NERFineRegexNERNoDefaultOverwriteLabels = "ner.fine.regexner.noDefaultOverwriteLabels";
            internal const string ParseModel = "parse.model";
            internal const string DeParseModel = "depparse.model";
            internal const string CoreFAlgorithm = "coref.algorithm";
            internal const string CoreFMdType = "coref.md.type";
            internal const string CoreFStatisticalRankingModel = "coref.statistical.rankingModel";
            internal const string SentimentModel = "sentiment.model";
        }

        private readonly IDictionary<string, string> configuration;

        public StanfordCoreNLPConfiguration(string rootDir)
        {
            configuration = new Dictionary<string, string>
            {
                [Properties.Annotators] = "tokenize, ssplit, pos, lemma, ner, parse, sentiment",
                [Properties.POSModel] = Path.Combine(rootDir, "edu/stanford/nlp/models/pos-tagger/english-left3words-distsim.tagger"),
                [Properties.NERModel] = $"{Path.Combine(rootDir, "edu/stanford/nlp/models/ner/english.all.3class.distsim.crf.ser.gz")},{Path.Combine(rootDir, "edu/stanford/nlp/models/ner/english.muc.7class.distsim.crf.ser.gz")},{Path.Combine(rootDir, "edu/stanford/nlp/models/ner/english.conll.4class.distsim.crf.ser.gz")}",
                [Properties.NERUseSUTime] = "false",
                [Properties.SUTimeRules] = $"{Path.Combine(rootDir, "edu/stanford/nlp/models/sutime/defs.sutime.txt")},{Path.Combine(rootDir, "edu/stanford/nlp/models/sutime/english.sutime.txt")},{Path.Combine(rootDir, "edu/stanford/nlp/models/sutime/english.holidays.sutime.txt")}",
                [Properties.NERFineRegexNERMapping] = $"ignorecase=true,validpospattern=(NN|JJ|ADD).*,{Path.Combine(rootDir, "edu/stanford/nlp/models/kbp/english/gazetteers/regexner_caseless.tab")};{Path.Combine(rootDir, "edu/stanford/nlp/models/kbp/english/gazetteers/regexner_cased.tab")}",
                [Properties.NERFineRegexNERNoDefaultOverwriteLabels] = "CITY",
                [Properties.ParseModel] = Path.Combine(rootDir, "edu/stanford/nlp/models/lexparser/englishPCFG.ser.gz"),
                [Properties.DeParseModel] = Path.Combine(rootDir, "edu/stanford/nlp/models/parser/nndep/english_UD.gz"),
                [Properties.CoreFAlgorithm] = "statistical",
                [Properties.CoreFMdType] = "dependency",
                [Properties.CoreFStatisticalRankingModel] = Path.Combine(rootDir, "edu/stanford/nlp/models/coref/statistical/ranking_model.ser.gz"),
                [Properties.SentimentModel] = Path.Combine(rootDir, "edu/stanford/nlp/models/sentiment/sentiment.ser.gz")
            };
        }

        /// <summary>
        /// Set a property to be passed into the CoreNLP pipeline.
        /// <para/>
        /// Not all of the available properties may be defined here.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        public void SetProperty(string name, string value) => configuration[name] = value;

        /// <summary>
        /// Get a property by name.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>The property value.</returns>
        public string GetProperty(string name) => configuration.TryGetValue(name, out string value) ? value : null;


        public string Annotators
        {
            get => configuration[Properties.Annotators];
            set => configuration[Properties.Annotators] = value;
        }

        public string POSModel
        {
            get => configuration[Properties.POSModel];
            set => configuration[Properties.POSModel] = value;
        }

        public string NERModel
        {
            get => configuration[Properties.NERModel];
            set => configuration[Properties.NERModel] = value;
        }

        public string NERUseSUTime
        {
            get => configuration[Properties.NERUseSUTime];
            set => configuration[Properties.NERUseSUTime] = value;
        }

        public string SUTimeRules
        {
            get => configuration[Properties.SUTimeRules];
            set => configuration[Properties.SUTimeRules] = value;
        }

        public string NERFineRegexNERMapping
        {
            get => configuration[Properties.NERFineRegexNERMapping];
            set => configuration[Properties.NERFineRegexNERMapping] = value;
        }

        public string NERFineRegexNERNoDefaultOverwriteLabels
        {
            get => configuration[Properties.NERFineRegexNERNoDefaultOverwriteLabels];
            set => configuration[Properties.NERFineRegexNERNoDefaultOverwriteLabels] = value;
        }

        public string ParseModel
        {
            get => configuration[Properties.ParseModel];
            set => configuration[Properties.ParseModel] = value;
        }

        public string DeParseModel
        {
            get => configuration[Properties.DeParseModel];
            set => configuration[Properties.DeParseModel] = value;
        }

        public string CoreFAlgorithm
        {
            get => configuration[Properties.CoreFAlgorithm];
            set => configuration[Properties.CoreFAlgorithm] = value;
        }

        public string CoreFMdType
        {
            get => configuration[Properties.CoreFMdType];
            set => configuration[Properties.CoreFMdType] = value;
        }

        public string CoreFStatisticalRankingModel
        {
            get => configuration[Properties.CoreFStatisticalRankingModel];
            set => configuration[Properties.CoreFStatisticalRankingModel] = value;
        }

        public string SentimentModel
        {
            get => configuration[Properties.SentimentModel];
            set => configuration[Properties.SentimentModel] = value;
        }

        internal java.util.Properties ToProperties()
        {
            var result = new java.util.Properties();
            foreach (var property in configuration)
            {
                result.setProperty(property.Key, property.Value);
            }
            return result;
        }
    }
}
