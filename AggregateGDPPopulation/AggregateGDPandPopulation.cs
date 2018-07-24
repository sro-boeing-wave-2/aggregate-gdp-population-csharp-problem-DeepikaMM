using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace AggregateGDPPopulation
{
    public static class FileUtilities
    {
        public static async Task<string> FileReadAsyncReturnString(string FilePath)
        {
            StreamReader FileContents = new StreamReader(FilePath);
            string ReadContents = await FileContents.ReadToEndAsync();
            return ReadContents;
        }
        public static async Task<List<string>> FileReadAsyncReturnList(string FilePath)
        {
            StreamReader FileContents = new StreamReader(FilePath);
            string s;
            List<string> list = new List<string>();
            try
            {
                while ((s = await FileContents.ReadLineAsync()) != null)
                {
                    list.Add(s);
                }
                return list;
            }

            catch (Exception)
            {
                return new List<string>();
            }
        }
        public static async void WriteFileAsync(string FilePath, string Content)
        {
            using (StreamWriter FileContents = new StreamWriter(FilePath))
            {
                await FileContents.WriteAsync(Content);
            }
        }
    }
    public static class JsonSerializor
    {
        public static string Serializor(Dictionary<string, GdpAndPopulation> Obj)
        {
            var AggregatedData = Newtonsoft.Json.JsonConvert.SerializeObject(Obj);
            return AggregatedData;
        }
        public static JObject DeSerializor(string Obj)
        {
            return JObject.Parse(Obj);        
        }
    }
    public class GdpAndPopulation
    {
        private float Gdp_2012;
        private float Population_2012;
        public float GDP_2012
        {
            get
            {
                return this.Gdp_2012;
            }
            set
            {
                this.Gdp_2012 = value;
            }
        }
        public float POPULATION_2012
        {
            get
            {
                return this.Population_2012;
            }
            set
            {
                this.Population_2012 = value;
            }
        }
    }
    public class AggregateGdpAndPopulation
    {
        public Dictionary<string, GdpAndPopulation> AggregateGdpPopulation;
        public AggregateGdpAndPopulation()
        {
            AggregateGdpPopulation = new Dictionary<string, GdpAndPopulation>();
        }
        public void AddorUpdate(string Continent, float Gdp, float Population)
        {
            try
            {
                try
                {
                    AggregateGdpPopulation[Continent].GDP_2012 += Gdp;
                    AggregateGdpPopulation[Continent].POPULATION_2012 += Population;
                }
                catch (Exception)
                {
                    GdpAndPopulation g = new GdpAndPopulation() { GDP_2012 = Gdp, POPULATION_2012 = Population };
                    AggregateGdpPopulation.Add(Continent, g);
                }
            }
            catch (Exception)
            { }
         }
    }
    public class CalculateAggragate
    {
        public async Task<JObject> Main()
        {
            Task<string> CountryContinentGettingData = FileUtilities.FileReadAsyncReturnString("../../../../AggregateGDPPopulation/data/country-continent-map.json");
            string CountryContinentMapWithoutParsing = await CountryContinentGettingData;
            Task<List<string>> CSVDataGetting = FileUtilities.FileReadAsyncReturnList("../../../../AggregateGDPPopulation/data/datafile.csv");
            List<string> CSVFileContents = await CSVDataGetting;
            JObject CountryContinentMap = JsonSerializor.DeSerializor(CountryContinentMapWithoutParsing);
            string headerText = CSVFileContents[0];
            List<string> headers = headerText.Split(',').ToList();
            int IndexOfCountry = headers.IndexOf("\"Country Name\"");
            int IndexOfPopulation = headers.IndexOf("\"Population (Millions) 2012\"");
            int IndexOfGDP = headers.IndexOf("\"GDP Billions (USD) 2012\"");
            AggregateGdpAndPopulation aggregateGdpAndPopulation = new AggregateGdpAndPopulation();
            for (int i = 1; i < CSVFileContents.Count; i++)
            {
                List<string> RowOfData = CSVFileContents[i].Split(',').ToList();
                string Country = RowOfData[IndexOfCountry].Trim('\"');
                float Population = float.Parse((RowOfData[IndexOfPopulation].Trim('\"')));
                float Gdp = float.Parse((RowOfData[IndexOfGDP].Trim('\"')));
                try
                {
                    string Continent = CountryContinentMap.GetValue(RowOfData[IndexOfCountry].Trim('\"')).ToString();
                    aggregateGdpAndPopulation.AddorUpdate(Continent, Gdp, Population);
                }
                catch { }
                
            }
            var FinalAggregatedGdpAndPopulation = JsonSerializor.Serializor(aggregateGdpAndPopulation.AggregateGdpPopulation);
            
            FileUtilities.WriteFileAsync("../../../../AggregateGDPPopulation/output/output.json", FinalAggregatedGdpAndPopulation);
            return JsonSerializor.DeSerializor(FinalAggregatedGdpAndPopulation);
        }
    }
 }

