using System.Diagnostics;
using YiJingFramework.PrimitiveTypes;

#pragma warning disable IDE0059
#pragma warning disable IDE0071

#region to get or convert yin-yangs
YinYang yin = YinYang.Yin;
YinYang yang = new YinYang(isYang: true);
Console.WriteLine($"{yin.ToString()}-{yang}-{yin.ToString("C")}-{yang:C}!");
Console.WriteLine();
// Output: Yin-Yang-阴-阳!

yang = YinYang.Parse("                 阳     ");
_ = YinYang.TryParse(" yANG \t\n", out yang);
// case-insensitive and allows white spaces preceding and trailing.

yin = (YinYang)false;

Console.WriteLine($"-1 0 1 2: {(YinYang)(-1)} {(YinYang)0} {(YinYang)1} {(YinYang)2}");
Console.WriteLine();
// Output: -1 0 1 2: Yang Yin Yang Yang

Console.WriteLine($"2 -> {(int)((YinYang)2)}");
Console.WriteLine();
// Output: 2 -> 1
#endregion

#region to do calculation on yin-yangs
// Calculations on yin-yangs works as booleans (yang: true, yin: false).
Console.WriteLine($"yin&yang: {yin & yang} yin|yang: {yin | yang} yin^yang: {yin ^ yang} !yin: {!yin}");
// Output: yin&yang: Yin yin|yang: Yang yin^yang: Yang !yin: Yang
#endregion

#region to create guas
var duiTrigram = new Gua(YinYang.Yang, YinYang.Yang, YinYang.Yin); // 兑

var lineArray = new YinYang[] { YinYang.Yin, YinYang.Yang };
var shaoYang = new Gua(lineArray); // 少阳

static IEnumerable<YinYang> GetRandomLines()
{
    Random random = new Random();
    for (; ; )
        yield return (YinYang)random.Next(0, 2); // 0, 1 -> yin, yang
}
var randomP = new Gua(GetRandomLines().Take(5)); // A painting with five lines.

// to create by strings, see 'convert to string and parse'
#endregion

#region to use as a list of lines
Console.WriteLine(duiTrigram.Count);
Console.WriteLine();
// Output: 3

for (int i = 0; i < shaoYang.Count; i++)
    Console.Write(shaoYang[i]);
Console.WriteLine();
Console.WriteLine();
// Output: YinYang

foreach (var line in randomP)
    Console.Write(line);
Console.WriteLine();
Console.WriteLine();
// The output will be 5 random lines.
#endregion

#region to convert to string and parse
Console.WriteLine(duiTrigram.ToString()); // yang: 1, yin: 0, yang-yang-yin: 110
Console.WriteLine();
// Output: 110

Console.WriteLine(shaoYang);
Console.WriteLine();
// Output: 01

var r = Gua.TryParse("111011111", out var myPainting);
// 111011111 -> yang-yang-yang-yin-yang-yang-yang-yang-yang
Debug.Assert(r is true);
Debug.Assert(myPainting is not null);
Console.WriteLine(myPainting[3]);
Console.WriteLine();
// Output: Yin

myPainting = Gua.Parse(myPainting.ToString()); // still the same
#endregion

#region to convert to bytes and back
// For this part,
// you don't have to understand what the result actually is.
// All you need to know is that it can be converted to the bytes without any loss,
// so you can choose to save or transmit your paintings with these methods.

byte[] bytes = duiTrigram.ToBytes();
//
// yang -> 1, yin -> 0
// yang,yang,yin -> 1,1,0 (the left digits are at the lower bit)
// 
// Add a '1' to represent that it has ended: 1,1,0 -> 1,1,0,1
// 
// Expand to a complete byte:
// 1,1,0,1 -> 1,1,0,1, 0,0,0,0 (lower to higher) -> 0b0000_1011 (higher to lower)
//
Debug.Assert(bytes.Length == 1);
Console.WriteLine(Convert.ToString(bytes[0], 2));
Console.WriteLine();
// Output: 1011

bytes = myPainting.ToBytes();
//
//    yang-yang-yang-yin - yang-yang-yang-yang - yang
// -> 1,1,1,0, 1,1,1,1, 1
// -> 1,1,1,0, 1,1,1,1, 1,1 
// -> (1110 1111) (1100 0000)
// -> 0b0000_1100 at bytes[1]
//    0b1111_0111 at bytes[0]
//
Debug.Assert(bytes.Length == 2);
Console.Write(Convert.ToString(bytes[0], 2));
Console.Write(" ");
Console.WriteLine(Convert.ToString(bytes[1], 2));
Console.WriteLine();
// Output: 11110111 11

var myPainting2 = Gua.FromBytes(bytes);
Console.WriteLine(myPainting2.Equals(myPainting));
Console.WriteLine();
// Output: True
#endregion

#region to compare two paintings
Console.WriteLine($"{myPainting2.Equals(myPainting)} " +
    $"{myPainting2 == myPainting} " +
    $"{myPainting2.CompareTo(myPainting)} " +
    $"{myPainting2 != myPainting}");
// Output: True True 0 False
#endregion