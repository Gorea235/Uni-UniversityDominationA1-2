using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class TestCoord
{
    [Test]
    public void TestInit()
    {
        
    }

    [Test]
    public void TestRotateLeft()
    {
        //Tests.EqualHex("hex_rotate_left", Hex.RotateLeft(new Hex(1, -3, 2)), new Hex(-2, -1, 3));
    }

    [Test]
    public void TestRotateRight()
    {
        //Tests.EqualHex("hex_rotate_right", Hex.RotateRight(new Hex(1, -3, 2)), new Hex(3, -2, -1));
    }

    [Test]
    public void TestNeighbor()
    {
        //Tests.EqualHex("hex_neighbor", new Hex(1, -3, 2), Hex.Neighbor(new Hex(1, -2, 1), 2));
    }

    [Test]
    public void TestLength()
    {
        
    }

    [Test]
    public void TestDistance()
    {
        //Tests.EqualInt("hex_distance", 7, Hex.Distance(new Hex(3, -7, 4), new Hex(0, 0, 0)));
    }

    [Test]
    public void TestLinedraw()
    {
        //Tests.EqualHexArray("hex_linedraw", new List<Hex> { new Hex(0, 0, 0), new Hex(0, -1, 1), new Hex(0, -2, 2), new Hex(1, -3, 2), new Hex(1, -4, 3), new Hex(1, -5, 4) }, FractionalHex.HexLinedraw(new Hex(0, 0, 0), new Hex(1, -5, 4)));
    }

    [Test]
    public void TestAdd()
    {
        //Tests.EqualHex("hex_subtract", new Hex(-2, 4, -2), Hex.Subtract(new Hex(1, -3, 2), new Hex(3, -7, 4)));
    }

    [Test]
    public void TestSubtract()
    {
        //Tests.EqualHex("hex_subtract", new Hex(-2, 4, -2), Hex.Subtract(new Hex(1, -3, 2), new Hex(3, -7, 4)));
    }

    [Test]
    public void TestScalarMult()
    {
        
    }

    [Test]
    public void TestEqual()
    {

    }

    [Test]
    public void TestNotEqual()
    {
        
    }

    [Test]
    public void TestHashCode()
    {
        
    }
}
