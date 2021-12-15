using AoCTools;

const string inputFile = @"../../../../input15.txt";


Console.WriteLine("Day 15 - Chiton");
Console.WriteLine("Star 1");
Console.WriteLine();


string[] inputFileLines = File.ReadAllLines(inputFile);

int[,] grid = inputFileLines
    .Select(x => x
        .Select(c => int.Parse(c.ToString()))).ToArrayGrid();

int[,] totalCost = new int[grid.GetLength(0), grid.GetLength(1)];

int gridXSize = grid.GetLength(0);
int gridYSize = grid.GetLength(1);


for (int y = 0; y < grid.GetLength(1); y++)
{
    for (int x = 0; x < grid.GetLength(0); x++)
    {
        totalCost[x, y] = int.MaxValue;
    }
}

totalCost[0, 0] = 0;

//HashSet<Point2D> pendingPoints = new HashSet<Point2D>() { (0, 0) };
HashSet<Point2D> pendingPoints = new HashSet<Point2D>() { };

while (pendingPoints.Count > 0)
{
    Point2D nextPoint = pendingPoints.First();
    pendingPoints.Remove(nextPoint);

    int cumulativeCost = totalCost[nextPoint.x, nextPoint.y];

    foreach (Point2D adjacent in nextPoint.GetAdjacent().Where(InRange))
    {
        int newCost = cumulativeCost + grid[adjacent.x, adjacent.y];
        if (newCost < totalCost[adjacent.x, adjacent.y])
        {
            totalCost[adjacent.x, adjacent.y] = newCost;
            pendingPoints.Add(adjacent);
        }
    }
}

Console.WriteLine($"The answer is: {totalCost[grid.GetLength(0) - 1, grid.GetLength(1) - 1]}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

PriorityQueue<Point2D, long> priorityQueue = new PriorityQueue<Point2D, long>();
Dictionary<Point2D, int> totalCost2 = new Dictionary<Point2D, int>()
{
    { (0,0), 0 }
};

Point2D newTarget = (gridXSize * 5 - 1, gridYSize * 5 - 1);

priorityQueue.Enqueue(Point2D.Zero, GetPriority(Point2D.Zero));
HashSet<Point2D> enqueuedPoints = new HashSet<Point2D>() { Point2D.Zero };

while (priorityQueue.Count > 0)
{
    Point2D nextPoint = priorityQueue.Dequeue();
    enqueuedPoints.Remove(nextPoint);

    int cumulativeCost = totalCost2[nextPoint];

    foreach (Point2D adjacent in nextPoint.GetAdjacent().Where(InRange2))
    {
        int newCost = cumulativeCost + GetCost(adjacent);
        if (!totalCost2.ContainsKey(adjacent) ||
            newCost < totalCost2[adjacent])
        {
            totalCost2[adjacent] = newCost;

            if (adjacent == newTarget)
            {
                goto Done;
            }

            if (enqueuedPoints.Add(adjacent))
            {
                priorityQueue.Enqueue(adjacent, GetPriority(adjacent));
            }
        }
    }
}

Done:

Console.WriteLine($"The answer is: {totalCost2[newTarget]}");


Console.WriteLine();
Console.ReadKey();


bool InRange(Point2D point)
{
    if (point.x < 0 || point.y < 0)
    {
        return false;
    }

    if (point.x >= grid.GetLength(0) || point.y >= grid.GetLength(1))
    {
        return false;
    }

    return true;
}

int GetCost(in Point2D point)
{
    int xWrap = point.x / gridXSize;
    int yWrap = point.y / gridYSize;

    return ((grid[point.x % gridXSize, point.y % gridYSize] + xWrap + yWrap - 1) % 9) + 1;
}

long GetPriority(in Point2D point) => totalCost2[point];

bool InRange2(Point2D point)
{
    if (point.x < 0 || point.y < 0)
    {
        return false;
    }

    if (point.x >= 5 * grid.GetLength(0) || point.y >= 5 * grid.GetLength(1))
    {
        return false;
    }

    return true;
}
