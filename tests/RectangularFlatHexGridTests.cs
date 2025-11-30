using System.Linq;
using System.Numerics;
using FsCheck.Fluent;

namespace HexEngine.Tests;

public class RectangularFlatHexGridTests
{
    public static class WidthHeightArb
    {
        public static Arbitrary<int> SmallSize() =>
            Gen.Choose(1, 20).ToArbitrary();
    }
    
    [Fact]
    public void Constructor_InitializesProperties()
    {
        var grid = new RectangularFlatHexGrid<int>(10, 8, 1.5f);

        Assert.Equal(10, grid.Width);
        Assert.Equal(8, grid.Height);
        Assert.Equal(1.5f, grid.Size);
    }

    [Fact]
    public void Constructor_InitializesAllHexes()
    {
        var grid = new RectangularFlatHexGrid<string>(4, 3, 1.0f);

        var hexes = grid.Hexes().ToList();
        Assert.Equal(12, hexes.Count);

        foreach (var hex in hexes)
        {
            Assert.Equal(1.0f, hex.Size);
        }
    }

    [Fact]
    public void SetPayload_AndGetPayload_WorkCorrectly()
    {
        var grid = new RectangularFlatHexGrid<string>(5, 5, 1.0f);
        var coord = new HexCoord(0, 0);

        grid.SetPayload(coord, "test");

        Assert.Equal("test", grid.GetPayload(coord));
    }

    [Fact]
    public void GetHex_ReturnsHexWithCorrectCoord()
    {
        var grid = new RectangularFlatHexGrid<int>(5, 5, 1.0f);
        var coord = new HexCoord(1, -1);

        var hex = grid.GetHex(coord);

        Assert.Equal(coord, hex.Coord);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 8)]
    [InlineData(10, 10)]
    [InlineData(7, 7)]
    [InlineData(3, 21)]
    public void IsInBounds_True_ForValidCoords(int w, int h)
    {
        var grid = new RectangularFlatHexGrid<int>(w, h, 1);
        for (int x = -w / 2; x < -w / 2 + w; x++)
        {
            for (int y = -h / 2; y > -h / 2 + h; y++)
            {
                var offsetCoord = new OffsetCoord(y, x);
                var coord = HexCoord.FromOffsetOddRow(offsetCoord);
                Assert.True(grid.IsInBounds(offsetCoord));
                Assert.True(grid.IsInBounds(coord));
            }
        }
    }
    
    [Theory]
    [InlineData(100, 100)]
    [InlineData(-100, -100)]
    [InlineData(0, 10)]
    [InlineData(10, 0)]
    [InlineData(0, -6)]
    [InlineData(0, 5)]
    [InlineData(-6, 0)]
    [InlineData(6, 0)]
    public void IsInBounds_False_ForInvalidCoords(int x, int y)
    {
        var grid = new RectangularFlatHexGrid<int>(11, 10, 1);
        var offsetCoord = new OffsetCoord(y, x);
        var coord = HexCoord.FromOffsetOddRow(offsetCoord);
        Assert.False(grid.IsInBounds(offsetCoord));
        Assert.False(grid.IsInBounds(coord));
    }
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 8)]
    [InlineData(10, 10)]
    [InlineData(7, 7)]
    [InlineData(3, 21)]
    public void AllHexCoordsAreInBounds(int w, int h)
    {
        var grid = new RectangularFlatHexGrid<int>(w, h, 1f);

        foreach (var coord in grid.Coords())
        {
            Assert.True(grid.IsInBounds(coord));
        }
    }
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 8)]
    [InlineData(10, 10)]
    [InlineData(7, 7)]
    [InlineData(3, 21)]
    public void AllOffsetCoordsAreInBounds(int w, int h)
    {
        var grid = new RectangularFlatHexGrid<int>(w, h, 1f);

        foreach (var coord in grid.Coords())
        {
            OffsetCoord offsetCoord = coord.ToOffsetOddRow();
            Assert.True(grid.IsInBounds(offsetCoord));
        }
    }

    [Fact]
    public void Indexer_ThrowsForOutOfBoundsCoord()
    {
        var grid = new RectangularFlatHexGrid<int>(3, 3, 1.0f);
        var coord = new HexCoord(100, 100);

        Assert.Throws<ArgumentOutOfRangeException>(() => grid.GetHex(coord));
    }

    [Fact]
    public void Coords_ReturnsAllCoordinates()
    {
        var grid = new RectangularFlatHexGrid<int>(4, 3, 1.0f);

        var coords = grid.Coords().ToList();

        Assert.Equal(12, coords.Count);
    }

    [Fact]
    public void WorldCoordinate_CalculatesCorrectPosition()
    {
        var grid = new RectangularFlatHexGrid<int>(5, 5, 2.0f);
        var coord = new HexCoord(0, 0);

        var worldCoord = grid.WorldCoordinate(coord);

        Assert.Equal(0f, worldCoord.X);
        Assert.Equal(0f, worldCoord.Y);
    }

    [Fact]
    public void WorldCoordinate_ScalesBySize()
    {
        var grid = new RectangularFlatHexGrid<int>(5, 5, 2.0f);
        var coord = new HexCoord(2, 0);

        var worldCoord = grid.WorldCoordinate(coord);

        Assert.Equal(6f, worldCoord.X);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    public void SetAndGetPayload_WorksForMultipleCoords(int q, int r)
    {
        var grid = new RectangularFlatHexGrid<int>(5, 5, 1.0f);
        var coord = new HexCoord(q, r);
        var value = q * 10 + r;

        grid.SetPayload(coord, value);

        Assert.Equal(value, grid.GetPayload(coord));
    }

    [Fact]
    public void MultiplePayloads_MaintainIndependence()
    {
        var grid = new RectangularFlatHexGrid<string>(5, 5, 1.0f);
        var coord1 = new HexCoord(0, 0);
        var coord2 = new HexCoord(1, 0);

        grid.SetPayload(coord1, "first");
        grid.SetPayload(coord2, "second");

        Assert.Equal("first", grid.GetPayload(coord1));
        Assert.Equal("second", grid.GetPayload(coord2));
    }

}