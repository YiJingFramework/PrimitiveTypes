using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace YiJingFramework.PrimitiveTypes;

/// <summary>
/// 天干。
/// Tiangan.
/// </summary>
public readonly struct Tiangan :
    IComparable<Tiangan>, IEquatable<Tiangan>, IFormattable,
    IParsable<Tiangan>, IEqualityOperators<Tiangan, Tiangan, bool>,
    IAdditionOperators<Tiangan, int, Tiangan>,
    ISubtractionOperators<Tiangan, int, Tiangan>
{
    private readonly int indexMinusOne;

    private Tiangan(int indexMinusOneChecked)
    {
        Debug.Assert(indexMinusOneChecked is >= 0 and < 10);
        this.indexMinusOne = indexMinusOneChecked;
    }

    /// <summary>
    /// 获取此天干的下第 <paramref name="n"/> 个天干。
    /// Get the <paramref name="n"/>th Tiangan next to this instance.
    /// </summary>
    /// <param name="n">
    /// 数字 <paramref name="n"/> 。
    /// 可以小于零以表示另一个方向。
    /// The number <paramref name="n"/>.
    /// It could be smaller than zero which means the other direction.
    /// </param>
    /// <returns>
    /// 指定天干。
    /// The Tiangan.
    /// </returns>
    public Tiangan Next(int n = 1)
    {
        n %= 10;
        n += 10;
        n += this.indexMinusOne;
        return new Tiangan(n % 10);
    }

    /// <inheritdoc/>
    public static Tiangan operator +(Tiangan left, int right)
    {
        right %= 10;
        right += 10;
        right += left.indexMinusOne;
        return new Tiangan(right % 10);
    }

    /// <inheritdoc/>
    public static Tiangan operator -(Tiangan left, int right)
    {
        right %= 10;
        right = -right;
        right += 10;
        right += left.indexMinusOne;
        return new Tiangan(right % 10);
    }

    #region converting
    /// <inheritdoc/>
    public override string ToString()
    {
        return this.indexMinusOne switch
        {
            0 => "Jia",
            1 => "Yi",
            2 => "Bing",
            3 => "Ding",
            4 => "Wu",
            5 => "Ji",
            6 => "Geng",
            7 => "Xin",
            8 => "Ren",
            _ => "Gui"
        };
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
            "C" => this.indexMinusOne switch
            {
                0 => "甲",
                1 => "乙",
                2 => "丙",
                3 => "丁",
                4 => "戊",
                5 => "己",
                6 => "庚",
                7 => "辛",
                8 => "壬",
                _ => "癸"
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
    public static Tiangan Parse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TryParse(s, out var result))
            return result;
        throw new FormatException($"Cannot parse \"{s}\" as {nameof(Tiangan)}.");
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
        [MaybeNullWhen(false)] out Tiangan result)
    {
        switch (s?.Trim()?.ToLowerInvariant())
        {
            case "jia":
            case "甲":
                result = Jia;
                return true;
            case "yi":
            case "乙":
                result = Yi;
                return true;
            case "bing":
            case "丙":
                result = Bing;
                return true;
            case "ding":
            case "丁":
                result = Ding;
                return true;
            case "wu":
            case "戊":
                result = Wu;
                return true;
            case "ji":
            case "己":
                result = Ji;
                return true;
            case "geng":
            case "庚":
                result = Geng;
                return true;
            case "xin":
            case "辛":
                result = Xin;
                return true;
            case "ren":
            case "壬":
                result = Ren;
                return true;
            case "gui":
            case "癸":
                result = Gui;
                return true;
            default:
                result = default;
                return false;
        }
    }

    static Tiangan IParsable<Tiangan>.Parse(string s, IFormatProvider? provider)
    {
        return Parse(s);
    }

    static bool IParsable<Tiangan>.TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Tiangan result)
    {
        return TryParse(s, out result);
    }

    /// <inheritdoc/>
    public static explicit operator int(Tiangan tiangan)
    {
        return tiangan.indexMinusOne;
    }

    /// <inheritdoc/>
    public static explicit operator Tiangan(int value)
    {
        value %= 10;
        value += 10;
        return new Tiangan(value % 10);
    }

    /// <summary>
    /// 获取天干的序数。
    /// 以甲为 <c>1</c> ，直到癸为 <c>10</c> 。
    /// Get the index of the Tiangan.
    /// For example, it will be <c>1</c> for Jia and <c>10</c> for Gui.
    /// </summary>
    public int Index => this.indexMinusOne + 1;

    /// <summary>
    /// 从 <seealso cref="Index"/> 获取天干。
    /// The a Tiangan from its <seealso cref="Index"/>.
    /// </summary>
    /// <param name="index">
    /// 序数。
    /// The index.
    /// </param>
    /// <returns>
    /// 天干。
    /// The Tiangan.
    /// </returns>
    public static Tiangan FromIndex(int index)
    {
        index %= 10;
        index += 10 - 1;
        return new Tiangan(index % 10);
    }
    #endregion

    #region comparing
    /// <inheritdoc/>
    public int CompareTo(Tiangan other)
    {
        return this.indexMinusOne.CompareTo(other.indexMinusOne);
    }

    /// <inheritdoc/>
    public bool Equals(Tiangan other)
    {
        return this.indexMinusOne.Equals(other.indexMinusOne);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not Tiangan other)
            return false;
        return this.indexMinusOne.Equals(other.indexMinusOne);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.indexMinusOne.GetHashCode();
    }

    /// <inheritdoc/>
    public static bool operator ==(Tiangan left, Tiangan right)
    {
        return left.indexMinusOne == right.indexMinusOne;
    }

    /// <inheritdoc/>
    public static bool operator !=(Tiangan left, Tiangan right)
    {
        return left.indexMinusOne != right.indexMinusOne;
    }
    #endregion

    #region values
    /// <summary>
    /// 甲。
    /// Jia.
    /// </summary>
    public static Tiangan Jia => new Tiangan(0);
    /// <summary>
    /// 乙。
    /// Yi.
    /// </summary>
    public static Tiangan Yi => new Tiangan(1);
    /// <summary>
    /// 丙。
    /// Bing.
    /// </summary>
    public static Tiangan Bing => new Tiangan(2);
    /// <summary>
    /// 丁。
    /// Ding.
    /// </summary>
    public static Tiangan Ding => new Tiangan(3);
    /// <summary>
    /// 戊。
    /// Wu.
    /// </summary>
    public static Tiangan Wu => new Tiangan(4);
    /// <summary>
    /// 己。
    /// Ji.
    /// </summary>
    public static Tiangan Ji => new Tiangan(5);
    /// <summary>
    /// 庚。
    /// Geng.
    /// </summary>
    public static Tiangan Geng => new Tiangan(6);
    /// <summary>
    /// 辛。
    /// Xin.
    /// </summary>
    public static Tiangan Xin => new Tiangan(7);
    /// <summary>
    /// 壬。
    /// Ren.
    /// </summary>
    public static Tiangan Ren => new Tiangan(8);
    /// <summary>
    /// 癸。
    /// Gui.
    /// </summary>
    public static Tiangan Gui => new Tiangan(9);
    #endregion
}