using AoCTools;

const string inputFile = @"../../../../input09.txt";


Console.WriteLine("Day 09 - Smoke Basin");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] inputFileLines = File.ReadAllLines(inputFile);

int[,] grid = inputFileLines
    .Select(x => x
        .Select(c => int.Parse(c.ToString()))).ToArrayGrid();

long output1 = 0;

Point2D gridMin = (0, 0);
Point2D gridMax = (grid.GetLength(0), grid.GetLength(1));

List<Point2D> localMinima = new List<Point2D>();

for (int x = 0; x < grid.GetLength(0); x++)
{
    for (int y = 0; y < grid.GetLength(1); y++)
    {
        if (CheckCoordinates(x, y))
        {
            localMinima.Add((x, y));
            output1 += 1 + grid[x, y];
        }
    }
}

Console.WriteLine($"The answer is: {output1}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

List<long> basinSizes = localMinima
    .Select(CalculateSize)
    .OrderByDescending(x => x)
    .ToList();


Console.WriteLine($"The answer is: {basinSizes[0] * basinSizes[1] * basinSizes[2]}");



Console.WriteLine();
Console.ReadKey();


bool CheckCoordinates(int x, int y)
{
    int targetValue = grid[x, y];

    if (x > 0)
    {
        if (grid[x - 1, y] <= targetValue)
        {
            return false;
        }
    }

    if (y > 0)
    {
        if (grid[x, y - 1] <= targetValue)
        {
            return false;
        }
    }

    if (x < grid.GetLength(0) - 1)
    {
        if (grid[x + 1, y] <= targetValue)
        {
            return false;
        }
    }

    if (y < grid.GetLength(1) - 1)
    {
        if (grid[x, y + 1] <= targetValue)
        {
            return false;
        }
    }

    return true;
}

long CalculateSize(Point2D min)
{
    long count = 0;
    HashSet<Point2D> pendingPoints = new HashSet<Point2D>() { min };
    HashSet<Point2D> handledPoints = new HashSet<Point2D>();

    while (pendingPoints.Count > 0)
    {
        Point2D newPoint = pendingPoints.First();
        pendingPoints.Remove(newPoint);

        if (grid[newPoint.x, newPoint.y] != 9)
        {
            count++;

            foreach (Point2D neighbor in newPoint.GetAdjacent())
            {
                if (!handledPoints.Contains(neighbor))
                {
                    if (IsInBounds(neighbor))
                    {
                        pendingPoints.Add(neighbor);
                    }
                }
            }
        }
        handledPoints.Add(newPoint);
    }

    return count;
}

bool IsInBounds(Point2D point)
{
    if (point.x < gridMin.x || point.x >= gridMax.x)
    {
        return false;
    }

    if (point.y < gridMin.y || point.y >= gridMax.y)
    {
        return false;
    }

    return true;
}
