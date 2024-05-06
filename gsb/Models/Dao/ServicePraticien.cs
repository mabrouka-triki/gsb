using System;
using System.Collections.Generic;
using System.Data;
using gsb.Models.MesExceptions;
using gsb.Models.Metier;
using gsb.Models.Persistance;
using gsb.Models.Metier;

namespace gsb.Models.Dao
{
    public class ServicePraticien
    {
        public static DataTable GetTousLesPraticiens()
        {
            DataTable mesPraticiens;
            Serreurs er = new Serreurs("Erreur sur lecture des Praticiens.", "praticien.getPraticien()");
            try
            {
                string mysql = @"
                    SELECT 
                        p.id_praticien,
                        p.nom_praticien, 
                        p.prenom_praticien, 
                        p.adresse_praticien,
                        p.cp_praticien,
                        p.ville_praticien,
                        p.coef_notoriete,
                        i.id_activite_compl, 
                        a.date_activite, 
                        a.lieu_activite, 
                        a.theme_activite, 
                        a.motif_activite
                    FROM 
                        PRATICIEN p
                    LEFT JOIN 
                        INVITER i ON p.id_praticien = i.id_praticien
                    LEFT JOIN 
                        ACTIVITE_COMPL a ON i.id_activite_compl = a.id_activite_compl
                ";

                // Exécuter la requête SQL
                mesPraticiens = DBInterface.Lecture(mysql, er);

                return mesPraticiens;
            }
            catch (MonException e)
            {
                // Gérer les exceptions
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

        public static DataTable RechercherParNomPraticien(string nomPraticien)
        {
            DataTable praticiens = new DataTable();
            Serreurs er = new Serreurs("Erreur lors de la recherche du praticien par nom.", "ServicePraticien.RechercherParNomPraticien()");

            try
            {
                string mysql = @"
            SELECT 
                p.id_praticien,
                p.nom_praticien,
                p.prenom_praticien,
                a.motif_activite
            FROM 
                PRATICIEN p
            LEFT JOIN 
                INVITER i ON p.id_praticien = i.id_praticien
            LEFT JOIN 
                ACTIVITE_COMPL a ON i.id_activite_compl = a.id_activite_compl
            WHERE 
                p.nom_praticien LIKE '%" + nomPraticien + "%'";

                praticiens = DBInterface.Lecture(mysql, er);

                return praticiens;
            }
            catch (MonException e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

        public static void AjouterInvitation(int idActiviteCompl, int idPraticien, bool specialiste)
        {
            Serreurs er = new Serreurs("Erreur lors de l'ajout de l'invitation.", "ServicePraticien.AjouterInvitation()");

            try
            {
                string requete = @"
            INSERT INTO INVITER (id_activite_compl, id_praticien, specialiste)
            VALUES (" + idActiviteCompl + ", " + idPraticien + ", " + (specialiste ? 1 : 0) + ")";

                DBInterface.Execute_Transaction(requete);
            }
            catch (MonException erreur)
            {
                throw erreur;
            }
        }
    }
}
