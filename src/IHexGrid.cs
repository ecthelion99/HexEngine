using System.Collections.Generic;

namespace HexEngine;

public interface IHexGrid<T>
{

    public void SetPayload(HexCoordI coordI, T payload);

    public Hex<T> GetHex(HexCoordI coordI);

    public IEnumerable<HexCoordI> Coords();
    
    public IEnumerable<Hex<T>> Hexes();

}