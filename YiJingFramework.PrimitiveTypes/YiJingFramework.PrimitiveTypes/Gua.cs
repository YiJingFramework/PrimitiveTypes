using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using YiJingFramework.PrimitiveTypes.Serialization;

namespace YiJingFramework.PrimitiveTypes
{
    /// <summary>
    /// 卦。
    /// 爻位置越低，序号越小。
    /// A Gua, which is made up by the yin and yang lines (like trigrams and hexagrams).
    /// The lower a line, the smaller its index.
    /// </summary>
    [JsonConverter(typeof(JsonConverterOfStringConvertibleForJson<Gua>))]
    public sealed class Gua :
        IReadOnlyList<Yinyang>, IComparable<Gua>, IEquatable<Gua>,
        IParsable<Gua>, IEqualityOperators<Gua, Gua, bool>,
        IStringConvertibleForJson<Gua>
    {
        private readonly Yinyang[] lines;
        /// <summary>
        /// 创建新实例。
        /// Initializes a new instance.
        /// </summary>
        /// <param name="lines">
        /// 各爻的性质。
        /// The lines' attributes.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lines"/> 是 <c>null</c> 。
        /// <paramref name="lines"/> is <c>null</c>.
        /// </exception>
        public Gua(params Yinyang[] lines)
            : this((IEnumerable<Yinyang>)lines) { }

        /// <summary>
        /// 创建新实例。
        /// Initializes a new instance.
        /// </summary>
        /// <param name="lines">
        /// 各爻的性质。
        /// The lines' attributes.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lines"/> 是 <c>null</c> 。
        /// <paramref name="lines"/> is <c>null</c>.
        /// </exception>
        public Gua(IEnumerable<Yinyang> lines)
        {
            ArgumentNullException.ThrowIfNull(lines);
            this.lines = lines.ToArray();
        }

        #region Collecting
        /// <summary>
        /// 获取某一根爻的性质。
        /// Get the attribute of a line.
        /// </summary>
        /// <param name="index">
        /// 爻的序号。
        /// The index of the line.
        /// </param>
        /// <returns>
        /// 爻的性质。
        /// The line.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="index"/> 超出范围。
        /// <paramref name="index"/> is out of range.
        /// </exception>
        public Yinyang this[int index]
            => this.lines[index];

        /// <summary>
        /// 获取爻的个数。
        /// Get the count of the lines.
        /// </summary>
        public int Count
            => this.lines.Length;

        /// <inheritdoc/>
        public IEnumerator<Yinyang> GetEnumerator()
        {
            return ((IEnumerable<Yinyang>)this.lines).GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.lines.GetEnumerator();
        }
        #endregion

        #region Comparing

        /// <inheritdoc/>
        public int CompareTo(Gua? other)
        {
            if (other is null)
                return 1;

            var thisLength = this.lines.Length;
            {
                var otherLength = other.lines.Length;
                if (thisLength > otherLength)
                    return 1;
                else if (thisLength < otherLength)
                    return -1;
            }

            for (int i = thisLength - 1; i >= 0; i--)
            {
                var cur = this.lines[i];
                var com = other.lines[i];

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
                foreach (var line in this.lines)
                {
                    result <<= 1;
                    result += (int)line;
                }
                return result;
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object? other)
        {
            if (other is Gua gua)
                return this.lines.SequenceEqual(gua.lines);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(Gua? other)
        {
            if (other is null)
                return false;
            return this.lines.SequenceEqual(other.lines);
        }

        /// <inheritdoc/>
        public static bool operator ==(Gua? left, Gua? right)
        {
            if (left is null)
                return right is null;
            if (right is null)
                return false;
            return left.lines.SequenceEqual(right.lines);
        }

        /// <inheritdoc/>
        public static bool operator !=(Gua? left, Gua? right)
        {
            if (left is null)
                return right is not null;
            if (right is null)
                return true;
            return !left.lines.SequenceEqual(right.lines);
        }
        #endregion

        #region Converting
        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder stringBuilder = new(this.lines.Length);
            foreach (var line in this.lines)
                _ = stringBuilder.Append((int)line);
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

            Yinyang yin = Yinyang.Yin;
            Yinyang yang = Yinyang.Yang;

            List<Yinyang> r = new(s.Length);
            foreach (var c in s)
            {
                r.Add(c switch {
                    '0' => yin,
                    '1' => yang,
                    _ => throw new FormatException($"Cannot parse \"{s}\" as {nameof(Gua)}.")
                });
            }
            return new(r);
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

            Yinyang yin = Yinyang.Yin;
            Yinyang yang = Yinyang.Yang;

            List<Yinyang> r = new(s.Length);
            foreach (var c in s)
            {
                switch (c)
                {
                    case '0':
                        r.Add(yin);
                        break;
                    case '1':
                        r.Add(yang);
                        break;
                    default:
                        result = null;
                        return false;
                }
            }
            result = new Gua(r);
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
            var thisLength = this.lines.Length;
            BitArray bitArray = new(thisLength + 1);
            for (int i = 0; i < thisLength; i++)
                bitArray.Set(i, (bool)this.lines[i]);
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

            Yinyang[] r = new Yinyang[position];
            for (position--; position >= 0; position--)
                r[position] = new Yinyang(bitArray[position]);

            return new Gua(r);
        }
        #endregion

        #region Serializing
        static bool IStringConvertibleForJson<Gua>.FromStringForJson(
            string s, [MaybeNullWhen(false)] out Gua result)
        {
            return TryParse(s, out result);
        }

        string IStringConvertibleForJson<Gua>.ToStringForJson()
        {
            return this.ToString();
        }
        #endregion
    }
}
