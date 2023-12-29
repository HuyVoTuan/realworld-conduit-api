namespace RealWorldConduit.Domain.Entities
{
    public class RefreshToken : BaseEntity<Guid>
    {
        public User User { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
