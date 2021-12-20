
const string inputFile = @"../../../../input16.txt";


Console.WriteLine("Day 16 - Packet Decoder");
Console.WriteLine("Star 1");
Console.WriteLine();

int index = 0;

string inputText = string.Join("", File.ReadAllText(inputFile).Select(ToBinaryString));
//string inputText = string.Join("", "620080001611562C8802118E34".Select(ToBinaryString));

DataPacket packet = DataPacket.Parse(
    inputString: inputText,
    ref index);

int versionSum = packet.GetAllPackets().Select(x => x.Version).Sum();

Console.WriteLine($"The answer is: {versionSum}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

Console.WriteLine($"The answer is: {packet.GetValue()}");



Console.WriteLine();
Console.ReadKey();

string ToBinaryString(char hexInput) => hexInput switch
{
    '0' => "0000",
    '1' => "0001",
    '2' => "0010",
    '3' => "0011",
    '4' => "0100",
    '5' => "0101",
    '6' => "0110",
    '7' => "0111",
    '8' => "1000",
    '9' => "1001",
    'A' => "1010",
    'B' => "1011",
    'C' => "1100",
    'D' => "1101",
    'E' => "1110",
    'F' => "1111",
    _ => throw new NotImplementedException()
};

abstract class DataPacket
{
    public int Version { get; init; }
    public int Type { get; init; }

    public virtual IEnumerable<DataPacket> GetAllPackets()
    {
        yield return this;
    }

    public abstract long GetValue();

    public static DataPacket Parse(string inputString, ref int index)
    {
        //Parse the Version - 3 bits
        int version = ParseBits(inputString, 3, ref index);
        int dataType = ParseBits(inputString, 3, ref index);

        if (dataType == 4)
        {
            LiteralValue packet = new LiteralValue()
            {
                Version = version,
                Type = dataType,
                Value = ParseVariableLength(inputString, ref index)
            };

            return packet;
        }
        else
        {
            //Operator
            int lengthType = ParseBits(inputString, 1, ref index);

            Operator packet = new Operator()
            {
                Version = version,
                Type = dataType,
                LengthType = lengthType
            };

            if (lengthType == 0)
            {
                //Next 15 bits are the total length
                int endIndex = ParseBits(inputString, 15, ref index);
                endIndex += index;

                while (index < endIndex)
                {
                    packet.InternalDataPackets.Add(Parse(inputString, ref index));
                }
            }
            else
            {
                //Next 11 bits are subPacketCount
                int totalSubpackets = ParseBits(inputString, 11, ref index);

                for (int i = 0; i < totalSubpackets; i++)
                {
                    packet.InternalDataPackets.Add(Parse(inputString, ref index));
                }
            }

            return packet;
        }
    }

    private static int ParseBits(string input, int bitCount, ref int index)
    {
        int cumulativeValue = 0;

        for (int i = 0; i < bitCount; i++)
        {
            cumulativeValue <<= 1;

            if (input[index++] == '1')
            {
                cumulativeValue |= 1;
            }
        }

        return cumulativeValue;
    }

    private static long ParseVariableLength(string input, ref int index)
    {
        long cumulativeValue = 0;
        bool keepGoing;
        do
        {
            keepGoing = input[index++] == '1';

            for (int i = 0; i < 4; i++)
            {
                cumulativeValue <<= 1;

                if (input[index++] == '1')
                {
                    cumulativeValue |= 1;
                }
            }
        }
        while (keepGoing);

        return cumulativeValue;
    }
}

class LiteralValue : DataPacket
{
    public long Value { get; init; }
    public override long GetValue() => Value;

}

class Operator : DataPacket
{
    public List<DataPacket> InternalDataPackets { get; } = new List<DataPacket>();
    public int LengthType { get; init; }

    public override IEnumerable<DataPacket> GetAllPackets()
    {
        yield return this;

        foreach (DataPacket internalPacket in InternalDataPackets)
        {
            foreach (DataPacket packet in internalPacket.GetAllPackets())
            {
                yield return packet;
            }
        }
    }

    public override long GetValue()
    {
        switch (Type)
        {
            case 0:
                //Sum
                return InternalDataPackets.Sum(x => x.GetValue());

            case 1:
                //Product
                return InternalDataPackets
                    .Select(x => x.GetValue())
                    .Aggregate(1L, (x,y) => x*y, x => x);

            case 2:
                //Min
                return InternalDataPackets.Min(x => x.GetValue());

            case 3:
                //Max
                return InternalDataPackets.Max(x => x.GetValue());

            case 5:
                //GreaterThan
                return InternalDataPackets[0].GetValue() > InternalDataPackets[1].GetValue() ?
                       1 : 0;

            case 6:
                //LessThan
                return InternalDataPackets[0].GetValue() < InternalDataPackets[1].GetValue() ?
                       1 : 0;

            case 7:
                //EqualTo
                return InternalDataPackets[0].GetValue() == InternalDataPackets[1].GetValue() ?
                       1 : 0;

            default: throw new NotImplementedException();
        }
    }

}
