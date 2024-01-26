using System.Numerics;

namespace IOTest;

internal static class Program
{
    private static void Main()
    {
        var dataName = new Vector3
        {
            X = 0,
            Y = 1,
            Z = 2
        };
        
        Console.WriteLine($"X: {dataName.X}, Y: {dataName.Y}, Z: {dataName.Z}");

        Console.WriteLine($"{nameof(dataName)}");
        
        Console.WriteLine($"{nameof(dataName.X)}");
    }
}