using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_Management;



namespace Hotel_Management.Controllers
{
    [Authorize]
    public class UserdatasController : Controller
    {
        private HotelEntities db = new HotelEntities();

        // GET: Userdatas
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Userdatas.ToList());
        }
        public ActionResult Home(string search,string cat,string name)
        {
            return View(db.Hotel_Data.Where(x => x.City.Contains(search) && x.Rating.Contains(cat) && x.Hotel_name.Contains(name)).ToList());
        }
      

        // GET: Userdatas/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userdata userdata = db.Userdatas.Find(id);
            if (userdata == null)
            {
                return HttpNotFound();
            }
            return View(userdata);
        }
        /*
        // GET: Userdatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Userdatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,Email")] Userdata userdata)
        {
            if (ModelState.IsValid)
            {
                db.Userdatas.Add(userdata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userdata);
        }
        */
        // GET: Userdatas/Edit/5
        /*
        [Authorize(Roles ="User")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userdata userdata = db.Userdatas.Find(id);
            if (userdata == null)
            {
                return HttpNotFound();
            }
            return View(userdata);
        }

        // POST: Userdatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,Email")] Userdata userdata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userdata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userdata);
        }
        */
        // GET: Userdatas/Delete/5
        /*
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userdata userdata = db.Userdatas.Find(id);
            if (userdata == null)
            {
                return HttpNotFound();
            }
            return View(userdata);
        }
        [Authorize(Roles = "Admin")]
        // POST: Userdatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public ActionResult DeleteConfirmed(int id)
        {
            Userdata userdata = db.Userdatas.Find(id);
            db.Userdatas.Remove(userdata);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        */

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
