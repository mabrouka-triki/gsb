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



        // pour  ajouter  une invitation 
        [HttpPost]
        [HttpPost]
        public IActionResult AjouterInvitation(Activite uneActivite, int id_praticien)
        {
            try
            {
                if (ModelState.IsValid && id_praticien > 0)
                {
                    // Ajout de l'activité et récupération de son ID
                    int idActiviteCompl = ServicePraticien.AjouterActivite(uneActivite.date_activite, uneActivite.lieu_activite, uneActivite.motif_activite, uneActivite.theme_activite);

                    // Vérifier si l'ajout de l'activité a réussi
                    if (idActiviteCompl > 0)
                    {
                        // Insertion de l'invitation en utilisant l'ID de l'activité et l'ID du praticien
                        ServicePraticien.InsertInvitation(new Invitation { Id_activite_compl = idActiviteCompl, Id_praticien = id_praticien });

                        return RedirectToAction("ListePraticien");
                    }
                    else
                    {
                        // Si l'ajout de l'activité a échoué, retourne la vue avec le modèle d'activité
                        ModelState.AddModelError(string.Empty, "Erreur lors de l'ajout de l'activité.");
                        return View(uneActivite);
                    }
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
