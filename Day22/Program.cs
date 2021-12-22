using System.Text.RegularExpressions;
using AoCTools;

const string inputFile = @"../../../../input22.txt";

Regex parser = new Regex(@"(on|off) x=([-\d]+)..([-\d]+),y=([-\d]+)..([-\d]+),z=([-\d]+)..([-\d]+)");

Console.WriteLine("Day 22 - Reactor Reboot");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] inputLines = File.ReadAllLines(inputFile);

List<Box> ranges = inputLines.Select(Parse).ToList();


Point3D zoneMin = (-50, -50, -50);
Point3D zoneMax = (50, 50, 50);

HashSet<Point3D> activatedPoints = new HashSet<Point3D>();

foreach(Box box in ranges)
{
    if (box.max.x < zoneMin.x ||
        box.max.y < zoneMin.y ||
        box.max.z < zoneMin.z)
    {
        continue;
    }

    if (box.min.x > zoneMax.x ||
        box.min.y > zoneMax.y ||
        box.min.z > zoneMax.z)
    {
        continue;
    }

    Point3D tempMin = AoCMath.Clamp(box.min, zoneMin, zoneMax);
    Point3D tempMax = AoCMath.Clamp(box.max, zoneMin, zoneMax);

    for (int x = tempMin.x; x <= tempMax.x; x++)
    {
        for (int y = tempMin.y; y <= tempMax.y; y++)
        {
            for (int z = tempMin.z; z <= tempMax.z; z++)
            {
                if (box.on)
                {
                    activatedPoints.Add((x, y, z));
                }
                else
                {
                    activatedPoints.Remove((x, y, z));
                }
            }
        }
    }
}

Console.WriteLine($"The answer is: {activatedPoints.Count}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

//int collisionCount = 0;

//for (int i = 21; i < ranges.Count - 1; i++)
//{
//    for (int j = i + 1; j < ranges.Count; j++)
//    {
//        (bool aOn, Point3D aMin, Point3D aMax) = ranges[i];
//        (bool bOn, Point3D bMin, Point3D bMax) = ranges[j];

//        if (GetCollision(aMin, aMax, bMin, bMax) && collisionCount < 100)
//        {
//            collisionCount++;

//            Console.WriteLine($"{(aOn ? "On " : "Off")} at {aMin,20} to {aMax,20} and {(bOn ? "On " : "Off")} at {bMin,20} to {bMax,20}");
//        }
//    }
//}

List<Box> boxes = new List<Box>();

foreach (Box box in (ranges as IEnumerable<Box>).Reverse())
{
    if (!boxes.Any(x=> box.Collides(x)))
    {
        boxes.Add(box);
    }
    else
    {
        //Nightmares begin
        List<Box> newBoxes = new List<Box>();



    }
}



Console.WriteLine($"The answer is: {0}");

Console.WriteLine();
Console.ReadKey();


Box Parse(string line)
{
    Match match = parser.Match(line);

    if (!match.Success)
    {
        throw new Exception();
    }

    bool on = (match.Groups[1].Value == "on");

    Point3D min = new Point3D(
        x: int.Parse(match.Groups[2].Value),
        y: int.Parse(match.Groups[4].Value),
        z: int.Parse(match.Groups[6].Value));

    Point3D max = new Point3D(
        x: int.Parse(match.Groups[3].Value),
        y: int.Parse(match.Groups[5].Value),
        z: int.Parse(match.Groups[7].Value));

    return new Box(on, min, max);
}

readonly struct Box
{
    public readonly bool on;
    public readonly Point3D min;
    public readonly Point3D max;

    public Box(
        bool on,
        Point3D min,
        Point3D max)
    {
        this.on = on;
        this.min = min;
        this.max = max;
    }

    public bool Collides(in Box other)
    {
        if (other.max.x < min.x || max.x < other.min.x ||
            other.max.y < min.y || max.y < other.min.y ||
            other.max.z < min.z || max.z < other.min.z)
        {
            return false;
        }

        return true;
    }
}
