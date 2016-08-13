/*    
    file: controllers/MenuController.cs
    Controllers for the viewing menu items
    author: Douglas Brunner, Alex Nicholls
    website: http://dunerestaurant.azurewebsites.net
    history:
        initial commit
        display menu contents from database
        display individual dish from database
*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DuneRestaurant.Models;

namespace DuneRestaurant.Controllers
{
    public class MenuController : Controller
    {
        AzureConnection azure = new AzureConnection();

        // GET: Menu
        public ActionResult Index()
        {

            return View(azure.Dishes.ToList());
        }

        public ActionResult Dish(int dishID = -1) {

            // do nothing if no id supplied
            if (dishID == -1)
            {
                Response.Redirect("/Inventory");
            }

            Dish dish = azure.Dishes.Single(g => g.DishID == dishID);
            return View(dish);
        }

        public ActionResult Add()
        {
            return View();
        }

        // POST: Dishes/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //auto security!
        [ValidateAntiForgeryToken]
        //never bind with a star, because it leaves you open to injection attacks ZAC
        public async Task<ActionResult> Add([Bind(Include = "DishID,Name,ShortDescription,LongDescription,Price,URL")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                azure.Dishes.Add(dish);
                await azure.SaveChangesAsync();
                //redirect to the index action so you can see the Students table!
                return RedirectToAction("Index");
            }

            return View(dish);
        }
    }
}