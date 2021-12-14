using AoCTools;

const string inputFile = @"../../../../input11.txt";
//const string inputFile = @"../../../../input11Test.txt";



Console.WriteLine("Day 11 - Dumbo Octopus");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] inputFileLines = File.ReadAllLines(inputFile);

int[,] grid = inputFileLines
    .Select(x => x
        .Select(c => int.Parse(c.ToString()))).ToArrayGrid();

HashSet<Point2D> explodedOctopodes = new HashSet<Point2D>();
HashSet<Point2D> pendingExplosion = new HashSet<Point2D>();

long explosionCount = 0;

for (int step = 0; step < 100; step++)
{
    //First, Increment All
    for (int x = 0; x < grid.GetLength(0); x++)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            if (grid[x, y]++ >= 9)
            {
                pendingExplosion.Add((x, y));
            }
        }
    }

    while (pendingExplosion.Count > 0)
    {
        Point2D p = pendingExplosion.First();
        pendingExplosion.Remove(p);
        explosionCount++;

        foreach (Point2D neighbor in p.GetFullAdjacent().Where(IsInRange))
        {
            if (grid[neighbor.x, neighbor.y]++ >= 9 && 
                !explodedOctopodes.Contains(neighbor))
            {
                pendingExplosion.Add(neighbor);
            }
        }

        explodedOctopodes.Add(p);
    }

    foreach (Point2D octopus in explodedOctopodes)
    {
        grid[octopus.x, octopus.y] = 0;
    }

    explodedOctopodes.Clear();
}


Console.WriteLine($"The answer is: {explosionCount}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

int output2 = 100;

while (true)
{
    output2++;
    //First, Increment All
    for (int x = 0; x < grid.GetLength(0); x++)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            if (grid[x, y]++ >= 9)
            {
                pendingExplosion.Add((x, y));
            }
        }
    }

    while (pendingExplosion.Count > 0)
    {
        Point2D p = pendingExplosion.First();
        pendingExplosion.Remove(p);

        foreach (Point2D neighbor in p.GetFullAdjacent().Where(IsInRange))
        {
            if (grid[neighbor.x, neighbor.y]++ >= 9 &&
                !explodedOctopodes.Contains(neighbor))
            {
                pendingExplosion.Add(neighbor);
            }
        }

        explodedOctopodes.Add(p);
    }

    foreach (Point2D octopus in explodedOctopodes)
    {
        grid[octopus.x, octopus.y] = 0;
    }

    if (explodedOctopodes.Count == grid.Length)
    {
        break;
    }

    explodedOctopodes.Clear();
}


Console.WriteLine($"The answer is: {output2}");



Console.WriteLine();
Console.ReadKey();


bool IsInRange(Point2D point)
{
    if (point.x < 0)
    {
        return false;
    }

    if (point.y < 0)
    {
        return false;
    }

    if (point.x >= grid.GetLength(0))
    {
        return false;
    }

    if (point.y >= grid.GetLength(1))
    {
        return false;
    }

    return true;
}