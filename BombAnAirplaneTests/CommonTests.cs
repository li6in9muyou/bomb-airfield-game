using Common;
using GameLogic;
using Newtonsoft.Json;

namespace BombAnAirplaneTests;

public class CommonTests
{
    [Fact]
    public void ShouldLoadAndDumpCoordinate()
    {
        const string t = @"{""X"":1,""Y"":11}";
        Assert.Equal(
            t,
            JsonConvert.SerializeObject(new Coordinate(1, 11))
        );
        var cc = JsonConvert.DeserializeObject<Coordinate>(t);
        Assert.NotNull(cc);
        Assert.Equal(1, cc!.X);
        Assert.Equal(11, cc.Y);
    }

    [Fact]
    public void ShouldLoadAndDumpAirplanePlacement()
    {
        const string t = @"{""Direction"":""u"",""HeadCoord"":{""X"":1,""Y"":11}}";
        Assert.Equal(
            t,
            JsonConvert.SerializeObject(new AirplanePlacement
            {
                Direction = "u",
                HeadCoord = new Coordinate(1, 11)
            })
        );
        var cc = JsonConvert.DeserializeObject<AirplanePlacement>(t);
        Assert.NotNull(cc);
        Assert.Equal("u", cc.Direction);
        Assert.Equal(new Coordinate(1, 11), cc.HeadCoord);
    }

    [Fact]
    public void ShouldSerializeBombResultToJson()
    {
        Assert.Equal(
            "Miss",
            JsonConvert.SerializeObject(BombResult.Miss)
        );
    }
}