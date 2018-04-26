using System.Web;
using System.Web.Mvc;

namespace SDJ3_3rd_tier
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
