namespace HexEngine;

public readonly struct OffsetCoord(int row, int col)
{
    public int Col { get; init; } = col;
    public int Row { get; init; } = row;
}