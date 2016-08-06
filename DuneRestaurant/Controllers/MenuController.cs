/*    
    file: controllers/MenuController.cs
    Controllers for the viewing menu items
    author: Douglas Brunner
    website: http://dunerestaurant.azurewebsites.net
    history:
        initial commit
        display menu contents from database
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DuneRestaurant.Models;

namespace DuneRestaurant.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            AzureConnection azure = new AzureConnection();

            return View(azure.Dishes.ToList());
        }

        public ActionResult Dish() {
            return View();
        }
    }
}