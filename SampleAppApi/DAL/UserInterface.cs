using SampleAppApi.Model;

namespace SampleAppApi.DAL
{
    public interface UserInterface
    {
        List<User> GetUser();
        User GetUser(Guid id);
        User Register(User user);   
        User Update(string  email);

    }
}
