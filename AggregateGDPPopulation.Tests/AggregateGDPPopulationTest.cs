using System;
using Xunit;
using System.IO;
using AggregateGDPPopulation;
namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async void CheckForFileContents()
        {
            AggregateGDP c = new AggregateGDP();
            c.AggregateGdp();
            string actual;
            string expected = String.Empty;
            using (StreamReader FileContentsByLine = new StreamReader("../../../../AggregateGDPPopulation/output/output.json"))
            {
                actual = await FileContentsByLine.ReadToEndAsync();
            }
            using (StreamReader ActualFileContents = new StreamReader("../../../expected-output.json"))
            {
                while (true)
                {
                    try
                    {
                        string content = await ActualFileContents.ReadLineAsync();
                        expected += content.Trim();
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }
            Assert.Equal(expected, actual);
        }
    }
}
