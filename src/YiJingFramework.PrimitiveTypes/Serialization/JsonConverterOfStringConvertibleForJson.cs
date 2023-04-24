using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YiJingFramework.PrimitiveTypes.Serialization;

/// <summary>
/// 针对 <seealso cref="IStringConvertibleForJson{TSelf}"/> 的 <seealso cref="JsonConverter{T}"/> 。
/// A <seealso cref="JsonConverter{T}"/> for <seealso cref="IStringConvertibleForJson{TSelf}"/>.
/// </summary>
/// <typeparam name="T">
/// 针对的类型。
/// The targeted type. 
/// </typeparam>
public sealed class JsonConverterOfStringConvertibleForJson<T> : JsonConverter<T>
    where T : IStringConvertibleForJson<T>
{
    /// <inheritdoc/>
    public override bool HandleNull => false;

    /// <inheritdoc/>
    public override T Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException();

        var s = reader.GetString();
        Debug.Assert(s is not null);
        if (!T.FromStringForJson(s, out var result))
            throw new JsonException();
        return result;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToStringForJson());
    }
}
