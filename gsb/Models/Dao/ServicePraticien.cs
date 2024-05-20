using System;
using System.Collections.Generic;
using System.Data;
using gsb.Models.MesExceptions;
using gsb.Models.Metier;
using gsb.Models.Persistance;

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
                        p.coef_notoriete
                    FROM 
                        PRATICIEN p
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
                        p.prenom_praticien
                    FROM 
                        PRATICIEN p
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

        public static int AjouterInvitation(string dateActivite, string lieuActivite, string motifActivite, string themeActivite)
        {
            // Insérer l'activité on  récupérer ID 
            int idActiviteCompl = 0;
            return idActiviteCompl;
        }

        public static void InsertInvitation(Invitation uneInvitation)
        {
            try
            {
                DBInterface.Execute_Transaction("INSERT INTO inviter (id_activite_compl, id_praticien) VALUES (" + uneInvitation.Id_activite_compl + ", " + uneInvitation.Id_praticien + ")");
            }
            catch (MonException erreur)
            {
                throw erreur;
            }
        }


        public static void ModifierInvitation(int idActiviteCompl, bool specialiste)
        {
            Serreurs er = new Serreurs("Erreur lors de la modification de l'invitation.", "ServicePraticien.ModifierInvitation()");

            try
            {
                string requete = @"
                UPDATE INVITER 
                SET specialiste = " + (specialiste ? 1 : 0) + @" 
                WHERE id_activite_compl = " + idActiviteCompl + ";";

                DBInterface.Execute_Transaction(requete);
            }
            catch (MonException erreur)
            {
                throw erreur;
            }
        }

    }
}
