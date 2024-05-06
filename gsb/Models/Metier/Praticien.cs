namespace gsb.Models.Metier
{
    public class Praticien
    {
        private int id_praticien;
        private string nom_praticien;
        private string prenom_praticien;
        private string adresse_praticien;
        private string cp_praticien;
        private string ville_praticien;
        private double coef_notoriete;

        public int Id_praticien { get => id_praticien; set => id_praticien = value; }
        public string Nom_praticien { get => nom_praticien; set => nom_praticien = value; }
        public string Prenom_praticien { get => prenom_praticien; set => prenom_praticien = value; }
        public string Adresse_praticien { get => adresse_praticien; set => adresse_praticien = value; }
        public string Cp_praticien { get => cp_praticien; set => cp_praticien = value; }
        public string Ville_praticien { get => ville_praticien; set => ville_praticien = value; }
        public double Coef_notoriete { get => coef_notoriete; set => coef_notoriete = value; }
    }
}
