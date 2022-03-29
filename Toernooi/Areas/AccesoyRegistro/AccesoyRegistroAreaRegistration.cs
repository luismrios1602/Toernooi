using System.Web.Mvc;

namespace Toernooi.Areas.AccesoyRegistro
{
    public class AccesoyRegistroAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AccesoyRegistro";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AccesoyRegistro_default",
                "AccesoyRegistro/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}