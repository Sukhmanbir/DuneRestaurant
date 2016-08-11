/*    
    file: controllers/CartController.cs
    Controllers for the shopping cart
    author: Douglas Brunner, Alex Nicholls
    website: http://dunerestaurant.azurewebsites.net
    history:
        initial commit
        connecting time!
*/
using DuneRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DuneRestaurant.Controllers
{
    public class CartController : Controller
    {
        //set up an azure connection to be used by the whole class
        AzureConnection azure = new AzureConnection();

        // GET: Cart
        public ActionResult Index()
        {
            return View(azure.Carts.ToList());
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