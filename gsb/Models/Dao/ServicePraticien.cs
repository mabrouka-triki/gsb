using System;
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

        public static void AjouterInvitation(int idActiviteCompl, DateTime dateActivite, string lieuActivite, string motifActivite, string themeActivite)
        {
            Serreurs er = new Serreurs("Erreur lors de l'ajout de l'invitation.", "ServicePraticien.AjouterInvitation()");

            try
            {
                string requeteActivite = @"
            INSERT INTO ACTIVITE_COMPL (id_activite_compl, date_activite, lieu_activite, motif_activite, theme_activite) 
            VALUES (" + idActiviteCompl + ", '" + dateActivite.ToString("yyyy-MM-dd") + "', '" + lieuActivite + "', '" + motifActivite + "', '" + themeActivite + "')";

                DBInterface.Execute_Transaction(requeteActivite);
            }
            catch (MonException erreur)
            {
                throw erreur;
            }
        }



        public static List<Praticien> ObtenirPraticiens()
        {
            try
            {
                Serreurs er = new Serreurs("Erreur lors de la récupération des praticiens.", "ServicePraticien.ObtenirPraticiens()");

                var praticiens = DBInterface.Lecture("SELECT id_praticien, nom_praticien, prenom_praticien FROM praticien", er);

                List<Praticien> listePraticiens = new List<Praticien>();

                foreach (DataRow row in praticiens.Rows)
                {
                    Praticien praticien = new Praticien();
                    praticien.Id_praticien = Convert.ToInt32(row["id_praticien"]);
                    praticien.Nom_praticien = row["nom_praticien"].ToString();
                    praticien.Prenom_praticien = row["prenom_praticien"].ToString();

                    listePraticiens.Add(praticien);
                }
                return listePraticiens;
            }
            catch (Exception ex)
            {
                throw new MonException("Erreur lors de la récupération des praticiens.", "ServicePraticien.ObtenirPraticiens()", ex.Message);
            }
        }

        public static void InsertInvitation(Invitation uneInvitation)
        {
            Serreurs er = new Serreurs("Erreur sur l'écriture d'une invitation.", "ServicePraticien.InsertInvitation()");
            String requete = "INSERT INTO inviter (id_activite_compl, id_praticien, specialiste) " +
                             "VALUES (" + uneInvitation.Id_activite_compl + ", " + uneInvitation.Id_praticien + ", '" + uneInvitation.Specialiste + "')";
            try
            {
                DBInterface.Execute_Transaction(requete);
            }
            catch (MonException erreur)
            {
                throw erreur;
            }
        }

        public static List<Activite> ObtenirActivites()
        {
            try
            {
                Serreurs er = new Serreurs("Erreur lors de la récupération des activites.", "ServicePraticien.ObtenirActivites()");

                var activites = DBInterface.Lecture("SELECT id_activite_compl, motif_activite FROM activite_compl", er);

                List<Activite> listeActivites = new List<Activite>();

                foreach (DataRow row in activites.Rows)
                {
                    Activite activite = new Activite();
                    activite.id_activite_compl = Convert.ToInt32(row["id_activite_compl"]);
                    activite.motif_activite = row["motif_activite"].ToString();
                    listeActivites.Add(activite);
                }
                return listeActivites;
            }
            catch (Exception ex)
            {
                throw new MonException("Erreur lors de la récupération des activites.", "ServicePraticien.ObtenirActivites()", ex.Message);
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
        public static Invitation GetInvitation(int idInvitation)
        {
            try
            {
                Serreurs er = new Serreurs("Erreur lors de la récupération de l'invitation.", "ServicePraticien.GetInvitation()");
                var invitation = DBInterface.Lecture("SELECT id_activite_compl, id_praticien, specialiste FROM inviter WHERE id_activite_compl = " + idInvitation, er);

                if (invitation.Rows.Count > 0)
                {
                    Invitation uneInvitation = new Invitation();
                    uneInvitation.Id_activite_compl = Convert.ToInt32(invitation.Rows[0]["id_activite_compl"]);
                    uneInvitation.Id_praticien = Convert.ToInt32(invitation.Rows[0]["id_praticien"]);
                    uneInvitation.Specialiste = Convert.ToBoolean(invitation.Rows[0]["specialiste"]);

                    return uneInvitation;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new MonException("Erreur lors de la récupération de l'invitation.", "ServicePraticien.GetInvitation()", e.Message);
            }
        }

    }
}