using EasyPizza.Entities;

namespace EasyPizza.Models.UserModels
{
    public class AuthorizationModel
    {
        public long Id { get; set; }
        public UserGroup UserGroup { get; set; }
    }
}
