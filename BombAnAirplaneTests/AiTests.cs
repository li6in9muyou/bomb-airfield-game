using Ai;
using Common;
using Xunit.Abstractions;

namespace BombAnAirplaneTests;

public class AiTests
{
    private readonly ITestOutputHelper _output;

    public AiTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void ShouldFindAllPossibleAirplanePlacements()
    {
        var ai = new BombAnAirplaneAi();
        var possibilities = ai.GetAllPrediction(
            Array.Empty<Tuple<Coordinate, BombResult>>()
        );
        Assert.Equal(3, possibilities.Length);
    }

    [Fact]
    public void ShouldPrettyPrintAirfield()
    {
        var node = new Node(9);
        _output.WriteLine(node.ToString());
    }
}