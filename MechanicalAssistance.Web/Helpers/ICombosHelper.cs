using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MechanicalAssistance.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboProductsBrand();
    }
}
