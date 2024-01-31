namespace Lucene.Net.Analysis.OpenNlp
{
    /// <summary>
    /// A configuration class to pass in the location of the OpenNLP model files. The file paths
    /// may be absolute or relative to the output in the /bin/Configuration/TargetFramework directory.
    /// </summary>
    public class OpenNLPConfiguration
    {
        public string TokenizerModel { get; set; } = "en-token.bin";
        public string ChunkerModel { get; set; } = "en-chunker.bin";
        public string NERModel { get; set; } = "en-ner-person.bin";
        public string POSModel { get; set; } = "en-pos-maxent.bin";
        public string SentenceModel { get; set; } = "en-sent.bin";
    }
}
