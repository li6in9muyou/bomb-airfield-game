using Common;

namespace Database;

public record GameStateSnapShot
(
    //Airplane[] AiPredictedOpponentAirplanes,
    Tuple<Coordinate, BombResult>[] BombResultsOnOpponentAirfield,
    Tuple<Coordinate, BombResult>[] MyAirfieldWasBombedAt,
    Airplane[] MyAirplanes
);