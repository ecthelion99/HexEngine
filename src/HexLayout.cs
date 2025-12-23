using System.Numerics;

namespace HexEngine;

public readonly struct HexLayout
{
    private readonly float _m11, _m12;
    private readonly float _m21, _m22;

    private readonly float _inv11, _inv12;
    private readonly float _inv21, _inv22;

    public HexLayout(float m11, float m12, float m21, float m22)
    {
        _m11 = m11;
        _m12 = m12;
        _m21 = m21;
        _m22 = m22;

        var det = 1f / (m11 * m22 - m12 * m21);
        _inv11 = det * m22;
        _inv12 = -det * m12;
        _inv21 = -det * m21;
        _inv22 = det * m11;
    }

    public static HexLayout FlatLayout { get; } = new 
        HexLayout(3f / 2, 0f, (float) Math.Sqrt(3) / 2f, (float) Math.Sqrt(3));
    
    public static HexLayout PointyLayout { get; } = new 
        HexLayout((float) Math.Sqrt(3), (float) Math.Sqrt(3)/2f, 0, 3f/2f);

    public Vector2 ToLocal(HexCoordI coordI, float size)
    {
        return size * (new Vector2(_m11 * coordI.Q + _m12 * coordI.R, _m21 * coordI.Q + _m22*coordI.R));
    }

    public HexCoordF FromLocal(Vector2 coord, float size)
    {
        var q = _inv11 * coord.X + _inv12 * coord.Y;
        var r = _inv21 * coord.X + _inv22 * coord.Y;
        return new HexCoordF(q / size, r / size);
    }
}