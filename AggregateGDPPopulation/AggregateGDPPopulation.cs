using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AggregateGDPPopulation
{
    public class class1
    {
        public float GDP_2012 { get; set; }
        public float POPULATION_2012 { get; set; }
    }
    public class AggregateGDP
    {
        public List<string> FileContents { get; set; }
        public string CountryContinentJSONFormat { get; set; }

        public async Task<List<string>> ReadFile(string path)
        {
            try
            {
                StreamReader FileContents = new StreamReader(path);
                List<string> Contents = new List<string>();
                string s;
                while ((s = await FileContents.ReadLineAsync()) != null)
                {
                    Contents.Add(s);
                }
                return Contents;
            }
            catch (Exception) { return new List<string>(); }
        }
        public async Task<string> ReadCountryContinent(string path)
        {
            StreamReader FileContents = new StreamReader(path);
            string s = await FileContents.ReadToEndAsync();
            return s;
        }
        public async void ReadMapAndCSVFile(string CSVPath, string CountryContinentMapPath)
        {
            Task<List<string>> FileContentsTask = ReadFile(CSVPath);
            Task<string> JSONMapTask = ReadCountryContinent(CountryContinentMapPath);
            FileContents = await FileContentsTask;
            CountryContinentJSONFormat = await JSONMapTask;
        }
        public async void AggregatedDataWrite(string FilePath, string Content)
        {
            using (StreamWriter FileContents = new StreamWriter(FilePath))
            {
                await FileContents.WriteAsync(Content);
            }
        }
        public void AggregateGdp()
        {
           
            ReadMapAndCSVFile("../../../../AggregateGDPPopulation/data/datafile.csv", "../../../../AggregateGDPPopulation/data/country-continent-map.json");
            var CountryContinentMap = JObject.Parse(CountryContinentJSONFormat);
           
            string headerText = FileContents[0];
            List<string> headers = headerText.Split(',').ToList();
            int IndexOfCountry = headers.IndexOf("\"Country Name\"");
            int IndexOfPopulation = headers.IndexOf("\"Population (Millions) 2012\"");
            int IndexOfGDP = headers.IndexOf("\"GDP Billions (USD) 2012\"");
            Dictionary<string, class1> JSONObject = new Dictionary<string, class1>();
            for (int i = 1; i < FileContents.Count; i++)
            {
                List<string> RowOfData = FileContents[i].Split(',').ToList();
                string Country = RowOfData[IndexOfCountry].Trim('\"');
                float Population = float.Parse(RowOfData[IndexOfPopulation].Trim('\"'));
                float Gdp = float.Parse(RowOfData[IndexOfGDP].Trim('\"'));
                try
                {
                    string Continent = CountryContinentMap.GetValue(RowOfData[IndexOfCountry].Trim('\"')).ToString();
                    try
                    {
                        JSONObject[Continent].GDP_2012 += Gdp;
                        JSONObject[Continent].POPULATION_2012 += Population;
                    }
                    catch (Exception)
                    {
                        class1 g = new class1() { GDP_2012 = Gdp, POPULATION_2012 = Population };
                        JSONObject.Add(Continent, g);
                    }
                }
                catch (Exception) { }
            }
            var AggregatedData = Newtonsoft.Json.JsonConvert.SerializeObject(JSONObject);
            AggregatedDataWrite("../../../../AggregateGDPPopulation/output/output.json", AggregatedData);
            
        }
    }
}