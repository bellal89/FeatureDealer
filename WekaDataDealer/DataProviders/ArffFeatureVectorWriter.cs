using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FeatureDealer.Converters;

namespace FeatureDealer.DataProviders
{
    class ArffFeatureVectorWriter
    {
        private readonly string fileName;
        private readonly string relationName;

        public ArffFeatureVectorWriter(string fileName, string relationName)
        {
            this.fileName = fileName;
            this.relationName = relationName;
        }

        public void SaveData(List<List<object>> items)
        {
                File.WriteAllText(fileName,
                                  GetRelationBlock() + GetAttributesBlock(items) +
                                  GetDataBlock(items));
        }

        private string GetRelationBlock()
        {
            return "@relation " + relationName + "\n\n";
        }

        private string GetAttributesBlock(IEnumerable<List<object>> vectors)
        {
            var converter = new VectorToArffConverter(vectors.First());

            return
                String.Join("\n", converter.FeatureInfos
                                      .Select(
                                          info => "@attribute " + info.Name + " " + info.Type))
                + "\n\n";
        }

        private string GetDataBlock(IEnumerable<List<object>> vectors)
        {
            return "@data\n" + String.Join("\n", vectors.Select(vector => String.Join(", ", vector.Select(item => item is double ? ((double)item).ToString(CultureInfo.InvariantCulture):item.ToString())))) + "\n\n";
        }
    }
}
