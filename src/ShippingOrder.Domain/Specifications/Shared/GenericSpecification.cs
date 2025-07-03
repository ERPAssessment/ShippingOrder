using System.Linq.Expressions;

namespace ShippingOrder.Domain.Specifications.Shared;
public class GenericSpecification<T>
{
  public Expression<Func<T, bool>> Expression { get; }

  public GenericSpecification(Expression<Func<T, bool>> expression)
  {
    Expression = expression;
  }

  public bool IsSatisfiedBy(T entity)
  {
    return Expression.Compile().Invoke(entity);
  }
}
