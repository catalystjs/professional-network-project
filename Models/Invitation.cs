// Common Libraries
using System;
// Validation Annotations
using System.ComponentModel.DataAnnotations;

namespace beltexam2.Models
{
    public class Invitation : BaseEntity
    {
        [Key]
        public int Id { get; set;}
        public int InvitationUserId { get; set; }
        public User InvitationUser { get; set; }
        public int InvitationRelatedUserId { get; set; }
        public User InvitationRelatedUser { get; set; }
        public DateTime CreatedAt { set; get; }
        // This is the constructor
        public Invitation()
        {
            CreatedAt = System.DateTime.Now;
        }
        // String override
        public override string ToString()
        {
            return $"Invitation Data: ID: {this.Id} User ID: {this.InvitationUserId} Related User ID: {this.InvitationRelatedUserId}";
        }
    }
}