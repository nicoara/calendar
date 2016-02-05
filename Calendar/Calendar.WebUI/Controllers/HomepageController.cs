using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace Calendar.WebUI.Controllers
{
    public class HomepageController : Controller
    {

        public ViewResult Home()
        {            
            return View();
        }

    }
}