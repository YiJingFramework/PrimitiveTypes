using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YiJingFramework.PrimitiveTypes.Tests;

[TestClass()]
public class WuxingTests
{
    [TestMethod()]
    public void ConvertingTest()
    {
        Assert.AreEqual(0, (int)Wuxing.Mu);
        Assert.AreEqual(1, (int)Wuxing.Huo);
        Assert.AreEqual(2, (int)Wuxing.Tu);
        Assert.AreEqual(3, (int)Wuxing.Jin);
        Assert.AreEqual(4, (int)Wuxing.Shui);

        Assert.AreEqual("Mu", Wuxing.Mu.ToString());
        Assert.AreEqual("Huo", Wuxing.Huo.ToString());
        Assert.AreEqual("Tu", Wuxing.Tu.ToString());
        Assert.AreEqual("Jin", Wuxing.Jin.ToString());
        Assert.AreEqual("Shui", Wuxing.Shui.ToString());
        Assert.AreEqual("Mu", Wuxing.Mu.ToString("G"));
        Assert.AreEqual("Huo", Wuxing.Huo.ToString(null));
        Assert.AreEqual("木", Wuxing.Mu.ToString("C"));
        Assert.AreEqual("火", Wuxing.Huo.ToString("C"));
        Assert.AreEqual("土", Wuxing.Tu.ToString("C"));
        Assert.AreEqual("金", Wuxing.Jin.ToString("C"));
        Assert.AreEqual("水", Wuxing.Shui.ToString("C"));

        Assert.IsTrue(Wuxing.TryParse("Mu", out var r));
        Assert.AreEqual(Wuxing.Mu, r);
        Assert.IsTrue(Wuxing.TryParse("\t\t  Mu\r\n", out r));
        Assert.AreEqual(Wuxing.Mu, r);
        Assert.IsTrue(Wuxing.TryParse("Huo", out r));
        Assert.AreEqual(Wuxing.Huo, r);
        Assert.IsTrue(Wuxing.TryParse("\t   \t  huo\r\n", out r));
        Assert.AreEqual(Wuxing.Huo, r);
        Assert.IsTrue(Wuxing.TryParse("tu", out r));
        Assert.AreEqual(Wuxing.Tu, r);
        Assert.IsTrue(Wuxing.TryParse("Jin", out r));
        Assert.AreEqual(Wuxing.Jin, r);
        Assert.IsTrue(Wuxing.TryParse("  shui ", out r));
        Assert.AreEqual(Wuxing.Shui, r);
        Assert.IsFalse(Wuxing.TryParse("false", out _));
        Assert.IsFalse(Wuxing.TryParse(null, out _));

        T Parse<T>(string s) where T : IParsable<T>
        {
            return T.Parse(s, null);
        }

        bool TryParse<T>(string s, out T? result) where T : IParsable<T>
        {
            return T.TryParse(s, null, out result);
        }

        Assert.AreEqual(Wuxing.Mu, Wuxing.Parse("Mu"));
        Assert.AreEqual(Wuxing.Mu, Parse<Wuxing>("Mu"));
        _ = TryParse<Wuxing>("Mu", out var p);
        Assert.AreEqual(Wuxing.Mu, p);

        var allWuxing = new Wuxing[] {
            Wuxing.Mu,
            Wuxing.Huo,
            Wuxing.Tu,
            Wuxing.Jin,
            Wuxing.Shui
        };
        for (int i = -1000, j = 0; i < 1000; i++)
        {
            Assert.AreEqual(allWuxing[j], (Wuxing)i);
            j++;
            if (j == 5)
                j = 0;
        }
    }

    [TestMethod()]
    public void ComparingTest()
    {
        Random r = new Random();
        for (int i = 0; i < 20000; i++)
        {
            var fir = (r.Next(-10000, 9999) % 5 + 5) % 5;
            var sec = (r.Next(-10000, 9999) % 5 + 5) % 5;
            var firF = (Wuxing)fir;
            var secF = (Wuxing)sec;
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
    [TestMethod()]
    public void CalculationTest()
    {
        for (int i = 0; i < 5; i++)
        {
            var woodP = (Wuxing)i;
            var fireP = (Wuxing)(i + 1);
            var earthP = (Wuxing)(i + 2);
            var metalP = (Wuxing)(i + 3);
            var waterP = (Wuxing)(i + 4);

            Assert.AreEqual(woodP, woodP + 0);
            Assert.AreEqual(fireP, woodP + 1);
            Assert.AreEqual(earthP, woodP + 2);
            Assert.AreEqual(metalP, woodP + 3);
            Assert.AreEqual(waterP, woodP + 4);
            Assert.AreEqual(woodP, woodP + 5);
            Assert.AreEqual(fireP, woodP + 6);

            Assert.AreEqual(woodP, woodP - 5);
            Assert.AreEqual(fireP, woodP - 4);
            Assert.AreEqual(earthP, woodP - 3);
            Assert.AreEqual(metalP, woodP - 2);
            Assert.AreEqual(waterP, woodP - 1);
            Assert.AreEqual(woodP, woodP - 0);
            Assert.AreEqual(fireP, woodP - (-1));
        }
    }
}