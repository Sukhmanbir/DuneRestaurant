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
        added items to cart
        display only items belonging to specific user
*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using DuneRestaurant.Models;

namespace DuneRestaurant.Controllers
{
    public class CartController : Controller
    {

        // holds random number generator
        private static Random random = new Random();
        public readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

        //establish a class wide azure connection
        AzureConnection azure = new AzureConnection();

        // GET: Cart
        public ActionResult Index()
        {

            var userID = User.Identity.GetUserId();

            // there is no userID, so we must use the session id
            if (userID == null && Session["userID"] != null)
            {
                userID = Session["userID"].ToString();
            }

            return View(azure.CartViews.Where(x => x.UserID == userID).ToList());

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

        /// <summary>
        ///  adds an dish to the cart, or updates the quantity if already present
        /// </summary>
        public ActionResult Add() {

            // get the dish id
            string dishIDString = Request["dishId"];

            // do nothing if the dish id is missing
            if (string.IsNullOrEmpty(dishIDString) ||
                string.IsNullOrWhiteSpace(dishIDString))
            {
                Response.Redirect("/Menu");
            }

            // try to convert the dishID to a string
            int dishID = -1;
            if (!Int32.TryParse(dishIDString, out dishID)) { Response.Redirect("/Dish"); }
            
            // get the user id if present, if not, generate a random ID and store it in a session
            // this random session id will be used to identify the user until they login
            var userID = User.Identity.GetUserId();

            // there is no userID or sessionID
            if (userID == null && Session["userID"] == null)
            {
                int randomID = random.Next(0, 100);
                Session["userID"] = randomID;
                userID = Convert.ToString(randomID);

                // there is no userID but there is a SessionID
            } else if (userID == null && Session["userID"] != null) {

                userID = Session["userID"].ToString();

            }

                using (AzureConnection db = new AzureConnection())
            {

                // attempt to get an existing record
                Cart cartModel = (from Cart in db.Carts
                                 where Cart.UserID == userID
                                 where Cart.DishID == dishID
                                 select Cart).FirstOrDefault();

                // add the dish to the cart if nothing was returned
                if (cartModel == null) {

                    // create a new model and add that to the database
                    cartModel = new Cart();

                    cartModel.DishID = dishID;
                    cartModel.UserID = userID;
                    cartModel.Quantity = 1;
                    db.Carts.Add(cartModel);
                    db.SaveChanges();

                // just update the quantity if the entry was found
                } else {
                    cartModel.Quantity += 1;
                    db.SaveChanges();
                }
                
            }

            return View();
                
        }

        /*
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
        */

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