using Dotnet.Homeworks.DataAccess.Specs.Infrastructure;
using Dotnet.Homeworks.Domain.Entities;

namespace Dotnet.Homeworks.DataAccess.Specs;

public class UsersSpecs : IUsersSpecs
{
    public Specification<User> HasGoogleEmail()
    {
        return new Specification<User>(u => u.Email.EndsWith("gmail.com"));
    }

    public Specification<User> HasYandexEmail()
    {
        return new Specification<User>(u => u.Email.EndsWith("yandex.ru"));
    }

    public Specification<User> HasMailEmail()
    {
        return new Specification<User>(u => u.Email.EndsWith("mail.ru"));
    }

    public Specification<User> HasPopularEmailVendor()
    {
        return HasGoogleEmail() || HasYandexEmail() || HasMailEmail();
    }

    public Specification<User> HasLongName()
    {
        return new Specification<User>(u => u.Name.Length > 15);
    }

    public Specification<User> HasCompositeNameWithWhitespace()
    {
        return new Specification<User>(u => u.Name.Contains(' '));
    }

    public Specification<User> HasCompositeNameWithHyphen()
    {
        return new Specification<User>(u => u.Name.Contains('-'));
    }

    public Specification<User> HasCompositeName()
    {
        return HasCompositeNameWithHyphen() || HasCompositeNameWithWhitespace();
    }

    public Specification<User> HasComplexName()
    {
        return HasLongName() && HasCompositeName();
    }
}