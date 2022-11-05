using Common;

namespace Database;

public record GameStateSnapShot
(
    Airplane[] AiPredictedOpponentAirplanes,
    Tuple<Coordinate, BombResult>[] BombResultsOnOpponentAirfield,
    Coordinate[] MyAirfieldWasBombedAt,
    Airplane[] MyAirplanes
);