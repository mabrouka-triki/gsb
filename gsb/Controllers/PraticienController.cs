using Microsoft.AspNetCore.Mvc;
using gsb.Models.Dao;
using gsb.Models.MesExceptions;
using System;
using System.Data;
using gsb.Models.Metier;

namespace gsb.Controllers
{
    public class PraticienController : Controller
    {
        // Méthode d'action pour afficher la liste des praticiens
        public IActionResult ListePraticien()
        {
            try
            {
                // Récupérer les données des praticiens à partir du service
                var mesPraticiens = ServicePraticien.GetTousLesPraticiens();

                // Passer les données à la vue
                return View(mesPraticiens);
            }
            catch (MonException e)
            {
                // Gérer les exceptions ici, par exemple retourner un code d'erreur
                return StatusCode(418);
            }
        }

        // Méthode d'action pour afficher le formulaire de recherche de praticien
        [HttpGet]
        public IActionResult RechercherPraticien()
        {
            return View("RechercherPraticien");
        }

        [HttpPost]
        public IActionResult RechercherParNom(string nomPraticien)
        {
            try
            {
                DataTable praticiens = ServicePraticien.RechercherParNomPraticien(nomPraticien);
                return PartialView("_DetailsPraticien", praticiens);
            }
            catch (MonException e)
            {
                // Gérer les exceptions ici
                return StatusCode(500);
            }
        }

        

        public IActionResult AjouterInvitation()
        {
            try
            {
                var praticiens = ServicePraticien.ObtenirPraticiens();
                var activites = ServicePraticien.ObtenirActivites();
                Invitation uneInvitation = new Invitation();

                ViewBag.Praticiens = praticiens;
                ViewBag.Activites = activites;

                return View(uneInvitation);
            }
            catch (MonException e)
            {
                return NotFound();
            }
        }

        // Méthode d'action pour traiter la soumission du formulaire d'ajout d'une invitation (POST)
        [HttpPost]
        public IActionResult AjouterInvitation(Invitation uneInvitation)
        {
            try
            {
                ServicePraticien.InsertInvitation(uneInvitation);
                return RedirectToAction("ListePraticien"); // Redirection vers une autre action
            }
            catch (MonException e)
            {
                return NotFound();
            }
        }


    }
}
