using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FeatureDealer.Converters;

namespace WekaDataDealer.SVMFormat
{
    class ArffGratitudeFeatureVectorWriter
    {
         private readonly string fileName;
        private readonly string relationName;

        public ArffGratitudeFeatureVectorWriter(string fileName, string relationName)
        {
            this.fileName = fileName;
            this.relationName = relationName;
        }

        public void SaveData(IEnumerable<List<object>> items)
        {
            File.WriteAllText(fileName, GetRelationBlock() + GetAttributesBlock());
            File.AppendAllLines(fileName, GetDataBlock(items));
        }

        private string GetRelationBlock()
        {
            return "@relation " + relationName + "\n\n";
        }

        private string GetAttributesBlock()
        {
            var converter = new GratitudeVectorToArffConverter();

            return
                String.Join("\n", converter.FeatureInfos
                                      .Select(
                                          info => "@attribute " + info.Name + " " + info.Type))
                + "\n\n" + "@data\n";
        }

        private IEnumerable<string> GetDataBlock(IEnumerable<List<object>> vectors)
        {
            return  vectors.Select(vector => String.Join(", ", vector.Select(item => item is double ? ((double)item).ToString(CultureInfo.InvariantCulture):item.ToString())));
        }
    }
}
