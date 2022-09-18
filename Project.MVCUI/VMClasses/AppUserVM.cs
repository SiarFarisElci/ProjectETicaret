using Project.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.VMClasses
{
    public class AppUserVM
    {
        public AppUser AppUser { get; set; }
        public Profile Profile { get; set; }
        public List<AppUser> AppUsers { get; set; }
        public List<Profile> Profiles { get; set; }
    }
}