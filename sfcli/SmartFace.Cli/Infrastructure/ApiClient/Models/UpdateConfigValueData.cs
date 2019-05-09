using System.Collections.Generic;

namespace SmartFace.Cli.Infrastructure.ApiClient.Models
{
    public class UpdateConfigValuesDescriptor
    {
        public IList<UpdateConfigValueData> ConfigValues { get; set; } = new List<UpdateConfigValueData>();

        public static UpdateConfigValuesDescriptor Create(params UpdateConfigValueData[] configUpdateData)
        {
            var configDescriptor = new UpdateConfigValuesDescriptor();

            foreach (var updateConfigValueData in configUpdateData)
            {
                configDescriptor.ConfigValues.Add(updateConfigValueData);
            }

            return configDescriptor;
        }
    }

    public class UpdateConfigValueData
    {
        public string Name { get; set; }
        public string Context { get; set; }
        public string Property { get; set; }
        public object Value { get; set; }

        public UpdateConfigValueData()
        {
            
        }

        public UpdateConfigValueData(string name, string context, string property, object value)
        {
            Name = name;
            Context = context;
            Property = property;
            Value = value;
        }
    }
}
