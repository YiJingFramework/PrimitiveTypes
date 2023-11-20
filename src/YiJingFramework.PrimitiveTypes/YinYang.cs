using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace YiJingFramework.PrimitiveTypes;

/// <summary>
/// 阴阳。
/// Yinyang.
/// </summary>
/// <param name="isYang">
/// 若值为 <c>true</c> ，则此实例将表示阳；否则表示阴。
/// If the value is <c>true</c>, the instance will represents Yang; otherwise, Yin.
/// </param>
public readonly struct Yinyang(bool isYang) :
    IComparable<Yinyang>, IEquatable<Yinyang>, IFormattable,
    IParsable<Yinyang>, IEqualityOperators<Yinyang, Yinyang, bool>,
    IBitwiseOperators<Yinyang, Yinyang, Yinyang>
{
    /// <summary>
    /// 获取此实例是否表示阳。
    /// Get whether the instance represents Yang.
    /// </summary>
    public bool IsYang { get; } = isYang;

    #region creating
    /// <summary>
    /// 阳。
    /// Yang.
    /// </summary>
    public static Yinyang Yang => new Yinyang(true);

    /// <summary>
    /// 阴。
    /// Yin.
    /// </summary>
    public static Yinyang Yin => new Yinyang(false);
    #endregion

    #region calculating
    /// <inheritdoc/>
    public static Yinyang operator &(Yinyang left, Yinyang right)
    {
        return new Yinyang(left.IsYang & right.IsYang);
    }

    /// <inheritdoc/>
    public static Yinyang operator |(Yinyang left, Yinyang right)
    {
        return new Yinyang(left.IsYang | right.IsYang);
    }

    /// <inheritdoc/>
    public static Yinyang operator ^(Yinyang left, Yinyang right)
    {
        return new Yinyang(left.IsYang ^ right.IsYang);
    }

    /// <inheritdoc/>
    public static Yinyang operator !(Yinyang yinYang)
    {
        return new Yinyang(!yinYang.IsYang);
    }

    /// <inheritdoc/>
    static Yinyang IBitwiseOperators<Yinyang, Yinyang, Yinyang>.operator ~(Yinyang yinYang)
    {
        return !yinYang;
    }
    #endregion

    #region converting
    /// <inheritdoc/>
    public override string ToString()
    {
        return this.IsYang ? "Yang" : "Yin";
    }

    /// <summary>
    /// 按照指定格式转换为字符串。
    /// Convert to a string with the given format.
    /// </summary>
    /// <param name="format">
    /// 要使用的格式。
    /// <c>"G"</c> 表示拼音； <c>"C"</c> 表示中文。
    /// The format to use.
    /// <c>"G"</c> means to be in Pinyin; and <c>"C"</c> means in Chinese.
    /// </param>
    /// <param name="formatProvider">
    /// 不会使用此参数。
    /// This parameter won't be used.
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
            "C" => this.IsYang ? "阳" : "阴",
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
    public static Yinyang Parse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        return s.Trim().ToLowerInvariant() switch
        {
            "阳" or "yang" => Yang,
            "阴" or "yin" => Yin,
            _ => throw new FormatException(
                $"Cannot parse \"{s}\" as {nameof(Yinyang)}."),
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
        [MaybeNullWhen(false)] out Yinyang result)
    {
        switch (s?.Trim()?.ToLowerInvariant())
        {
            case "阳":
            case "yang":
                result = Yang;
                return true;
            case "阴":
            case "yin":
                result = Yin;
                return true;
            default:
                result = default;
                return false;
        }
    }

    static Yinyang IParsable<Yinyang>.Parse(string s, IFormatProvider? provider)
    {
        return Parse(s);
    }

    static bool IParsable<Yinyang>.TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Yinyang result)
    {
        return TryParse(s, out result);
    }

    /// <inheritdoc/>
    public static explicit operator bool(Yinyang yinYang)
    {
        return yinYang.IsYang;
    }

    /// <inheritdoc/>
    public static explicit operator Yinyang(bool value)
    {
        return new Yinyang(value);
    }

    /// <inheritdoc/>
    public static explicit operator int(Yinyang yinYang)
    {
        return Convert.ToInt32(yinYang.IsYang);
    }

    /// <inheritdoc/>
    public static explicit operator Yinyang(int value)
    {
        return new Yinyang(Convert.ToBoolean(value));
    }
    #endregion

    #region comparing
    /// <inheritdoc/>
    public int CompareTo(Yinyang other)
    {
        return this.IsYang.CompareTo(other.IsYang);
    }

    /// <inheritdoc/>
    public bool Equals(Yinyang other)
    {
        return this.IsYang.Equals(other.IsYang);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not Yinyang other)
            return false;
        return this.IsYang.Equals(other.IsYang);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.IsYang.GetHashCode();
    }

    /// <inheritdoc/>
    public static bool operator ==(Yinyang left, Yinyang right)
    {
        return left.IsYang == right.IsYang;
    }

    /// <inheritdoc/>
    public static bool operator !=(Yinyang left, Yinyang right)
    {
        return left.IsYang != right.IsYang;
    }
    #endregion
}