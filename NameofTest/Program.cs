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

        var a = new TestStruct();
        a.GetName();
    }
}

public class TestStruct
{
    public TestStructure Data;

    public void GetName()
    {
        Data = new TestStructure(new Vector3(1,2,3), new Vector2(1, 30));
        Console.WriteLine($"{nameof(Data.PupilSize)}, {nameof(Data.GazePosition)}");
    }
    
    public struct TestStructure(Vector3 pupilSize, Vector2 gazePosition)
    {
        public Vector3 PupilSize = pupilSize;
        public Vector2 GazePosition = gazePosition;
    }
}