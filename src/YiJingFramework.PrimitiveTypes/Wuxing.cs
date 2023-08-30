using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace YiJingFramework.PrimitiveTypes;

/// <summary>
/// 五行。
/// Wuxing. (The Five Elements.)
/// </summary>
public readonly struct Wuxing :
    IComparable<Wuxing>, IEquatable<Wuxing>, IFormattable,
    IParsable<Wuxing>, IEqualityOperators<Wuxing, Wuxing, bool>,
    IAdditionOperators<Wuxing, int, Wuxing>,
    ISubtractionOperators<Wuxing, int, Wuxing>
{
    private readonly int index;
    private Wuxing(int indexChecked)
    {
        Debug.Assert(indexChecked is >= 0 and < 5);
        this.index = indexChecked;
    }

    #region creating
    /// <summary>
    /// 木。
    /// Wood.
    /// </summary>
    public static Wuxing Mu => new Wuxing(0);
    /// <summary>
    /// 火。
    /// Fire.
    /// </summary>
    public static Wuxing Huo => new Wuxing(1);
    /// <summary>
    /// 土。
    /// Earth.
    /// </summary>
    public static Wuxing Tu => new Wuxing(2);
    /// <summary>
    /// 金。
    /// Metal.
    /// </summary>
    public static Wuxing Jin => new Wuxing(3);
    /// <summary>
    /// 水。
    /// Water.
    /// </summary>
    public static Wuxing Shui => new Wuxing(4);
    #endregion

    #region converting
    /// <inheritdoc/>
    public override string ToString()
    {
        return this.index switch
        {
            0 => "Mu",
            1 => "Huo",
            2 => "Tu",
            3 => "Jin",
            _ => "Shui"
        };
    }

    /// <summary>
    /// 按照指定格式转换为字符串。
    /// Convert to a string with the given format.
    /// </summary>
    /// <param name="format">
    /// 要使用的格式。
    /// <c>"G"</c> 表示英文； <c>"C"</c> 表示中文。
    /// <c>"G"</c> represents English; and <c>"C"</c> represents Chinese.
    /// </param>
    /// <param name="formatProvider">
    /// 不会使用此参数。
    /// This parameter will won't be used.
    /// </param>
    /// <returns>
    /// 结果。
    /// The result.
    /// </returns>
    /// <exception cref="FormatException">
    /// 给出的格式化字符串不受支持。
    /// The given format is not supported.
    /// </exception>
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        if (string.IsNullOrEmpty(format))
            format = "G";

        return format.ToUpperInvariant() switch
        {
            "G" => this.ToString(),
            "C" => this.index switch
            {
                0 => "木",
                1 => "火",
                2 => "土",
                3 => "金",
                _ => "水" // 4 => "水"
            },
            _ => throw new FormatException($"The format string \"{format}\" is not supported.")
        };
    }

    /// <summary>
    /// 从字符串转换。
    /// Convert from a string.
    /// </summary>
    /// <param name="s">
    /// 字符串。
    /// The string.
    /// </param>
    /// <returns>
    /// 结果。
    /// The result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> 是 <c>null</c> 。
    /// <paramref name="s"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="FormatException">
    /// 传入字符串的格式不受支持。
    /// The input string was not in the supported format.
    /// </exception>
    public static Wuxing Parse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TryParse(s, out var result))
            return result;
        throw new FormatException($"Cannot parse \"{s}\" as {nameof(Wuxing)}.");
    }

    /// <summary>
    /// 从字符串转换。
    /// Convert from a string.
    /// </summary>
    /// <param name="s">
    /// 字符串。
    /// The string.
    /// </param>
    /// <param name="result">
    /// 结果。
    /// The result.
    /// </param>
    /// <returns>
    /// 一个指示转换成功与否的值。
    /// A value indicates whether it has been successfully converted or not.
    /// </returns>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        [MaybeNullWhen(false)] out Wuxing result)
    {
        switch (s?.Trim()?.ToLowerInvariant())
        {
            case "wood":
            case "mu":
            case "木":
                result = Mu;
                return true;
            case "fire":
            case "huo":
            case "火":
                result = Huo;
                return true;
            case "earth":
            case "tu":
            case "土":
                result = Tu;
                return true;
            case "metal":
            case "jin":
            case "金":
                result = Jin;
                return true;
            case "water":
            case "shui":
            case "水":
                result = Shui;
                return true;
            default:
                result = default;
                return false;
        }
    }

    static Wuxing IParsable<Wuxing>.Parse(string s, IFormatProvider? provider)
    {
        return Parse(s);
    }

    static bool IParsable<Wuxing>.TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Wuxing result)
    {
        return TryParse(s, out result);
    }


    /// <inheritdoc/>
    public static explicit operator int(Wuxing wuxing)
    {
        return wuxing.index;
    }

    /// <inheritdoc/>
    public static explicit operator Wuxing(int value)
    {
        value %= 5;
        value += 5;
        return new Wuxing(value % 5);
    }
    #endregion

    #region comparing
    /// <inheritdoc/>
    public int CompareTo(Wuxing other)
    {
        return this.index.CompareTo(other.index);
    }

    /// <inheritdoc/>
    public bool Equals(Wuxing other)
    {
        return this.index.Equals(other.index);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not Wuxing other)
            return false;
        return this.index.Equals(other.index);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.index.GetHashCode();
    }

    /// <inheritdoc/>
    public static bool operator ==(Wuxing left, Wuxing right)
    {
        return left.index == right.index;
    }

    /// <inheritdoc/>
    public static bool operator !=(Wuxing left, Wuxing right)
    {
        return left.index != right.index;
    }
    #endregion

    #region calculating
    /// <inheritdoc/>
    public static Wuxing operator +(Wuxing left, int right)
    {
        right %= 5;
        right += 5;
        right += left.index;
        return new Wuxing(right % 5);
    }

    /// <inheritdoc/>
    public static Wuxing operator -(Wuxing left, int right)
    {
        right %= 5;
        right = -right;
        right += 5;
        right += left.index;
        return new Wuxing(right % 5);
    }
    #endregion
}
