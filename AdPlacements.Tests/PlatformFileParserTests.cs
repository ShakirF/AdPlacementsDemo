using AdPlacements.Infrastructure.Parsing;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text;

namespace AdPlacements.Tests
{
    public class PlatformFileParserTests
    {
        private readonly PlatformFileParser _parser = new(NullLogger<PlatformFileParser>.Instance);

        private const string Data = """
        Яндекс.Директ:/ru
        bad line without colon
        Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
        Газета:4564876
        Крутая реклама:/ru/svrd
        """;

        [Fact]
        public void Parse_Should_Return_Only_Valid_Platforms()
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(Data));
            var platforms = _parser.Parse(stream).ToList();

            Assert.Equal(3, platforms.Count);            
            Assert.Contains(platforms, p => p.Name == "Яндекс.Директ");
            Assert.Contains(platforms, p => p.Name == "Ревдинский рабочий");
            Assert.Contains(platforms, p => p.Name == "Крутая реклама");
        }
    }
}