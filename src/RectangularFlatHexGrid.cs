using System;
using System.Collections.Generic;
using System.Numerics;

namespace HexEngine;

public class RectangularFlatHexGrid<T> : IHexGrid<T>
{
    private Hex<T>[,] _grid;

    public int Width { get;}
    public int Height { get;}
    public float Size { get; init; }
    
    private int MinX => -Width / 2;
    private int MaxX => MinX + (Width - 1);

    private int MinY => -Height / 2;
    private int MaxY => MinY + (Height - 1);
    
    public RectangularFlatHexGrid(int width, int height, float size)
    {
        Width = width;
        Height = height;
        Size = size;
        _grid = new Hex<T>[Height, Width];
        foreach (var coord in this.Coords())
        {
            ref var hex = ref this[coord];
            hex.Coord = coord;
            hex.Size = Size;

        }
    }
    private ref Hex<T> this[HexCoord coord]
    {
        get
        {
            var offsetCoord = coord.ToOffsetOddRow();
            if (!IsInBounds(offsetCoord))
            {
                throw new ArgumentOutOfRangeException(nameof(coord), 
                    $"Hex coordinate Q: {coord.Q}, R: {coord.R} is out of range." +
                    $"               Row: {offsetCoord.Row}, Col: {offsetCoord.Col}");
            }
            return ref _grid[offsetCoord.Row - MinY, offsetCoord.Col - MinX];
        }
    }

    public bool IsInBounds(OffsetCoord offsetCoord)
    {
        return offsetCoord.Col >= MinX && offsetCoord.Col <= MaxX && offsetCoord.Row >= MinY && offsetCoord.Row <= MaxY;
    }

    public bool IsInBounds(HexCoord coord)
    {
        var offsetCoord = coord.ToOffsetOddRow();
        return IsInBounds(offsetCoord);
    }

    public void SetPayload(HexCoord coord, T payload)
    {
        ref var hexRef = ref this[coord];
        hexRef.Payload = payload;
    }

    public T GetPayload(HexCoord coord)
    {
        return this[coord].Payload;
    }
    
    public Hex<T> GetHex(HexCoord coord)
    {
        return this[coord];
    }
    public IEnumerable<HexCoord> Coords()
    {
        for (var i = MinY; i <= MaxY; i++)
        {
            for (var j = MinX; j <= MaxX; j++)
            {
                var offsetCoord = new OffsetCoord(i, j);
                yield return HexCoord.FromOffsetOddRow(offsetCoord);
            }
        }
    }

    public IEnumerable<Hex<T>> Hexes()
    {
        foreach (var coord in Coords())
        {
            yield return this[coord];
        }
    }

    public Vector2 WorldCoordinate(HexCoord coord)
    {
        var x = coord.Q * 3f / 2;
        var y = (coord.Q / 2f + coord.R) * (float)Math.Sqrt(2);
        return new Vector2(Size * x, Size * y);
    }
}

