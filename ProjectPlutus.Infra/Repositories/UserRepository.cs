using ProjectPlutus.Data.Context;
using ProjectPlutus.Domain.Models;
using System.Linq;

namespace ProjectPlutus.Infra.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(ProjectPlutusContext context) : base(context)
        {
        }
        public override User Update(User entity)
        {
            var user = _context.Users
                .SingleOrDefault(e => e.Id == entity.Id);

            if (entity.FirstName != null || !string.IsNullOrWhiteSpace(entity.FirstName))
                user.FirstName = entity.FirstName;
            else if (entity.LastName != null || !string.IsNullOrWhiteSpace(entity.LastName))
                user.LastName = entity.LastName;
            else if (entity.Password != null || !string.IsNullOrWhiteSpace(entity.Password))
                user.Password = entity.Password;

            return base.Update(user);
        }
    }
}
