using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YiJingFramework.PrimitiveTypes.Tests;

[TestClass()]
public class DizhiTests
{
    [TestMethod()]
    public void ConvertingTest()
    {
        Assert.AreEqual(1, Dizhi.Zi.Index);
        Assert.AreEqual(4, Dizhi.Mao.Index);
        Assert.AreEqual(2, Dizhi.FromIndex(14).Index);

        Assert.AreEqual("Yin", Dizhi.Yin.ToString());
        Assert.AreEqual("未", Dizhi.Wei.ToString("C"));

        Assert.AreEqual("Hai", Dizhi.FromIndex(0).ToString());
        Assert.AreEqual("You", Dizhi.FromIndex(-2).ToString());

        for (int i = -1007, j = 1; i < 1000; i++)
        {
            var dizhi = Dizhi.FromIndex(i);
            Assert.AreEqual(Dizhi.FromIndex(j), dizhi);
            j++;
            if (j == 13)
                j = 1;

            Assert.AreEqual(dizhi.Index - 1, (int)dizhi);
            Assert.AreEqual(dizhi.Next(), (Dizhi)i);
        }

        T Parse<T>(string s) where T : IParsable<T>
        {
            return T.Parse(s, null);
        }

        bool TryParse<T>(string s, out T? result) where T : IParsable<T>
        {
            return T.TryParse(s, null, out result);
        }

        Assert.AreEqual(Dizhi.Zi, Dizhi.Parse("zI"));
        Assert.AreEqual(Dizhi.Yin, Parse<Dizhi>("寅"));
        _ = TryParse<Dizhi>("wei     ", out var p);
        Assert.AreEqual(Dizhi.Wei, p);

        Assert.AreEqual(Dizhi.Hai, Dizhi.Zi.Next(12 + 11));
        Assert.AreEqual(Dizhi.Xu, Dizhi.Zi.Next(-2));

        Assert.AreEqual(Dizhi.Mao, Dizhi.Zi + 15);
        Assert.AreEqual(Dizhi.You, Dizhi.Zi - 15);

        Assert.AreEqual(1, Dizhi.Chou - Dizhi.Zi);
        Assert.AreEqual(0, Dizhi.Zi - Dizhi.Zi);
        Assert.AreEqual(11, Dizhi.Zi - Dizhi.Chou);
    }

    [TestMethod()]
    public void ComparingTest()
    {
        Random r = new Random();
        for (int i = 0; i < 20000; i++)
        {
            var fir = r.Next(1, 13);
            var sec = r.Next(1, 13);
            var firF = Dizhi.FromIndex(fir);
            var secF = Dizhi.FromIndex(sec);
            if (fir == sec)
            {
                Assert.AreEqual(0, firF.CompareTo(secF));
                Assert.AreEqual(0, secF.CompareTo(firF));
                Assert.AreEqual(true, firF.Equals(secF));
                Assert.AreEqual(true, secF.Equals(firF));
                Assert.AreEqual(true, firF.Equals((object)secF));
                Assert.AreEqual(true, secF.Equals((object)firF));
                Assert.AreEqual(firF.GetHashCode(), secF.GetHashCode());
                Assert.AreEqual(true, firF == secF);
                Assert.AreEqual(true, secF == firF);
                Assert.AreEqual(false, firF != secF);
                Assert.AreEqual(false, secF != firF);
            }

            else if (fir < sec)
            {
                Assert.AreEqual(-1, firF.CompareTo(secF));
                Assert.AreEqual(1, secF.CompareTo(firF));
                Assert.AreEqual(false, firF.Equals(secF));
                Assert.AreEqual(false, secF.Equals(firF));
                Assert.AreEqual(false, firF.Equals((object)secF));
                Assert.AreEqual(false, secF.Equals((object)firF));
                Assert.AreNotEqual(firF.GetHashCode(), secF.GetHashCode());
                Assert.AreEqual(false, firF == secF);
                Assert.AreEqual(false, secF == firF);
                Assert.AreEqual(true, firF != secF);
                Assert.AreEqual(true, secF != firF);
            }

            else // fir > sec
            {
                Assert.AreEqual(1, firF.CompareTo(secF));
                Assert.AreEqual(-1, secF.CompareTo(firF));
                Assert.AreEqual(false, firF.Equals(secF));
                Assert.AreEqual(false, secF.Equals(firF));
                Assert.AreEqual(false, firF.Equals((object)secF));
                Assert.AreEqual(false, secF.Equals((object)firF));
                Assert.AreNotEqual(firF.GetHashCode(), secF.GetHashCode());
                Assert.AreEqual(false, firF == secF);
                Assert.AreEqual(false, secF == firF);
                Assert.AreEqual(true, firF != secF);
                Assert.AreEqual(true, secF != firF);
            }
            Assert.AreEqual(false, firF.Equals(null));
            Assert.AreEqual(false, secF.Equals(new object()));
        }
    }
}