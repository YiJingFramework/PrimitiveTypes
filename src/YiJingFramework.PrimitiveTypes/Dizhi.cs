using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace YiJingFramework.PrimitiveTypes;

/// <summary>
/// 地支。
/// Dizhi. (The Earthly Branches.)
/// </summary>
public readonly struct Dizhi :
    IComparable<Dizhi>, IEquatable<Dizhi>, IFormattable,
    IParsable<Dizhi>, IEqualityOperators<Dizhi, Dizhi, bool>,
    IAdditionOperators<Dizhi, int, Dizhi>,
    ISubtractionOperators<Dizhi, int, Dizhi>
{
    /// <summary>
    /// 地支的序数。
    /// 如以 <c>1</c> 对应子。
    /// The index of the Dizhi.
    /// For example, <c>1</c> represents Zi.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// 初始化一个实例。
    /// Initialize an instance.
    /// </summary>
    /// <param name="index">
    /// 地支的序数。
    /// 如以 <c>1</c> 对应子。
    /// The index of the Dizhi.
    /// For example, <c>1</c> represents Zi.
    /// </param>
    public Dizhi(int index)
    {
        this.Index = ((index - 1) % 12 + 12) % 12 + 1;
    }

    /// <summary>
    /// 获取此地支的前第 <paramref name="n"/> 个地支。
    /// Get the <paramref name="n"/>th Dizhi in front of this instance.
    /// 前，如子前为丑。
    /// Chou is thought to be in front of Zi for example.
    /// </summary>
    /// <param name="n">
    /// 数字 <paramref name="n"/> 。
    /// The number <paramref name="n"/>.
    /// 可以小于零以表示另一个方向。
    /// It could be smaller than zero which means the other direction.
    /// </param>
    /// <returns>
    /// 指定地支。
    /// The Dizhi.
    /// </returns>
    public Dizhi Next(int n = 1)
    {
        return new Dizhi(this.Index + n);
    }

    /// <inheritdoc/>
    public static Dizhi operator +(Dizhi left, int right)
    {
        return left.Next(right);
    }

    /// <inheritdoc/>
    public static Dizhi operator -(Dizhi left, int right)
    {
        right = right % 12;
        return left.Next(-right);
    }

    #region converting

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.Index switch
        {
            1 => "Zi",
            2 => "Chou",
            3 => "Yin",
            4 => "Mao",
            5 => "Chen",
            6 => "Si",
            7 => "Wu",
            8 => "Wei",
            9 => "Shen",
            10 => "You",
            11 => "Xu",
            _ => "Hai" // 12 => "Hai"
        };
    }

    /// <summary>
    /// 按照指定格式转换为字符串。
    /// Convert to a string with the given format.
    /// </summary>
    /// <param name="format">
    /// 要使用的格式。
    /// The format to be used.
    /// <c>"G"</c> 表示拼音字母； <c>"C"</c> 表示中文； <c>"N"</c> 表示数字。
    /// <c>"G"</c> represents the phonetic alphabets; <c>"C"</c> represents chinese characters; and <c>"N"</c> represents numbers.
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
            "C" => this.Index switch
            {
                1 => "子",
                2 => "丑",
                3 => "寅",
                4 => "卯",
                5 => "辰",
                6 => "巳",
                7 => "午",
                8 => "未",
                9 => "申",
                10 => "酉",
                11 => "戌",
                _ => "亥" // 12 => "亥"
            },
            "N" => this.Index.ToString(),
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
            case "1":
                result = new Dizhi(1);
                return true;
            case "chou":
            case "丑":
            case "2":
                result = new Dizhi(2);
                return true;
            case "yin":
            case "寅":
            case "3":
                result = new Dizhi(3);
                return true;
            case "mao":
            case "卯":
            case "4":
                result = new Dizhi(4);
                return true;
            case "chen":
            case "辰":
            case "5":
                result = new Dizhi(5);
                return true;
            case "si":
            case "巳":
            case "6":
                result = new Dizhi(6);
                return true;
            case "wu":
            case "午":
            case "7":
                result = new Dizhi(7);
                return true;
            case "wei":
            case "未":
            case "8":
                result = new Dizhi(8);
                return true;
            case "shen":
            case "申":
            case "9":
                result = new Dizhi(9);
                return true;
            case "you":
            case "酉":
            case "10":
                result = new Dizhi(10);
                return true;
            case "xu":
            case "戌":
            case "11":
                result = new Dizhi(11);
                return true;
            case "hai":
            case "亥":
            case "12":
                result = new Dizhi(12);
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
        return dizhi.Index;
    }


    /// <inheritdoc/>
    public static explicit operator Dizhi(int value)
    {
        return new Dizhi(value);
    }
    #endregion

    #region comparing

    /// <inheritdoc/>
    public int CompareTo(Dizhi other)
    {
        return this.Index.CompareTo(other.Index);
    }

    /// <inheritdoc/>
    public bool Equals(Dizhi other)
    {
        return this.Index.Equals(other.Index);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not Dizhi other)
            return false;
        return this.Index.Equals(other.Index);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.Index.GetHashCode();
    }

    /// <inheritdoc/>
    public static bool operator ==(Dizhi left, Dizhi right)
    {
        return left.Index == right.Index;
    }

    /// <inheritdoc/>
    public static bool operator !=(Dizhi left, Dizhi right)
    {
        return left.Index != right.Index;
    }
    #endregion

    #region values
    /// <summary>
    /// 子。
    /// Zi.
    /// </summary>
    public static Dizhi Zi => new Dizhi(1);
    /// <summary>
    /// 丑。
    /// Chou.
    /// </summary>
    public static Dizhi Chou => new Dizhi(2);
    /// <summary>
    /// 寅。
    /// Yin.
    /// </summary>
    public static Dizhi Yin => new Dizhi(3);
    /// <summary>
    /// 卯。
    /// Mao.
    /// </summary>
    public static Dizhi Mao => new Dizhi(4);
    /// <summary>
    /// 辰。
    /// Chen.
    /// </summary>
    public static Dizhi Chen => new Dizhi(5);
    /// <summary>
    /// 巳。
    /// Si.
    /// </summary>
    public static Dizhi Si => new Dizhi(6);
    /// <summary>
    /// 午。
    /// Wu.
    /// </summary>
    public static Dizhi Wu => new Dizhi(7);
    /// <summary>
    /// 未。
    /// Wei.
    /// </summary>
    public static Dizhi Wei => new Dizhi(8);
    /// <summary>
    /// 申。
    /// Shen.
    /// </summary>
    public static Dizhi Shen => new Dizhi(9);
    /// <summary>
    /// 酉。
    /// You.
    /// </summary>
    public static Dizhi You => new Dizhi(10);
    /// <summary>
    /// 戌。
    /// Xu.
    /// </summary>
    public static Dizhi Xu => new Dizhi(11);
    /// <summary>
    /// 亥。
    /// Hai.
    /// </summary>
    public static Dizhi Hai => new Dizhi(12);
    #endregion
}