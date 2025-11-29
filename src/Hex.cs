namespace HexEngine;

public struct Hex<T>
{
    public HexCoord Coord { get; set; }
    public float Size { get; set; }
    public T Payload { get; set; }
}