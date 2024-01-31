using Lucene.Net.Analysis.OpenNlp.StanfordCoreNlp.TokenAttributes;
using Lucene.Net.Analysis.OpenNlp.TokenAttributes;
using Lucene.Net.Analysis.Util;
using NUnit.Framework;
using System;
using System.IO;

namespace Lucene.Net.Analysis.OpenNlp.StanfordCoreNlp
{
    public class TestSentimentNERAnalyzer : BaseTokenStreamTestCase
    {
        [Test]
        public void TestBasic()
        {
            // Load files based off of the /bin/configuration/targetframework directory.
            var rootDir = AppDomain.CurrentDomain.BaseDirectory;
            var loader = new FilesystemResourceLoader(new System.IO.DirectoryInfo(rootDir));

            Analyzer analyzer = new SentimentNERAnalyzer(new OpenNLPConfiguration(loader), new StanfordCoreNLPConfiguration(rootDir));

            // Example text
            string text = "John Doe is a software engineer.";

            // Expected values for terms, NER, and Sentiment
            string[] expectedTerms = { "John", "Doe", "is", "a", "software", "engineer", "." };
            string[] expectedChunkValues = { "B-NP", "I-NP", "B-VP", "B-NP", "I-NP", "I-NP", "O" };
            string[] expectedNERValues = { "person", null, null, null, null, null, null };
            string[] expectedSentimentValues = { "Neutral", "Neutral", "Neutral", "Neutral", "Neutral", "Neutral", "Neutral" };

            // Custom AssertAnalyzesTo for checking custom attributes
            AssertAnalyzesTo(analyzer, text, expectedTerms, expectedChunkValues, expectedNERValues, expectedSentimentValues);

            // Example text with multiple persons and potential varied sentiment
            text = "John and Jane are excellent engineers, but Bob is not satisfied with their work.";

            // Expected values for terms, NER, and Sentiment
            expectedTerms = new string[] { "John", "and", "Jane", "are", "excellent", "engineers", ",", "but", "Bob", "is", "not", "satisfied", "with", "their", "work", "." };
            expectedChunkValues = new string[] { "B-NP", "I-NP", "I-NP", "B-VP", "B-NP", "I-NP", "O", "O", "B-NP", "B-VP", "I-VP", "I-VP", "B-PP", "B-NP", "I-NP", "O" };
            expectedNERValues = new string[] { "person", null, "person", null, null, null, null, null, "person", null, null, null, null, null, null, null };
            expectedSentimentValues = new string[] { "Neutral", "Neutral", "Neutral", "Neutral", "Very positive", "Neutral", "Neutral", "Neutral", "Neutral", "Neutral", "Negative", "Positive", "Neutral", "Neutral", "Neutral", "Neutral" };

            AssertAnalyzesTo(analyzer, text, expectedTerms, expectedChunkValues, expectedNERValues, expectedSentimentValues);
        }

        public void AssertAnalyzesTo(Analyzer analyzer, string input, string[] expectedTerms, string[] expectedChunkValues, string[] expectedNERValues, string[] expectedSentimentValues)
        {
            // Base method for basic token checks
            AssertAnalyzesTo(analyzer, input, expectedTerms, expectedChunkValues);

            using (TokenStream tokenStream = analyzer.GetTokenStream("dummy", new StringReader(input)))
            {
                tokenStream.Reset();

                // Custom attribute interfaces
                INamedEntityTypeAttribute nerTypeAttr = tokenStream.AddAttribute<INamedEntityTypeAttribute>();
                ISentimentTypeAttribute sentimentTypeAttr = tokenStream.AddAttribute<ISentimentTypeAttribute>();

                // Check custom attributes
                for (int i = 0; tokenStream.IncrementToken(); i++)
                {
                    // Check NER values
                    if (expectedNERValues != null && i < expectedNERValues.Length)
                    {
                        Assert.AreEqual(expectedNERValues[i], nerTypeAttr.NamedEntityType, $"Token {i} had an unexpected NER type");
                    }

                    // Check Sentiment values
                    if (expectedSentimentValues != null && i < expectedSentimentValues.Length)
                    {
                        Assert.AreEqual(expectedSentimentValues[i], sentimentTypeAttr.SentimentType, $"Token {i} had an unexpected Sentiment type");
                    }
                }

                tokenStream.End();
            }
        }
    }
}
