namespace HexEngine.Tests;

using HexEngine;

public class HexCoordITests
{
    private static void AssertCubeInvariant(HexCoordI h)
    {
        Assert.Equal(0, h.Q + h.R + h.S);
    }
    
    [Property]
    public void CubeInvariant_AlwaysHolds(int q, int r)
    {
        HexCoordI coordI = new HexCoordI(q, r);
        AssertCubeInvariant(coordI);
    }

    [Property]
    public void OffsetOddRow_RoundTrip(int q, int r)
    {
        HexCoordI hexCoord1 = new HexCoordI(q, r);
        OffsetCoord offsetCoord = hexCoord1.ToOffsetOddRow();
        HexCoordI hexCoord2 = HexCoordI.FromOffsetOddRow(offsetCoord);
        Assert.Equal(hexCoord1, hexCoord2);
    }
}
