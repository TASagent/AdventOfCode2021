using AoCTools;

const string inputFile = @"../../../../input25.txt";

Console.WriteLine("Day 25 - Sea Cucumber");
Console.WriteLine("Star 1");
Console.WriteLine();

Dictionary<Point2D, Moocumber> moocumbers = new Dictionary<Point2D, Moocumber>();


string[] inputLines = File.ReadAllLines(inputFile);
Point2D max = (inputLines[0].Length, inputLines.Length);

for (int y = 0; y < inputLines.Length; y++)
{
    for (int x = 0; x < inputLines[y].Length; x++)
    {
        switch (inputLines[y][x])
        {
            case '>':
                moocumbers.Add((x, y), Moocumber.Right);
                break;

            case 'v':
                moocumbers.Add((x, y), Moocumber.Down);
                break;

            default:
                //Do nothing
                break;
        }
    }
}

bool movement;
int steps = 0;

do
{
    //Console.WriteLine();
    //Console.WriteLine($"Step {steps}");

    //for (int y = 0; y < max.y; y++)
    //{
    //    for (int x = 0; x < max.x; x++)
    //    {
    //        if (moocumbers.ContainsKey((x, y)))
    //        {
    //            if (moocumbers[(x, y)] == Moocumber.Down)
    //            {
    //                Console.Write('v');
    //            }
    //            else
    //            {
    //                Console.Write('>');
    //            }
    //        }
    //        else
    //        {
    //            Console.Write('.');
    //        }
    //    }
    //    Console.WriteLine();
    //}
    //Console.WriteLine();




    steps++;
    movement = false;

    //Right first

    List<Point2D> rightPositions = moocumbers
        .Where(x => x.Value == Moocumber.Right)
        .Select(x => x.Key)
        .Where(x => !moocumbers.ContainsKey(((x.x + 1) % max.x, x.y)))
        .ToList();

    foreach (Point2D position in rightPositions)
    {
        movement = true;
        moocumbers.Remove(position);
        moocumbers.Add(((position.x + 1) % max.x, position.y), Moocumber.Right);
    }

    //Then "Down"
    List<Point2D> downPositions = moocumbers
        .Where(x => x.Value == Moocumber.Down)
        .Select(x => x.Key)
        .Where(x => !moocumbers.ContainsKey((x.x, (x.y + 1) % max.y)))
        .ToList();

    foreach (Point2D position in downPositions)
    {
        movement = true;
        moocumbers.Remove(position);
        moocumbers.Add((position.x, (position.y + 1) % max.y), Moocumber.Down);
    }
}
while (movement);




Console.WriteLine($"The answer is: {steps}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

int output2 = 0;

Console.WriteLine($"The answer is: {output2}");



Console.WriteLine();
Console.ReadKey();


enum Moocumber
{
    Right,
    Down
}