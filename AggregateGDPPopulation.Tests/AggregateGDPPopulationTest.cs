using System;
using Xunit;
using System.IO;
using AggregateGDPPopulation;
namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //string[] expected = File.ReadAllLines(@"../../../../AggregateGDPPopulation.Tests/expected-output.json");
            Class1 c = new Class1();
            c.GenerateOutput();
            //string[] actual = File.ReadAllLines(@"../../../../AggregateGDPPopulation/output/output.json");
            
            //gdp.CalculateAggregateGdp();
            StreamReader FileRead = new StreamReader("../../../../AggregateGDPPopulation/output/output.json");
            StreamReader ExpectedFileRead = new StreamReader("../../../expected-output.json");
            string actual = FileRead.ReadToEnd();
            string expected = String.Empty;
            while (true)
            {
                try
                {
                    string content = ExpectedFileRead.ReadLine().Trim();
                    expected += content;
                }
                catch (Exception e)
                {
                    break;
                }
            }
            Console.WriteLine(actual);
            Console.WriteLine(expected);
            Assert.Equal(expected, actual);
        }
    }
}
