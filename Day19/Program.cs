using AoCTools;

const string inputFile = @"../../../../input19.txt";
//const string inputFile = @"../../../../input19Test.txt";

const int MATCH_COUNT = 12;

Console.WriteLine("Day 19 - Beacon Scanner");
Console.WriteLine("Star 1");
Console.WriteLine();


string[] inputLines = File.ReadAllLines(inputFile);

List<Scanner> scanners = new List<Scanner>();

foreach (string line in inputLines)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        continue;
    }

    if (line.StartsWith("---"))
    {
        scanners.Add(new Scanner());
    }
    else
    {
        string[] values = line.Split(',');

        scanners[^1].Beacons.Add(
            new Point3D(
                x: int.Parse(values[0]),
                y: int.Parse(values[1]),
                z: int.Parse(values[2])));
    }
}

foreach (Scanner scanner in scanners)
{
    scanner.PopulateDistances();
}

HashSet<Point3D> beacons = new HashSet<Point3D>(scanners[0].Beacons);

HashSet<Scanner> scannerSet = new HashSet<Scanner>(scanners);
HashSet<Scanner> placedScanners = new HashSet<Scanner>();

placedScanners.Add(scanners[0]);
scannerSet.Remove(scanners[0]);
scanners[0].PlacedBeacons.AddRange(scanners[0].Beacons);

//Subtracting a few from the required matches to account for spurious overlap
int distanceIntersectCount = ((MATCH_COUNT * (MATCH_COUNT - 1)) / 2) - 3;

//Important
//When a scanner gets placed - make sure to UPDATE its recorded beacons
//to match the final proper orientation

while (scannerSet.Count > 0)
{
    int initialCount = scannerSet.Count;

    foreach (Scanner scanner in scannerSet.ToArray())
    {
        foreach (Scanner targetScanner in placedScanners.ToArray())
        {
            if (targetScanner.Distances.Intersect(scanner.Distances).Count() > distanceIntersectCount)
            {
                if (HandleMatch(scanner, targetScanner))
                {
                    break;
                }
            }
        }
    }

    if (initialCount == scannerSet.Count)
    {
        throw new Exception("No progress was made on a pass");
    }
}

Console.WriteLine($"The answer is: {beacons.Count}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

int largestDistance = 0;


for (int i = 0; i < scanners.Count - 1; i++)
{
    for (int j = i + 1; j < scanners.Count; j++)
    {
        largestDistance = Math.Max(largestDistance,
            (scanners[i].Position - scanners[j].Position).TaxiCabLength);
    }
}

Console.WriteLine($"The answer is: {largestDistance}");



Console.WriteLine();
Console.ReadKey();


bool HandleMatch(Scanner scanner, Scanner targetScanner)
{
    for (int targetBeacon = 0; targetBeacon < targetScanner.PlacedBeacons.Count - (MATCH_COUNT - 1); targetBeacon++)
    {
        for (int newBeacon = 0; newBeacon < scanner.Beacons.Count; newBeacon++)
        {
            for (Facing facing = 0; facing < Facing.MAX; facing++)
            {
                for (int rotation = 0; rotation < 4; rotation++)
                {
                    Point3D displacement = targetScanner.PlacedBeacons[targetBeacon] - scanner.GetRotatedBeacon(newBeacon, facing, rotation);
                    IEnumerable<Point3D> adjustedPoints = scanner.GetRotatedBeacons(facing, rotation).Select(x => x + displacement);

                    if (targetScanner.PlacedBeacons.Intersect(adjustedPoints).Count() >= MATCH_COUNT)
                    {
                        //Found a match
                        scanner.PlacedBeacons.AddRange(scanner.GetRotatedBeacons(facing, rotation).Select(x => x + displacement));
                        scannerSet.Remove(scanner);
                        placedScanners.Add(scanner);
                        scanner.Position = displacement;

                        foreach (Point3D updatedPosition in scanner.PlacedBeacons)
                        {
                            beacons.Add(updatedPosition);
                        }

                        return true;
                    }
                }
            }
        }
    }

    return false;
}


class Scanner
{
    public Point3D Position { get; set; } = Point3D.Zero;

    public List<Point3D> PlacedBeacons { get; } = new List<Point3D>();
    public List<Point3D> Beacons { get; } = new List<Point3D>();
    public HashSet<int> Distances { get; } = new HashSet<int>();

    private static MatrixPoint3D GetRotationMatrix(Facing facing, int rotation)
    {
        MatrixPoint3D rotationMatrix = MatrixPoint3D.Identity;

        for (int i = 0; i < rotation; i++)
        {
            rotationMatrix *= MatrixPoint3D.RotateX;
        }

        switch (facing)
        {
            case Facing.posX:
                return rotationMatrix;

            case Facing.posY:
                return rotationMatrix * MatrixPoint3D.RotateZ;

            case Facing.posZ:
                return rotationMatrix * MatrixPoint3D.RotateY * MatrixPoint3D.RotateY * MatrixPoint3D.RotateY;

            case Facing.negX:
                return rotationMatrix * MatrixPoint3D.RotateZ * MatrixPoint3D.RotateZ;

            case Facing.negY:
                return rotationMatrix * MatrixPoint3D.RotateZ * MatrixPoint3D.RotateZ * MatrixPoint3D.RotateZ;

            case Facing.negZ:
                return rotationMatrix * MatrixPoint3D.RotateY;

            default: throw new Exception();
        }

    }

    //For 6 facings and 4 orientations
    public IEnumerable<Point3D> GetRotatedBeacons(Facing facing, int rotation)
    {
        MatrixPoint3D rotationMatrix = GetRotationMatrix(facing, rotation);

        foreach (Point3D beacon in Beacons)
        {
            yield return rotationMatrix * beacon;
        }
    }

    public Point3D GetRotatedBeacon(int index, Facing facing, int rotation) =>
        GetRotationMatrix(facing, rotation) * Beacons[index];

    public void PopulateDistances()
    {
        for (int i = 0; i < Beacons.Count - 1; i++)
        {
            for (int j = i + 1; j < Beacons.Count; j++)
            {
                Distances.Add((Beacons[j] - Beacons[i]).TaxiCabLength);
            }
        }
    }
}

enum Facing
{
    posX,
    posY,
    posZ,
    negX,
    negY,
    negZ,
    MAX
}
