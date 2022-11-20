using Common;

namespace Ai;

public enum AirfieldGridStatus
{
    Unknown,
    Empty,
    Fuselage,
    Cockpit
}

public class Node
{
    private readonly uint _airfieldSideLength;
    public readonly AirfieldGridStatus[][] Airfield;

    public Node(uint airfieldSideLength)
    {
        _airfieldSideLength = airfieldSideLength;
        var tmp = new List<AirfieldGridStatus[]>();
        for (var i = 0; i < _airfieldSideLength; i++)
        {
            var row = new List<AirfieldGridStatus>();
            for (var j = 0; j < _airfieldSideLength; j++) row.Add(AirfieldGridStatus.Empty);
            tmp.Add(row.ToArray());
        }

        Airfield = tmp.ToArray();
    }

    public Node(AirfieldGridStatus[][] airfield)
    {
        Airfield = (AirfieldGridStatus[][])airfield.Clone();
    }

    public override int GetHashCode()
    {
        const long m = 10000000000037, p = 107;
        long ret = 1;
        for (var i = 1; i <= _airfieldSideLength; i++)
        for (var j = 1; j <= _airfieldSideLength; j++)
            if (Airfield[i][j] == AirfieldGridStatus.Cockpit || Airfield[i][j] == AirfieldGridStatus.Fuselage)
                ret = ret * (i * _airfieldSideLength + j) % m * p % m;
        return (int)ret;
    }

    public static string PrettyPrint(AirfieldGridStatus[][] airfield)
    {
        var ans = "";
        var n = (int)Math.Floor(Math.Sqrt(airfield.Length));
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                ans += airfield[i][j] switch
                {
                    AirfieldGridStatus.Empty => "_",
                    AirfieldGridStatus.Fuselage => "*",
                    AirfieldGridStatus.Cockpit => "&",
                    AirfieldGridStatus.Unknown => "?",
                    _ => throw new ArgumentOutOfRangeException()
                };
                ans += " ";
            }

            ans += "\n";
        }

        return ans;
    }

    public override string ToString()
    {
        return PrettyPrint(Airfield);
    }
}

public class BombAnAirplaneAi
{
    private readonly List<AirfieldGridStatus[][]> _allAirfields = new(70000);

    private readonly AirfieldGridStatus[][] _knownOpponentAirfield =
    {
        new AirfieldGridStatus[10],
        new AirfieldGridStatus[10],
        new AirfieldGridStatus[10],
        new AirfieldGridStatus[10],
        new AirfieldGridStatus[10],
        new AirfieldGridStatus[10],
        new AirfieldGridStatus[10],
        new AirfieldGridStatus[10],
        new AirfieldGridStatus[10],
        new AirfieldGridStatus[10]
    };

    private AirfieldGridStatus[][][] _prospectiveAirfields;

    public BombAnAirplaneAi()
    {
        var allAirfields = File.ReadAllText("./dump.txt").Split("\r\n\r\n").Where(t => t.Length > 5);
        foreach (var airfield in allAirfields)
        {
            var text = airfield.Split("\r\n").Where(t => t.Length == 10).ToArray();
            var thisAirfield = new[]
            {
                new AirfieldGridStatus[10],
                new AirfieldGridStatus[10],
                new AirfieldGridStatus[10],
                new AirfieldGridStatus[10],
                new AirfieldGridStatus[10],
                new AirfieldGridStatus[10],
                new AirfieldGridStatus[10],
                new AirfieldGridStatus[10],
                new AirfieldGridStatus[10],
                new AirfieldGridStatus[10]
            };
            for (var i = 0; i < 10; i++)
            for (var j = 0; j < 10; j++)
                thisAirfield[i][j] = text[i][j] switch
                {
                    '@' => AirfieldGridStatus.Cockpit,
                    '*' => AirfieldGridStatus.Fuselage,
                    _ => AirfieldGridStatus.Empty
                };
            _allAirfields.Add(thisAirfield);
        }

        _prospectiveAirfields = _allAirfields.Where(_ => true).ToArray();
    }

    public void LogBombResult(int x, int y, BombResult result)
    {
        _knownOpponentAirfield[x][y] = result switch
        {
            BombResult.Destroyed => AirfieldGridStatus.Cockpit,
            BombResult.Hit => AirfieldGridStatus.Fuselage,
            BombResult.Miss => AirfieldGridStatus.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
        _prospectiveAirfields = _prospectiveAirfields.Where(
            airfield =>
            {
                return airfield[x][y] == result switch
                {
                    BombResult.Destroyed => AirfieldGridStatus.Cockpit,
                    BombResult.Hit => AirfieldGridStatus.Fuselage,
                    BombResult.Miss => AirfieldGridStatus.Empty,
                    _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
                };
            }).ToArray();
    }

    public Coordinate SuggestNextBombLocation()
    {
        int ii = 0, jj = 0, maxEarn = 0;
        for (var i = 0; i < 10; i++)
        for (var j = 0; j < 10; j++)
            if (_knownOpponentAirfield[i][j] == AirfieldGridStatus.Unknown)
            {
                int p1 = 0, p2 = 0, p3 = 0;
                foreach (var airfield in _prospectiveAirfields)
                    switch (airfield[i][j])
                    {
                        case AirfieldGridStatus.Fuselage:
                        {
                            p1++;
                            break;
                        }
                        case AirfieldGridStatus.Cockpit:
                        {
                            p2++;
                            break;
                        }
                        case AirfieldGridStatus.Empty:
                            p3++;
                            break;
                        case AirfieldGridStatus.Unknown:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                var earn = p3 * (p1 + p2) + p2 * (p1 + p3) + p1 * (p2 + p3);
                if (earn > maxEarn)
                {
                    ii = i;
                    jj = j;
                    maxEarn = earn;
                }
            }

        return new Coordinate(ii, jj);
    }

    public AirfieldGridStatus[][][] GetAllPrediction()
    {
        return _prospectiveAirfields;
    }

    public static Coordinate SuggestNextBombLocationAccordingToBombResults(
        IEnumerable<Tuple<Coordinate, BombResult>> history
    )
    {
        var ai = new BombAnAirplaneAi();
        foreach (var (coord, result) in history) ai.LogBombResult(coord.X, coord.Y, result);

        return ai.SuggestNextBombLocation();
    }
}