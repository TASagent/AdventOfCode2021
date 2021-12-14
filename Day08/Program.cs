const string inputFile = @"../../../../input08.txt";

Console.WriteLine("Day 08 - Seven Segment Search");
Console.WriteLine("Star 1");
Console.WriteLine();

//Console.WriteLine(File.ReadAllText(inputFile));

//1 requires 2 segments
//4 requires 4 segments
//7 requires 3 segments
//8 requires 7 segments

string[] lines = File.ReadAllLines(inputFile);

List<string> output = lines
    .Select(x => x.Split('|')[1])
    .ToList();

int output1 = output.Select(CountEZSegments).Sum();

Console.WriteLine($"The answer is: {output1}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

long cumulativeTotal = 0;


foreach (string line in lines)
{
    var splitLine = line.Split('|');

    Dictionary<string, int> lookup = new Dictionary<string, int>()
    {
        { "abcdefg", 8 }
    };

    HashSet<string> digits =
        new HashSet<string>(
            splitLine[0].Split(' ')
            .Select(x => new string(x.OrderBy(c => c).ToArray())));

    List<string> outputSegment = splitLine[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(x => new string(x.OrderBy(c => c).ToArray()))
        .ToList();

    //Step 1: Isolate the 1, 7, 4, 8

    string one = digits.First(x => x.Length == 2);
    string seven = digits.First(x => x.Length == 3);
    string four = digits.First(x => x.Length == 4);

    digits.Remove(one);
    digits.Remove(seven);
    digits.Remove(four);

    lookup[one] = 1;
    lookup[seven] = 7;
    lookup[four] = 4;

    //Step 2: Isolate 6-segment Digits 6, 9, 0

    List<string> sixSegments = digits.Where(x => x.Length == 6).ToList();

    string six = sixSegments.First(x => !x.Contains(one[0]) || !x.Contains(one[1]));
    sixSegments.Remove(six);
    digits.Remove(six);

    lookup[six] = 6;

    string zero = sixSegments
        .First(x => !four.All(c => x.Contains(c)));
    sixSegments.Remove(zero);
    digits.Remove(zero);
    lookup[zero] = 0;

    string nine = sixSegments[0];
    digits.Remove(nine);
    lookup[nine] = 9;

    List<string> fiveSegments = digits.Where(x => x.Length == 5).ToList();

    string three = fiveSegments.First(x => one.All(c => x.Contains(c)));
    fiveSegments.Remove(three);
    digits.Remove(three);
    lookup[three] = 3;

    string five = fiveSegments.First(x => x.All(c => six.Contains(c)));
    fiveSegments.Remove(five);
    digits.Remove(five);
    lookup[five] = 5;

    string two = fiveSegments[0];
    digits.Remove(two);
    lookup[two] = 2;

    int outputValue = 0;
    foreach(string value in outputSegment)
    {
        outputValue *= 10;
        outputValue += lookup[value];
    }

    cumulativeTotal += outputValue;
}

Console.WriteLine($"The answer is: {cumulativeTotal}");



Console.WriteLine();
Console.ReadKey();


int CountEZSegments(string outputLine) =>
    outputLine
        .Split(' ')
        .Select(x => x.Length)
        .Count(IsItEasy);

bool IsItEasy(int length) => length switch
{
    2 or 3 or 4 or 7 => true,
    _ => false
};