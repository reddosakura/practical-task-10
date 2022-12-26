using allUsers;

namespace ConsoleuiNames
{
    public class ConsoleUi
    {
        public static void drawAuthWin()
        {
            Console.Clear();
            Console.WriteLine("  Логин:\n  Пароль:\n  Авторизоваться");
        }
        public static void drawUsersWin1(string name = "admin")
        {
            Console.Clear();
            Console.WriteLine($"Роль: администратор\t\t\t\t\tДобро пожаловать, {name}");
            Console.SetCursorPosition(56, 2);
            Console.WriteLine("F1 - Добавить запись F2 - Найти запись");
            Console.SetCursorPosition(3, 2);
            Console.WriteLine("id\t\tlogin\t\tpassword\trole");
            try
            {
                foreach (var a in Operations.Read<Admin>("db_admin.json"))
                {
                    Console.WriteLine("( ) " + string.Join("\t\t", a));
                }
                foreach (var hr in Operations.Read<HRmanager>("db_hr.json"))
                {
                    Console.WriteLine("( ) " + string.Join("\t\t", hr));
                }
                foreach (var ac in Operations.Read<Accountant>("db_account.json"))
                {
                    Console.WriteLine("( ) " + string.Join("\t\t", ac));
                }
                foreach (var cm in Operations.Read<CargoManager>("db_cargo.json"))
                {
                    Console.WriteLine("( ) " + string.Join("\t\t", cm));
                }
                foreach (var c in Operations.Read<Cashier>("db_cashier.json"))
                {
                    Console.WriteLine("( ) " + string.Join("\t\t", c));
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("");

            }
        }

        public static void drawUsersWin2(string id, string login, string passwd, string role)
        {
            Console.Clear();
            Console.WriteLine($"( ) ID: {id}\n( ) Логин: {login}\n( ) Пароль: {passwd}\n( ) Роль: {role}");
            Console.SetCursorPosition(0, 9);
            Console.WriteLine("F1 - Изменить запись(для сохранения нажмите S, для выхода без сохранения Esc) | Del - удалить | Esc - вернутся в меню");
        }
        public static void drawUserCreatewin()
        {
            Console.Clear();
            Console.WriteLine("( )ID:\n( )Логин:\n( )Пароль:\n( )Роль:\n( )Создать");
        }
        public static void drawSearchOptions()
        {
            Console.Clear();
            Console.WriteLine("Поиск по:");
            Console.WriteLine("( )ID\n( )Логин\n( )Пароль\n( )Роль");
        }
        public static void drawCargoWin()
        {
            Console.Clear();
        }
        public static void drawAccountantWin()
        {

            Console.Clear();
        }
    }
}
