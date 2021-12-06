using AoCTools;

const string inputFile = @"../../../../input05.txt";


Console.WriteLine("Day 05 - Hydrothermal Venture");
Console.WriteLine("Star 1");
Console.WriteLine();

List<Line> lines = File.ReadAllLines(inputFile)
    .Select(x => new Line(x))
    .ToList();

Console.WriteLine();

Dictionary<Point2D, int> ventMap = new Dictionary<Point2D, int>();

foreach (Line line in lines.Where(x => x.IsOrthogonal))
{
    Point2D startPoint = line.start;
    Point2D difference = line.end - line.start;
    Point2D step = difference / Math.Max(Math.Abs(difference.x), Math.Abs(difference.y));

    int stepCount = Math.Max(Math.Abs(difference.x), Math.Abs(difference.y)) + 1;

    for (int i = 0; i < stepCount; i++)
    {
        if (ventMap.ContainsKey(startPoint + i * step))
        {
            ventMap[startPoint + i * step]++;
        }
        else
        {
            ventMap[startPoint + i * step] = 1;
        }
    }
}

int output1 = ventMap.Values.Count(x => x > 1);

Console.WriteLine($"The answer is: {output1}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

//Continue accumulating
foreach (Line line in lines.Where(x => !x.IsOrthogonal))
{
    Point2D startPoint = line.start;
    Point2D difference = line.end - line.start;
    Point2D step = difference / Math.Max(Math.Abs(difference.x), Math.Abs(difference.y));

    //Console.WriteLine($"From {line.start} to {line.end}:");

    int stepCount = Math.Max(Math.Abs(difference.x), Math.Abs(difference.y)) + 1;

    for (int i = 0; i < stepCount; i++)
    {
        //Console.WriteLine($" Adding {startPoint + i * step}");
        if (ventMap.ContainsKey(startPoint + i * step))
        {
            ventMap[startPoint + i * step]++;
        }
        else
        {
            ventMap[startPoint + i * step] = 1;
        }
    }
}

int output2 = ventMap.Values.Count(x => x > 1);

Console.WriteLine($"The answer is: {output2}");



Console.WriteLine();
Console.ReadKey();

class Line
{
    public readonly Point2D start;
    public readonly Point2D end;

    public bool IsOrthogonal =>
        start.x == end.x ||
        start.y == end.y;

    public Line(string input)
    {
        List<int> values = input.Split(
            new[] { " -> ", "," },
            StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToList();

        start = (values[0], values[1]);
        end = (values[2], values[3]);
    }
}

