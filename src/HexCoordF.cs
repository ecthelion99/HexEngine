namespace HexEngine;

public readonly struct HexCoordF
{
    public float Q { get; }
    public float R { get; }
    public float S => -Q - R;
    
    public HexCoordF(float q, float r)
    {
        Q = q;
        R = r;
    }
    
    public HexCoordI ToHexCoordI()
    {
        var qInt = (int)Math.Round(Q);
        var rInt = (int)Math.Round(R);
        var sInt = (int)Math.Round(S);

        var deltaQ = Q - qInt;
        var deltaR = R - rInt;
        var deltaS = S - sInt;
        
        if (deltaQ > deltaR && deltaQ > deltaS)
        {
            qInt = -sInt - rInt;
        } 
        else if (deltaR > deltaS)
        {
            rInt = -qInt - sInt;
        }

        return new HexCoordI(qInt, rInt);
    }
}