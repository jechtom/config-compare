using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            bool EnqueueChildren(IEnumerable<IConfigurationSection> children)
            {
                bool any = false;
                foreach (var child in children.OrderBy(s => s.Key))
                {
                    any = true;
                    sections.Enqueue(child);
                }
                return any;
            }

            EnqueueChildren(configuration.GetChildren());

            while(sections.TryDequeue(out IConfigurationSection section))
            {
                if(EnqueueChildren(section.GetChildren()))
                {
                    // has any children? continue
                    continue;
                }

                // no children - return value
                // - no matter if that is null or not as this is leaf node)
                yield return new KeyValuePair<string, string>(section.Path, section.Value);
            }
        }
    }
}
