using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FeatureDealer.FeatureSettings;

namespace FeatureDealer.FeatureCalculators
{
    // todo метод Calculate отличается от FeatureCalculator.Calculate только типом аттрибута признаков и названием аттрибута => вынести
    public class QueryDependentFeatureCalculator : IFeatureCalculator
    {
        private static readonly List<MethodToCall> methodsToCall = new List<MethodToCall>();
        private static Dictionary<string, int> featureNumbers = new Dictionary<string, int>();

        public QueryDependentFeatureCalculator(FeatureList featureList)
        {
            Console.WriteLine("Initializing Query dependent feature calculator");
            var featuresToCalculate =
                featureList.Features.Where(f => f.QueryDependent && f.Ignore == false);
            featureNumbers = featuresToCalculate.ToDictionary(f => f.FeatureName, f => f.Number);
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            foreach (Type type in executingAssembly.GetTypes())
            {
                var callData = new List<KeyValuePair<Attribute, MethodInfo>>();
                MethodInfo[] methodInfos = type.GetMethods();
                object instance = null;
                foreach (MethodInfo methodInfo in methodInfos)
                {
                    QueryDependentFeatureAttribute featureAttribute =
                        methodInfo.GetCustomAttributes(typeof(QueryDependentFeatureAttribute), false).OfType<QueryDependentFeatureAttribute>().
                            FirstOrDefault();
                    if (NeedToCall(featureAttribute))
                    {
                        if (instance == null)
                            instance = (Activator.CreateInstance(type));
                        callData.Add(new KeyValuePair<Attribute, MethodInfo>(featureAttribute, methodInfo));
                    }
                }
                if (callData.Any())
                    methodsToCall.Add(new MethodToCall { Instance = instance, CallData = callData });
            }
        }

        #region IFeatureCalculator Members

        public IEnumerable<Feature> Calculate(IFeatureParameters data)
        {
            var featureParameters = data as QueryDependentFeatureParameters;
            var features = new List<Feature>();
            foreach (MethodToCall methodToCall in methodsToCall)
            {
                object instance = methodToCall.Instance;
                IEnumerable<KeyValuePair<Attribute, MethodInfo>> callData = methodToCall.CallData;
                foreach (var call in callData)
                {
                    var featureAttribute = call.Key as QueryDependentFeatureAttribute;
                    MethodInfo methodInfo = call.Value;
                    object value = methodInfo.Invoke(instance, new object[] { featureParameters });
                    string featureName = featureAttribute.FeatureName;
                    features.Add(new Feature
                    {
                        Value = Convert.ToDouble(value),
                        Name = featureName,
                        Number = featureNumbers[featureName]
                    });
                }
                Type instanceType = instance.GetType();
                Type interfaceType = instanceType.GetInterfaces().FirstOrDefault();
                if (interfaceType == null)
                    continue;
                MethodInfo[] instanceMethods = interfaceType.GetMethods();
                MethodInfo getValueMethodInfo =
                    instanceMethods.FirstOrDefault(
                        m => m.GetCustomAttributes(typeof(GetValueAttribute), true).Any());
                if (getValueMethodInfo != null)
                {
                    object value = getValueMethodInfo.Invoke(instance, new object[] { featureParameters });
                    if (value.GetType().IsGenericType)
                    {
                        var values = value as IEnumerable<Feature>;
                        features.AddRange(values.Select(v => new Feature { Value = v.Value, Name = v.Name, Number = v.Number }));
                    }
                    else
                        features.Add(new Feature { Value = (double)value });
                }
            }
            return features;
        }

        #endregion

        private bool NeedToCall(QueryDependentFeatureAttribute customAttribute)
        {
            if (customAttribute == null)
                return false;
            if (!featureNumbers.ContainsKey(customAttribute.FeatureName) || customAttribute.Ignore)
                return false;
            return true;
        }
    }
}