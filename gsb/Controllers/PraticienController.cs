using Microsoft.AspNetCore.Mvc;
using gsb.Models.Dao;
using gsb.Models.MesExceptions;
using System;
using System.Data;
using gsb.Models.Metier;
using gsb.Models.Persistance;

namespace gsb.Controllers
{
    public class PraticienController : Controller
    {
        public IActionResult ListePraticien()
        {
            try
            {
                var mesPraticiens = ServicePraticien.GetTousLesPraticiens();
                return View(mesPraticiens);
            }
            catch (MonException e)
            {
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
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult AjouterInvitation()
        {
            try
            {
                var praticiens = ServicePraticien.ObtenirPraticiens();
                var activites = ServicePraticien.ObtenirActivites();
                var uneActivite = new Activite();

                ViewBag.Praticiens = praticiens;
                ViewBag.Activites = activites;

                return View(uneActivite);
            }
            catch (MonException e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult AjouterInvitation(Activite uneActivite)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Convertir la chaîne de caractères en DateTime
                    DateTime dateActivite = DateTime.Parse(uneActivite.date_activite);
                    string lieuActivite = uneActivite.lieu_activite;
                    string motifActivite = uneActivite.motif_activite;
                    string theme_activite = uneActivite.theme_activite;
                    ServicePraticien.AjouterInvitation(0, dateActivite, lieuActivite, motifActivite, theme_activite); 
                    return RedirectToAction("listePraticien");
                }
                else
                {
                    return View(uneActivite);
                }
            }
            catch (MonException e)
            {
                return StatusCode(500);
            }
        }




        [HttpGet]
        public IActionResult Modifier(int idInvitation)
        {
            try
            {
                var invitation = ServicePraticien.GetInvitation(idInvitation);

                if (invitation == null)
                {
                    return NotFound();
                }

                return View(invitation);
            }
            catch (MonException e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Modifier(Invitation invitation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServicePraticien.ModifierInvitation(invitation.Id_activite_compl, invitation.Specialiste);
                    return RedirectToAction("ListePraticien");
                }
                else
                {
                    return View(invitation);
                }
            }
            catch (MonException e)
            {
                return StatusCode(500);
            }
        }
    }
}
