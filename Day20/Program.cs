using AoCTools;

const string inputFile = @"../../../../input20.txt";

Console.WriteLine("Day 20 - Trench Map");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] inputLines = File.ReadAllLines(inputFile);
string[] imageLines = inputLines[2..];

string lookup = inputLines[0];

Point2D min = (0, 0);
Point2D max = (imageLines[0].Length, imageLines.Length);
HashSet<Point2D> priorPixels = new HashSet<Point2D>();
HashSet<Point2D> illuminatedPixels = new HashSet<Point2D>();
bool outsideLit = false;


for (int y = 0; y < imageLines.Length; y++)
{
    for (int x = 0; x < imageLines[y].Length; x++)
    {
        if (imageLines[y][x] == '#')
        {
            priorPixels.Add((x, y));
        }
    }
}

for (int i = 0; i < 2; i++)
{
    //Do Stuff
    for (int y = min.y - 1; y <= max.y + 1; y++)
    {
        for (int x = min.x - 1; x <= max.x + 1; x++)
        {
            int lookupIndex = DecodePixel((x, y));
            if (lookup[lookupIndex] == '#')
            {
                illuminatedPixels.Add((x, y));
            }
        }
    }

    //Expand borders
    min += (-1, -1);
    max += (1, 1);

    //Flip Buffers
    (priorPixels, illuminatedPixels) = (illuminatedPixels, priorPixels);
    illuminatedPixels.Clear();
    outsideLit = !outsideLit;
}

Console.WriteLine($"The answer is: {priorPixels.Count}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

for (int i = 2; i < 50; i++)
{
    //Do Stuff
    for (int y = min.y - 1; y <= max.y + 1; y++)
    {
        for (int x = min.x - 1; x <= max.x + 1; x++)
        {
            int lookupIndex = DecodePixel((x, y));
            if (lookup[lookupIndex] == '#')
            {
                illuminatedPixels.Add((x, y));
            }
        }
    }

    //Expand borders
    min += (-1, -1);
    max += (1, 1);

    //Flip Buffers
    (priorPixels, illuminatedPixels) = (illuminatedPixels, priorPixels);
    illuminatedPixels.Clear();
    outsideLit = !outsideLit;
}
Console.WriteLine($"The answer is: {priorPixels.Count}");



Console.WriteLine();
Console.ReadKey();


int DecodePixel(Point2D position)
{
    int outputIndex = 0;
    for (int y = position.y - 1; y <= position.y + 1; y++)
    {
        for (int x = position.x - 1; x <= position.x + 1; x++)
        {
            Point2D point = (x, y);
            outputIndex <<= 1;

            if (point.x < min.x || point.x > max.x || point.y < min.y || point.y > max.y)
            {
                //Toggling exterior
                if (outsideLit)
                {
                    outputIndex |= 1;
                }
            }
            else if (priorPixels.Contains(point))
            {
                outputIndex |= 1;
            }
        }
    }

    return outputIndex;
}
