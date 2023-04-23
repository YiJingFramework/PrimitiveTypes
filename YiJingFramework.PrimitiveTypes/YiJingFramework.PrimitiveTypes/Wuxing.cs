using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using YiJingFramework.PrimitiveTypes.Serialization;

namespace YiJingFramework.PrimitiveTypes;

/// <summary>
/// 五行。
/// Wuxing. (The Five Elements.)
/// </summary>
[JsonConverter(typeof(JsonConverterOfStringConvertibleForJson<Wuxing>))]
public readonly struct Wuxing :
    IComparable<Wuxing>, IEquatable<Wuxing>, IFormattable,
    IParsable<Wuxing>, IEqualityOperators<Wuxing, Wuxing, bool>,
    IStringConvertibleForJson<Wuxing>,
    IAdditionOperators<Wuxing, int, Wuxing>,
    ISubtractionOperators<Wuxing, int, Wuxing>
{
    private readonly int int32Value;
    private Wuxing(int int32ValueNotSmallerThanZero)
    {
        Debug.Assert(int32ValueNotSmallerThanZero >= 0);
        this.int32Value = int32ValueNotSmallerThanZero % 5;
    }

    #region creating
    /// <summary>
    /// 木。
    /// Wood.
    /// </summary>
    public static Wuxing Wood => default; // => new Wuxing(0);
    /// <summary>
    /// 火。
    /// Fire.
    /// </summary>
    public static Wuxing Fire => new Wuxing(1);
    /// <summary>
    /// 土。
    /// Earth.
    /// </summary>
    public static Wuxing Earth => new Wuxing(2);
    /// <summary>
    /// 金。
    /// Metal.
    /// </summary>
    public static Wuxing Metal => new Wuxing(3);
    /// <summary>
    /// 水。
    /// Water.
    /// </summary>
    public static Wuxing Water => new Wuxing(4);
    #endregion

    #region converting
    /// <inheritdoc/>
    public override string ToString()
    {
        return this.int32Value switch {
            0 => "Wood",
            1 => "Fire",
            2 => "Earth",
            3 => "Metal",
            _ => "Water" // 4 => "Water"
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

        return format.ToUpperInvariant() switch {
            "G" => this.ToString(),
            "C" => this.int32Value switch {
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
            case "木":
                result = Wood;
                return true;
            case "fire":
            case "火":
                result = Fire;
                return true;
            case "earth":
            case "土":
                result = Earth;
                return true;
            case "metal":
            case "金":
                result = Metal;
                return true;
            case "water":
            case "水":
                result = Water;
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
        return wuxing.int32Value;
    }

    /// <inheritdoc/>
    public static explicit operator Wuxing(int value)
    {
        return new Wuxing(value % 5 + 5);
    }
    #endregion

    #region comparing
    /// <inheritdoc/>
    public int CompareTo(Wuxing other)
    {
        return this.int32Value.CompareTo(other.int32Value);
    }

    /// <inheritdoc/>
    public bool Equals(Wuxing other)
    {
        return this.int32Value.Equals(other.int32Value);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not Wuxing other)
            return false;
        return this.int32Value.Equals(other.int32Value);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.int32Value.GetHashCode();
    }

    /// <inheritdoc/>
    public static bool operator ==(Wuxing left, Wuxing right)
    {
        return left.int32Value == right.int32Value;
    }

    /// <inheritdoc/>
    public static bool operator !=(Wuxing left, Wuxing right)
    {
        return left.int32Value != right.int32Value;
    }
    #endregion

    #region generating and overcoming
    /// <inheritdoc/>
    public static Wuxing operator +(Wuxing left, int right)
    {
        right = right % 5 + 5;
        return new Wuxing(left.int32Value + right);
    }

    /// <inheritdoc/>
    public static Wuxing operator -(Wuxing left, int right)
    {
        right = -(right % 5) + 5;
        return new Wuxing(left.int32Value + right);
    }

    /// <summary>
    /// 获取与另一五行之间的关系。
    /// Get the relationship with another Wuxing.
    /// </summary>
    /// <param name="another">
    /// 另一五行。
    /// The another Wuxing.
    /// </param>
    /// <returns>
    /// 关系。
    /// The relationship.
    /// </returns>
    public WuxingRelationship GetRelationship(Wuxing another)
    {
        return (WuxingRelationship)((another.int32Value - this.int32Value + 5) % 5);
    }
    /// <summary>
    /// 通过五行关系获取另一五行。
    /// Get another Wuxing with the relationship.
    /// </summary>
    /// <param name="relation">
    /// 关系。
    /// The relationship.
    /// </param>
    /// <returns>
    /// 另一五行。
    /// The another Wuxing.
    /// </returns>
    public Wuxing GetWuxing(WuxingRelationship relation)
    {
        return this + (int)relation;
    }
    #endregion

    #region serializing
    static bool IStringConvertibleForJson<Wuxing>.FromStringForJson(string s, out Wuxing result)
    {
        return TryParse(s, out result);
    }

    string IStringConvertibleForJson<Wuxing>.ToStringForJson()
    {
        return this.ToString();
    }
    #endregion
}
