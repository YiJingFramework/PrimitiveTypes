using System.Diagnostics.CodeAnalysis;

namespace YiJingFramework.PrimitiveTypes.Serialization;

/// <summary>
/// 可以通过转化为字符串的方式来进行 json 序列化。
/// Could be serialized to json by being converted to a string value.
/// </summary>
/// <typeparam name="TSelf">
/// 实现此接口的类型本身。
/// The type that implements this interface.
/// </typeparam>
public interface IStringConvertibleForJson<TSelf> where TSelf : IStringConvertibleForJson<TSelf>
{
    /// <summary>
    /// 返回实例的等效字符串，从而进行后续的 json 序列化。
    /// 应该和 <seealso cref="FromStringForJson(string, out TSelf)"/> 的实现相匹配。
    /// Returns an equivalent string of this instance for json serialization.
    /// Should be matched with the implementation of <seealso cref="FromStringForJson(string, out TSelf)"/>.
    /// </summary>
    /// <returns>
    /// 等效字符串。
    /// The equivalent string.
    /// </returns>
    string ToStringForJson();

    /// <summary>
    /// 在 json 反序列化时，尝试通过字符串创建一个此类型的实例。
    /// 应该和 <seealso cref="ToStringForJson"/> 的实现相匹配。
    /// Create an instance of this type from a string during json deserialization.
    /// Should be matched with the implementation of <seealso cref="ToStringForJson"/>.
    /// </summary>
    /// <param name="s">
    /// 给定的字符串。
    /// The given string.
    /// </param>
    /// <param name="result">
    /// 结果。
    /// 如果失败，可以为任意值。
    /// The result.
    /// Could be any value if it failed.
    /// </param>
    /// <returns>
    /// 指示转换是否成功。
    /// Indicates whether the conversion has successed.
    /// </returns>
    static abstract bool FromStringForJson(string s, [MaybeNullWhen(false)] out TSelf result);
}