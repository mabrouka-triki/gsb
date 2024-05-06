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

        // Méthode d'action pour traiter l'ajout d'invitation
        [HttpPost]
        public IActionResult AjouterInvitation(int idActiviteCompl, int idPraticien, bool specialiste)
        {
            try
            {
                // Appeler le service pour ajouter l'invitation
                ServicePraticien.AjouterInvitation(idActiviteCompl, idPraticien, specialiste);

                // Redirection vers une autre vue après l'ajout
                return RedirectToAction("ListePraticien");
            }
            catch (MonException e)
            {
                // Gérer les exceptions ici
                return NotFound();
            }
        }
    }
}
