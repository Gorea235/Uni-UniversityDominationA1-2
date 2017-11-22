using System;
using System.Collections;
using Map.Hex;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class TestCoordDouble
{
    bool EnsureCubeCoord(CoordDouble coord)
    {
        return Math.Abs(coord.Q + coord.R + coord.S) < double.Epsilon;
    }

    [Test]
    public void TestInit()
    {
        Assert.That(EnsureCubeCoord(new CoordDouble(0.5, 1.2)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new CoordDouble(10.3, -4.7)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new CoordDouble(-6, 4.1)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new CoordDouble(-3.14, -8)),
                    Is.True);

        Assert.That(EnsureCubeCoord(new CoordDouble(5.5, 3, -8.5)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new CoordDouble(1.2, -3.8, 2.6)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new CoordDouble(-10.5, 3.25, 7.25)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new CoordDouble(-31.9, 46.4, -14.5)),
                    Is.True);
    }

    [Test]
    public void TestProperties()
    {
        CoordDouble c = new CoordDouble(3.5, 2.1);
        Assert.That(c.Q, Is.EqualTo(3.5));
        Assert.That(c.R, Is.EqualTo(2.1));
        c = new CoordDouble(12.74, -9.3);
        Assert.That(c.Q, Is.EqualTo(12.74));
        Assert.That(c.R, Is.EqualTo(-9.3));

        c = new CoordDouble(5.2, 6.8, -12);
        Assert.That(c.Q, Is.EqualTo(5.2));
        Assert.That(c.R, Is.EqualTo(6.8));
        Assert.That(c.S, Is.EqualTo(-12));
        c = new CoordDouble(-4.4, -8.8, 13.2);
        Assert.That(c.Q, Is.EqualTo(-4.4));
        Assert.That(c.R, Is.EqualTo(-8.8));
        Assert.That(c.S, Is.EqualTo(13.2));
    }

    [Test]
    public void TestRound()
    {
        //FractionalHex a = new FractionalHex(0, 0, 0);
        //FractionalHex b = new FractionalHex(1, -1, 0);
        //FractionalHex c = new FractionalHex(0, -1, 1);
        //Tests.EqualHex("hex_round 4", FractionalHex.HexRound(a), FractionalHex.HexRound(new FractionalHex(a.q * 0.4 + b.q * 0.3 + c.q * 0.3, a.r * 0.4 + b.r * 0.3 + c.r * 0.3, a.s * 0.4 + b.s * 0.3 + c.s * 0.3)));
        //Tests.EqualHex("hex_round 5", FractionalHex.HexRound(c), FractionalHex.HexRound(new FractionalHex(a.q * 0.3 + b.q * 0.3 + c.q * 0.4, a.r * 0.3 + b.r * 0.3 + c.r * 0.4, a.s * 0.3 + b.s * 0.3 + c.s * 0.4)));
        CoordDouble a = new CoordDouble(0, 0, 0);
        CoordDouble b = new CoordDouble(1, -1, 0);
        CoordDouble c = new CoordDouble(0, -1, 1);
        Assert.That(new CoordDouble(a.Q * 0.4 + b.Q * 0.3 + c.Q * 0.3,
                                    a.R * 0.4 + b.R * 0.3 + c.R * 0.3,
                                    a.S * 0.4 + b.S * 0.3 + c.S * 0.3),
                    Is.EqualTo(a.Round()));
        Assert.That(new CoordDouble(a.Q * 0.3 + b.Q * 0.3 + c.Q * 0.4,
                                    a.R * 0.3 + b.R * 0.3 + c.R * 0.4,
                                    a.S * 0.3 + b.S * 0.3 + c.S * 0.4),
                    Is.EqualTo(c.Round()));
    }

    [Test]
    public void TestLerp()
    {
        //Tests.EqualHex("hex_round 1", new Hex(5, -10, 5), FractionalHex.HexRound(FractionalHex.HexLerp(new FractionalHex(0, 0, 0), new FractionalHex(10, -20, 10), 0.5)));
        //Tests.EqualHex("hex_round 2", FractionalHex.HexRound(a), FractionalHex.HexRound(FractionalHex.HexLerp(a, b, 0.499)));
        //Tests.EqualHex("hex_round 3", FractionalHex.HexRound(b), FractionalHex.HexRound(FractionalHex.HexLerp(a, b, 0.501)));
        CoordDouble a = new CoordDouble(0, 0, 0);
        CoordDouble b = new CoordDouble(1, -1, 0);
        Assert.That(new CoordDouble(0, 0, 0).Lerp(new CoordDouble(10, -20, 10), 0.5).Round(),
                    Is.EqualTo(new CoordDouble(5, -10, 5)));
        Assert.That(a.Lerp(b, 0.499).Round(), Is.EqualTo(a.Round()));
        Assert.That(a.Lerp(b, 0.499).Round(), Is.EqualTo(b.Round()));
    }
}
