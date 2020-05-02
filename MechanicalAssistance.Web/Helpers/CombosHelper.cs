using MechanicalAssistance.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> GetComboProductsBrand()
        {
            List<SelectListItem> list = _context.ProductBrands.Select(t => new SelectListItem
            {
                Text = t.BrandName,
                Value = $"{t.Id}"
            })
              .OrderBy(t => t.Text)
              .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a brand..",
                Value = "0"
            });

            return list;
        }
    }
}
