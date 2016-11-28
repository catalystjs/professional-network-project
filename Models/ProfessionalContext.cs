// Common Libraries
using System;
using System.Collections.Generic;
// LINQ Libraries
using System.Linq;
// ASP.NET Entity Framework
using Microsoft.EntityFrameworkCore;

namespace beltexam2.Models
{
    public class ProfessionalContext : DbContext
    {
        public ProfessionalContext(DbContextOptions<ProfessionalContext> options) : base(options) { }
        // These are the tables
        public DbSet<User> Users { get; set; }
        public DbSet<Network> Networks { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        // Override the SaveChanges to handle CreatedAt and UpdatedAt
        public override int SaveChanges()
        {
            // update entities that are tracked and inherit from BaseEntity (UpdatedAt Property)
            var trackables = ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Modified);
            if (trackables != null)
            {
                // added and modified changes only
                foreach (var item in trackables)
                {
                    item.Entity.UpdatedAt = System.DateTime.Now;
                }
            }
            // Return the base SaveChanges
            return base.SaveChanges();
        }
        // Full populate methods for DRY
        public User PopulateUserSingle(int userid)
        {
            return Users.Where(x => x.Id == userid).Include(x => x.Networks).ThenInclude(x => x.NetworkRelatedUser).Include(x => x.Invitations).ThenInclude(x => x.InvitationRelatedUser).SingleOrDefault();
        }
        public ICollection<User> PopulateUsersAll()
        {
            return Users.Include(x => x.Networks).ThenInclude(x => x.NetworkRelatedUser).Include(x => x.Invitations).ThenInclude(x => x.InvitationRelatedUser).ToList();
        }
        public ICollection<User> PopulateUsersAllNotAssociated(int userid)
        {
            // Get the current user
            User currentuser = PopulateUserSingle(userid);
            // Get the users
            var users = PopulateUsersAll();
            // Create the new reduced list of users
            List<User> reducedusers = new List<User>();
            // Iterate through the users
            foreach (var user in users)
            {
                // Compound booleans
                bool InRelationships = user.InNetworks(currentuser.Id) && user.InInvitations(currentuser.Id);
                // LINQ queries based on Model lookups
                if (InRelationships && currentuser.InInvitations(user.Id) && user.Id != currentuser.Id)
                {
                    reducedusers.Add(user);
                }
            }
            // Return the reduced users
            return reducedusers;
        }
    }
}