namespace RealWorldConduit.Domain.Entities
{
    public class RefreshToken : BaseEntity<Guid>
    {
        public virtual User User { get; set; }
        public Guid UserId { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
