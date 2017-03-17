using System.Web.Http;

namespace backendApp
{
    public class UserController : ApiController
    {        
        public User Get(int id)
        {
            return UserStorageManager.getUser(id);
        }
        
        public int Put(User user)
        {
            return UserStorageManager.addUser(user);
        }
    }
}