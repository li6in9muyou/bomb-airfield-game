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
        var possibilities = ai.GetAllPrediction();
        Assert.Equal(66816, possibilities.Length);
    }

    [Fact]
    public void ShouldReproduceOriginalBlogPostResults()
    {
        var ai = new BombAnAirplaneAi();
        Assert.Equal(new Coordinate(1, 3), ai.SuggestNextBombLocation());
        Assert.Equal(66816, ai.GetAllPrediction().Length);

        ai.LogBombResult(1, 3, BombResult.Miss);
        Assert.Equal(44758, ai.GetAllPrediction().Length);
        Assert.Equal(new Coordinate(3, 2), ai.SuggestNextBombLocation());

        ai.LogBombResult(3, 2, BombResult.Destroyed);
        Assert.Equal(3122, ai.GetAllPrediction().Length);
        Assert.Equal(new Coordinate(6, 6), ai.SuggestNextBombLocation());

        ai.LogBombResult(6, 6, BombResult.Miss);
        Assert.Equal(1566, ai.GetAllPrediction().Length);
        Assert.Equal(new Coordinate(4, 7), ai.SuggestNextBombLocation());
    }

    [Fact]
    public void ShouldPrettyPrintAirfield()
    {
        var node = new Node(9);
        _output.WriteLine(node.ToString());
    }
}