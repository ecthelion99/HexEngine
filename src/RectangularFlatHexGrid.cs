using System;
using System.Collections.Generic;
using System.Numerics;

namespace HexEngine;

public class RectangularFlatHexGrid<T> : IHexGrid<T>
{
    private Hex<T>[,] _grid;
    private HexLayout _layout = HexLayout.FlatLayout;

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
            hex.CoordI = coord;
            hex.Size = Size;

        }
    }
    private ref Hex<T> this[HexCoordI coordI]
    {
        get
        {
            var offsetCoord = coordI.ToOffsetOddRow();
            if (!IsInBounds(offsetCoord))
            {
                throw new ArgumentOutOfRangeException(nameof(coordI), 
                    $"Hex coordinate Q: {coordI.Q}, R: {coordI.R} is out of range." +
                    $"               Row: {offsetCoord.Row}, Col: {offsetCoord.Col}");
            }
            return ref _grid[offsetCoord.Row - MinY, offsetCoord.Col - MinX];
        }
    }

    public bool IsInBounds(OffsetCoord offsetCoord)
    {
        return offsetCoord.Col >= MinX && offsetCoord.Col <= MaxX && offsetCoord.Row >= MinY && offsetCoord.Row <= MaxY;
    }

    public bool IsInBounds(HexCoordI coordI)
    {
        var offsetCoord = coordI.ToOffsetOddRow();
        return IsInBounds(offsetCoord);
    }

    public void SetPayload(HexCoordI coordI, T payload)
    {
        ref var hexRef = ref this[coordI];
        hexRef.Payload = payload;
    }

    public T GetPayload(HexCoordI coordI)
    {
        return this[coordI].Payload;
    }
    
    public Hex<T> GetHex(HexCoordI coordI)
    {
        return this[coordI];
    }
    public IEnumerable<HexCoordI> Coords()
    {
        for (var i = MinY; i <= MaxY; i++)
        {
            for (var j = MinX; j <= MaxX; j++)
            {
                var offsetCoord = new OffsetCoord(i, j);
                yield return HexCoordI.FromOffsetOddRow(offsetCoord);
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

    public Vector2 ToWorld(HexCoordI coordI)
    {
        return _layout.ToLocal(coordI, Size);
    }

    public HexCoordF FromWorld(Vector2 coord)
    {
        return _layout.FromLocal(coord, Size);
    }
}

