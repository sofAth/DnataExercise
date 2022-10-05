using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DnataExercise.Common.Infrastructure.Extensions {
    public static class EnumerableExtensions {
        public static Expression<Func<T, bool>> AnyOf<T>(params Expression<Func<T, bool>>[] expressions) {
            // Note: I admit I used the internet to resource this method; however, I needed a way to build lambda expressions progressively
            // in order to provide flexibility in the way the web api sends queries for movies. My initial thought was to have an 
            // argument on the controller which receives an "Expression" object with all the queries specified in it. However, this object
            // is not easily serialized which led me back to having to separate the filter conditions on the controller
            if (expressions == null || expressions.Length == 0) return x => false;
            if (expressions.Length == 1) return expressions[0];

            var body = expressions[0].Body;
            var param = expressions[0].Parameters.Single();
            for (int i = 1; i < expressions.Length; i++) {
                var expr = expressions[i];
                var swappedParam = new SwapVisitor(expr.Parameters.Single(), param).Visit(expr.Body);
                body = Expression.OrElse(body, swappedParam);
            }
            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        class SwapVisitor : ExpressionVisitor {
            private readonly Expression from, to;
            public SwapVisitor(Expression from, Expression to) {
                this.from = from;
                this.to = to;
            }
            public override Expression Visit(Expression node) {
                return node == from ? to : base.Visit(node);
            }
        }
    }
}
