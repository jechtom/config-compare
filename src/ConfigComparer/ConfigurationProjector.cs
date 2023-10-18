using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ConfigComparer
{
    /// <summary>
    /// Projects configuration to flat and ordered list.
    /// </summary>
    internal class ConfigurationProjector
    {
        public ConfigurationProjector()
        {
        }

        public IEnumerable<KeyValuePair<string, string>> Project(IConfiguration configuration)
        {
            Queue<IConfigurationSection> sections = new();

            IEnumerable<KeyValuePair<string, string>> VisitChildren(IEnumerable<IConfigurationSection> children)
            {
                foreach (var child in children.OrderBy(s => s.Key))
                {
                    bool any = false;
                    foreach (var item in VisitChildren(child.GetChildren()))
                    {
                        yield return item;
                        any = true;
                    }

                    if (!any)
                    {
                        // no children - return value
                        // - no matter if that is null or not as this is leaf node)
                        yield return new KeyValuePair<string, string>(child.Path, child.Value);
                        continue;
                    }
                }
            }

            return VisitChildren(configuration.GetChildren());
        }
    }
}
