using RealEstateNet14Web.DbStuff.Models;

namespace RealEstateNet14Web.Services;

public class UpdateUser
{
    public RealEstateOwner Update(List<RealEstateOwner> userViewModelsApartmentOwners,int id,string name,int age,string kindOfActivity)
    {
        var user = userViewModelsApartmentOwners.FirstOrDefault(x => x.Id == id);
        user.Name = name;
        user.Age = age;
        user.KindOfActivity = kindOfActivity;
        return user;
    }
}