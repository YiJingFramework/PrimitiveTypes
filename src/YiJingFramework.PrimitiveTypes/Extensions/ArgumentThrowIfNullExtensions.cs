using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace YiJingFramework.PrimitiveTypes.Extensions;
internal static class ArgumentThrowIfNullExtensions
{
    public static T ThrowIfNull<T>(
        [NotNull] this T? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(argument, paramName);
        return argument;
    }
}
