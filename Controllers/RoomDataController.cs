using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Hotel_Management;

namespace Hotel_Management.Controllers
{
    public class RoomDataController : Controller
    {
        private HotelEntities db = new HotelEntities();

        // GET: RoomData
        public ActionResult Index(int? id)
        {
            List<RoomData> roomData = db.RoomDatas.Where(x => x.R_Hotel_Id == id).ToList();
            return View(roomData);
        }

        public ActionResult Index2(int? id)
        {
            List<RoomData> roomData = db.RoomDatas.Where(x => x.R_Hotel_Id == id).ToList();
            return View(roomData);
        }



        // GET: RoomData/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomData roomData = db.RoomDatas.Find(id);
            if (roomData == null)
            {
                return HttpNotFound();
            }
            return View(roomData);
        }

        // GET: RoomData/Create
        public ActionResult Create()
        {
            ViewBag.R_Hotel_Id = new SelectList(db.Hotel_Data, "Hotel_Id", "Hotel_name");
            return View();
        }

        // POST: RoomData/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Room_Id,R_Hotel_Id,Type,Price,Capacity,No_rooms")] RoomData roomData)
        {
            if (ModelState.IsValid)
            {
                db.RoomDatas.Add(roomData);
                db.SaveChanges();
                return RedirectToAction("Index", "Hotel_Data");
            }

            ViewBag.R_Hotel_Id = new SelectList(db.Hotel_Data, "Hotel_Id", "Hotel_name", roomData.R_Hotel_Id);
            return View(roomData);
        }

        // GET: RoomData/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomData roomData = db.RoomDatas.Find(id);
            if (roomData == null)
            {
                return HttpNotFound();
            }
            ViewBag.R_Hotel_Id = new SelectList(db.Hotel_Data, "Hotel_Id", "Hotel_name", roomData.R_Hotel_Id);
            return View(roomData);
        }

        // POST: RoomData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Room_Id,R_Hotel_Id,Type,Price,Capacity,No_rooms")] RoomData roomData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Hotel_Data");
            }
            ViewBag.R_Hotel_Id = new SelectList(db.Hotel_Data, "Hotel_Id", "Hotel_name", roomData.R_Hotel_Id);
            return View(roomData);
        }

        // GET: RoomData/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomData roomData = db.RoomDatas.Find(id);
            if (roomData == null)
            {
                return HttpNotFound();
            }
            return View(roomData);
        }

        // POST: RoomData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomData roomData = db.RoomDatas.Find(id);
            db.RoomDatas.Remove(roomData);
            db.SaveChanges();
            return RedirectToAction("Index", "Hotel_Data");
        }
        public ActionResult Book(int? id)
        {
            RoomData room = db.RoomDatas.Find(id);
            var b = db.Bookings.Max(i => i.Booking_Id);
            b++;
            Booking booking = new Booking() {
                Booking_Id = b,
                B_User_Id = Convert.ToInt32(Session["id"]),
                B_Room_Id = id,
                Status = "pending",
            };
            var r = Convert.ToInt32(room.No_rooms);
            r--;
            room.No_rooms = r;
            db.Entry(room).State = EntityState.Modified;
            db.Bookings.Add(booking);
            db.SaveChanges();
            return RedirectToAction("MyBookings", "RoomData");
        }
        public ActionResult MyBookings()
        {
            var id = Convert.ToInt32(Session["id"]);
            var booking = db.Bookings.Where(b => b.B_User_Id == id);
            return View(booking);
        }
        public ActionResult ConfirmBooking()
        {
            var id = Convert.ToInt32(Session["id"]);
            var booking = db.Bookings.Include(b=>b.RoomData).Include(b=>b.Userdata);
            return View(booking);
        }
        public ActionResult Confirm(int? id)
        {
            var booking = db.Bookings.FirstOrDefault(b => b.Booking_Id == id);
            booking.Status="Booking Confirmed";
            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();

            var senderEmail = new MailAddress("dlms.mvc@gmail.com", "Hotel Management");
            var receiverEmail = new MailAddress(booking.Userdata.Email, "Receiver");
            var password = "dlms@123";
            var sub = "Booking Confirmation";
            var body = "Your Booking Has Been Confirmed.\nBooking Id is :"+booking.Booking_Id+"\nHotel Name:"+booking.RoomData.Hotel_Data.Hotel_name+"\nRoom Type:"+booking.RoomData.Type+"\nPrice:"+booking.RoomData.Price;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }

            return RedirectToAction("ConfirmBooking");
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
