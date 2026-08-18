using System.ComponentModel;
using Data;
using Data.Models;
using Logic;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class PeopleController : Controller
    {
        public IActionResult Index()
        {
            List<Person> people = ManagePeople.ListAllPeople();
            return View(people); // pass list of all people to the view
        }

        [HttpGet]
        public IActionResult AddNew()
        {
            List<Country> countries = ManageCountries.ListAllCountries();
            ViewData["Countries"] = countries;
            return View();
        }
        [HttpPost]
        public IActionResult AddNew(Person person)
        {
            int PersonID = ManagePeople.AddNewPerson(person);
            
            if (PersonID == 0)
            {
                return View("Error");                
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            Person? person = People.GetById(id);

            if (person == null)
                return View("Error");

            return View(person);
        }
        [HttpPost]
        public IActionResult Update(Person PersonUpdated)
        {
            if (!People.Update(PersonUpdated.ID, PersonUpdated))
            {
                return View("Error");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if(!People.Delete(id))
                return View("Error");

            return RedirectToAction("Index");
        }
    }
}
