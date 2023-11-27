namespace RealWorldConduit.Domain.Common
{
    public interface IAuditEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
