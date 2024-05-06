using gsb.Models.MesExceptions;
using MySql.Data.MySqlClient;


namespace gsb.Models.Persistance
{
    public class Connexion
    {
        private static MySqlConnection macnx;
        private static Connexion instance;

        /// <summary>
        /// Constructeur privé
        /// </summary>
        private Connexion() { }
        /// <summary>
        /// Connexion à MySql
        /// </summary>
        /// <returns>MysqlConnexion</returns>
        public MySqlConnection getConnexion()
        {
            string strConnexion;
            try
            {
                var builder = new
               ConfigurationBuilder();
                builder.AddJsonFile("appsettings.json");
                IConfiguration configuration =
               builder.Build();
                strConnexion =
               configuration.GetConnectionString("bddCourante");
                macnx = new
               MySqlConnection(strConnexion);
                macnx.Open();
                return macnx;
            }
            catch (MySqlException err)
            {
                throw new MonException("", "Erreur d'acces à la base.", err.Message);
            }
            catch (Exception e)
            {
                throw new MonException("", " Erreur d'accès", e.Message);
            }
        }
        /// <summary>
        /// Obtenir le singleton et le créer s'il n'existe pas
        /// </summary>
        public static Connexion getInstance()
        {
            if (Connexion.instance == null)
                Connexion.instance = new Connexion();
            return Connexion.instance;
        }
        /// <summary>
        /// Fermer la c
        public static void closeConnexion()
        {
            if (instance != null && macnx != null)
                macnx.Close();
        }
    }
}