using Microsoft.AspNetCore.Mvc;

namespace MvcLab.Controllers
{
    public class LabController : Controller
    {
        public IActionResult Info()
        {
            var model = new
            {
                Number = 1,
                Topic = "Вступ до ASP.NET Core\r\n",
                Goal = "ознайомитися з основними принципами роботи .NET, " +
                "навчитися налаштовувати середовище розробки та встановлювати необхідні компоненти, " +
                "набути навичок створення рішень та проектів різних типів, набути навичок обробки запитів " +
                "з використанням middleware.\r\n",
                Author = "Ірина Клосович"
            };
            return View(model);
        }
    }
}
