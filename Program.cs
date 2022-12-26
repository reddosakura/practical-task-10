using ConsoleuiNames;
using allUsers;
using KeyEvents;
using AuthTools;

namespace MainProgram
{
    class MainSys
    {
        internal enum roles
        {
            adm = 0,
            hrm = 1,
            cm = 2,
            csh = 3,
            accnt = 4
        }
        internal enum keys
        {
            key_Darr = 40,
            key_Uarr = 38,
            key_Ent = 13,
            key_Esc = 27,
            key_F1 = 112,
            key_F2 = 113,
            key_Bckspc = 8
        }
        public static void AuthL()
        {


            int coord = 0;
            //int mincoord = 0;
            int leftstep = 0;
            int step = 1;
            string selector = "->";
            int min = 0;
            int max = 3;
            List<string> output = new List<string>();
            while (true)
            {
                Console.SetCursorPosition(leftstep, coord);
                Console.WriteLine(selector);
                ConsoleKeyInfo key = Console.ReadKey(true);
                if ((int)key.Key == (int)keys.key_Uarr)
                {
                    if (coord - step >= min)
                    {
                        Console.SetCursorPosition(leftstep, coord);
                        Console.WriteLine("  ");
                        coord -= step;
                    }
                }
                else if ((int)key.Key == (int)keys.key_Darr)
                {
                    if (coord + step < max)
                    {
                        Console.SetCursorPosition(leftstep, coord);
                        Console.WriteLine("  ");
                        coord += step;

                    }
                }
                else if ((int)key.Key == (int)keys.key_Ent)
                {
                    // Console.Clear();
                    if (coord == 0)
                    {

                        int intrcoord = 8;
                        List<string> letLogin = new List<string>();
                        while (true)
                        {
                            ConsoleKeyInfo intrkeys = Console.ReadKey(true);
                            if (((int)intrkeys.Key != (int)keys.key_Esc) && ((int)intrkeys.Key != (int)keys.key_Bckspc))
                            {
                                Console.SetCursorPosition(intrcoord, 0);
                                Console.Write(intrkeys.KeyChar);
                                letLogin.Add(intrkeys.KeyChar.ToString());
                                intrcoord++;
                            }
                            else if ((int)intrkeys.Key == (int)keys.key_Bckspc)
                            {
                                if (intrcoord - 1 >= 8)
                                {

                                    intrcoord--;
                                    Console.SetCursorPosition(intrcoord, 0);
                                    Console.Write(" ");
                                }
                                if (letLogin.Count > 0)
                                {
                                    letLogin.Remove(letLogin.Last());
                                }

                            }
                            else
                            {
                                output.Add(string.Join("", letLogin));
                                break;
                            }
                        }
                    }
                    else if (coord == 1)
                    {
                        int intrcoord = 9;
                        List<string> letPasswd = new List<string>();
                        while (true)
                        {
                            ConsoleKeyInfo intrkeys = Console.ReadKey(true);
                            if (((int)intrkeys.Key != (int)keys.key_Esc) && ((int)intrkeys.Key != (int)keys.key_Bckspc))
                            {
                                Console.SetCursorPosition(intrcoord, 1);
                                Console.Write("*");
                                letPasswd.Add(intrkeys.KeyChar.ToString());
                                intrcoord++;
                            }
                            else if ((int)intrkeys.Key == (int)keys.key_Bckspc)
                            {
                                if (intrcoord - 1 >= 9)
                                {

                                    intrcoord--;
                                    Console.SetCursorPosition(intrcoord, 1);
                                    Console.Write(" ");
                                }
                                if (letPasswd.Count - 1 >= 0)
                                {
                                    letPasswd.Remove(letPasswd[letPasswd.Count - 1]);
                                }

                            }
                            else
                            {
                                output.Add(string.Join("", letPasswd));
                                break;
                            }
                        }

                    }
                    else if (coord == 2)
                    {
                        //Console.WriteLine(string.Join(" ", output));
                        if (output.Count < 2)
                        {
                            Console.SetCursorPosition(0, max + 1);
                            Console.WriteLine("Не хватает данных для авторизации");
                        }
                        else
                        {
                            if (Auth.CheckLogin<Admin>(output[0], "db_admin.json") != null)
                            {
                                //string passwd = Auth.CheckLogin<Admin>(output[0], "db_admin.json");
                                if (Auth.CheckPassword<Admin>(output[1], "db_admin.json"))
                                {
                                    Admin.initAdminFuncs();
                                    break;
                                }
                                else
                                {
                                    Console.SetCursorPosition(0, max + 1);
                                    Console.WriteLine("Неверный пароль");
                                }

                            }
                            else if (Auth.CheckLogin<HRmanager>(output[0], "db_hr.json") != null)
                            {
                                if (Auth.CheckPassword<HRmanager>(output[1], "db_hr.json"))
                                {
                                    HRmanager.initHrFuncs();
                                    break;
                                }
                                else
                                {
                                    Console.SetCursorPosition(0, max + 1);
                                    Console.WriteLine("Неверный пароль");

                                }
                            }
                            else if (Auth.CheckLogin<CargoManager>(output[0], "db_cargo.json") != null)
                            {
                                if (Auth.CheckPassword<CargoManager>(output[1], "db_cargo.json"))
                                {
                                    CargoManager.initCargoFuncs();
                                    break;
                                }
                                else
                                {
                                    Console.SetCursorPosition(0, max + 1);
                                    Console.WriteLine("Неверный пароль");

                                }
                            }
                            else if (Auth.CheckLogin<Accountant>(output[0], "db_account.json") != null)
                            {
                                if (Auth.CheckPassword<Accountant>(output[1], "db_account.json"))
                                {
                                    Accountant.initAccntntFuncs();
                                    break;
                                }
                                else
                                {
                                    Console.SetCursorPosition(0, max + 1);
                                    Console.WriteLine("Неверный пароль");
                                }

                            }
                            else if (Auth.CheckLogin<Cashier>(output[0], "db_cashier.json") != null)
                            {
                                if (Auth.CheckPassword<Cashier>(output[1], "db_cashier.json"))
                                {
                                    Cashier.initCashFuncs();
                                    break;
                                }
                                else
                                {
                                    Console.SetCursorPosition(0, max + 1);
                                    Console.WriteLine("Неверный пароль");
                                }
                            }
                            else
                            {
                                output.Clear();
                                Console.SetCursorPosition(0, max + 1);
                                Console.WriteLine("Пользователя не существует");

                            }
                        }
                    }
                }
                else if ((int)key.Key == (int)keys.key_Esc)
                {
                    break;
                }
            }
        }
        private static void init()
        {
            ConsoleUi.drawAuthWin();
            AuthL();

        }
        private static void Main()
        {
            init();
        }
    }
}

//Console.WriteLine(Auth.CheckLogin<Admin>("admin3", "db_admin.json"));
//Operations.Read<Admin>("db_admin.json");
//Operations.Delete<Admin>("db_admin.json", 2);
//Operations.Update<CargoManager>("db_cargo.json", 1, "passwd", "pass");
//Operations.Create(new Admin(2, "admin1", "admin"), "db_admin.json");
//Operations.Create(new HRmanager(2, "hr1", "hr"), "db_hr.json");
//Operations.Create(new CargoManager(2, "cargo1", "cargo"), "db_cargo.json");
//Operations.Create(new Cashier(2, "cash1", "cash"), "db_cashier.json");
//Operations.Create(new Accountant(2, "account1", "account"), "db_account.json");
// ConsoleUi.drawUsersWin1();
//Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
// ConsoleUi.drawAuthWin();
//Arrows arr = new Arrows(0, 3);
//arr.ShowArrow(0, 0, 1, "->");
//Arrows.ShowArrow(0, 0, 1, "->");
