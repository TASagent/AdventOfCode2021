using AoCTools;
using System.Text.RegularExpressions;

const string inputFile = @"../../../../input17.txt";


Console.WriteLine("Day 17 - Trick Shot");
Console.WriteLine("Star 1");
Console.WriteLine();


Regex valueIdentifier = new Regex(@"([-0-9])+");

int[] positions = valueIdentifier
    .Matches(File.ReadAllText(inputFile))
    .Select(x => int.Parse(x.Value))
    .ToArray();

Point2D min = (positions[0], positions[2]);
Point2D max = (positions[1], positions[3]);

//Step 1: Find the Largest Y-velocity that could be a bit.

//Max Y-velocity is -min.y
//It takes a step from (X, 0) to (?, min.y)

int maxYVelocity = -min.y - 1;
int maxHeight = (maxYVelocity * (maxYVelocity + 1)) / 2;

Console.WriteLine($"The answer is: {maxHeight}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();


int minYVelocity = min.y;

//(x^2 + x) / 2 >= min.x
//(x^2 + x) >= 2 * min.x
//x^2 + x - 2 * min.x >= 0

//(-b +/- Sqrt(b^2 - 4 a c) ) / 2 a

int minXVelocity = 10;
int maxXVelocity = max.x;

int hitTargetCount = 0;


for (int xVelocity = minXVelocity; xVelocity <= maxXVelocity; xVelocity++)
{
    for (int yVelocity = minYVelocity; yVelocity <= maxYVelocity; yVelocity++)
    {
        //Test to see if it can hit.
        Point2D velocity = (xVelocity, yVelocity);
        Point2D position = (0, 0);

        while (position.x <= max.x && position.y >= min.y)
        {
            //Step Forward
            position += velocity;

            //Check Target
            if (CheckPoint(position))
            {
                hitTargetCount++;
                break;
            }

            //Adjust Velocity
            velocity = (Math.Max(0, velocity.x - 1), velocity.y - 1);
        }
    }
}

Console.WriteLine($"The answer is: {hitTargetCount}");

Console.WriteLine();
Console.ReadKey();


bool CheckPoint(Point2D position)
{
    if (position.x < min.x || position.x > max.x)
    {
        return false;
    }

    if (position.y < min.y || position.y > max.y)
    {
        return false;
    }

    return true;
}