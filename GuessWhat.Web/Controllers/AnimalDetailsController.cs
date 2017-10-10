using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GuessWhat.Web.Data;

namespace GuessWhat.Web.Controllers
{
    public class AnimalDetailsController : Controller
    {
        private readonly GuessWhatContext db = new GuessWhatContext();

        // GET: AnimalDetails
        public ActionResult Index()
        {
            var animalDetails = db.AnimalDetails.Include(a => a.Animal);

            return View(animalDetails.OrderBy(ad => ad.Animal.Name).ToList());
        }

        // GET: AnimalDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var animalDetail = db.AnimalDetails.Find(id);

            if (animalDetail == null)
            {
                return HttpNotFound();
            }

            return View(animalDetail);
        }

        // GET: AnimalDetails/Create
        public ActionResult Create()
        {
            ViewBag.AnimalId = new SelectList(GetOrderedAnimals(), "Id", "Name");

            return View();
        }

        // POST: AnimalDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Detail,AnimalId")] AnimalDetail animalDetail)
        {
            if (ModelState.IsValid)
            {
                db.AnimalDetails.Add(animalDetail);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.AnimalId = new SelectList(GetOrderedAnimals(), "Id", "Name", animalDetail.AnimalId);

            return View(animalDetail);
        }

        // GET: AnimalDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var animalDetail = db.AnimalDetails.Find(id);

            if (animalDetail == null)
            {
                return HttpNotFound();
            }

            ViewBag.AnimalId = new SelectList(GetOrderedAnimals(), "Id", "Name", animalDetail.AnimalId);

            return View(animalDetail);
        }

        // POST: AnimalDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Detail,AnimalId")] AnimalDetail animalDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animalDetail).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.AnimalId = new SelectList(GetOrderedAnimals(), "Id", "Name", animalDetail.AnimalId);

            return View(animalDetail);
        }

        // GET: AnimalDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var animalDetail = db.AnimalDetails.Find(id);

            if (animalDetail == null)
            {
                return HttpNotFound();
            }

            return View(animalDetail);
        }

        // POST: AnimalDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var animalDetail = db.AnimalDetails.Find(id);

            db.AnimalDetails.Remove(animalDetail);
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

        private IQueryable<Animal> GetOrderedAnimals()
        {
            return db.Animals.OrderBy(a => a.Name);
        }
    }
}