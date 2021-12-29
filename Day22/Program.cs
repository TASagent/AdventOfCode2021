using System.Text.RegularExpressions;
using System.Numerics;
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

foreach (Box box in ranges)
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

List<Box> placedBoxes = new List<Box>();
Stack<Box> pendingBoxes = new Stack<Box>();

foreach (Box inputBox in ranges)
{
    pendingBoxes.Push(inputBox);
}


while (pendingBoxes.Count > 0)
{
    Box newBox = pendingBoxes.Pop();

    if (!placedBoxes.Any(x => newBox.Collides(x)))
    {
        placedBoxes.Add(newBox);
    }
    else
    {
        //Nightmares begin

        //Grab the first colliding box
        Box collidingBox = placedBoxes.First(x => newBox.Collides(x));

        if (newBox.min.x < collidingBox.min.x)
        {
            pendingBoxes.Push(new Box(
                on: newBox.on,
                min: newBox.min,
                max: new Point3D(
                    x: collidingBox.min.x - 1,
                    y: newBox.max.y,
                    z: newBox.max.z)));

            newBox = new Box(
                on: newBox.on,
                min: (collidingBox.min.x, newBox.min.y, newBox.min.z),
                max: newBox.max);
        }

        if (newBox.max.x > collidingBox.max.x)
        {
            pendingBoxes.Push(new Box(
                on: newBox.on,
                min: new Point3D(
                    x: collidingBox.max.x + 1,
                    y: newBox.min.y,
                    z: newBox.min.z),
                max: newBox.max));

            newBox = new Box(
                on: newBox.on,
                min: newBox.min,
                max: (collidingBox.max.x, newBox.max.y, newBox.max.z));
        }

        if (newBox.min.y < collidingBox.min.y)
        {
            pendingBoxes.Push(new Box(
                on: newBox.on,
                min: newBox.min,
                max: new Point3D(
                    x: newBox.max.x,
                    y: collidingBox.min.y - 1,
                    z: newBox.max.z)));

            newBox = new Box(
                on: newBox.on,
                min: (newBox.min.x, collidingBox.min.y, newBox.min.z),
                max: newBox.max);
        }

        if (newBox.max.y > collidingBox.max.y)
        {
            pendingBoxes.Push(new Box(
                on: newBox.on,
                min: new Point3D(
                    x: newBox.min.x,
                    y: collidingBox.max.y + 1,
                    z: newBox.min.z),
                max: newBox.max));

            newBox = new Box(
                on: newBox.on,
                min: newBox.min,
                max: (newBox.max.x, collidingBox.max.y, newBox.max.z));
        }

        if (newBox.min.z < collidingBox.min.z)
        {
            pendingBoxes.Push(new Box(
                on: newBox.on,
                min: newBox.min,
                max: new Point3D(
                    x: newBox.max.x,
                    y: newBox.max.y,
                    z: collidingBox.min.z - 1)));
        }

        if (newBox.max.z > collidingBox.max.z)
        {
            pendingBoxes.Push(new Box(
                on: newBox.on,
                min: new Point3D(
                    x: newBox.min.x,
                    y: newBox.min.y,
                    z: collidingBox.max.z + 1),
                max: newBox.max));
        }
    }
}

BigInteger total = placedBoxes.Aggregate(0, (BigInteger value, Box box) =>
    value + box.GetLitCount());

Console.WriteLine($"The answer is: {total}");

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

    public BigInteger GetLitCount()
    {
        if (!on)
        {
            return 0;
        }

        Point3D size = ((1, 1, 1) + max - min);

        return (BigInteger)size.x * (BigInteger)size.y * (BigInteger)size.z;
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

    public override string ToString() =>
        $"{(on ? "on " : "off")} x={min.x}..{max.x},y={min.y}..{max.y},z={min.z}..{max.z} : {GetLitCount()}";
}
