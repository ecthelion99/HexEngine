using System.Collections.Generic;

namespace HexEngine;

public interface IHexGrid<T>
{

    public void SetPayload(HexCoord coord, T payload);

    public Hex<T> GetHex(HexCoord coord);

    public IEnumerable<HexCoord> Coords();
    
    public IEnumerable<Hex<T>> Hexes();

}