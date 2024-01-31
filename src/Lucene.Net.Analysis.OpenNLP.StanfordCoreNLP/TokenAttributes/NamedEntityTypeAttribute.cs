using Lucene.Net.Util;
using System;
using Attribute = Lucene.Net.Util.Attribute;

namespace Lucene.Net.Analysis.OpenNlp.TokenAttributes
{
    /// <summary>
    /// A custom attribute to store the NER type value for a given token.
    /// </summary>
    public class NamedEntityTypeAttribute : Attribute, INamedEntityTypeAttribute
    {
        public string NamedEntityType { get; set; }

        public override void Clear()
        {
            NamedEntityType = null;
        }

        public override void CopyTo(IAttribute target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (target is not INamedEntityTypeAttribute t)
                throw new ArgumentException($"Argument type {target.GetType().FullName} must implement {nameof(INamedEntityTypeAttribute)}", nameof(target));
            t.NamedEntityType = NamedEntityType;
        }

        public override void ReflectWith(IAttributeReflector reflector)
        {
            if (reflector is null)
                throw new ArgumentNullException(nameof(reflector));

            reflector.Reflect(typeof(NamedEntityTypeAttribute), "namedEntityType", NamedEntityType);
        }
    }
}
