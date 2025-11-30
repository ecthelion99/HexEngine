namespace HexEngine;

public readonly struct HexCoord
{
    public int Q { get; init; }
    public int R { get; init; }
    public int S => -Q - R;

    public HexCoord(int q, int r)
    {
        Q = q;
        R = r;
    }

    public HexCoord(float q, float r)
    {
        var s = -q - r;
        var qInt = (int)Math.Round(q);
        var rInt = (int)Math.Round(r);
        var sInt = (int)Math.Round(s);

        var deltaQ = q - qInt;
        var deltaR = r - rInt;
        var deltaS = s - sInt;
        
        if (deltaQ > deltaR && deltaQ > deltaS)
        {
            qInt = -sInt - rInt;
        } 
        else if (deltaR > deltaS)
        {
            rInt = -qInt - sInt;
        }

        Q = qInt;
        R = rInt;
    }

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