namespace RealWorldCondui.Infrastructure.Auth
{
    public interface ICurrentUser
    {
        public Guid? Id { get; }
    }
}
