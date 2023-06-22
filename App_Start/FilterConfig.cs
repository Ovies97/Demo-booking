using System.Web;
using System.Web.Mvc;

namespace Demo_Booking_Lessons_For_Driving
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
