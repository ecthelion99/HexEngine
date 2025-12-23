namespace HexEngine;

public struct Hex<T>
{
    public HexCoordI CoordI { get; set; }
    public float Size { get; set; }
    public T Payload { get; set; }
}