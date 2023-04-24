using System.Diagnostics;
using System.Text.Json;
using YiJingFramework.PrimitiveTypes;

#pragma warning disable IDE0071

#region to get or convert Wuxings
Wuxing wood = Wuxing.Wood;

_ = Wuxing.TryParse(" metal \t\n", out Wuxing metal);
_ = Wuxing.TryParse(" 木 \t\n", out _);
// case-insensitive and allows white spaces preceding and trailing.

Wuxing fire = (Wuxing)6;
// we don't suggest this, unless you are (de)serializing it in size sensitive cases.
// ...
// -2: 金 Metal
// -1: 水 Water
// 0: 木 Wood *
// 1: 火 Fire *
// 2: 土 Earth *
// 3: 金 Metal *
// 4: 水 Water *
// 5: 木 Wood
// 6: 火 Fire
// ...

Console.WriteLine($"{fire.ToString()} {(int)fire} OVERCOMES {metal} {(int)metal}!");
Console.WriteLine($"{fire.ToString("C")}克{metal:C}");
Console.WriteLine();
// Output: Fire 1 OVERCOMES Metal 3!
// 火克金

var serialized = JsonSerializer.Serialize(wood);
var deserializedWood = JsonSerializer.Deserialize<Wuxing>(serialized);
Debug.Assert(wood == deserializedWood);
// json (de)serializable
#endregion