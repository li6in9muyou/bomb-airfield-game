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

    public override string ToString()
    {
        var ans = "";
        var n = _airfieldSideLength;
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                ans += Airfield[i][j] switch
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
}

public class BombAnAirplaneAi
{
    private static readonly int[][] AirplaneHeadingNorth = Reshape(
        9, 2,
        new[]
        {
            +1, -2, +1, -1, +1, 0, +1, +1, +1, +2, +2, 0, +3, -1, +3, 0, +3, +1
        }
    );

    private static readonly int[][] AirplaneHeadingSouth = Reshape(
        9, 2,
        new[]
        {
            -1, -2, -1, -1, -1, 0, -1, +1, -1, +2, -2, 0, -3, -1, -3, 0, -3, +1
        }
    );

    private static readonly int[][] AirplaneHeadingWest = Reshape(
        9, 2,
        new[]
        {
            -2, +1, -1, +1, 0, +1, +1, +1, +2, +1, 0, +2, -1, +3, 0, +3, +1, +3
        }
    );

    private static readonly int[][] AirplaneHeadingEast = Reshape(
        9, 2,
        new[]
        {
            -2, -1, -1, -1, 0, -1, +1, -1, +2, -1, 0, -2, -1, -3, 0, -3, +1, -3
        }
    );

    private readonly uint _airfieldSideLength;
    private readonly uint _airplaneCount;
    private AirfieldGridStatus[][] _workingAirfield;

    private BombAnAirplaneAi(uint airfieldSideLength, uint airplaneCount)
    {
        _airfieldSideLength = airfieldSideLength;
        _airplaneCount = airplaneCount;

        var tmp = new List<AirfieldGridStatus[]>();
        for (var i = 0; i < _airfieldSideLength; i++)
        {
            var row = new List<AirfieldGridStatus>();
            for (var j = 0; j < _airfieldSideLength; j++) row.Add(AirfieldGridStatus.Empty);
            tmp.Add(row.ToArray());
        }

        _workingAirfield = tmp.ToArray();
    }


    public BombAnAirplaneAi() : this(10, 3)
    {
    }

    private static int[][] Reshape(int rows, int cols, IReadOnlyList<int> array)
    {
        var ans = new List<int[]>();
        for (var i = 0; i < rows; i += 1)
        {
            var row = new int[cols];
            for (var j = 0; j < cols; j++) row[j] = array[i * cols + j];
            ans.Add(row);
        }

        return ans.ToArray();
    }

    private void Dfs(int nthAirplane, List<Node> vn)
    {
        if (nthAirplane > _airplaneCount)
        {
            var node = new Node(_workingAirfield);
            vn.Add(node);
            return;
        }

        var b = DeepCopy(_workingAirfield);

        for (var dir = 0; dir < 4; dir++)
        for (var i = 0; i < _airfieldSideLength; i++)
        for (var j = 0; j < _airfieldSideLength; j++)
        {
            _workingAirfield = DeepCopy(b);
            var validPlacement = true;
            if (_workingAirfield[i][j] != AirfieldGridStatus.Empty) continue;

            _workingAirfield[i][j] = AirfieldGridStatus.Cockpit;
            for (var k = 0; k < 9; k++)
            {
                int ii, jj;
                switch (dir)
                {
                    case 0:
                        ii = i + AirplaneHeadingNorth[k][0];
                        jj = j + AirplaneHeadingNorth[k][1];
                        break;
                    case 1:
                        ii = i + AirplaneHeadingSouth[k][0];
                        jj = j + AirplaneHeadingSouth[k][1];
                        break;
                    case 2:
                        ii = i + AirplaneHeadingWest[k][0];
                        jj = j + AirplaneHeadingWest[k][1];
                        break;
                    default:
                        ii = i + AirplaneHeadingEast[k][0];
                        jj = j + AirplaneHeadingEast[k][1];
                        break;
                }

                if (ii < 0 || ii >= _airfieldSideLength || jj < 0 || jj >= _airfieldSideLength)
                {
                    validPlacement = false;
                    break;
                }

                if (_workingAirfield[ii][jj] != AirfieldGridStatus.Empty)
                {
                    validPlacement = false;
                    break;
                }

                _workingAirfield[ii][jj] = AirfieldGridStatus.Fuselage;
            }

            if (validPlacement) Dfs(nthAirplane + 1, vn);
        }
    }

    public AirfieldGridStatus[][][] GetAllPrediction(
        Tuple<Coordinate, BombResult>[] currentBombResult
    )
    {
        for (var i = 0; i < _airfieldSideLength; i++)
        for (var j = 0; j < _airfieldSideLength; j++)
            _workingAirfield[i][j] = AirfieldGridStatus.Empty;
        List<Node> temp = new(), ret = new();

        Dfs(1, temp);

        HashSet<Node> unique = new();
        foreach (var node in temp.Where(node => !unique.Contains(node)))
        {
            ret.Add(node);
            Console.Out.WriteLine(node);
            unique.Add(node);
        }

        return ret.Select(node => node.Airfield).ToArray();
    }

    private static AirfieldGridStatus[][] DeepCopy(AirfieldGridStatus[][] source)
    {
        var tmp = new List<AirfieldGridStatus[]>();
        foreach (var t in source)
        {
            var row = new List<AirfieldGridStatus>();
            for (var j = 0; j < source[0].Length; j++)
                row.Add(t[j]);
            tmp.Add(row.ToArray());
        }

        return tmp.ToArray();
    }
}