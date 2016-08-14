/*    
    file: controllers/CartController.cs
    Controllers for the shopping cart
    author: Douglas Brunner
    website: http://dunerestaurant.azurewebsites.net
    history:
        initial commit
        display cart information from CartView
        update cart quantity
        delete cart item
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
    public class CartController : Controller
    {
        //establish a class wide azure connection
        AzureConnection azure = new AzureConnection();

        // GET: Cart
        public ActionResult Index()
        {

            
            return View(azure.CartViews.ToList());

        }

        public void Delete() {

            string cartIDString = Request["cartId"];

            // do nothing if either field is missing
            if (string.IsNullOrEmpty(cartIDString) ||
                string.IsNullOrWhiteSpace(cartIDString))
            {
                Response.Redirect("/Cart");
            }

            // try to convert the cartID to a string
            int cartID = -1;
            if (!Int32.TryParse(cartIDString, out cartID)) { Response.Redirect("/Cart"); }

            // update the cart item quantity
            using (AzureConnection db = new AzureConnection())
            {
                Models.Cart cartModel = (from Cart in db.Carts
                                         where Cart.CartID == cartID
                                         select Cart).FirstOrDefault();

                db.Carts.Remove(cartModel);

                db.SaveChanges();

            }

            Response.Redirect("/Cart");

        }

        public ActionResult Add() {
            return View();
        }
        public async Task<ActionResult> Add([Bind(Include = "DishID,CartID,UserId,Quantity")] Cart cart)
        {
            //add the dish to the cart
            if (ModelState.IsValid)
            {
                azure.Carts.Add(cart);
                await azure.SaveChangesAsync();
                //redirect to the index action so you can see the Students table!
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        [HttpPost]
        public void Update() {

            string cartIDString = Request.Form["cartId"];
            string quantityString = Request.Form["quantity"];

            // do nothing if either field is missing
            if (string.IsNullOrEmpty(cartIDString) ||
                string.IsNullOrWhiteSpace(cartIDString) ||
                string.IsNullOrEmpty(quantityString) ||
                string.IsNullOrWhiteSpace(quantityString))
            {
                Response.Redirect("/Cart");
            }



            // try to convert the cartID to a string
            int cartID = -1;
            int quantity = -1;
            if (!Int32.TryParse(cartIDString, out cartID)) { Response.Redirect("/Cart"); }
            if (!Int32.TryParse(quantityString, out quantity)) { Response.Redirect("/Cart"); }

            // update the cart item quantity
            using (AzureConnection db = new AzureConnection())
            {
                Models.Cart cartModel = (from Cart in db.Carts
                                         where Cart.CartID == cartID
                                         select Cart).FirstOrDefault();

                cartModel.Quantity = quantity;

                db.SaveChanges();

            }
            
            Response.Redirect("/Cart");

        }

        public ActionResult Checkout() {
            AzureConnection azure = new AzureConnection();

            return View(azure.CartViews.ToList());
        }

        public ActionResult ConfirmOrder() {
            AzureConnection azure = new AzureConnection();

            return View(azure.CartViews.ToList());
        }

        public ActionResult Complete()
        {
            return View();
        }
        
    }
}