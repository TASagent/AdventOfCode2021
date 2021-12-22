
using System.Diagnostics.CodeAnalysis;

const string inputFile = @"../../../../input21.txt";


Console.WriteLine("Day 21 - Dirac Dice");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] inputLines = File.ReadAllLines(inputFile);

int player1StartingPos = inputLines[0][^1] - '0';
int player2StartingPos = inputLines[1][^1] - '0';

int nextDieValue = 1;

int[] playerPositions = new int[] { player1StartingPos - 1, player2StartingPos - 1 };
int[] playerPoints = new int[] { 0, 0 };
int currentPlayer = 0;

//Die value is 1
// Roll 1 + 2 + 3 = 6

// Die value is 99
// Roll 99 + 100 + 1 = 200
// OR 99 + 100 + 101 = 300 % 10 = 0

while (true)
{
    int dieRoll = 3 * nextDieValue + 3;
    nextDieValue += 3;

    playerPositions[currentPlayer] = (playerPositions[currentPlayer] + dieRoll) % 10;

    playerPoints[currentPlayer] += playerPositions[currentPlayer] + 1;

    if (playerPoints[currentPlayer] >= 1000)
    {
        break;
    }

    currentPlayer = (currentPlayer + 1) % 2;
}

Console.WriteLine($"The answer is: {(nextDieValue - 1L) * (long)playerPoints.Min()}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

//21 points per player
//10 positions per player

//10 * 10 * 21 * 21 = 44100 unique board states


Dictionary<BoardState, long> pathCounts = new Dictionary<BoardState, long>();

HashSet<BoardState> pendingBoardStates = new HashSet<BoardState>();

pendingBoardStates.Add(new BoardState(0, 0, player1StartingPos - 1, player2StartingPos - 1, true));
pathCounts[pendingBoardStates.First()] = 1;

while (pendingBoardStates.Count > 0)
{
    //Handle next board state
    BoardState nextState = pendingBoardStates
        .OrderBy(x => x.player1Score)
        .ThenBy(x => x.player2Score)
        .First();
    pendingBoardStates.Remove(nextState);

    if (nextState.player1Score >= 21 || nextState.player2Score >= 21)
    {
        continue;
    }

    if (nextState.player1Turn)
    {
        foreach ((int newPlayer1Space, int universes) in GetRolls(nextState.player1Position))
        {
            BoardState newState = new BoardState(
                player1Score: nextState.player1Score + 1 + newPlayer1Space,
                player2Score: nextState.player2Score,
                player1Position: newPlayer1Space,
                player2Position: nextState.player2Position,
                player1Turn: false);

            if (pathCounts.ContainsKey(newState))
            {
                pathCounts[newState] += pathCounts[nextState] * universes;
            }
            else
            {
                pendingBoardStates.Add(newState);
                pathCounts[newState] = pathCounts[nextState] * universes;
            }
        }
    }
    else
    {
        foreach ((int newPlayer2Space, int universes) in GetRolls(nextState.player2Position))
        {
            BoardState newState = new BoardState(
                player1Score: nextState.player1Score,
                player2Score: nextState.player2Score + 1 + newPlayer2Space,
                player1Position: nextState.player1Position,
                player2Position: newPlayer2Space,
                player1Turn: true);

            if (pathCounts.ContainsKey(newState))
            {
                pathCounts[newState] += pathCounts[nextState] * universes;
            }
            else
            {
                pendingBoardStates.Add(newState);
                pathCounts[newState] = pathCounts[nextState] * universes;
            }
        }
    }
}

long player1Wins = pathCounts.Where(x => x.Key.player1Score >= 21).Select(x => x.Value).Sum();
long player2Wins = pathCounts.Where(x => x.Key.player2Score >= 21).Select(x => x.Value).Sum();

Console.WriteLine($"The answer is: {Math.Max(player1Wins, player2Wins)}");

Console.WriteLine();
Console.ReadKey();


//Rolling Distribution
// 0, 1, 2
// 0, 1, 2
// 0, 1, 2
// + 3

// 1x 3
// 3x 4
// 6x 5
// 7x 6
// 6x 7
// 3x 8
// 1x 9
static IEnumerable<(int spaces, int universes)> GetRolls(int position)
{
    yield return ((position + 3) % 10, 1);
    yield return ((position + 4) % 10, 3);
    yield return ((position + 5) % 10, 6);
    yield return ((position + 6) % 10, 7);
    yield return ((position + 7) % 10, 6);
    yield return ((position + 8) % 10, 3);
    yield return ((position + 9) % 10, 1);
}


class BoardState
{
    public readonly int player1Score;
    public readonly int player2Score;

    public readonly int player1Position;
    public readonly int player2Position;

    public readonly bool player1Turn;

    public BoardState(
        int player1Score,
        int player2Score,
        int player1Position,
        int player2Position,
        bool player1Turn)
    {
        this.player1Score = player1Score;
        this.player2Score = player2Score;
        this.player1Position = player1Position;
        this.player2Position = player2Position;
        this.player1Turn = player1Turn;
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is not BoardState other)
        {
            return false;
        }

        return this == other;
    }

    public override int GetHashCode() => HashCode.Combine(
        player1Score, player2Score, player1Position, player2Position, player1Turn);

    public static bool operator ==(in BoardState lhs, in BoardState rhs) =>
        lhs.player1Score == rhs.player1Score &&
        lhs.player2Score == rhs.player2Score &&
        lhs.player1Position == rhs.player1Position &&
        lhs.player2Position == rhs.player2Position &&
        lhs.player1Turn == rhs.player1Turn;

    public static bool operator !=(in BoardState lhs, in BoardState rhs) =>
        lhs.player1Score != rhs.player1Score ||
        lhs.player2Score != rhs.player2Score ||
        lhs.player1Position != rhs.player1Position ||
        lhs.player2Position != rhs.player2Position ||
        lhs.player1Turn != rhs.player1Turn;
}
