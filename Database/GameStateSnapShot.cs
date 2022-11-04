using Common;

namespace Database;

public record GameStateSnapShot
{
    public IEnumerable<Airplane> AiPredictedOpponentAirplanes = Array.Empty<Airplane>();

    public IEnumerable<Tuple<Coordinate, BombResult>> BombResultsOnOpponentAirfield =
        Array.Empty<Tuple<Coordinate, BombResult>>();

    public IEnumerable<Coordinate> MyAirfieldWasBombedAt = Array.Empty<Coordinate>();

    public IEnumerable<Airplane> MyAirplanes = Array.Empty<Airplane>();
}