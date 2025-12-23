namespace HexEngine;

public readonly struct HexCoordI
{
    public int Q { get; }
    public int R { get; }
    public int S => -Q - R;

    public HexCoordI(int q, int r)
    {
        Q = q;
        R = r;
    }

    public OffsetCoord ToOffsetOddRow()
    {
        // using (bitwise and) as % doesn't work with negative numbers
        var parity = Q & 1;
        var col = Q;
        var row = R + (Q - parity) / 2;
        return new OffsetCoord(row, col);
    }

    public static HexCoordI FromOffsetOddRow(OffsetCoord offsetCoord)
    {
        var parity = offsetCoord.Col & 1;
        var q = offsetCoord.Col;
        var r = offsetCoord.Row - (offsetCoord.Col - parity) / 2;
        return new HexCoordI(q, r);
    }
    
}