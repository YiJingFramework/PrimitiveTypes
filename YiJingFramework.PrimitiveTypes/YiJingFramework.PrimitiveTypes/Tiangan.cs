using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using YiJingFramework.PrimitiveTypes.Serialization;

namespace YiJingFramework.PrimitiveTypes;

/// <summary>
/// 天干。
/// Tiangan. (The Heavenly Stems.)
/// </summary>
[JsonConverter(typeof(JsonConverterOfStringConvertibleForJson<Tiangan>))]
public readonly struct Tiangan :
    IComparable<Tiangan>, IEquatable<Tiangan>, IFormattable,
    IParsable<Tiangan>, IEqualityOperators<Tiangan, Tiangan, bool>,
    IStringConvertibleForJson<Tiangan>,
    IAdditionOperators<Tiangan, int, Tiangan>,
    ISubtractionOperators<Tiangan, int, Tiangan>
{
    /// <summary>
    /// 天干的序数。
    /// 如以 <c>1</c> 对应甲。
    /// The index of the Tiangan.
    /// For example, <c>1</c> represents Jia.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// 初始化一个实例。
    /// Initialize an instance.
    /// </summary>
    /// <param name="index">
    /// 天干的序数。
    /// 如以 <c>1</c> 对应甲。
    /// The index of the Tiangan.
    /// For example, <c>1</c> represents Jia.
    /// </param>
    public Tiangan(int index)
    {
        this.Index = ((index - 1) % 10 + 10) % 10 + 1;
    }

    /// <summary>
    /// 获取此天干的前第 <paramref name="n"/> 个天干。
    /// Get the <paramref name="n"/>th Tiangan in front of this instance.
    /// 前，如甲前为乙。
    /// Jia is thought to be in front of Yi for example.
    /// </summary>
    /// <param name="n">
    /// 数字 <paramref name="n"/> 。
    /// The number <paramref name="n"/>.
    /// 可以小于零以表示另一个方向。
    /// It could be smaller than zero which means the other direction.
    /// </param>
    /// <returns>
    /// 指定天干。
    /// The Tiangan.
    /// </returns>
    public Tiangan Next(int n = 1)
    {
        return new Tiangan(this.Index + n);
    }

    /// <inheritdoc/>
    public static Tiangan operator +(Tiangan left, int right)
    {
        return left.Next(right);
    }

    /// <inheritdoc/>
    public static Tiangan operator -(Tiangan left, int right)
    {
        right = right % 10;
        return left.Next(-right);
    }

    #region converting
    /// <inheritdoc/>
    public override string ToString()
    {
        return this.Index switch {
            1 => "Jia",
            2 => "Yi",
            3 => "Bing",
            4 => "Ding",
            5 => "Wu",
            6 => "Ji",
            7 => "Geng",
            8 => "Xin",
            9 => "Ren",
            _ => "Gui" // 10 => "Gui"
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

        return format.ToUpperInvariant() switch {
            "G" => this.ToString(),
            "C" => this.Index switch {
                1 => "甲",
                2 => "乙",
                3 => "丙",
                4 => "丁",
                5 => "戊",
                6 => "己",
                7 => "庚",
                8 => "辛",
                9 => "壬",
                _ => "癸" // 10 => "癸"
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
            case "1":
                result = new Tiangan(1);
                return true;
            case "yi":
            case "乙":
            case "2":
                result = new Tiangan(2);
                return true;
            case "bing":
            case "丙":
            case "3":
                result = new Tiangan(3);
                return true;
            case "ding":
            case "丁":
            case "4":
                result = new Tiangan(4);
                return true;
            case "wu":
            case "戊":
            case "5":
                result = new Tiangan(5);
                return true;
            case "ji":
            case "己":
            case "6":
                result = new Tiangan(6);
                return true;
            case "geng":
            case "庚":
            case "7":
                result = new Tiangan(7);
                return true;
            case "xin":
            case "辛":
            case "8":
                result = new Tiangan(8);
                return true;
            case "ren":
            case "壬":
            case "9":
                result = new Tiangan(9);
                return true;
            case "gui":
            case "癸":
            case "10":
                result = new Tiangan(10);
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
        return tiangan.Index;
    }

    /// <inheritdoc/>
    public static explicit operator Tiangan(int value)
    {
        return new Tiangan(value);
    }
    #endregion

    #region comparing
    /// <inheritdoc/>
    public int CompareTo(Tiangan other)
    {
        return this.Index.CompareTo(other.Index);
    }

    /// <inheritdoc/>
    public bool Equals(Tiangan other)
    {
        return this.Index.Equals(other.Index);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not Tiangan other)
            return false;
        return this.Index.Equals(other.Index);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.Index.GetHashCode();
    }

    /// <inheritdoc/>
    public static bool operator ==(Tiangan left, Tiangan right)
    {
        return left.Index == right.Index;
    }

    /// <inheritdoc/>
    public static bool operator !=(Tiangan left, Tiangan right)
    {
        return left.Index != right.Index;
    }
    #endregion

    #region serializing
    static bool IStringConvertibleForJson<Tiangan>.FromStringForJson(string s, out Tiangan result)
    {
        return TryParse(s, out result);
    }

    string IStringConvertibleForJson<Tiangan>.ToStringForJson()
    {
        return this.ToString();
    }
    #endregion

    #region values
    /// <summary>
    /// 甲。
    /// Jia.
    /// </summary>
    public static Tiangan Jia => new Tiangan(1);
    /// <summary>
    /// 乙。
    /// Yi.
    /// </summary>
    public static Tiangan Yi => new Tiangan(2);
    /// <summary>
    /// 丙。
    /// Bing.
    /// </summary>
    public static Tiangan Bing => new Tiangan(3);
    /// <summary>
    /// 丁。
    /// Ding.
    /// </summary>
    public static Tiangan Ding => new Tiangan(4);
    /// <summary>
    /// 戊。
    /// Wu.
    /// </summary>
    public static Tiangan Wu => new Tiangan(5);
    /// <summary>
    /// 己。
    /// Ji.
    /// </summary>
    public static Tiangan Ji => new Tiangan(6);
    /// <summary>
    /// 庚。
    /// Geng.
    /// </summary>
    public static Tiangan Geng => new Tiangan(7);
    /// <summary>
    /// 辛。
    /// Xin.
    /// </summary>
    public static Tiangan Xin => new Tiangan(8);
    /// <summary>
    /// 壬。
    /// Ren.
    /// </summary>
    public static Tiangan Ren => new Tiangan(9);
    /// <summary>
    /// 癸。
    /// Gui.
    /// </summary>
    public static Tiangan Gui => new Tiangan(10);
    #endregion
}