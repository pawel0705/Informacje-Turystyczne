using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.ViewModels
{
    public class InfoRecordVM
    {
        public struct Property
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string DisplayName { get; set; }
            public string DisplayValue { get; set; }

            //name - nazwa właściwości (np. name, region, contact, open itp.)
            //value - value wartość właściwości na potrzeby filtrowania, może być pusta jeśli własciwość nie będzie filtrowana
            //displayName - nazwa właściwości na potrzeby wyświetlenia np. Region, Typ, Godziny otwarcia. może być pusta
            //displayValue - wartosć, która będzie wyświetlona
            //np. Property("region", "slask", "Region", "Śląsk")
            //zostanie wyświetlona jako Region: Śląsk, a filtrowana będzie po wartości slask
            //np. Property("region", "", "", "Śląsk")
            //zostanie wyświetlona jako Śląsk, i nie będzie filtrowana
            public Property(string name, string value, string displayName, string displayValue)
            {
                Name = name;
                Value = value;
                DisplayName = displayName;
                DisplayValue = displayValue;
            }
        }
        public List<Property> Properties { get; set; }
        public InfoRecordVM()
        {
            Properties = new List<Property>();
        }

        public void AddProperty(string name, string value, string displayName, string displayValue)
        {
            Properties.Add(new Property { 
                Name = name,
                Value = value,
                DisplayName = displayName,
                DisplayValue = displayValue
            });
        }
    }
}
