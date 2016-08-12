/*    
    file: controllers/CartController.cs
    Controllers for the shopping cart
    author: Douglas Brunner
    website: http://dunerestaurant.azurewebsites.net
    history:
        initial commit
        display cart information from CartView
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DuneRestaurant.Models;

namespace DuneRestaurant.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {

            AzureConnection azure = new AzureConnection();
            
            return View(azure.CartViews.ToList());

        }

        public ActionResult Delete() {
            return View();
        }

        public ActionResult Add() {
            return View();
        }

        public ActionResult Update() {
            return View();
        }

        public ActionResult Checkout() {
            return View();
        }

        public ActionResult ConfirmOrder() {
            return View();
        }
        
    }
}