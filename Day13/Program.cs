using AoCTools;

const string inputFile = @"../../../../input13.txt";


Console.WriteLine("Day 13 - Transparent Origami");
Console.WriteLine("Star 1");
Console.WriteLine();

int output1 = 0;


string inputFileText = File.ReadAllText(inputFile).Replace("\r", "");

string[] inputFileSections = inputFileText.Split("\n\n");

List<Point2D> points = inputFileSections[0].Split("\n").Select(Parse).ToList();
List<(char axis, int position)> folds = inputFileSections[1].Split("\n").Select(ParseFold).ToList();

List<Point2D> foldedPoints = points.Select(x => Fold(x, folds[0])).Distinct().ToList();

Console.WriteLine($"The answer is: {foldedPoints.Count}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

for (int i = 1; i < folds.Count; i++)
{
    foldedPoints = foldedPoints
        .Select(x => Fold(x, folds[i]))
        .Distinct()
        .ToList();
}

HashSet<Point2D> finalPoints = new HashSet<Point2D>(foldedPoints);


Point2D minPoint = foldedPoints.MinCoordinate();
Point2D maxPoint = foldedPoints.MaxCoordinate();

Console.WriteLine();

for (int y = 0; y <= maxPoint.y; y++)
{
    for (int x = 0; x <= maxPoint.x; x++)
    {
        Console.BackgroundColor = finalPoints.Contains((x, y)) ?
            ConsoleColor.White : ConsoleColor.Black;
        Console.Write(" ");
    }

    Console.BackgroundColor = ConsoleColor.Black;
    Console.WriteLine();
}
Console.WriteLine();



Console.WriteLine();
Console.ReadKey();


Point2D Parse(string line)
{
    string[] segments = line.Split(",");
    return new Point2D(int.Parse(segments[0]), int.Parse(segments[1]));
}

(char axis, int position) ParseFold(string line)
{
    string[] segments = line.Split(new[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
    return (segments[2][0], int.Parse(segments[3]));
}

Point2D Fold(Point2D startingPoint, (char axis, int position) fold)
{
    if (fold.axis == 'x')
    {
        if (startingPoint.x <= fold.position)
        {
            return startingPoint;
        }

        return (2 * fold.position - startingPoint.x, startingPoint.y);
    }
    else
    {
        //y
        if (startingPoint.y <= fold.position)
        {
            return startingPoint;
        }

        return (startingPoint.x, 2 * fold.position - startingPoint.y);
    }


}