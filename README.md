# Lucene.NET/OpenNLP/Stanford CoreNLP/MavenReference Demo

A demo showing how to use `<MavenReference>` from the [`IKVM.Maven.Sdk` library](https://www.nuget.org/packages/IKVM.Maven.Sdk) to add additional functionality to `Lucene.Net.Analysis.OpenNLP`.

This example extends `Lucene.Net.Analysis.OpenNLP` to provide NER analysis based on the [OpenNLP](https://opennlp.apache.org/) `en-ner-person.bin` model and sentiment analysis based on the [Stanford CoreNLP](https://stanfordnlp.github.io/CoreNLP/) sentiment model.

## Scope

This demo shows how to:

- Properly integrate functionality from 3rd-party libraries into a custom analyzer made up of a tokenizer and multiple token filters
- Use `<MavenReference>` (in the .csproj file) to include Maven packages from Java in a .NET application
- Make token filters that follow single responsibilty principle practices
- Use the [Lucene.NET test framework](https://www.nuget.org/packages/Lucene.Net.TestFramework/) to verify compatibility of the custom analyzer with the [TokenStream contract](https://lucenenet.apache.org/docs/4.8.0-beta00016/api/core/Lucene.Net.Analysis.TokenStream.html).

It does **not** demonstrate how the analysis data may be utilized after the analyzer is invoked (although the tests show how to read the NLP data from the token stream).

> **Further Reading**
> [Introduction to the Lucene.NET Analysis API](https://lucenenet.apache.org/docs/4.8.0-beta00016/api/core/Lucene.Net.Analysis.html)

## Getting Model Files

The models are downloaded automatically during the build.

- The `download-urls/` directory contains text files that include one or more URLs to download the models from.
- The `PrepareModels.targets` file uses MSBuild to download and unpack the models to the `assets/` directory.
- The `Lucene.Net.Analysis.OpenNLP.StanfordCoreNLP.csproj` file contains `CopyToOutputDirectory` commands to move the files to the `bin/[Configuration]/[TargetFramework]` directory.

This is a simple approach used for the demo. It may differ significantly from how the model files are deployed in a production application.

For example, for CoreNLP it is possible to use a model package that is avilable on Maven by adding a MavenReference to the language you want to use.

OpenNLP on the other hand has models available for download from the website.

- [OpenNLP Models on SourceForge](https://opennlp.sourceforge.net/models-1.5/)

> **NOTE**
> At the time of this writing, IKVM only supports Java SE 8, so it is not possible to use it with OpenNLP 2.x or its models.

## Model File Loading

### OpenNLP

The OpenNLP model files are loaded using an injected class that implements [`Lucene.Net.Analysis.Util.IResourceLoader`](https://lucenenet.apache.org/docs/4.8.0-beta00016/api/analysis-common/Lucene.Net.Analysis.Util.IResourceLoader.html). This interface has an `OpenResource(string)` method that returns a stream to a model file. There are 2 implementations of this interface in `Lucene.Net.Analysis.Util` (in the `Lucene.Net.Analysis.Common` package):

1. [`FilesystemResourceLoader`](https://lucenenet.apache.org/docs/4.8.0-beta00016/api/analysis-common/Lucene.Net.Analysis.Util.FilesystemResourceLoader.html) - Loads the file from the file system using either relative or absolute paths. For relative paths, base directory can be passed in through the constructor.
2. [`ClasspathResourceLoader`](https://lucenenet.apache.org/docs/4.8.0-beta00016/api/analysis-common/Lucene.Net.Analysis.Util.ClasspathResourceLoader.html) - Loads the file from the current assembly's embedded resources. This class attempts to match the behavior in Java of using the location of a class to load the resource in the same directory. However, do note it is a best effort - for best results you should keep the namespace and folder names in sync.

This example uses `FilesystemResourceLoader` to load the files from the `bin/[Configuration]/[TargetFramework]` directory.

The model file names and paths for OpenNLP are configured using the `OpenNLPConfiguration` class, which is passed into the constructor of the `SentimentNERAnalyzer`. File paths can be either relative or absolute.

### CoreNLP

The CoreNLP model file paths are passed into the constructor of `SentimentNERAnalyzer` using the `StandfordCoreNLPConfiguration` class. File names must be absolute paths. There are other properties that may be passed into the CoreNLP pipeline using this settings class, as well.

> **NOTE**
> It would be preferable to use `<MavenReference>` to add a reference to the model files, but it won't be supported until the `Automatic-Module-Name` issue is resolved. See: https://github.com/stanfordnlp/CoreNLP/issues/1411 and https://github.com/ikvmnet/ikvm-maven/issues/51. In that case, we wouldn't need this extra configuration class because the model files would be loaded automatically via class path.

## Licensing

This repository is licensed under the Apache 2.0 license and all code here is free to use under the terms of the license.

However, Stanford CoreNLP is licensed under the [GNU General Public License](http://www.gnu.org/licenses/gpl.html) v3 or later. More precisely, all the Stanford NLP code is GPL v2+, but CoreNLP uses some Apache-licensed libraries, and so our understanding is that the the composite is correctly licensed as v3+. You can run almost all of CoreNLP under GPL v2; you simply need to omit the time-related libraries, and then you lose the functionality of SUTime. Note that the license is the full GPL, which allows many free uses, but not its use in [proprietary software](http://www.gnu.org/licenses/gpl-faq.html#GPLInProprietarySystem) which is distributed to others. For distributors of [proprietary software](http://www.gnu.org/licenses/gpl-faq.html#GPLInProprietarySystem), CoreNLP is also available from Stanford under a [commercial licensing](http://techfinder.stanford.edu/technology_detail.php?ID=29724) You can contact us at [java-nlp-support@lists.stanford.edu](mailto:java-nlp-support@lists.stanford.edu). If you don’t need a commercial license, but would like to support maintenance of these tools, we welcome gift funding: use [this form](http://giving.stanford.edu/goto/writeingift) and write “Stanford NLP Group open source software” in the Special Instructions.

### Citing Stanford CoreNLP in papers

If you’re just running the CoreNLP pipeline, please cite this CoreNLP paper:

Manning, Christopher D., Mihai Surdeanu, John Bauer, Jenny Finkel, Steven J. Bethard, and David McClosky. 2014. [The Stanford CoreNLP Natural Language Processing Toolkit](http://nlp.stanford.edu/pubs/StanfordCoreNlp2014.pdf) In *Proceedings of the 52nd Annual Meeting of the Association for Computational Linguistics: System Demonstrations*, pp. 55-60. [pdf](http://nlp.stanford.edu/pubs/StanfordCoreNlp2014.pdf) [bib](http://nlp.stanford.edu/pubs/StanfordCoreNlp2014.bib)

If you’re dealing in depth with particular annotators, you’re also encouraged to cite the papers that cover individual components: [POS tagging](http://nlp.stanford.edu/software/tagger.html), [NER](http://nlp.stanford.edu/software/CRF-NER.html), [constituency parsing](http://nlp.stanford.edu/software/lex-parser.html), [dependency parsing](http://nlp.stanford.edu/software/nndep.html), [coreference resolution](http://nlp.stanford.edu/software/dcoref.html), [sentiment](http://nlp.stanford.edu/sentiment/), or [Open IE](http://nlp.stanford.edu/software/openie.html). You can find more information on the Stanford NLP [software pages](http://nlp.stanford.edu/software/) and/or [publications page](http://nlp.stanford.edu/pubs/).