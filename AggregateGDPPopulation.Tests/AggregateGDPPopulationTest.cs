using System;
using Xunit;
using System.IO;
using AggregateGDPPopulation;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace AggregateGDPPopulation.Tests

{
    public class UnitTest1
    {
        [Fact]
        public async void CheckForFileContents()
        {
            CalculateAggragate obj = new CalculateAggragate();
            JObject actual = await obj.Main();
            string ExpectedOutput = await FileUtilities.FileReadAsyncReturnString("../../../expected-output.json");
            JObject expected = JsonSerializor.DeSerializor(ExpectedOutput);
            Assert.Equal(expected, actual);
        }
    }
}
