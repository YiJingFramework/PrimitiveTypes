using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using YiJingFramework.PrimitiveTypes.Extensions;

namespace YiJingFramework.PrimitiveTypes;

/// <summary>
/// 卦。
/// 爻位置越低，序号越小。
/// Gua.
/// The lower the position of a Yao, the smaller its index.
/// </summary>
/// <param name="yaos">
/// 各爻的性质。
/// The Yaos' attributes.
/// </param>
public sealed class Gua(ImmutableArray<Yinyang> yaos) :
    IReadOnlyList<Yinyang>, IComparable<Gua>, IEquatable<Gua>,
    IParsable<Gua>, IEqualityOperators<Gua, Gua, bool>,
    IBitwiseOperators<Gua, Gua, Gua>
{
    private readonly ImmutableArray<Yinyang> yaos = yaos;

    /// <inheritdoc cref="Gua(ImmutableArray{Yinyang})" />
    /// <exception cref="ArgumentNullException"></exception>
    public Gua(params Yinyang[] yaos)
        : this(yaos.ThrowIfNull().ToImmutableArray()) { }

    /// <inheritdoc cref="Gua(ImmutableArray{Yinyang})" />
    public Gua(IEnumerable<Yinyang> yaos)
        : this(yaos.ThrowIfNull().ToImmutableArray()) { }
    #region Collecting
    /// <summary>
    /// 获取某一根爻的性质。
    /// Get the attribute of a Yao.
    /// </summary>
    /// <param name="index">
    /// 爻的序号。
    /// The index of the Yao.
    /// </param>
    /// <returns>
    /// 爻的性质。
    /// The Yao's attribute.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="index"/> 超出范围。
    /// <paramref name="index"/> is out of range.
    /// </exception>
    public Yinyang this[int index] => this.yaos[index];

    /// <summary>
    /// 获取爻的个数。
    /// Get the count of the Yaos.
    /// </summary>
    public int Count => this.yaos.Length;

    /// <inheritdoc/>
    public IEnumerator<Yinyang> GetEnumerator()
    {
        return ((IEnumerable<Yinyang>)this.yaos).GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)this.yaos).GetEnumerator();
    }
    #endregion

    #region Comparing

    /// <inheritdoc/>
    public int CompareTo(Gua? other)
    {
        if (other is null)
            return 1;

        var thisLength = this.yaos.Length;
        {
            var otherLength = other.yaos.Length;
            if (thisLength > otherLength)
                return 1;
            else if (thisLength < otherLength)
                return -1;
        }

        for (int i = thisLength - 1; i >= 0; i--)
        {
            var cur = this.yaos[i];
            var com = other.yaos[i];

            var cr = cur.CompareTo(com);
            if (cr != 0)
                return cr;
        }

        return 0;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            int result = 1;
            foreach (var yao in this.yaos)
            {
                result <<= 1;
                result += (int)yao;
            }
            return result;
        }
    }

    /// <inheritdoc/>
    public override bool Equals(object? other)
    {
        if (other is Gua gua)
            return this.yaos.SequenceEqual(gua.yaos);
        return false;
    }

    /// <inheritdoc/>
    public bool Equals(Gua? other)
    {
        if (other is null)
            return false;
        return this.yaos.SequenceEqual(other.yaos);
    }

    /// <inheritdoc/>
    public static bool operator ==(Gua? left, Gua? right)
    {
        if (left is null)
            return right is null;
        if (right is null)
            return false;
        return left.yaos.SequenceEqual(right.yaos);
    }

    /// <inheritdoc/>
    public static bool operator !=(Gua? left, Gua? right)
    {
        if (left is null)
            return right is not null;
        if (right is null)
            return true;
        return !left.yaos.SequenceEqual(right.yaos);
    }
    #endregion

    #region Converting
    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder stringBuilder = new(this.yaos.Length);
        foreach (var yao in this.yaos)
            _ = stringBuilder.Append((int)yao);
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 从字符串转回。
    /// Convert from a string.
    /// </summary>
    /// <param name="s">
    /// 可以表示此卦的字符串。
    /// The string that represents the Gua.
    /// </param>
    /// <returns>
    /// 卦。
    /// The Gua.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> 是 <c>null</c> 。
    /// <paramref name="s"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="FormatException">
    /// 传入字符串的格式不受支持。
    /// The input string was not in the supported format.
    /// </exception>
    public static Gua Parse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        s = s.Trim();
        var r = ImmutableArray.CreateBuilder<Yinyang>(s.Length);
        foreach (var c in s)
        {
            r.Add(c switch
            {
                '0' => Yinyang.Yin,
                '1' => Yinyang.Yang,
                _ => throw new FormatException($"Cannot parse \"{s}\" as {nameof(Gua)}.")
            });
        }
        return new(r.MoveToImmutable());
    }

    /// <summary>
    /// 从字符串转回。
    /// Convert from a string.
    /// </summary>
    /// <param name="s">
    /// 可以表示此卦的字符串。
    /// The string that represents the Gua.
    /// </param>
    /// <param name="result">
    /// 卦。
    /// The Gua.
    /// </param>
    /// <returns>
    /// 一个指示转换成功与否的值。
    /// A value indicates whether it has been successfully converted or not.
    /// </returns>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        [MaybeNullWhen(false)] out Gua result)
    {
        if (s is null)
        {
            result = null;
            return false;
        }

        s = s.Trim();
        var r = ImmutableArray.CreateBuilder<Yinyang>(s.Length);
        foreach (var c in s)
        {
            switch (c)
            {
                case '0':
                    r.Add(Yinyang.Yin);
                    break;
                case '1':
                    r.Add(Yinyang.Yang);
                    break;
                default:
                    result = null;
                    return false;
            }
        }
        result = new Gua(r.MoveToImmutable());
        return true;
    }

    static Gua IParsable<Gua>.Parse(
        string s, IFormatProvider? provider)
    {
        return Parse(s);
    }

    static bool IParsable<Gua>.TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Gua result)
    {
        return TryParse(s, out result);
    }

    /// <summary>
    /// 返回一个可以完全表示此卦的字节数组。
    /// 可以使用 <seealso cref="FromBytes(byte[])"/> 转换回。
    /// Returns a byte array that can completely represents the Gua.
    /// You can use <seealso cref="FromBytes(byte[])"/> to convert it back.
    /// </summary>
    /// <returns>
    /// 结果字节数组。
    /// The array.
    /// </returns>
    public byte[] ToBytes()
    {
        var thisLength = this.yaos.Length;
        BitArray bitArray = new(thisLength + 1);
        for (int i = 0; i < thisLength; i++)
            bitArray.Set(i, (bool)this.yaos[i]);
        bitArray.Set(thisLength, true);
        byte[] bytes = new byte[(bitArray.Length + 7) / 8];
        bitArray.CopyTo(bytes, 0);
        return bytes;
    }

    /// <summary>
    /// 从字节数组转回。
    /// Convert from a byte array.
    /// </summary>
    /// <param name="bytes">
    /// 可以表示此卦的字节数组。
    /// The byte array that represents the Gua.
    /// </param>
    /// <returns>
    /// 卦。
    /// The Gua.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> 是 <c>null</c> 。
    /// <paramref name="bytes"/> is <c>null</c>.
    /// </exception>
    public static Gua FromBytes(params byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);

        BitArray bitArray = new(bytes);

        int position = bitArray.Length - 1;
        for (; position >= 0; position--)
        {
            if (bitArray[position])
                break;
        }

        var r = ImmutableArray.CreateBuilder<Yinyang>(position);
        for (int i = 0; i < position; i++)
            r.Add(new(bitArray[i]));
        return new Gua(r);
    }
    #endregion

    #region calculating
    /// <exception cref="ArithmeticException">
    /// 两个卦的爻数不同。
    /// Count of the two Guas are not equal.
    /// </exception>
    /// <inheritdoc/>
    public static Gua operator &(Gua left, Gua right)
    {
        static IEnumerable<Yinyang> Calculate(Gua g1, Gua g2)
        {
            foreach (var (y1, y2) in g1.Zip(g2))
                yield return y1 & y2;
        }
        if (left.Count != right.Count)
            throw new ArithmeticException(
                $"Cannot calculate {left} & {right} " +
                $"because their count are not the same.");
        return new Gua(Calculate(left, right));
    }

    /// <exception cref="ArithmeticException">
    /// 两个卦的爻数不同。
    /// Count of the two Guas are not equal.
    /// </exception>
    /// <inheritdoc />
    public static Gua operator |(Gua left, Gua right)
    {
        static IEnumerable<Yinyang> Calculate(Gua g1, Gua g2)
        {
            foreach (var (y1, y2) in g1.Zip(g2))
                yield return y1 | y2;
        }
        if (left.Count != right.Count)
            throw new ArithmeticException(
                $"Cannot calculate {left} | {right} " +
                $"because their count are not the same.");
        return new Gua(Calculate(left, right));
    }

    /// <exception cref="ArithmeticException">
    /// 两个卦的爻数不同。
    /// Count of the two Guas are not equal.
    /// </exception>
    /// <inheritdoc />
    public static Gua operator ^(Gua left, Gua right)
    {
        static IEnumerable<Yinyang> Calculate(Gua g1, Gua g2)
        {
            foreach (var (y1, y2) in g1.Zip(g2))
                yield return y1 ^ y2;
        }
        if (left.Count != right.Count)
            throw new ArithmeticException(
                $"Cannot calculate {left} ^ {right} " +
                $"because their count are not the same.");
        return new Gua(Calculate(left, right));
    }

    /// <inheritdoc />
    public static Gua operator ~(Gua gua)
    {
        static IEnumerable<Yinyang> Calculate(Gua g)
        {
            foreach (var y in g)
                yield return !y;
        }
        return new Gua(Calculate(gua));
    }
    #endregion
}
