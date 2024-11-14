using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AYLogistics.BusinessRules
{
    public interface ISpecification<TEntity>
    {
        bool IsSatisfiedBy(TEntity entity);
    }

    internal class AndSpecification<TEntity> : ISpecification<TEntity>
    {
        private ISpecification<TEntity> spec1;
        private ISpecification<TEntity> spec2;

        internal AndSpecification(ISpecification<TEntity> s1, ISpecification<TEntity> s2)
        {
            spec1 = s1;
            spec2 = s2;
        }

        public bool IsSatisfiedBy(TEntity candidate)
        {
            return spec1.IsSatisfiedBy(candidate) && spec2.IsSatisfiedBy(candidate);
        }
    }

    internal class OrSpecification<TEntity> : ISpecification<TEntity>
    {
        private ISpecification<TEntity> spec1;
        private ISpecification<TEntity> spec2;

        internal OrSpecification(ISpecification<TEntity> s1, ISpecification<TEntity> s2)
        {
            spec1 = s1;
            spec2 = s2;
        }

        public bool IsSatisfiedBy(TEntity candidate)
        {
            return spec1.IsSatisfiedBy(candidate) || spec2.IsSatisfiedBy(candidate);
        }
    }

    internal class NotSpecification<TEntity> : ISpecification<TEntity>
    {
        private ISpecification<TEntity> spec;

        internal NotSpecification(ISpecification<TEntity> s)
        {
            spec = s;
        }

        public bool IsSatisfiedBy(TEntity candidate)
        {
            return !spec.IsSatisfiedBy(candidate);
        }
    }

    public static class ExtensionMethods
    {
        public static ISpecification<TEntity> And<TEntity>(this ISpecification<TEntity> s1, ISpecification<TEntity> s2)
        {
            return new AndSpecification<TEntity>(s1, s2);
        }

        public static ISpecification<TEntity> Or<TEntity>(this ISpecification<TEntity> s1, ISpecification<TEntity> s2)
        {
            return new OrSpecification<TEntity>(s1, s2);
        }

        public static ISpecification<TEntity> Not<TEntity>(this ISpecification<TEntity> s)
        {
            return new NotSpecification<TEntity>(s);
        }
    }
}