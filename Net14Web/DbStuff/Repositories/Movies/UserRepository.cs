using Microsoft.EntityFrameworkCore;
using Net14Web.DbStuff.Models.Movies;

namespace Net14Web.DbStuff.Repositories.Movies
{
    public class UserRepository : BaseRepository<User>
    {

        
        public UserRepository(WebDbContext context) : base(context)
        {
            
        }

        public User? GetUserByLoginAndPassword(string login, string password)
        {
            return _entyties
                .FirstOrDefault(user => user.Login == login && user.Password!.Equals(password));
        }

        public void SwitchLocal(int userId, string locale)
        {
            var user = GetById(userId);
            user.PreferLocale = locale;
            _context.SaveChanges();
        }

        public IEnumerable<User> GetUsers(int maxCount = 10)
        {
            return _context.Users.Take(maxCount).ToList();
        }
        public void DeleteUsers(int id)
        {
            var user = _context.Users.First(x => x.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public void EditUserPassword(int id, string password)
        {
            var user = _context.Users.First(x => x.Id == id);
            user.Password = password;
            _context.SaveChanges();
        }


        public int Registration(User user)
        {
            user.PreferLocale = user.PreferLocale ?? "en-US";
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }
    }
}
