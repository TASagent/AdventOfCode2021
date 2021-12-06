using AoCTools;

const string inputFile = @"../../../../input02.txt";
 

Console.WriteLine("Day 02 - Dive");
Console.WriteLine("Star 1");
Console.WriteLine();

List<Point2D> directions = File.ReadAllLines(inputFile)
    .Select(Parse)
    .ToList();

Point2D position = (0, 0);

foreach(Point2D direction in directions)
{
    position += direction;
}


Console.WriteLine();

Console.WriteLine($"The answer is {position} and thus {position.x * position.y}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

position = (0, 0);
int depth = 0;

foreach (Point2D direction in directions)
{
    position += direction;

    if(direction.x > 0)
    {
        depth += position.y * direction.x;
    }
}

Console.WriteLine($"The answer is: {position.x * depth}");

Console.WriteLine();
Console.ReadKey();


static Point2D Parse(string input)
{
    if (input.StartsWith("forward"))
    {
        return new Point2D(int.Parse(input[8..]), 0);
    }
    else if (input.StartsWith("up"))
    {
        return new Point2D(0, -int.Parse(input[3..]));
    }
    else if (input.StartsWith("down"))
    {
        return new Point2D(0, int.Parse(input[5..]));
    }
    else throw new Exception();
}
