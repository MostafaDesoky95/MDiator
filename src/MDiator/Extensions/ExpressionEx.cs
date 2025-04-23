using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MDiator.Extensions
{
    public static class ExpressionEx
    {
        public static Expression ForEach(Expression collection, ParameterExpression loopVar, Expression loopContent)
        {
            var elementType = loopVar.Type;
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);
            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(elementType);

            var enumeratorVar = Expression.Variable(enumeratorType, "enumerator");

            var getEnumeratorCall = Expression.Call(collection, enumerableType.GetMethod("GetEnumerator")!);
            var moveNextCall = Expression.Call(enumeratorVar, typeof(IEnumerator).GetMethod("MoveNext")!);
            var breakLabel = Expression.Label("LoopBreak");

            var block = Expression.Block(
                new[] { enumeratorVar },
                Expression.Assign(enumeratorVar, getEnumeratorCall),
                Expression.TryFinally(
                    Expression.Loop(
                        Expression.IfThenElse(
                            Expression.IsFalse(moveNextCall),
                            Expression.Break(breakLabel),
                            Expression.Block(
                                new[] { loopVar },
                                Expression.Assign(loopVar, Expression.Property(enumeratorVar, "Current")),
                                loopContent
                            )
                        ),
                        breakLabel
                    ),
                    Expression.Call(enumeratorVar, typeof(IDisposable).GetMethod("Dispose")!)
                )
            );

            return block;
        }
    }
}
