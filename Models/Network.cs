// Common Libraries
using System;
// Validation Annotations
using System.ComponentModel.DataAnnotations;

namespace beltexam2.Models
{
    public class Network : BaseEntity
    {
        [Key]
        public int Id { get; set;}
        public int NetworkUserId {get; set; }
        public User NetworkUser { get; set; }
        public int NetworkRelatedUserId { get; set; }
        public User NetworkRelatedUser { get; set; }
        public DateTime CreatedAt { set; get; }
        // This is the constructor
        public Network()
        {
            CreatedAt = System.DateTime.Now;
        }
        // String override
        public override string ToString()
        {
            return $"Network Data: ID: {this.Id} User ID: {this.NetworkUserId} Related User ID: {this.NetworkRelatedUserId}";
        }
    }
}