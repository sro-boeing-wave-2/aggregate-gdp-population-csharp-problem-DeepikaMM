using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AggregateGDPPopulation
{
    public class Class1
    {
        class Object
        {
            public float GDP_2012 { get; set; }
            public float Population_2012 { get; set; }
        }
        public void GenerateOutput()
        {
            string[] ReadFile = File.ReadAllLines(@"../../../../AggregateGDPPopulation/data/datafile.csv");
            string[] headersIndex = ReadFile[0].Split(',');
            int IndexOfCountry = Array.IndexOf(headersIndex, "\"Country Name\"");
            int IndexOfPopulation = Array.IndexOf(headersIndex, "\"Population (Millions) 2012\"");
            int IndexOfGdp = Array.IndexOf(headersIndex, "\"GDP Billions (USD) 2012\"");
            StreamReader r = new StreamReader(@"../../../../AggregateGDPPopulation/data/country-continent-map.json");
            var json = r.ReadToEnd();
            Dictionary<string, string> items = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Dictionary<string, Object> AggregatedData = new Dictionary<string, Object>();
            for (int i = 1; i < ReadFile.Length; i++)
            {
                string[] SplitByCommaOfSingleLine = ReadFile[i].Split(',');
                try
                {
                    string continent = items[SplitByCommaOfSingleLine[IndexOfCountry].Trim('"')];
                    try
                    {
                        AggregatedData[continent].GDP_2012 += float.Parse(SplitByCommaOfSingleLine[IndexOfGdp].Trim('"'));
                        AggregatedData[continent].Population_2012 += float.Parse(SplitByCommaOfSingleLine[IndexOfPopulation].Trim('"'));
                    }
                    catch
                    {
                        Object newObject = new Object()
                        {
                            GDP_2012 = float.Parse(SplitByCommaOfSingleLine[IndexOfGdp].Trim('"')),
                            Population_2012 = float.Parse(SplitByCommaOfSingleLine[IndexOfPopulation].Trim('"'))
                        };
                        AggregatedData.Add(continent, newObject);
                    }
                }
                catch { }
            }
            var output = Newtonsoft.Json.JsonConvert.SerializeObject(AggregatedData);
            File.WriteAllText(@"../../../../AggregateGDPPopulation/output/output.json", output);
        }
    }
}