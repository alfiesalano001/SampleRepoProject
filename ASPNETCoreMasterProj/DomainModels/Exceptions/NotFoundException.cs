namespace DomainModels.Exceptions
{
    public class NotFoundException : GuardException
    {
        public NotFoundException(string message) : base(message) { }
    }
}
