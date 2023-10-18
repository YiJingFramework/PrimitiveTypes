using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YiJingFramework.PrimitiveTypes.Tests;

[TestClass()]
public class TianganTests
{
    [TestMethod()]
    public void ConvertingTest()
    {
        Assert.AreEqual(1, Tiangan.Jia.Index);
        Assert.AreEqual(4, Tiangan.Ding.Index);
        Assert.AreEqual(2, Tiangan.FromIndex(12).Index);

        Assert.AreEqual("Bing", Tiangan.Bing.ToString());
        Assert.AreEqual("辛", Tiangan.Xin.ToString("C"));

        Assert.AreEqual("Gui", Tiangan.FromIndex(0).ToString());
        Assert.AreEqual("Xin", Tiangan.FromIndex(-2).ToString());

        for (int i = -999, j = 1; i < 1000; i++)
        {
            var tiangan = Tiangan.FromIndex(i);
            Assert.AreEqual(Tiangan.FromIndex(j), tiangan);
            j++;
            if (j == 11)
                j = 1;

            Assert.AreEqual(tiangan.Index - 1, (int)tiangan);
            Assert.AreEqual(tiangan.Next(), (Tiangan)i);
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
        _ = TryParse<Tiangan>("ding    ", out var p);
        Assert.AreEqual(Tiangan.Ding, p);


        Assert.AreEqual(Tiangan.Ding, Tiangan.Jia.Next(10 + 3));
        Assert.AreEqual(Tiangan.Ren, Tiangan.Jia.Next(-2));

        Assert.AreEqual(Tiangan.Ji, Tiangan.Jia + 15);
        Assert.AreEqual(Tiangan.Ji, Tiangan.Jia - 15);

        Assert.AreEqual(1, Tiangan.Yi - Tiangan.Jia);
        Assert.AreEqual(0, Tiangan.Jia - Tiangan.Jia);
        Assert.AreEqual(9, Tiangan.Jia - Tiangan.Yi);
    }

    [TestMethod()]
    public void ComparingTest()
    {
        Random r = new Random();
        for (int i = 0; i < 20000; i++)
        {
            var fir = r.Next(1, 11);
            var sec = r.Next(1, 11);
            var firF = Tiangan.FromIndex(fir);
            var secF = Tiangan.FromIndex(sec);
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