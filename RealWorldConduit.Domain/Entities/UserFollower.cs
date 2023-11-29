using RealWorldConduit.Domain.Common;

namespace RealWorldConduit.Domain.Entities
{
    public class UserFollower : IAuditEntity
    {   
        public virtual User FollowedUser { get; set; }
        public Guid FollowedUserId { get; set; }
        public virtual User Follower { get; set; }
        public Guid FollowerId { get; set; }
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime LastUpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
