using System.Collections;

namespace System.Linq.Expressions
{
    internal static class ExpressionExentions
    {
        public static Expression ForEach(this Expression collectionExpression, ParameterExpression loopParameterExpression, Expression loopBodyExpression)
        {
            ArgumentNullException.ThrowIfNull(collectionExpression);
            ArgumentNullException.ThrowIfNull(loopParameterExpression);
            ArgumentNullException.ThrowIfNull(loopBodyExpression);

            var enumerableType = typeof(IEnumerable<>).MakeGenericType(loopParameterExpression.Type);
            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(loopParameterExpression.Type);
            var enumeratorVariableExpression = Expression.Variable(enumeratorType, "enumerator");

            var getEnumeratorCall = Expression.Call(collectionExpression, enumerableType.GetMethod(nameof(IEnumerable.GetEnumerator))!);
            var moveNextCall = Expression.Call(enumeratorVariableExpression, typeof(IEnumerator).GetMethod(nameof(IEnumerator.MoveNext))!);
            var breakLabel = Expression.Label("LoopBreak");

            var block = Expression.Block(
                [enumeratorVariableExpression],
                Expression.Assign(enumeratorVariableExpression, getEnumeratorCall),
                Expression.TryFinally(
                    Expression.Loop(
                        Expression.IfThenElse(
                            Expression.IsFalse(moveNextCall),
                            Expression.Break(breakLabel),
                            Expression.Block(
                                [loopParameterExpression],
                                Expression.Assign(loopParameterExpression, Expression.Property(enumeratorVariableExpression, nameof(IEnumerator.Current))),
                                loopBodyExpression
                            )
                        ),
                        breakLabel
                    ),
                    Expression.Call(enumeratorVariableExpression, typeof(IDisposable).GetMethod(nameof(IDisposable.Dispose))!)
                )
            );

            return block;
        }
    }
}
