using MechanicalAssistance.Common.Models;
using System.Collections.Generic;

namespace MechanicalAssistance.Prism.Helpers
{
    public static class CombosHelper
    {
        public static List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role { Id = 1, Name = Languages.User},
                new Role { Id = 2, Name = Languages.Mechanic}
            };
        }

    }
}
