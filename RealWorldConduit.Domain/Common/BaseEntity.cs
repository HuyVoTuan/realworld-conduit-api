using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealWorldConduit.Domain.Common
{
    public class BaseEntity<T> : IAuditEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime LastUpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
