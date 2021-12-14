const string inputFile = @"../../../../input10.txt";

Console.WriteLine("Day 10 - Syntax Scoring");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);

Dictionary<char, long> errorValue = new Dictionary<char, long>()
{
    { ')', 3 },
    { ']', 57 },
    { '}', 1197 },
    { '>', 25137 }
};

Dictionary<char, char> complementMap = new Dictionary<char, char>()
{
    { '(', ')' },
    { '[', ']' },
    { '{', '}' },
    { '<', '>' }
};

Dictionary<char, long> completionValue = new Dictionary<char, long>()
{
    { ')', 1 },
    { ']', 2 },
    { '}', 3 },
    { '>', 4 }
};

long totalErrorValue = lines.Select(CalculateErrorValue).Sum();

Console.WriteLine($"The answer is: {totalErrorValue}");


Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();
int output2 = 0;

List<string> nonCorruptLines = lines.Where(x => CalculateErrorValue(x) == 0).ToList();

List<long> completionValues = nonCorruptLines
    .Select(CalculateCompletionValue)
    .OrderByDescending(x => x)
    .ToList();

int valueCount = completionValues.Count();

Console.WriteLine($"The answer is: {completionValues.Skip(valueCount / 2).First()}");

Console.WriteLine();
Console.ReadKey();


long CalculateErrorValue(string line)
{
    Stack<char> bracketStack = new Stack<char>();

    foreach (char c in line)
    {
        switch (c)
        {
            case '(':
            case '[':
            case '{':
            case '<':
                bracketStack.Push(complementMap[c]);
                break;

            case ')':
            case ']':
            case '}':
            case '>':
                if (bracketStack.Pop() != c)
                {
                    return errorValue[c];
                }
                break;


            default: throw new Exception();
        }
    }

    return 0;
}

long CalculateCompletionValue(string line)
{
    Stack<char> bracketStack = new Stack<char>();

    foreach (char c in line)
    {
        switch (c)
        {
            case '(':
            case '[':
            case '{':
            case '<':
                bracketStack.Push(complementMap[c]);
                break;

            case ')':
            case ']':
            case '}':
            case '>':
                if (bracketStack.Pop() != c)
                {
                    throw new Exception();
                }
                break;

            default: throw new Exception();
        }
    }

    long completionScore = 0;

    while (bracketStack.Count > 0)
    {
        char c = bracketStack.Pop();
        completionScore *= 5;
        completionScore += completionValue[c];
    }

    return completionScore;
}