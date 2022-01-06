using System.ComponentModel.DataAnnotations;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public class ConfigHolder
    {
        [Required]
        public List<ConfigItem> Items { get; set; }

        public class ConfigItem
        {
            /// <summary>
            /// See <see cref="ConfigHolderKeys"/>
            /// </summary>
            [Required]
            public string Key { get; set; }
            [Required]
            public string Value { get; set; }
        }
    }
}
