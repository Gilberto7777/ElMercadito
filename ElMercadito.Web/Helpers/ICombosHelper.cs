using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElMercadito.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboBusinessTypes();
    }
}