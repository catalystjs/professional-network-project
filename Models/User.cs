// Common Libraries
using System;
using System.Collections.Generic;
// Validation Annotations
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
// ASP.NET Core Identity Libraries
using Microsoft.AspNetCore.Identity;


namespace beltexam2.Models
{
    public class User : BaseEntity
    {
        // Parameters for Model
        [Key]
        public int Id { get; private set; }
        [Display(Name = "First Name")]
        [Required]
        [RegularExpression(@"[a-zA-Z]+", ErrorMessage = "First Name can only comprise of letters")]
        [MinLength(2)]
        [MaxLength(100)]
        public string First_name { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        [RegularExpression(@"[a-zA-Z]+", ErrorMessage = "First Name can only comprise of letters")]
        [MinLength(2)]
        [MaxLength(100)]
        public string Last_name { get; set; }
        [Required]
        [EmailAddressAttribute]
        [MinLength(3)]
        [MaxLength(255)]
        [Display(Name = "Username")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public string Description { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(255)]
        [RegularExpression(@"[a-zA-Z0-9]+[\!\@\#\$\%]+", ErrorMessage = "Password must contain letters, numbers, and special characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Try Again!")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string Confirmpassword { get; set; }
        // Reverse references for FK's
        [InverseProperty("InvitationUser")]
        public ICollection<Invitation> Invitations { get; set; }
        [InverseProperty("NetworkUser")]
        public ICollection<Network> Networks { get; set; }
        public DateTime CreatedAt { get; set; }
        // Constructor for Model
        public User()
        {
            Invitations = new List<Invitation>();
            Networks = new List<Network>();
            CreatedAt = System.DateTime.Now;
        }
        // Methods for this Model
        public string name()
        {
            return $"{this.First_name} {this.Last_name}";
        }
        // String override
        public override string ToString()
        {
            return $"User Data: ID: {this.Id}, Name: {this.name()}, Email: {this.Email}";
        }
        // Hash the user password
        public void hash_password()
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            Password = Hasher.HashPassword(this, Password);
        }
        // Check the password against the hashed password
        public bool check_password(string password)
        {
            if (Password == null) { throw new Exception("String is required to check password"); }
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            return (0 != Hasher.VerifyHashedPassword(this, this.Password, password));
        }
        // Check if user in Networks
        public bool InNetworks(int referenceid)
        {
            Console.WriteLine("These are the networks the user has attached");
            Console.WriteLine(Networks);
            // Compound booleans
            bool NetworkUser = !Networks.Any(x => x.NetworkUserId == referenceid);
            bool NetworkRelatedUser = !Networks.Any(x => x.NetworkRelatedUserId == referenceid);
            // Return the evaluation
            return NetworkUser && NetworkRelatedUser;
        }
        public bool InInvitations(int referenceid)
        {
            Console.WriteLine("These are the invitations the user has attached");
            Console.WriteLine(Invitations);
            // Compound booleans
            bool InvitationUser = !Invitations.Any(x => x.InvitationUserId == referenceid);
            bool InvitationRelatedUser = !Invitations.Any(x => x.InvitationRelatedUserId == referenceid);
            // Return the evaluation
            return InvitationUser && InvitationRelatedUser;
        }
    }
}
