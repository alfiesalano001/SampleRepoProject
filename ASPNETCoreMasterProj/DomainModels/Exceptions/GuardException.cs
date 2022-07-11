using System;

namespace DomainModels.Exceptions
{
    public class GuardException : Exception
    {
        public GuardException(string errorMessage) : base(errorMessage ?? throw new ArgumentException(nameof(errorMessage))) { }
    }
}
