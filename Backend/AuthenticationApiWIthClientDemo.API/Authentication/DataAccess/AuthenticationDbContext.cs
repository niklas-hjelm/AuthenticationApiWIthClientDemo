using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApiWIthClientDemo.API.Authentication.DataAccess;

//Ärver av IdentityDbContext för att använda Identity, User är min identityklass som ärver av IdentityUser
public class AuthenticationDbContext : IdentityDbContext<User>
{
     public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) :
          base(options)
     {

     }
}