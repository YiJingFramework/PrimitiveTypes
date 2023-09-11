using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace YiJingFramework.PrimitiveTypes;

/// <summary>
/// 地支。
/// Dizhi.
/// </summary>
public readonly struct Dizhi :
    IComparable<Dizhi>, IEquatable<Dizhi>, IFormattable,
    IParsable<Dizhi>, IEqualityOperators<Dizhi, Dizhi, bool>,
    IAdditionOperators<Dizhi, int, Dizhi>,
    ISubtractionOperators<Dizhi, int, Dizhi>
{
    private readonly int indexMinusOne;
    private Dizhi(int indexMinusOneChecked)
    {
        Debug.Assert(indexMinusOneChecked is >= 0 and < 12);
        this.indexMinusOne = indexMinusOneChecked;
    }

    /// <summary>
    /// 获取此地支的下第 <paramref name="n"/> 个地支。
    /// Get the <paramref name="n"/>th Dizhi next to this instance.
    /// </summary>
    /// <param name="n">
    /// 数字 <paramref name="n"/> 。
    /// 可以小于零以表示另一个方向。
    /// The number <paramref name="n"/>.
    /// It could be smaller than zero which means the other direction.
    /// </param>
    /// <returns>
    /// 指定地支。
    /// The Dizhi.
    /// </returns>
    public Dizhi Next(int n = 1)
    {
        n %= 12;
        n += 12;
        n += this.indexMinusOne;
        return new Dizhi(n % 12);
    }

    /// <inheritdoc/>
    public static Dizhi operator +(Dizhi left, int right)
    {
        right %= 12;
        right += 12;
        right += left.indexMinusOne;
        return new Dizhi(right % 12);
    }

    /// <inheritdoc/>
    public static Dizhi operator -(Dizhi left, int right)
    {
        right %= 12;
        right = -right;
        right += 12;
        right += left.indexMinusOne;
        return new Dizhi(right % 12);
    }

    #region converting

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.indexMinusOne switch
        {
            0 => "Zi",
            1 => "Chou",
            2 => "Yin",
            3 => "Mao",
            4 => "Chen",
            5 => "Si",
            6 => "Wu",
            7 => "Wei",
            8 => "Shen",
            9 => "You",
            10 => "Xu",
            _ => "Hai"
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
                0 => "子",
                1 => "丑",
                2 => "寅",
                3 => "卯",
                4 => "辰",
                5 => "巳",
                6 => "午",
                7 => "未",
                8 => "申",
                9 => "酉",
                10 => "戌",
                _ => "亥"
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
    public static Dizhi Parse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TryParse(s, out var result))
            return result;
        throw new FormatException($"Cannot parse \"{s}\" as {nameof(Dizhi)}.");
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
        [MaybeNullWhen(false)] out Dizhi result)
    {
        switch (s?.Trim()?.ToLowerInvariant())
        {
            case "zi":
            case "子":
                result = Zi;
                return true;
            case "chou":
            case "丑":
                result = Chou;
                return true;
            case "yin":
            case "寅":
                result = Yin;
                return true;
            case "mao":
            case "卯":
                result = Mao;
                return true;
            case "chen":
            case "辰":
                result = Chen;
                return true;
            case "si":
            case "巳":
                result = Si;
                return true;
            case "wu":
            case "午":
                result = Wu;
                return true;
            case "wei":
            case "未":
                result = Wei;
                return true;
            case "shen":
            case "申":
                result = Shen;
                return true;
            case "you":
            case "酉":
                result = You;
                return true;
            case "xu":
            case "戌":
                result = Xu;
                return true;
            case "hai":
            case "亥":
                result = Hai;
                return true;
            default:
                result = default;
                return false;
        }
    }

    static Dizhi IParsable<Dizhi>.Parse(string s, IFormatProvider? provider)
    {
        return Parse(s);
    }

    static bool IParsable<Dizhi>.TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Dizhi result)
    {
        return TryParse(s, out result);
    }

    /// <inheritdoc/>
    public static explicit operator int(Dizhi dizhi)
    {
        return dizhi.indexMinusOne;
    }

    /// <inheritdoc/>
    public static explicit operator Dizhi(int value)
    {
        value %= 12;
        value += 12;
        return new Dizhi(value % 12);
    }

    /// <summary>
    /// 获取地支的序数。
    /// 以子为 <c>1</c> ，直到亥为 <c>12</c> 。
    /// Get the index of the Dizhi.
    /// For example, it will be <c>1</c> for Zi and <c>12</c> for Hai.
    /// </summary>
    public int Index => this.indexMinusOne + 1;

    /// <summary>
    /// 从 <seealso cref="Index"/> 获取地支。
    /// The a Dizhi from its <seealso cref="Index"/>.
    /// </summary>
    /// <param name="index">
    /// 序数。
    /// The index.
    /// </param>
    /// <returns>
    /// 地支。
    /// The Dizhi.
    /// </returns>
    public static Dizhi FromIndex(int index)
    {
        index %= 12;
        index += 12 - 1;
        return new Dizhi(index % 12);
    }
    #endregion

    #region comparing

    /// <inheritdoc/>
    public int CompareTo(Dizhi other)
    {
        return this.indexMinusOne.CompareTo(other.indexMinusOne);
    }

    /// <inheritdoc/>
    public bool Equals(Dizhi other)
    {
        return this.indexMinusOne.Equals(other.indexMinusOne);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not Dizhi other)
            return false;
        return this.indexMinusOne.Equals(other.indexMinusOne);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.indexMinusOne.GetHashCode();
    }

    /// <inheritdoc/>
    public static bool operator ==(Dizhi left, Dizhi right)
    {
        return left.indexMinusOne == right.indexMinusOne;
    }

    /// <inheritdoc/>
    public static bool operator !=(Dizhi left, Dizhi right)
    {
        return left.indexMinusOne != right.indexMinusOne;
    }
    #endregion

    #region values
    /// <summary>
    /// 子。
    /// Zi.
    /// </summary>
    public static Dizhi Zi => new Dizhi(0);
    /// <summary>
    /// 丑。
    /// Chou.
    /// </summary>
    public static Dizhi Chou => new Dizhi(1);
    /// <summary>
    /// 寅。
    /// Yin.
    /// </summary>
    public static Dizhi Yin => new Dizhi(2);
    /// <summary>
    /// 卯。
    /// Mao.
    /// </summary>
    public static Dizhi Mao => new Dizhi(3);
    /// <summary>
    /// 辰。
    /// Chen.
    /// </summary>
    public static Dizhi Chen => new Dizhi(4);
    /// <summary>
    /// 巳。
    /// Si.
    /// </summary>
    public static Dizhi Si => new Dizhi(5);
    /// <summary>
    /// 午。
    /// Wu.
    /// </summary>
    public static Dizhi Wu => new Dizhi(6);
    /// <summary>
    /// 未。
    /// Wei.
    /// </summary>
    public static Dizhi Wei => new Dizhi(7);
    /// <summary>
    /// 申。
    /// Shen.
    /// </summary>
    public static Dizhi Shen => new Dizhi(8);
    /// <summary>
    /// 酉。
    /// You.
    /// </summary>
    public static Dizhi You => new Dizhi(9);
    /// <summary>
    /// 戌。
    /// Xu.
    /// </summary>
    public static Dizhi Xu => new Dizhi(10);
    /// <summary>
    /// 亥。
    /// Hai.
    /// </summary>
    public static Dizhi Hai => new Dizhi(11);
    #endregion
}