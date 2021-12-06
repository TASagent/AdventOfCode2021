
const string inputFile = @"../../../../input03.txt";


Console.WriteLine("Day 03 - Binary Diagnostic");
Console.WriteLine("Star 1");
Console.WriteLine();

int output1 = 0;

string[] lines = File.ReadAllLines(inputFile);

List<int> report = lines
    .Select(x => Convert.ToInt32(x, 2))
    .ToList();

int count = lines.Length;
int digitCount = lines[0].Length;

int gammaRate = 0;
int epsilonRate = 0;

for (int place = digitCount - 1; place >= 0; place--)
{
    int mask = 1 << place;
    int setCount = 0;

    foreach (int value in report)
    {
        if ((value & mask) > 0)
        {
            setCount++;
        }
    }

    if (setCount > count / 2)
    {
        gammaRate |= mask;
    }
    else
    {
        epsilonRate |= mask;
    }
}

Console.WriteLine($"The answer is: {gammaRate * epsilonRate}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

HashSet<int> oxygenSet = new HashSet<int>(report);
for (int place = digitCount - 1; place >= 0; place--)
{
    int mask = 1 << place;
    int setCount = 0;

    foreach (int value in oxygenSet)
    {
        if ((value & mask) > 0)
        {
            setCount++;
        }
    }

    bool requireSet = (setCount >= oxygenSet.Count / 2);

    foreach (int value in oxygenSet.ToList())
    {
        if (oxygenSet.Count == 1)
        {
            break;
        }

        bool isSet = (value & mask) > 0;

        if (requireSet != isSet)
        {
            oxygenSet.Remove(value);
        }
    }

    if (oxygenSet.Count == 1)
    {
        break;
    }
}

HashSet<int> scrubberSet = new HashSet<int>(report);
for (int place = digitCount - 1; place >= 0; place--)
{
    int mask = 1 << place;
    int setCount = 0;

    foreach (int value in scrubberSet)
    {
        if ((value & mask) > 0)
        {
            setCount++;
        }
    }

    bool requireSet = (setCount < scrubberSet.Count / 2);

    foreach (int value in scrubberSet.ToList())
    {
        if (scrubberSet.Count == 1)
        {
            break;
        }

        bool isSet = (value & mask) > 0;

        if (requireSet != isSet)
        {
            scrubberSet.Remove(value);
        }
    }

    if (scrubberSet.Count == 1)
    {
        break;
    }
}


Console.WriteLine($"The answer is: {oxygenSet.First() * scrubberSet.First()}");



Console.WriteLine();
Console.ReadKey();
