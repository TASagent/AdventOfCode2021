
const string inputFile = @"../../../../input07.txt";


Console.WriteLine("Day 07 - The Treachery of Whales");
Console.WriteLine("Star 1");
Console.WriteLine();

int output1 = 0;


List<long> positions = File.ReadAllText(inputFile)
    .Split(',')
    .Select(long.Parse)
    .ToList();

long sum = positions.Sum();
long count = positions.Count();

long averagePosition = (long)Math.Round(sum / (double)count);

long averageFuelUsage = GetFuelUsage(averagePosition); 
long rightFuelUsage = GetFuelUsage(averagePosition + 1);

long diff = averageFuelUsage < rightFuelUsage ? -1 : 1;
long lastFuelUsage = averageFuelUsage;
long position = averagePosition + diff;

while (true)
{
    long newFuelUsage = GetFuelUsage(position);

    if (newFuelUsage > lastFuelUsage)
    {
        break;
    }

    lastFuelUsage = newFuelUsage;
    position += diff;
}

Console.WriteLine($"Min Fuel Usage is: {lastFuelUsage}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

averageFuelUsage = GetModifiedFuelUsage(averagePosition);
rightFuelUsage = GetModifiedFuelUsage(averagePosition + 1);

diff = averageFuelUsage < rightFuelUsage ? -1 : 1;
lastFuelUsage = averageFuelUsage;
position = averagePosition + diff;

while (true)
{
    long newFuelUsage = GetModifiedFuelUsage(position);

    if (newFuelUsage > lastFuelUsage)
    {
        break;
    }

    lastFuelUsage = newFuelUsage;
    position += diff;
}

Console.WriteLine($"The answer is: {lastFuelUsage}");



Console.WriteLine();
Console.ReadKey();


long GetFuelUsage(long position) =>
    positions
    .Select(x => Math.Abs(x - position))
    .Sum();

long GetModifiedFuelUsage(long position) =>
    positions
    .Select(x => GetModifiedFuelCost(Math.Abs(x - position)))
    .Sum();

long GetModifiedFuelCost(long diff) =>
    (diff * (diff + 1)) / 2;

