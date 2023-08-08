using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Numerics;
using System.Text.Json;

namespace YiJingFramework.PrimitiveTypes.Tests;

[TestClass()]
public class GuaTests
{
    private static Gua GetEmptyGua()
    {
        return new Gua();
    }
    private static Yinyang[] GetLinesOfGua1()
    {
        return new Yinyang[] { Yinyang.Yang, Yinyang.Yang, Yinyang.Yin };
    }
    private static Gua GetGua1()
    {
        var p1 = new Gua(Yinyang.Yang, Yinyang.Yang, Yinyang.Yin);
        var pp1 = new Gua(GetLinesOfGua1());
        Assert.IsTrue(p1.SequenceEqual(pp1));
        return pp1;
    }

    private static IEnumerable<Yinyang> GetLinesOfGua2()
    {
        yield return Yinyang.Yang;
        yield return Yinyang.Yang;
        yield return Yinyang.Yin;
        yield return Yinyang.Yang;
    }
    private static Gua GetGua2()
    {
        return new Gua(GetLinesOfGua2());
    }
    [TestMethod()]
    public void GuaTest()
    {
        _ = GetEmptyGua();
        _ = GetGua1();
        _ = GetGua2();
    }

    [TestMethod()]
    public void GetEnumeratorTest()
    {
        var p0 = GetEmptyGua();
#pragma warning disable CA1826 // Do not use Enumerable methods on indexable collections
#pragma warning disable CA1829 // Use Length/Count property instead of Count() when available
        Assert.AreEqual(0, p0.Count());
#pragma warning restore CA1829 // Use Length/Count property instead of Count() when available
#pragma warning restore CA1826 // Do not use Enumerable methods on indexable collections
        var p1 = GetGua1();
        Assert.IsTrue(p1.SequenceEqual(GetLinesOfGua1()));
        var p2 = GetGua2();
        Assert.IsTrue(p2.SequenceEqual(GetLinesOfGua2()));
    }

    [TestMethod()]
    public void CompareToTest()
    {
        var p0 = GetEmptyGua();
        var p1 = GetGua1();
        var p2 = GetGua2();
        Assert.AreEqual(-1, p0.CompareTo(p1));
        Assert.AreEqual(1, p1.CompareTo(p0));
        Assert.AreEqual(-1, p1.CompareTo(p2));
        Assert.AreEqual(1, p2.CompareTo(p1));
        Assert.AreEqual(1, p2.CompareTo(null));
        Assert.AreEqual(0, p2.CompareTo(p2));

        var p3 = GetGua2();
        Assert.AreEqual(0, p2.CompareTo(p3));

        var p4 = new Gua(Yinyang.Yin, Yinyang.Yin, Yinyang.Yang);
        var p5 = new Gua(Yinyang.Yang, Yinyang.Yang, Yinyang.Yang);
        Assert.AreEqual(-1, p4.CompareTo(p5));
        Assert.AreEqual(1, p5.CompareTo(p4));

        var p6 = new Gua(Yinyang.Yang, Yinyang.Yang, Yinyang.Yin);
        var p7 = new Gua(Yinyang.Yang, Yinyang.Yang, Yinyang.Yang);
        Assert.AreEqual(-1, p6.CompareTo(p7));
        Assert.AreEqual(1, p7.CompareTo(p6));

        p6 = new Gua(Yinyang.Yang, Yinyang.Yin, Yinyang.Yang, Yinyang.Yang);
        p7 = new Gua(Yinyang.Yin, Yinyang.Yang, Yinyang.Yang, Yinyang.Yang);
        Assert.AreEqual(-1, p6.CompareTo(p7));
        Assert.AreEqual(1, p7.CompareTo(p6));
    }

    [TestMethod()]
    public void GetHashCodeTest()
    {
        var p0 = GetEmptyGua();
        Assert.AreEqual(0b1, p0.GetHashCode());
        var p1 = GetGua1();
        Assert.AreEqual(0b1110, p1.GetHashCode());
        var p2 = GetGua2();
        Assert.AreEqual(0b11101, p2.GetHashCode());
    }

    [TestMethod()]
    public void EqualsTest()
    {
        Assert.IsFalse(new Gua().Equals(null));
        Random random = new Random();
        for (int i = 0; i < 20;)
        {
            var c = random.Next(5, 10);
            List<Yinyang> lines1 = new();
            for (int j = 0; j < c; j++)
            {
                lines1.Add((Yinyang)random.Next(0, 2));
            }
            List<Yinyang> lines2 = new();
            for (int j = 0; j < c; j++)
            {
                lines2.Add((Yinyang)random.Next(0, 2));
            }
            if (lines1.SequenceEqual(lines2))
            {
                Assert.IsTrue(new Gua(lines1).Equals(new Gua(lines2)));
                i++;
            }
            else
            {
                Assert.IsFalse(new Gua(lines1).Equals((object)new Gua(lines2)));
            }
        }
    }

    [TestMethod()]
    public void EqualsTest1()
    {
        Assert.IsFalse(new Gua().Equals((object?)null));
        Random random = new Random();
        for (int i = 0; i < 20;)
        {
            var c = random.Next(5, 10);
            List<Yinyang> lines1 = new();
            for (int j = 0; j < c; j++)
            {
                lines1.Add((Yinyang)random.Next(0, 2));
            }
            List<Yinyang> lines2 = new();
            for (int j = 0; j < c; j++)
            {
                lines2.Add((Yinyang)random.Next(0, 2));
            }
            if (lines1.SequenceEqual(lines2))
            {
                Assert.IsTrue(new Gua(lines1).Equals((object)new Gua(lines2)));
                i++;
            }
            else
            {
                Assert.IsFalse(new Gua(lines1).Equals((object)new Gua(lines2)));
            }
        }
    }

    [TestMethod()]
    public void ToStringTest()
    {
        var p0 = GetEmptyGua();
        Assert.AreEqual("", p0.ToString());
        var p1 = GetGua1();
        Assert.AreEqual("110", p1.ToString());
        var p2 = GetGua2();
        Assert.AreEqual("1101", p2.ToString());
    }

    [TestMethod()]
    public void ParseTest()
    {
        static T Parse<T>(string s) where T : IParsable<T>
        {
            return T.Parse(s, null);
        }

        Random random = new Random();
        for (int i = 0; i < 20; i++)
        {
            var c = random.Next(0, 100);
            List<Yinyang> lines1 = new();
            for (int j = 0; j < c; j++)
                lines1.Add((Yinyang)random.Next(0, 2));
            var gua = new Gua(lines1);
            Assert.IsTrue(Gua.Parse(gua.ToString()).SequenceEqual(gua));

            Assert.IsTrue(Parse<Gua>(gua.ToString()).SequenceEqual(gua));
        }
    }

    [TestMethod()]
    public void TryParseTest()
    {
        static bool TryParse<T>(string s, out T? result) where T : IParsable<T>
        {
            return T.TryParse(s, null, out result);
        }

        Assert.IsFalse(Gua.TryParse("1112", out var r));
        Assert.IsNull(r);
        Random random = new Random();
        for (int i = 0; i < 20; i++)
        {
            var c = random.Next(0, 100);
            List<Yinyang> lines1 = new();
            for (int j = 0; j < c; j++)
                lines1.Add((Yinyang)random.Next(0, 2));
            var gua = new Gua(lines1);
            Assert.IsTrue(Gua.TryParse(gua.ToString(), out var rr));
            Assert.IsTrue(rr.SequenceEqual(gua));

            Assert.IsTrue(TryParse<Gua>(gua.ToString(), out _));
        }
    }

    [TestMethod()]
    public void ToBytesTest()
    {
        var p0 = GetEmptyGua();
        BitArray bitArray0 = new BitArray(new bool[] { true });
        byte[] bytes0 = new byte[(bitArray0.Length + 7) / 8];
        bitArray0.CopyTo(bytes0, 0);
        Assert.IsTrue(p0.ToBytes().SequenceEqual(bytes0));
        var p1 = GetGua1();
        BitArray bitArray1 = new BitArray(new bool[] { true, true, false, true });
        byte[] bytes1 = new byte[(bitArray1.Length + 7) / 8];
        bitArray1.CopyTo(bytes1, 0);
        Assert.IsTrue(p1.ToBytes().SequenceEqual(bytes1));
        var p2 = GetGua2();
        BitArray bitArray2 = new BitArray(new bool[] { true, true, false, true, true });
        byte[] bytes2 = new byte[(bitArray2.Length + 7) / 8];
        bitArray2.CopyTo(bytes2, 0);
        Assert.IsTrue(p2.ToBytes().SequenceEqual(bytes2));
    }

    [TestMethod()]
    public void FromBytesTest()
    {
        Random random = new Random();
        for (int i = 0; i < 20; i++)
        {
            var c = random.Next(0, 100);
            List<Yinyang> lines1 = new();
            for (int j = 0; j < c; j++)
                lines1.Add((Yinyang)random.Next(0, 2));
            var gua = new Gua(lines1);
            Assert.IsTrue(Gua.FromBytes(gua.ToBytes())
                .SequenceEqual(gua));
        }
    }

    [TestMethod()]
    public void PropertiesTest()
    {
        var p0 = GetEmptyGua();
        var p1 = GetGua1();

        Assert.AreEqual(0, p0.Count);
        Assert.AreEqual(3, p1.Count);
        Assert.AreEqual(Yinyang.Yang, p1[0]);
        Assert.AreEqual(Yinyang.Yang, p1[1]);
        Assert.AreEqual(Yinyang.Yin, p1[2]);
    }
    [TestMethod()]
    public void OperatorsTest()
    {
        Assert.IsFalse(new Gua() == null);
        Assert.IsFalse(null == new Gua());
        Assert.IsTrue(new Gua() != null);
        Assert.IsTrue(null != new Gua());
        Random random = new Random();
        for (int i = 0; i < 20;)
        {
            var c = random.Next(5, 10);
            List<Yinyang> lines1 = new();
            for (int j = 0; j < c; j++)
            {
                lines1.Add((Yinyang)random.Next(0, 2));
            }
            List<Yinyang> lines2 = new();
            for (int j = 0; j < c; j++)
            {
                lines2.Add((Yinyang)random.Next(0, 2));
            }
            if (lines1.SequenceEqual(lines2))
            {
                Assert.IsTrue(new Gua(lines1) == new Gua(lines2));
                Assert.IsFalse(new Gua(lines1) != new Gua(lines2));
                Assert.IsTrue(new Gua(lines2) == new Gua(lines1));
                Assert.IsFalse(new Gua(lines2) != new Gua(lines1));
                i++;
            }
            else
            {
                Assert.IsFalse(new Gua(lines1) == new Gua(lines2));
                Assert.IsTrue(new Gua(lines1) != new Gua(lines2));
                Assert.IsFalse(new Gua(lines2) == new Gua(lines1));
                Assert.IsTrue(new Gua(lines2) != new Gua(lines1));
            }
        }
    }
    [TestMethod()]
    public void SerializationTest()
    {
        Random random = new Random();
        for (int i = 0; i < 20; i++)
        {
            var c = random.Next(0, 100);
            List<Yinyang> lines1 = new();
            for (int j = 0; j < c; j++)
                lines1.Add((Yinyang)random.Next(0, 2));
            var gua = new Gua(lines1);

            var s = JsonSerializer.Serialize(gua);
            var d = JsonSerializer.Deserialize<Gua>(s)!;
            Assert.IsTrue(d.SequenceEqual(gua));
        }
    }

    [TestMethod()]
    public void CalculatingTest()
    {
        Assert.AreEqual(Gua.Parse(""), Gua.Parse("") & Gua.Parse(""));
        Assert.AreEqual(Gua.Parse("1"), Gua.Parse("1") & Gua.Parse("1"));
        Assert.AreEqual(Gua.Parse("1000"), Gua.Parse("1100") & Gua.Parse("1010"));

        Assert.AreEqual(Gua.Parse(""), Gua.Parse("") | Gua.Parse(""));
        Assert.AreEqual(Gua.Parse("1"), Gua.Parse("1") | Gua.Parse("1"));
        Assert.AreEqual(Gua.Parse("1110"), Gua.Parse("1100") | Gua.Parse("1010"));

        Assert.AreEqual(Gua.Parse(""), Gua.Parse("") ^ Gua.Parse(""));
        Assert.AreEqual(Gua.Parse("0"), Gua.Parse("1") ^ Gua.Parse("1"));
        Assert.AreEqual(Gua.Parse("0110"), Gua.Parse("1100") ^ Gua.Parse("1010"));

        Assert.AreEqual(Gua.Parse(""),~Gua.Parse(""));
        Assert.AreEqual(Gua.Parse("0"), ~Gua.Parse("1"));
        Assert.AreEqual(Gua.Parse("01"), ~Gua.Parse("10"));
    }
}