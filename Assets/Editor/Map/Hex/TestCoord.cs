using System;
using System.Collections;
using System.Collections.Generic;
using Map.Hex;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class TestCoord
{
    bool EnsureCubeCoord(Coord coord)
    {
        return (coord.Q + coord.R + coord.S) == 0;
    }

    [Test]
    public void TestInit()
    {
        Assert.That(EnsureCubeCoord(new Coord(1, 2)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new Coord(12, -98)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new Coord(-6, 4)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new Coord(-3, -8)),
                    Is.True);

        Assert.That(EnsureCubeCoord(new Coord(5, 3, -8)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new Coord(1, -3, 2)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new Coord(-10, 3, 7)),
                    Is.True);
        Assert.That(EnsureCubeCoord(new Coord(-31, 45, -14)),
                    Is.True);
    }

    [Test]
    public void TestProperties()
    {
        Coord c = new Coord(3, 2);
        Assert.That(c.Q, Is.EqualTo(3));
        Assert.That(c.R, Is.EqualTo(2));
        c = new Coord(12, -9);
        Assert.That(c.Q, Is.EqualTo(12));
        Assert.That(c.R, Is.EqualTo(-9));

        c = new Coord(5, 6, -11);
        Assert.That(c.Q, Is.EqualTo(5));
        Assert.That(c.R, Is.EqualTo(6));
        Assert.That(c.S, Is.EqualTo(-11));
        c = new Coord(-4, -8, 12);
        Assert.That(c.Q, Is.EqualTo(-4));
        Assert.That(c.R, Is.EqualTo(-8));
        Assert.That(c.S, Is.EqualTo(12));
    }

    // these 2 will have to be switched if we invert the direction
    [Test]
    public void TestRotateLeft()
    {
        //Tests.EqualHex("hex_rotate_left", Hex.RotateLeft(new Hex(1, -3, 2)), new Hex(-2, -1, 3));
        Coord c = new Coord(1, -3, 2);
        Assert.That(c.RotateLeft(),
                    Is.EqualTo(new Coord(-2, -1, 3)));
        Assert.That(c.RotateLeft().RotateLeft(),
                    Is.EqualTo(new Coord(-3, 2, 1)));
        c = new Coord(10, -3, -7);
        Assert.That(c.RotateLeft(),
                    Is.EqualTo(new Coord(7, -10, 3)));
        Assert.That(c.RotateLeft().RotateLeft(),
                    Is.EqualTo(new Coord(-3, -7, 10)));
        c = new Coord(23, -14, -9);
        Assert.That(c.RotateLeft(),
                    Is.EqualTo(new Coord(9, -23, 14)));
        Assert.That(c.RotateLeft().RotateLeft(),
                    Is.EqualTo(new Coord(-14, -9, 23)));
        c = new Coord(4, 6, -10);
        Assert.That(c.RotateLeft(),
                    Is.EqualTo(new Coord(10, -4, -6)));
        Assert.That(c.RotateLeft().RotateLeft(),
                    Is.EqualTo(new Coord(6, -10, 4)));
    }

    [Test]
    public void TestRotateRight()
    {
        //Tests.EqualHex("hex_rotate_right", Hex.RotateRight(new Hex(1, -3, 2)), new Hex(3, -2, -1));
        Coord c = new Coord(1, -3, 2);
        Assert.That(c.RotateRight(),
                    Is.EqualTo(new Coord(3, -2, -1)));
        Assert.That(c.RotateRight().RotateRight(),
                    Is.EqualTo(new Coord(2, 1, -3)));
        c = new Coord(10, -3, -7);
        Assert.That(c.RotateRight(),
                    Is.EqualTo(new Coord(3, 7, -10)));
        Assert.That(c.RotateRight().RotateRight(),
                    Is.EqualTo(new Coord(-7, 10, -3)));
        c = new Coord(23, -14, -9);
        Assert.That(c.RotateRight(),
                    Is.EqualTo(new Coord(14, 9, -23)));
        Assert.That(c.RotateRight().RotateRight(),
                    Is.EqualTo(new Coord(-9, 23, -14)));
        c = new Coord(4, 6, -10);
        Assert.That(c.RotateRight(),
                    Is.EqualTo(new Coord(-6, 10, -4)));
        Assert.That(c.RotateRight().RotateRight(),
                    Is.EqualTo(new Coord(-10, 4, 6)));
    }

    [Test]
    public void TestRotateMultiple()
    {
        Coord c = new Coord(2, 7, -9);
        Assert.That(c.RotateLeft().RotateRight(),
                    Is.EqualTo(c));
        Assert.That(c.RotateLeft().RotateLeft().RotateRight().RotateRight(),
                    Is.EqualTo(c));
        Assert.That(c.RotateLeft().RotateRight().RotateLeft().RotateRight(),
                    Is.EqualTo(c));
        Assert.That(c.RotateLeft().RotateRight().RotateRight().RotateLeft(),
                    Is.EqualTo(c));
        Assert.That(c.RotateRight().RotateRight().RotateLeft().RotateLeft(),
                    Is.EqualTo(c));
        Assert.That(c.RotateRight().RotateLeft(),
                    Is.EqualTo(c));
        Assert.That(c.RotateRight().RotateLeft().RotateLeft().RotateRight(),
                    Is.EqualTo(c));
    }

    [Test]
    public void TestNeighbor()
    {
        //Tests.EqualHex("hex_neighbor", new Hex(1, -3, 2), Hex.Neighbor(new Hex(1, -2, 1), 2));
        Assert.Ignore("Not Implemented");
    }

    [Test]
    public void TestDiagonal()
    {
        Assert.Ignore("Not Implemented");
    }

    [Test]
    public void TestLength()
    {
        Assert.Ignore("Not Implemented");
    }

    [Test]
    public void TestDistance()
    {
        //Tests.EqualInt("hex_distance", 7, Hex.Distance(new Hex(3, -7, 4), new Hex(0, 0, 0)));
        Coord c = new Coord(3, -7, 4);
        Assert.That(c.DistanceTo(new Coord(0, 0, 0)),
                    Is.EqualTo(7));
    }

    [Test]
    public void TestLinedraw()
    {
        //Tests.EqualHexArray("hex_linedraw", new List<Hex> { new Hex(0, 0, 0), new Hex(0, -1, 1), new Hex(0, -2, 2), new Hex(1, -3, 2), new Hex(1, -4, 3), new Hex(1, -5, 4) }, FractionalHex.HexLinedraw(new Hex(0, 0, 0), new Hex(1, -5, 4)));
        Coord c = new Coord(0, 0, 0);
        Assert.That(c.Linedraw(new Coord(1, -5, 4)),
                    Is.EqualTo(new List<Coord> {
            new Coord(0, 0, 0), new Coord(0, -1, 1), new Coord(0, -2, 2),
            new Coord(1, -3, 2), new Coord(1, -4, 3), new Coord(1, -5, 4)
        }));
    }

    [Test]
    public void TestAdd()
    {
        //Tests.EqualHex("hex_add", new Hex(4, -10, 6), Hex.Add(new Hex(1, -3, 2), new Hex(3, -7, 4)));
        Assert.That(new Coord(1, -3, 2) + new Coord(3, -7, 4),
                    Is.EqualTo(new Coord(4, -10, 6)));
        Assert.That(new Coord(5, -9, 4) + new Coord(2, 3, -5),
                    Is.EqualTo(new Coord(7, -6, -1)));
        Assert.That(new Coord(6, -8, 2) + new Coord(-8, 4, 4),
                    Is.EqualTo(new Coord(-2, -4, 6)));
    }

    [Test]
    public void TestSubtract()
    {
        //Tests.EqualHex("hex_subtract", new Hex(-2, 4, -2), Hex.Subtract(new Hex(1, -3, 2), new Hex(3, -7, 4)));
        Assert.That(new Coord(1, -3, 2) - new Coord(3, -7, 4),
                    Is.EqualTo(new Coord(-2, 4, -2)));
        Assert.That(new Coord(5, -9, 4) - new Coord(2, 3, -5),
                    Is.EqualTo(new Coord(3, -12, 9)));
        Assert.That(new Coord(6, -8, 2) - new Coord(-8, 4, 4),
                    Is.EqualTo(new Coord(14, -12, -2)));
    }

    [Test]
    public void TestScalarMult()
    {
        Assert.That(1 * new Coord(1, -3, 2),
                    Is.EqualTo(new Coord(1, -3, 2)));
        Assert.That(new Coord(2, 3, -5) * 1,
                    Is.EqualTo(new Coord(2, 3, -5)));
        Assert.That(new Coord(5, -9, 4) * 2,
                    Is.EqualTo(new Coord(10, -18, 8)));
        Assert.That(new Coord(6, -8, 2) * 10,
                    Is.EqualTo(new Coord(60, -80, 20)));
        Assert.That(new Coord(3, -7, 4) * 4,
                    Is.EqualTo(new Coord(12, -28, 16)));
    }

    [Test]
    public void TestEqual()
    {
        Coord c = new Coord(1, 2, -3);
        Assert.That(c == new Coord(1, 2, -3), Is.True);
        Assert.That(new Coord(1, 2, -3) == c, Is.True);
        Assert.That(c.Equals(new Coord(1, 2, -3)), Is.True);
        c = new Coord(6, 3, -9);
        Assert.That(c == new Coord(6, 3, -9), Is.True);
        Assert.That(new Coord(6, 3, -9) == c, Is.True);
        Assert.That(c.Equals(new Coord(6, 3, -9)), Is.True);
    }

    [Test]
    public void TestNotEqual()
    {
        Coord c = new Coord(1, 2, -3);
        Assert.That(c != new Coord(1, 3, -4), Is.True);
        Assert.That(new Coord(1, 3, -4) != c, Is.True);
        Assert.That(!c.Equals(new Coord(1, 3, -4)), Is.True);
        c = new Coord(6, 3, -9);
        Assert.That(c != new Coord(2, 7, -9), Is.True);
        Assert.That(new Coord(2, 7, -9) != c, Is.True);
        Assert.That(!c.Equals(new Coord(2, 7, -9)), Is.True);
    }

    //[Test]
    //public void TestHashCode()
    //{
    //    Coord c = new Coord(1, 5, -6);
    //    Assert.That(c.GetHashCode(), Is.TypeOf<int>());
    //    Assert.That(c.GetHashCode(), Is.GreaterThan(0));
    //    Assert.That(c.GetHashCode(), Is.Not.EqualTo(new Coord(1,-1,0).GetHashCode()))
    //}
}
