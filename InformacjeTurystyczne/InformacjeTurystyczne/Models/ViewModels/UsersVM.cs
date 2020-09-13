using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.ViewModels
{
    public class UsersVM
    {
        public struct User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            //lista regionów, które moze modyfikować
            public List<string> RegionsPermissions { get; set; }
            public List<string> ShelterPermissions { get; set; }

            public List<string> PartysPermissions { get; set; }
            public List<string> TrailsPermissions { get; set; }
        }

        //lista wszystkich użytkowników
        public List<User> AllUsers { get; set; }
        //info o aktualnym userze
        public User CurrentUser { get; set; }
        //lista ról, które dany user może przydzielić
        public List<string> AllawedRoles { get; set; }
    }
}
