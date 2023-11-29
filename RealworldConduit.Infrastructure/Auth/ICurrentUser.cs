namespace RealWorldConduit.Infrastructure.Auth
{
    public interface ICurrentUser
    {
        public Guid? Id { get; }
    }
}
