namespace RealWorldConduit.Domain.Entities
{
    public class UserFollower : BaseEntity
    {
        public User FollowedUser { get; set; }
        public Guid FollowedUserId { get; set; }
        public User Follower { get; set; }
        public Guid FollowerId { get; set; }
    }
}
