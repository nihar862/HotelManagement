using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_Management;
using System.IO;

namespace Hotel_Management.Controllers
{
    [Authorize(Roles ="Admin")]
    public class Hotel_DataController : Controller
    {
        private HotelEntities db = new HotelEntities();

        // GET: Hotel_Data
        public ActionResult Index()
        {
            return View(db.Hotel_Data.ToList());
        }

        // GET: Hotel_Data/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel_Data hotel_Data = db.Hotel_Data.Find(id);
            if (hotel_Data == null)
            {
                return HttpNotFound();
            }
            return View(hotel_Data);
        }

        // GET: Hotel_Data/Create
        public ActionResult Create()
        {
            return View();
        }

        

        // POST: Hotel_Data/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Hotel_Data hotel_Data)
        {
           
            if (ModelState.IsValid)
            {
                string file1 = Path.GetFileNameWithoutExtension(hotel_Data.ImageFile1.FileName);
                string ext = Path.GetExtension(hotel_Data.ImageFile1.FileName);
                file1 = file1 + DateTime.Now.ToString("yymmssfff") + ext;
                hotel_Data.img1 = "~/Image/" + file1;
                file1 = Path.Combine(Server.MapPath("~/Image/"), file1);
                hotel_Data.ImageFile1.SaveAs(file1);

                string file2 = Path.GetFileNameWithoutExtension(hotel_Data.ImageFile2.FileName);
                string ext2 = Path.GetExtension(hotel_Data.ImageFile2.FileName);
                file2 = file2 + DateTime.Now.ToString("yymmssfff") + ext2;
                hotel_Data.img2 = "~/Image/" + file2;
                file2 = Path.Combine(Server.MapPath("~/Image/"), file2);
                hotel_Data.ImageFile2.SaveAs(file2);

                db.Hotel_Data.Add(hotel_Data);
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }

            return View(hotel_Data);
        }

        // GET: Hotel_Data/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel_Data hotel_Data = db.Hotel_Data.Find(id);
            if (hotel_Data == null)
            {
                return HttpNotFound();
            }
            return View(hotel_Data);
        }

        // POST: Hotel_Data/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Hotel_Data hotel_Data)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(hotel_Data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotel_Data);
        }

        // GET: Hotel_Data/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel_Data hotel_Data = db.Hotel_Data.Find(id);
            if (hotel_Data == null)
            {
                return HttpNotFound();
            }
            return View(hotel_Data);
        }

        // POST: Hotel_Data/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hotel_Data hotel_Data = db.Hotel_Data.Find(id);
            db.Hotel_Data.Remove(hotel_Data);
            db.SaveChanges();
            return RedirectToAction("Index");
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
