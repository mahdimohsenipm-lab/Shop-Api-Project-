using Entites.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entites.Users
{
    public class User : IdentityUser<int>, IEntity
    {

        public int Age { get; set; }

        public string FullName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset LoginTime { get; set; }

        public bool IsStatic { get; set; }

        public ICollection<Role> UserRole { get; set; } = new List<Role>();



        public void Configure(EntityTypeBuilder<User> builder)

        {
            builder.Property(u => u.Age).IsRequired();
            builder.Property(u => u.IsActive).HasDefaultValue(true);
        }


    }
}
