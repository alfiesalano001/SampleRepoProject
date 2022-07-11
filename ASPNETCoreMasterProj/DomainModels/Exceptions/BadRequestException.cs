namespace DomainModels.Exceptions
{
    public sealed class BadRequestException : GuardException
    {
        public BadRequestException(string errorMessage) : base(errorMessage) { }
    }
}
