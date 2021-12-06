const string inputFile = @"../../../../input04.txt";


Console.WriteLine("Day 04 - Giant Squid");
Console.WriteLine("Star 1");
Console.WriteLine();

List<string> rawLines = File.ReadAllLines(inputFile).ToList();

List<int> numbers = rawLines[0]
    .Split(',')
    .Select(int.Parse)
    .ToList();

IEnumerator<string> lineEnumerator = rawLines.GetEnumerator();
lineEnumerator.MoveNext();
lineEnumerator.MoveNext();

List<BingoBoard> boards = new List<BingoBoard>();

while (lineEnumerator.MoveNext())
{
    boards.Add(new BingoBoard(lineEnumerator));
}

int star1Answer = 0;
int star2Answer = 0;
foreach (int value in numbers)
{
    foreach(BingoBoard board in boards.ToList())
    {
        if (board.MarkNumber(value))
        {
            if (star1Answer == 0)
            {
                star1Answer = board.GetUnmarkedSum() * value;
            }

            if (boards.Count == 1)
            {
                star2Answer = board.GetUnmarkedSum() * value;
                break;
            }

            boards.Remove(board);
        }
    }

    if (star2Answer != 0)
    {
        break;
    }
}

Console.WriteLine();
Console.WriteLine($"The answer is: {star1Answer}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

Console.WriteLine($"The answer is: {star2Answer}");

Console.WriteLine();
Console.ReadKey();


class BingoBoard
{
    private readonly HashSet<int> numbers = new HashSet<int>();
    private readonly int[,] grid = new int[5, 5];
    private readonly bool[,] markedGrid = new bool[5, 5];

    public BingoBoard(IEnumerator<string> input)
    {
        for (int y = 0; y < 5; y++)
        {
            List<int> line = input.Current
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            for (int x = 0; x < 5; x++)
            {
                grid[x, y] = line[x];
                numbers.Add(line[x]);
            }

            input.MoveNext();
        }
    }

    public bool MarkNumber(int value)
    {
        if (!numbers.Contains(value))
        {
            return false;
        }

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (grid[x,y] == value)
                {
                    markedGrid[x, y] = true;

                    int rowCount = 0;
                    int colCount = 0;
                    for (int z = 0; z < 5; z++)
                    {
                        if (markedGrid[z, y]) rowCount++;
                        if (markedGrid[x, z]) colCount++;
                    }

                    if (rowCount == 5 || colCount == 5)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public int GetUnmarkedSum()
    {
        int sum = 0;
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0;x < 5; x++)
            {
                if (!markedGrid[x, y])
                {
                    sum += grid[x, y];
                }
            }
        }

        return sum;
    }
}
