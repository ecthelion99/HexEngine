namespace HexEngine;

public readonly struct HexCoord(int q, int r)
{
    public int Q { get; init; } = q;
    public int R { get; init; } = r;
    public int S => -Q - R;

    public OffsetCoord ToOffsetOddRow()
    {
        // using (bitwise and) as % doesn't work with negative numbers
        var parity = Q & 1;
        var col = Q;
        var row = R + (Q - parity) / 2;
        return new OffsetCoord(row, col);
    }

    public static HexCoord FromOffsetOddRow(OffsetCoord offsetCoord)
    {
        var parity = offsetCoord.Col & 1;
        var q = offsetCoord.Col;
        var r = offsetCoord.Row - (offsetCoord.Col - parity) / 2;
        return new HexCoord(q, r);
    }
    
}