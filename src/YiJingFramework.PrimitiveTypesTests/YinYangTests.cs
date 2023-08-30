using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace YiJingFramework.PrimitiveTypes.Tests;

[TestClass()]
public class YinyangTests
{
    [TestMethod()]
    public void CalculatingTest()
    {
        Assert.AreEqual(Yinyang.Yang, Yinyang.Yang & Yinyang.Yang);
        Assert.AreEqual(Yinyang.Yin, Yinyang.Yang & Yinyang.Yin);
        Assert.AreEqual(Yinyang.Yin, Yinyang.Yin & Yinyang.Yang);
        Assert.AreEqual(Yinyang.Yin, Yinyang.Yin & Yinyang.Yin);

        Assert.AreEqual(Yinyang.Yang, Yinyang.Yang | Yinyang.Yang);
        Assert.AreEqual(Yinyang.Yang, Yinyang.Yang | Yinyang.Yin);
        Assert.AreEqual(Yinyang.Yang, Yinyang.Yin | Yinyang.Yang);
        Assert.AreEqual(Yinyang.Yin, Yinyang.Yin | Yinyang.Yin);

        Assert.AreEqual(Yinyang.Yin, Yinyang.Yang ^ Yinyang.Yang);
        Assert.AreEqual(Yinyang.Yang, Yinyang.Yang ^ Yinyang.Yin);
        Assert.AreEqual(Yinyang.Yang, Yinyang.Yin ^ Yinyang.Yang);
        Assert.AreEqual(Yinyang.Yin, Yinyang.Yin ^ Yinyang.Yin);

        Assert.AreEqual(Yinyang.Yin, !Yinyang.Yang);
        Assert.AreEqual(Yinyang.Yang, !Yinyang.Yin);

        static T Tide<T>(T t1) where T : IBitwiseOperators<T, T, T>
        {
            return ~t1;
        }
        Assert.AreEqual(Yinyang.Yin, Tide(Yinyang.Yang));
        Assert.AreEqual(Yinyang.Yang, Tide(Yinyang.Yin));
    }

    [TestMethod()]
    public void ConvertingTest()
    {
        Assert.AreEqual(false, Yinyang.Yin.IsYang);
        Assert.AreEqual(true, Yinyang.Yang.IsYang);
        Assert.AreEqual(false, new Yinyang(false).IsYang);
        Assert.AreEqual(true, new Yinyang(true).IsYang);

        Assert.AreEqual("Yin", Yinyang.Yin.ToString());
        Assert.AreEqual("Yang", Yinyang.Yang.ToString());
        Assert.AreEqual("Yin", new Yinyang(false).ToString());
        Assert.AreEqual("Yang", new Yinyang(true).ToString());
        Assert.AreEqual("阴", Yinyang.Yin.ToString("C"));
        Assert.AreEqual("阳", Yinyang.Yang.ToString("C"));
        Assert.AreEqual("Yin", Yinyang.Yin.ToString("G"));
        Assert.AreEqual("Yang", Yinyang.Yang.ToString(null));

        Assert.AreEqual(Yinyang.Yin, Yinyang.Parse("Yin"));
        Assert.AreEqual(Yinyang.Yin, Yinyang.Parse("\r\nYIN "));
        Assert.AreEqual(Yinyang.Yin, Yinyang.Parse("阴"));
        Assert.AreEqual(Yinyang.Yang, Yinyang.Parse("yANg"));
        Assert.AreEqual(Yinyang.Yang, Yinyang.Parse("\r\nYANG "));
        Assert.AreEqual(Yinyang.Yang, Yinyang.Parse("\r\n阳 "));

        Assert.IsTrue(Yinyang.TryParse("Yin", out Yinyang r));
        Assert.AreEqual(Yinyang.Yin, r);
        Assert.IsTrue(Yinyang.TryParse("\r\nYIN ", out r));
        Assert.AreEqual(Yinyang.Yin, r);
        Assert.IsTrue(Yinyang.TryParse("阴", out r));
        Assert.AreEqual(Yinyang.Yin, r);
        Assert.IsTrue(Yinyang.TryParse("yANg", out r));
        Assert.AreEqual(Yinyang.Yang, r);
        Assert.IsTrue(Yinyang.TryParse("\r\nYANG ", out r));
        Assert.AreEqual(Yinyang.Yang, r);
        Assert.IsTrue(Yinyang.TryParse("\r\n阳 ", out r));
        Assert.AreEqual(Yinyang.Yang, r);
        Assert.IsFalse(Yinyang.TryParse("yinyang", out _));
        Assert.IsFalse(Yinyang.TryParse("false", out _));
        Assert.IsFalse(Yinyang.TryParse(null, out _));

        T Parse<T>(string s) where T : IParsable<T>
        {
            return T.Parse(s, null);
        }
        bool TryParse<T>(string s, out T? result) where T : IParsable<T>
        {
            return T.TryParse(s, null, out result);
        }
        Assert.AreEqual(Yinyang.Yang, Parse<Yinyang>("yang"));
        Assert.AreEqual(true, TryParse<Yinyang>("yang", out _));

        Assert.AreEqual(false, (bool)Yinyang.Yin);
        Assert.AreEqual(true, (bool)Yinyang.Yang);
        Assert.AreEqual(false, (bool)new Yinyang(false));
        Assert.AreEqual(true, (bool)new Yinyang(true));

        Assert.AreEqual(0, (int)Yinyang.Yin);
        Assert.AreEqual(1, (int)Yinyang.Yang);
        Assert.AreEqual(0, (int)new Yinyang(false));
        Assert.AreEqual(1, (int)new Yinyang(true));

        Assert.AreEqual(Yinyang.Yang, (Yinyang)2);
        Assert.AreEqual(Yinyang.Yang, (Yinyang)1);
        Assert.AreEqual(Yinyang.Yin, (Yinyang)0);
        Assert.AreEqual(Yinyang.Yang, (Yinyang)(-1));

        Assert.AreEqual(Yinyang.Yang, (Yinyang)true);
        Assert.AreEqual(Yinyang.Yin, (Yinyang)false);
    }

    [TestMethod()]
    public void ComparingTest()
    {
        Assert.AreEqual(0, Yinyang.Yang.CompareTo(Yinyang.Yang));
        Assert.AreEqual(1, Yinyang.Yang.CompareTo(Yinyang.Yin));
        Assert.AreEqual(-1, Yinyang.Yin.CompareTo(Yinyang.Yang));
        Assert.AreEqual(0, Yinyang.Yin.CompareTo(Yinyang.Yin));

        Assert.AreEqual(true, Yinyang.Yang.Equals(Yinyang.Yang));
        Assert.AreEqual(false, Yinyang.Yang.Equals(Yinyang.Yin));
        Assert.AreEqual(false, Yinyang.Yin.Equals(Yinyang.Yang));
        Assert.AreEqual(true, Yinyang.Yin.Equals(Yinyang.Yin));

        Assert.AreEqual(true, Yinyang.Yang.Equals((object)Yinyang.Yang));
        Assert.AreEqual(false, Yinyang.Yang.Equals((object)Yinyang.Yin));
        Assert.AreEqual(false, Yinyang.Yin.Equals((object)Yinyang.Yang));
        Assert.AreEqual(true, Yinyang.Yin.Equals((object)Yinyang.Yin));
        Assert.AreEqual(false, Yinyang.Yang.Equals(null));
        Assert.AreEqual(false, Yinyang.Yin.Equals(true));

        Assert.AreEqual(Yinyang.Yang.GetHashCode(), Yinyang.Yang.GetHashCode());
        Assert.AreEqual(Yinyang.Yin.GetHashCode(), Yinyang.Yin.GetHashCode());
        Assert.AreNotEqual(Yinyang.Yang.GetHashCode(), Yinyang.Yin.GetHashCode());

        Assert.AreEqual(true, Yinyang.Yang == Yinyang.Yang);
        Assert.AreEqual(false, Yinyang.Yang == Yinyang.Yin);
        Assert.AreEqual(false, Yinyang.Yin == Yinyang.Yang);
        Assert.AreEqual(true, Yinyang.Yin == Yinyang.Yin);

        Assert.AreEqual(false, Yinyang.Yang != Yinyang.Yang);
        Assert.AreEqual(true, Yinyang.Yang != Yinyang.Yin);
        Assert.AreEqual(true, Yinyang.Yin != Yinyang.Yang);
        Assert.AreEqual(false, Yinyang.Yin != Yinyang.Yin);
    }
}