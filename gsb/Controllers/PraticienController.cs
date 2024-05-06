using Microsoft.AspNetCore.Mvc;
using gsb.Models.Dao;
using gsb.Models.MesExceptions;
using System;
using gsb.Models.Metier;
using gsb.Models.Dao;
using gsb.Models.Metier;
using System.Data;

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

        // Méthodes pour gérer les invitations

        [HttpGet]
        public IActionResult AjouterInvitation()
        {
            return View("AjouterInvitation");
        }

        [HttpPost]
        public IActionResult AjouterInvitation(int idActiviteCompl, int idPraticien, bool specialiste)
        {
            try
            {
                ServicePraticien.AjouterInvitation(idActiviteCompl, idPraticien, specialiste);
                return RedirectToAction("Index", "listePraticien"); // Redirection vers une autre vue après l'ajout
            }
            catch (MonException e)
            {
                return NotFound();
            }
        }


        //  //rechercher par nom 

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



    }
}
