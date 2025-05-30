using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {


        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //seeding some roles(User,Admin,SuperAdmin)

            var adminRoleId = "86e3b599-23f8-4975-ace9-747acc7477b2";
            var superAdminRoleId = "5f55c41c-31b9-47f1-bb29-2dbe90cd0181";
            var userRoleId = "ab6b456b-8cbf-4d29-9fe2-cf066348d027";

            //We create the three roles (admin.superadmin,user)
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                 new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuoerAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                  new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id =userRoleId,
                    ConcurrencyStamp =userRoleId
                }


            };
            //this line says the EF core to seed the identity role to the database....
             builder.Entity<IdentityRole>().HasData(roles);

            //this is the type of defination of the superAdminUser
            var superAdminId = "ad772267-8a6b-4998-a307-f1456d2d12d4";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@blog.com",
                Email = "superadmin@blog.com",
                NormalizedEmail= "superadmin@blog.com".ToUpper(),
                NormalizedUserName= "superadmin@blog.com".ToUpper(),
                Id = superAdminId,
            };

            //this line securely hash the password before storing it to the databse
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "Superadmin@123");
            //this seed the data of the SuperAdminUser to the database...
            builder.Entity<IdentityUser>().HasData(superAdminUser);


            //thi assign the control of the all the tye of identity role to the SuperAdminUser
            var superAdminRoles = new List<IdentityUserRole<string>>
                {
                new IdentityUserRole<string>
                {
                    RoleId=adminRoleId ,
                    UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId=superAdminRoleId ,
                    UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId=userRoleId,
                    UserId=superAdminId
                }
            };
            //seed the assign control of the superAdminUser to the databse
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
