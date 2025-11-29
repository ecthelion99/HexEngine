namespace HexEngine.Tests;

using HexEngine;

public class HexCoordTests
{
    private static void AssertCubeInvariant(HexCoord h)
    {
        Assert.Equal(0, h.Q + h.R + h.S);
    }
    
    [Property]
    public void CubeInvariant_AlwaysHolds(int q, int r)
    {
        HexCoord coord = new HexCoord(q, r);
        AssertCubeInvariant(coord);
    }

    [Property]
    public void OffsetOddRow_RoundTrip(int q, int r)
    {
        HexCoord hexCoord1 = new HexCoord(q, r);
        OffsetCoord offsetCoord = hexCoord1.ToOffsetOddRow();
        HexCoord hexCoord2 = HexCoord.FromOffsetOddRow(offsetCoord);
        Assert.Equal(hexCoord1, hexCoord2);
    }
}
