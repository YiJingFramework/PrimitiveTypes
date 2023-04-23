using Microsoft.VisualStudio.TestTools.UnitTesting;
using YiJingFramework.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace YiJingFramework.PrimitiveTypes.Tests
{
    [TestClass()]
    public class WuxingTests
    {
        [TestMethod()]
        public void ConvertingTest()
        {
            Assert.AreEqual(0, (int)Wuxing.Wood);
            Assert.AreEqual(1, (int)Wuxing.Fire);
            Assert.AreEqual(2, (int)Wuxing.Earth);
            Assert.AreEqual(3, (int)Wuxing.Metal);
            Assert.AreEqual(4, (int)Wuxing.Water);

            Assert.AreEqual("Wood", Wuxing.Wood.ToString());
            Assert.AreEqual("Fire", Wuxing.Fire.ToString());
            Assert.AreEqual("Earth", Wuxing.Earth.ToString());
            Assert.AreEqual("Metal", Wuxing.Metal.ToString());
            Assert.AreEqual("Water", Wuxing.Water.ToString());
            Assert.AreEqual("Wood", Wuxing.Wood.ToString("G"));
            Assert.AreEqual("Fire", Wuxing.Fire.ToString(null));
            Assert.AreEqual("木", Wuxing.Wood.ToString("C"));
            Assert.AreEqual("火", Wuxing.Fire.ToString("C"));
            Assert.AreEqual("土", Wuxing.Earth.ToString("C"));
            Assert.AreEqual("金", Wuxing.Metal.ToString("C"));
            Assert.AreEqual("水", Wuxing.Water.ToString("C"));

            Assert.IsTrue(Wuxing.TryParse("Wood", out Wuxing r));
            Assert.AreEqual(Wuxing.Wood, r);
            Assert.IsTrue(Wuxing.TryParse("\t\t  Wood\r\n", out r));
            Assert.AreEqual(Wuxing.Wood, r);
            Assert.IsTrue(Wuxing.TryParse("Fire", out r));
            Assert.AreEqual(Wuxing.Fire, r);
            Assert.IsTrue(Wuxing.TryParse("\t   \t  fire\r\n", out r));
            Assert.AreEqual(Wuxing.Fire, r);
            Assert.IsTrue(Wuxing.TryParse("earth", out r));
            Assert.AreEqual(Wuxing.Earth, r);
            Assert.IsTrue(Wuxing.TryParse("Metal", out r));
            Assert.AreEqual(Wuxing.Metal, r);
            Assert.IsTrue(Wuxing.TryParse("  water ", out r));
            Assert.AreEqual(Wuxing.Water, r);
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

            Assert.AreEqual(Wuxing.Wood, Wuxing.Parse("Wood"));
            Assert.AreEqual(Wuxing.Wood, Parse<Wuxing>("Wood"));
            _ = TryParse<Wuxing>("Wood", out var p);
            Assert.AreEqual(Wuxing.Wood, p);

            var allWuxing = new Wuxing[] {
                Wuxing.Wood,
                Wuxing.Fire,
                Wuxing.Earth,
                Wuxing.Metal,
                Wuxing.Water
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
        public void GeneratingAndOvercomingTest()
        {
            Assert.AreEqual(WuxingRelationship.GeneratedByMe,
                Wuxing.Wood.GetRelationship(Wuxing.Fire));
            Assert.AreEqual(WuxingRelationship.GeneratedByMe,
                Wuxing.Fire.GetRelationship(Wuxing.Earth));
            Assert.AreEqual(WuxingRelationship.GeneratedByMe,
                Wuxing.Earth.GetRelationship(Wuxing.Metal));
            Assert.AreEqual(WuxingRelationship.GeneratedByMe,
                Wuxing.Metal.GetRelationship(Wuxing.Water));
            Assert.AreEqual(WuxingRelationship.GeneratedByMe,
                Wuxing.Water.GetRelationship(Wuxing.Wood));

            Assert.AreEqual(WuxingRelationship.GeneratingMe,
                Wuxing.Wood.GetRelationship(Wuxing.Water));
            Assert.AreEqual(WuxingRelationship.GeneratingMe,
                Wuxing.Water.GetRelationship(Wuxing.Metal));
            Assert.AreEqual(WuxingRelationship.GeneratingMe,
                Wuxing.Metal.GetRelationship(Wuxing.Earth));
            Assert.AreEqual(WuxingRelationship.GeneratingMe,
                Wuxing.Earth.GetRelationship(Wuxing.Fire));
            Assert.AreEqual(WuxingRelationship.GeneratingMe,
                Wuxing.Fire.GetRelationship(Wuxing.Wood));

            for (int i = 0; i < 5; i++)
            {
                var woodP = (Wuxing)i;
                var fireP = woodP.GetWuxing(WuxingRelationship.GeneratedByMe);
                var earthP = fireP.GetWuxing(WuxingRelationship.GeneratedByMe);
                var metalP = earthP.GetWuxing(WuxingRelationship.GeneratedByMe);
                var waterP = metalP.GetWuxing(WuxingRelationship.GeneratedByMe);

                Assert.AreEqual(WuxingRelationship.GeneratedByMe,
                    woodP.GetRelationship(fireP));
                Assert.AreEqual(WuxingRelationship.OvercameByMe,
                    woodP.GetRelationship(earthP));
                Assert.AreEqual(WuxingRelationship.OvercomingMe,
                    woodP.GetRelationship(metalP));
                Assert.AreEqual(WuxingRelationship.GeneratingMe,
                    woodP.GetRelationship(waterP));

                Assert.AreEqual(fireP,
                    woodP.GetWuxing(WuxingRelationship.GeneratedByMe));
                Assert.AreEqual(earthP,
                    woodP.GetWuxing(WuxingRelationship.OvercameByMe));
                Assert.AreEqual(metalP,
                    woodP.GetWuxing(WuxingRelationship.OvercomingMe));
                Assert.AreEqual(waterP,
                    woodP.GetWuxing(WuxingRelationship.GeneratingMe));

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

        [TestMethod()]
        public void SerializationTest()
        {
            for(int i = 0; i < 5; i++)
            {
                var wuxing = (Wuxing)i;
                var s = JsonSerializer.Serialize(wuxing);
                var d = JsonSerializer.Deserialize<Wuxing>(s);
                Assert.AreEqual(wuxing, d);
            }
        }
    }
}