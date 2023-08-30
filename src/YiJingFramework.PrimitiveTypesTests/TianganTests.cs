using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YiJingFramework.PrimitiveTypes.Tests;

[TestClass()]
public class TianganTests
{
    [TestMethod()]
    public void ConvertingTest()
    {
        Assert.AreEqual(1, (int)new Tiangan(1));
        Assert.AreEqual(4, (int)new Tiangan(4));
        Assert.AreEqual(2, (int)new Tiangan(12));

        Assert.AreEqual("Bing", new Tiangan(3).ToString());
        Assert.AreEqual("辛", new Tiangan(8).ToString("C"));
        Assert.AreEqual("8", new Tiangan(8).ToString("N"));

        Assert.AreEqual("Gui", new Tiangan(0).ToString());
        Assert.AreEqual("Xin", new Tiangan(-2).ToString());

        for (int i = -999, j = 1; i < 1000; i++)
        {
            Assert.AreEqual(new Tiangan(j), (Tiangan)i);
            j++;
            if (j == 11)
                j = 1;
        }

        T Parse<T>(string s) where T : IParsable<T>
        {
            return T.Parse(s, null);
        }

        bool TryParse<T>(string s, out T? result) where T : IParsable<T>
        {
            return T.TryParse(s, null, out result);
        }

        Assert.AreEqual(Tiangan.Jia, Tiangan.Parse("jIa"));
        Assert.AreEqual(Tiangan.Gui, Parse<Tiangan>("癸"));
        _ = TryParse<Tiangan>("4", out var p);
        Assert.AreEqual(Tiangan.Ding, p);


        Assert.AreEqual(new Tiangan(1).Next(10 + 3), new Tiangan(4));
        Assert.AreEqual(new Tiangan(1).Next(-2), new Tiangan(9));

        Assert.AreEqual(new Tiangan(1) + 15, new Tiangan(6));
        Assert.AreEqual(new Tiangan(1) - 15, new Tiangan(6));
    }

    [TestMethod()]
    public void ComparingTest()
    {
        Random r = new Random();
        for (int i = 0; i < 20000; i++)
        {
            var fir = r.Next(1, 11);
            var sec = r.Next(1, 11);
            var firF = (Tiangan)fir;
            var secF = (Tiangan)sec;
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