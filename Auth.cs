using allUsers;

namespace AuthTools
{
    public class Auth
    {
        //private
        public static bool CheckPassword<T>(string password, string dbname)
        {
            var users = Operations.Read<T>(dbname);
            foreach (var u in users)
            {
                if (u[2].ToString() == password)
                {
                    return true;
                }
            }
            return false;
        }
        public static string CheckLogin<T>(string login, string dbname)
        {
            try
            {
                var users = Operations.Read<T>(dbname);
                foreach (var u in users)
                {
                    if (u[1].ToString() == login)
                    {
                        return (string)u[2];
                        //break;
                    }
                }
                return null;
            }
            catch (System.NullReferenceException)
            {
                return null;
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }
        }
    }
}
