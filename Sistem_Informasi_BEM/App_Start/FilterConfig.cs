using System.Web;
using System.Web.Mvc;

namespace Sistem_Informasi_BEM
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
