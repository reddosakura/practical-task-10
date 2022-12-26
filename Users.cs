using Newtonsoft.Json;
using System.Xml.Serialization;
using System;
using System.Reflection;
using System.ComponentModel.Design;
using ConsoleuiNames;
using MainProgram;

namespace allUsers
{
    public class Operations
    {

        public static void Create<T>(T item, string dbname)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            if (File.Exists(path + "/db/" + dbname))
            {
                string jsondata = File.ReadAllText(path + "/db/" + dbname);
                var allData = JsonConvert.DeserializeObject<List<T>>(jsondata);
                allData.Add(item);
                //Console.WriteLine(allData[0].id);
                string savedata = JsonConvert.SerializeObject(allData);
                File.WriteAllText(path + "/db/" + dbname, savedata);
            }
            else
            {
                var newAllData = new List<T>();
                newAllData.Add(item);
                string savenew = JsonConvert.SerializeObject(newAllData);
                File.WriteAllText(path + "/db/" + dbname, savenew);

            }

        }
        public static List<List<object>> Read<T>(string dbname)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string jsondadta = File.ReadAllText(path + "/db/" + dbname);
            List<T> items = JsonConvert.DeserializeObject<List<T>>(jsondadta);
            List<FieldInfo[]> readData = new List<FieldInfo[]>();
            FieldInfo[] fields = typeof(T).GetFields();
            List<List<object>> allAttrs = new List<List<object>>();
            foreach (var i in items)
            {
                List<object> attrs = new List<object>();
                foreach (var f in fields)
                {
                    attrs.Add(f.GetValue(i));
                    //Console.WriteLine(f.GetValue(i));
                }
                allAttrs.Add(attrs);
            }
            return allAttrs;
        }
        public static void Update<T>(string dbname, int id, string attr, object nvalue)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string jsondata = File.ReadAllText(path + "/db/" + dbname);
            List<T> allData = JsonConvert.DeserializeObject<List<T>>(jsondata);
            foreach (var d in allData)
            {

                FieldInfo finfo = typeof(T).GetField("id");
                if ((int)finfo.GetValue(d) == id)
                {
                    FieldInfo nfield = typeof(T).GetField(attr);
                    nfield.SetValue(d, nvalue);
                    break;
                }
            }
            string savedata = JsonConvert.SerializeObject(allData);
            File.WriteAllText(path + "/db/" + dbname, savedata);
        }
        public static void Delete<T>(string dbname, int id)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string json = File.ReadAllText(path + "/db/" + dbname);
            List<T> allData = JsonConvert.DeserializeObject<List<T>>(json);
            foreach (var d in allData)
            {
                FieldInfo field = typeof(T).GetField("id");
                if ((int)field.GetValue(d) == id)
                {
                    allData.Remove(d);
                    break;
                }
            }
            string savedata = JsonConvert.SerializeObject(allData);
            File.WriteAllText(path + "/db/" + dbname, savedata);
        }
        //public static void Search<T>(string dbname, string attr, string searchvalue)
        //{
        //string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //string json = File.ReadAllText(path + "/db/" + dbname);
        //List<T> allData = JsonConvert.DeserializeObject<List<T>>(json);
        //}
    }
    public class Admin
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
            key_Bckspc = 8,
            key_S = 83,
            key_Del = 46
        }
        internal enum consts
        {
            m_min_c = 3,
            i_min_c = 0,
            s_min_c = 1,
            i_max = 4,
            step = 1,
            lstep = 1
        }
        public int id;
        public string login;
        public string passwd;
        public int role;
        public Admin(int _id, string _login, string _passwd)
        {
            id = _id;
            login = _login;
            passwd = _passwd;
            role = 0;
        }

        private static void keysProcessing(string lname)
        {
            int coord = (int)consts.m_min_c;
            //int max;
            List<string> output = new List<string>();
            List<List<object>> allUsers = new List<List<object>>();
            foreach (var a in Operations.Read<Admin>("db_admin.json"))
            {
                allUsers.Add(a);
            }
            foreach (var hr in Operations.Read<HRmanager>("db_hr.json"))
            {
                allUsers.Add(hr);
            }
            foreach (var acc in Operations.Read<Accountant>("db_account.json"))
            {
                allUsers.Add(acc);
            }
            foreach (var cm in Operations.Read<CargoManager>("db_cargo.json"))
            {
                allUsers.Add(cm);
            }
            foreach (var csh in Operations.Read<Cashier>("db_cashier.json"))
            {
                allUsers.Add(csh);
            }
            int max = allUsers.Count() + (int)consts.m_min_c;
            while (true)
            {
                Console.SetCursorPosition((int)consts.lstep, coord);
                Console.WriteLine("~");
                ConsoleKeyInfo key = Console.ReadKey(true);
                if ((int)key.Key == (int)keys.key_Uarr)
                {
                    if (coord - (int)consts.step >= (int)consts.m_min_c)
                    {
                        Console.SetCursorPosition((int)consts.lstep, coord);
                        Console.WriteLine(" ");
                        coord -= (int)consts.step;
                    }
                }
                else if ((int)key.Key == (int)keys.key_Darr)
                {
                    if (coord + (int)consts.step < max)
                    {
                        Console.SetCursorPosition((int)consts.lstep, coord);
                        Console.WriteLine(" ");
                        coord += (int)consts.step;
                    }
                }
                else if ((int)key.Key == (int)keys.key_Ent)
                {
                    ConsoleUi.drawUsersWin2(allUsers[coord - (int)consts.m_min_c][0].ToString(),
                            (string)allUsers[coord - (int)consts.m_min_c][1],
                            (string)allUsers[coord - (int)consts.m_min_c][2],
                            allUsers[coord - (int)consts.m_min_c][3].ToString());

                    int i_coord = (int)consts.i_min_c;

                    while (true)
                    {
                        Console.SetCursorPosition((int)consts.lstep, i_coord);
                        Console.WriteLine("#");
                        ConsoleKeyInfo i_key = Console.ReadKey(true);
                        if ((int)i_key.Key == (int)keys.key_Uarr)
                        {
                            if (i_coord - (int)consts.step >= (int)consts.i_min_c)
                            {
                                Console.SetCursorPosition((int)consts.lstep, i_coord);
                                Console.WriteLine(" ");
                                i_coord -= (int)consts.step;
                            }
                        }
                        else if ((int)i_key.Key == (int)keys.key_Darr)
                        {
                            if (i_coord + (int)consts.step < (int)consts.i_max)
                            {
                                Console.SetCursorPosition((int)consts.lstep, i_coord);
                                Console.WriteLine(" ");
                                i_coord += (int)consts.step;
                            }
                        }
                        else if ((int)i_key.Key == (int)keys.key_F1)
                        {
                            int intrcoord = 0;
                            string attr = null;
                            if (i_coord == 0)
                            {
                                intrcoord = 8;
                                attr = "id";
                            }
                            else if (i_coord == 1)
                            {
                                intrcoord = 11;
                                attr = "login";
                            }
                            else if (i_coord == 2)
                            {
                                intrcoord = 12;
                                attr = "passwd";
                            }
                            else if (i_coord == 3)
                            {
                                intrcoord = 10;
                                attr = "role";
                            }
                            int border = intrcoord;
                            int currRole = (int)allUsers[coord - (int)consts.m_min_c][3];
                            int currId = (int)allUsers[coord - (int)consts.m_min_c][0];
                            string login = (string)allUsers[coord - (int)consts.m_min_c][1];
                            string passwd = (string)allUsers[coord - (int)consts.m_min_c][2];
                            Console.SetCursorPosition(i_coord, intrcoord);
                            Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\t\t\t\t");
                            List<string> val = new List<string>();
                            while (true)
                            {
                                ConsoleKeyInfo intrkeys = Console.ReadKey(true);
                                if (((int)intrkeys.Key != (int)keys.key_Esc) && ((int)intrkeys.Key != (int)keys.key_Bckspc) && ((int)intrkeys.Key != (int)keys.key_S))
                                {
                                    Console.SetCursorPosition(intrcoord, i_coord);
                                    Console.Write(intrkeys.KeyChar);
                                    val.Add(intrkeys.KeyChar.ToString());
                                    intrcoord++;
                                }
                                else if ((int)intrkeys.Key == (int)keys.key_Bckspc)
                                {
                                    if (intrcoord - 1 >= border)
                                    {
                                        intrcoord--;
                                        Console.SetCursorPosition(intrcoord, i_coord);
                                        Console.Write(" ");
                                    }
                                    if (val.Count > 0)
                                    {
                                        val.Remove(val.Last());
                                    }

                                }
                                else if ((int)intrkeys.Key == (int)keys.key_Esc)
                                {
                                    output.Clear();
                                    //output.Add(string.Join("", val));
                                    break;
                                }
                                else if ((int)intrkeys.Key == (int)keys.key_S)
                                {
                                    output.Add(string.Join("", val));
                                    //Console.SetCursorPosition(0, 13);
                                    //Console.WriteLine($"ff {val[0]}");
                                    if (attr == "role")
                                    {
                                        allUsers[coord - (int)consts.m_min_c][3] = Convert.ToInt32(string.Join("", output));
                                    }
                                    else if (attr == "id")
                                    {
                                        allUsers[coord - (int)consts.m_min_c][0] = Convert.ToInt32(string.Join("", output));
                                    }
                                    else if (attr == "login")
                                    {
                                        allUsers[coord - (int)consts.m_min_c][1] = string.Join("", output);
                                    }
                                    else if (attr == "passwd")
                                    {
                                        allUsers[coord - (int)consts.m_min_c][2] = string.Join("", output);
                                    }
                                    if (attr == "role")
                                    {
                                        if (currRole == (int)roles.adm)
                                        {
                                            Operations.Delete<Admin>("db_admin.json", currId); // написать try catch
                                            if (Convert.ToInt32(string.Join("", output)) == (int)roles.cm)
                                            {
                                                Operations.Create(new CargoManager(currId, login, passwd), "db_cargo.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.csh)
                                            {
                                                Operations.Create(new Cashier(currId, login, passwd), "db_cashier.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.hrm)
                                            {
                                                Operations.Create(new HRmanager(currId, login, passwd), "db_hr.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.accnt)
                                            {
                                                Operations.Create(new Accountant(currId, login, passwd), "db_account.json");
                                            }
                                        }
                                        else if (currRole == (int)roles.hrm)
                                        {
                                            Operations.Delete<HRmanager>("db_hr.json", currId); // написать try catch
                                            if (Convert.ToInt32(string.Join("", output)) == (int)roles.cm)
                                            {
                                                Operations.Create(new CargoManager(currId, login, passwd), "db_cargo.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.csh)
                                            {
                                                Operations.Create(new Cashier(currId, login, passwd), "db_cashier.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.adm)
                                            {
                                                Operations.Create(new Admin(currId, login, passwd), "db_admin.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.accnt)
                                            {
                                                Operations.Create(new Accountant(currId, login, passwd), "db_account.json");
                                            }
                                        }
                                        else if (currRole == (int)roles.csh)
                                        {
                                            Operations.Delete<Cashier>("db_cashier.json", currId); // написать try catch
                                            if (Convert.ToInt32(string.Join("", output)) == (int)roles.cm)
                                            {
                                                Operations.Create(new CargoManager(currId, login, passwd), "db_cargo.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.csh)
                                            {
                                                Operations.Create(new Admin(currId, login, passwd), "db_admin.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.hrm)
                                            {
                                                Operations.Create(new HRmanager(currId, login, passwd), "db_hr.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.accnt)
                                            {
                                                Operations.Create(new Accountant(currId, login, passwd), "db_account.json");
                                            }
                                        }
                                        else if (currRole == (int)roles.accnt)
                                        {
                                            Operations.Delete<Accountant>("db_account.json", currId); // написать try catch
                                            if (Convert.ToInt32(string.Join("", output)) == (int)roles.cm)
                                            {
                                                Operations.Create(new CargoManager(currId, login, passwd), "db_cargo.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.csh)
                                            {
                                                Operations.Create(new Cashier(currId, login, passwd), "db_cashier.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.hrm)
                                            {
                                                Operations.Create(new HRmanager(currId, login, passwd), "db_hr.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.accnt)
                                            {
                                                Operations.Create(new Admin(currId, login, passwd), "db_admin.json");
                                            }
                                        }
                                        else if (currRole == (int)roles.cm)
                                        {
                                            Operations.Delete<CargoManager>("db_cargo.json", currId); // написать try catch
                                            if (Convert.ToInt32(string.Join("", output)) == (int)roles.cm)
                                            {
                                                Operations.Create(new Admin(currId, login, passwd), "db_admin.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.csh)
                                            {
                                                Operations.Create(new Cashier(currId, login, passwd), "db_cashier.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.hrm)
                                            {
                                                Operations.Create(new HRmanager(currId, login, passwd), "db_hr.json");
                                            }
                                            else if (Convert.ToInt32(string.Join("", output)) == (int)roles.accnt)
                                            {
                                                Operations.Create(new Accountant(currId, login, passwd), "db_account.json");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (currRole == (int)roles.adm)
                                        {
                                            try
                                            {
                                                Operations.Update<Admin>("db_admin.json", currId, attr, string.Join("", output));
                                            }
                                            catch (System.ArgumentException)
                                            {

                                                Operations.Update<Admin>("db_admin.json", currId, attr, Convert.ToInt32(string.Join("", output)));
                                            }
                                        }
                                        else if (currRole == (int)roles.hrm)
                                        {
                                            try
                                            {
                                                Operations.Update<HRmanager>("db_hr.json", currId, attr, string.Join("", output));
                                            }
                                            catch (System.ArgumentException)
                                            {

                                                Operations.Update<HRmanager>("db_hr.json", currId, attr, Convert.ToInt32(string.Join("", output)));
                                            }
                                        }
                                        else if (currRole == (int)roles.accnt)
                                        {
                                            try
                                            {
                                                Operations.Update<Accountant>("db_account.json", currId, attr, string.Join("", output));
                                            }
                                            catch (System.ArgumentException)
                                            {

                                                Operations.Update<Accountant>("db_account.json", currId, attr, Convert.ToInt32(string.Join("", output)));
                                            }
                                        }
                                        else if (currRole == (int)roles.cm)
                                        {
                                            try
                                            {
                                                Operations.Update<CargoManager>("db_cargo.json", currId, attr, string.Join("", output));
                                            }
                                            catch (System.ArgumentException)
                                            {

                                                Operations.Update<CargoManager>("db_cargo.json", currId, attr, Convert.ToInt32(string.Join("", output)));
                                            }
                                        }
                                        else if (currRole == (int)roles.csh)
                                        {
                                            try
                                            {
                                                Operations.Update<Cashier>("db_cashier.json", currId, attr, string.Join("", output));
                                            }
                                            catch (System.ArgumentException)
                                            {

                                                Operations.Update<Cashier>("db_cashier.json", currId, attr, Convert.ToInt32(string.Join("", output)));
                                            }
                                        }
                                    }
                                    output.Clear();
                                    break;
                                }
                            }
                        }
                        else if ((int)i_key.Key == (int)keys.key_Del)
                        {
                            int currId = (int)allUsers[coord - (int)consts.m_min_c][0];
                            int currRole = (int)allUsers[coord - (int)consts.m_min_c][3];
                            if (currRole == (int)roles.adm)
                            {
                                Operations.Delete<Admin>("db_admin.json", currId);
                            }
                            else if (currRole == (int)roles.hrm)
                            {

                                Operations.Delete<HRmanager>("db_hr.json", currId);
                            }
                            else if (currRole == (int)roles.accnt)
                            {

                                Operations.Delete<Accountant>("db_account.json", currId);
                            }
                            else if (currRole == (int)roles.cm)
                            {

                                Operations.Delete<CargoManager>("db_cargo.json", currId);
                            }
                            else if (currRole == (int)roles.csh)
                            {

                                Operations.Delete<Cashier>("db_cashier.json", currId);
                            }
                            allUsers.Remove(allUsers[coord - (int)consts.m_min_c]);
                            max--;
                            ConsoleUi.drawUsersWin1(lname);
                            break;
                        }
                        else if ((int)i_key.Key == (int)keys.key_Esc)
                        {
                            ConsoleUi.drawUsersWin1(lname);
                            break;
                        }

                    }
                }
                else if ((int)key.Key == (int)keys.key_F1)
                {
                    ConsoleUi.drawUserCreatewin();
                    List<string> data = new List<string>();
                    int i_coord = (int)consts.i_min_c;
                    List<string> id = new List<string>();
                    List<string> login = new List<string>();
                    List<string> passwd = new List<string>();
                    List<string> role = new List<string>();

                    while (true)
                    {
                        Console.SetCursorPosition((int)consts.lstep, i_coord);
                        Console.WriteLine("$");
                        ConsoleKeyInfo k = Console.ReadKey(true);

                        //ConsoleKeyInfo key = Console.ReadKey(true);
                        if ((int)k.Key == (int)keys.key_Uarr)
                        {
                            if (i_coord - (int)consts.step >= (int)consts.i_min_c)
                            {
                                Console.SetCursorPosition((int)consts.lstep, i_coord);
                                Console.WriteLine(" ");
                                i_coord -= (int)consts.step;
                            }
                        }
                        else if ((int)k.Key == (int)keys.key_Darr)
                        {
                            if (i_coord + (int)consts.step < 5)
                            {
                                Console.SetCursorPosition((int)consts.lstep, i_coord);
                                Console.WriteLine(" ");
                                i_coord += (int)consts.step;
                            }
                        }
                        else if ((int)k.Key == (int)keys.key_Ent)
                        {
                            int intrcoord = 0;
                            if (i_coord == 0)
                            {
                                intrcoord = 8;
                                int border = intrcoord;
                                List<string> val = new List<string>();
                                while (true)
                                {
                                    ConsoleKeyInfo input = Console.ReadKey(true);
                                    if (((int)input.Key != (int)keys.key_Esc) && ((int)input.Key != (int)keys.key_Bckspc) && ((int)input.Key != (int)keys.key_S))
                                    {
                                        Console.SetCursorPosition(intrcoord, i_coord);
                                        Console.Write(input.KeyChar);
                                        val.Add(input.KeyChar.ToString());
                                        intrcoord++;
                                    }
                                    else if ((int)input.Key == (int)keys.key_Bckspc)
                                    {
                                        if (intrcoord - 1 >= border)
                                        {
                                            intrcoord--;
                                            Console.SetCursorPosition(intrcoord, i_coord);
                                            Console.Write(" ");
                                        }
                                        if (val.Count > 0)
                                        {
                                            val.Remove(val.Last());
                                        }
                                    }
                                    else if ((int)input.Key == (int)keys.key_Esc)
                                    {
                                        data.Add(string.Join("", val));
                                        break;
                                    }
                                }
                            }
                            else if (i_coord == 1)
                            {
                                intrcoord = 11;
                                int border = intrcoord;
                                List<string> val = new List<string>();
                                while (true)
                                {
                                    ConsoleKeyInfo input = Console.ReadKey(true);
                                    if (((int)input.Key != (int)keys.key_Esc) && ((int)input.Key != (int)keys.key_Bckspc))
                                    {
                                        Console.SetCursorPosition(intrcoord, i_coord);
                                        Console.Write(input.KeyChar);
                                        val.Add(input.KeyChar.ToString());
                                        intrcoord++;
                                    }
                                    else if ((int)input.Key == (int)keys.key_Bckspc)
                                    {
                                        if (intrcoord - 1 >= border)
                                        {
                                            intrcoord--;
                                            Console.SetCursorPosition(intrcoord, i_coord);
                                            Console.Write(" ");
                                        }
                                        if (val.Count > 0)
                                        {
                                            val.Remove(val.Last());
                                        }
                                    }
                                    else if ((int)input.Key == (int)keys.key_Esc)
                                    {
                                        data.Add(string.Join("", val));
                                        break;
                                    }
                                }
                            }
                            else if (i_coord == 2)
                            {
                                intrcoord = 12;
                                int border = intrcoord;
                                List<string> val = new List<string>();
                                while (true)
                                {
                                    ConsoleKeyInfo input = Console.ReadKey(true);
                                    if (((int)input.Key != (int)keys.key_Esc) && ((int)input.Key != (int)keys.key_Bckspc))
                                    {
                                        Console.SetCursorPosition(intrcoord, i_coord);
                                        Console.Write(input.KeyChar);
                                        val.Add(input.KeyChar.ToString());
                                        intrcoord++;
                                    }
                                    else if ((int)input.Key == (int)keys.key_Bckspc)
                                    {
                                        if (intrcoord - 1 >= border)
                                        {
                                            intrcoord--;
                                            Console.SetCursorPosition(intrcoord, i_coord);
                                            Console.Write(" ");
                                        }
                                        if (val.Count > 0)
                                        {
                                            val.Remove(val.Last());
                                        }
                                    }
                                    else if ((int)input.Key == (int)keys.key_Esc)
                                    {
                                        data.Add(string.Join("", val));
                                        break;
                                    }
                                }
                            }
                            else if (i_coord == 3)
                            {
                                intrcoord = 10;
                                int border = intrcoord;
                                List<string> val = new List<string>();
                                while (true)
                                {
                                    ConsoleKeyInfo input = Console.ReadKey(true);
                                    if (((int)input.Key != (int)keys.key_Esc) && ((int)input.Key != (int)keys.key_Bckspc))
                                    {
                                        Console.SetCursorPosition(intrcoord, i_coord);
                                        Console.Write(input.KeyChar);
                                        val.Add(input.KeyChar.ToString());
                                        intrcoord++;
                                    }
                                    else if ((int)input.Key == (int)keys.key_Bckspc)
                                    {
                                        if (intrcoord - 1 >= border)
                                        {
                                            intrcoord--;
                                            Console.SetCursorPosition(intrcoord, i_coord);
                                            Console.Write(" ");
                                        }
                                        if (val.Count > 0)
                                        {
                                            val.Remove(val.Last());
                                        }
                                    }
                                    else if ((int)input.Key == (int)keys.key_Esc)
                                    {
                                        data.Add(string.Join("", val));
                                        break;
                                    }
                                }
                            }
                            else if (i_coord == 4)
                            {
                                max += 1;
                                List<object> user = new List<object>();
                                user.Add(Convert.ToInt32(data[0]));
                                user.Add(data[1]);
                                user.Add(data[2]);
                                user.Add(Convert.ToInt32(data[3]));
                                allUsers.Add(user);
                                if (Convert.ToInt32(data[3]) == (int)roles.adm)
                                {
                                    Operations.Create(new Admin(Convert.ToInt32(data[0]), data[1], data[2]), "db_admin.json");
                                }
                                else if (Convert.ToInt32(data[3]) == (int)roles.hrm)
                                {

                                    Operations.Create(new HRmanager(Convert.ToInt32(data[0]), data[1], data[2]), "db_hr.json");
                                }
                                else if (Convert.ToInt32(data[3]) == (int)roles.accnt)
                                {
                                    Operations.Create(new Accountant(Convert.ToInt32(data[0]), data[1], data[2]), "db_account.json");
                                }
                                else if (Convert.ToInt32(data[3]) == (int)roles.cm)
                                {
                                    Operations.Create(new CargoManager(Convert.ToInt32(data[0]), data[1], data[2]), "db_cargo.json");
                                }
                                else if (Convert.ToInt32(data[3]) == (int)roles.csh)
                                {

                                    Operations.Create(new Cashier(Convert.ToInt32(data[0]), data[1], data[2]), "db_cashier.json");
                                }
                                ConsoleUi.drawUsersWin1(lname);
                                break;
                            }
                        }
                    }
                }
                else if ((int)key.Key == (int)keys.key_F2)
                {
                    ConsoleUi.drawSearchOptions();
                    int c = (int)consts.s_min_c;
                    while (true)
                    {
                        Console.SetCursorPosition((int)consts.lstep, c);
                        Console.WriteLine("@");
                        ConsoleKeyInfo k = Console.ReadKey(true);
                        if ((int)k.Key == (int)keys.key_Uarr)
                        {
                            if (c - (int)consts.step >= (int)consts.s_min_c)
                            {
                                Console.SetCursorPosition((int)consts.lstep, c);
                                Console.WriteLine(" ");
                                c -= (int)consts.step;
                            }
                        }
                        else if ((int)k.Key == (int)keys.key_Darr)
                        {
                            if (c + (int)consts.step < 5)
                            {
                                Console.SetCursorPosition((int)consts.lstep, c);
                                Console.WriteLine(" ");
                                c += (int)consts.step;
                            }
                        }
                        else if ((int)k.Key == (int)keys.key_Ent)
                        {
                            Console.SetCursorPosition(0, 10);
                            Console.WriteLine("Введите поисковое значение:");
                            string req = Console.ReadLine();
                            int searchindex = c - 1;
                            List<List<object>> finditems = new List<List<object>>();
                            foreach (var u in allUsers)
                            {
                                if (u[searchindex].ToString() == req)
                                {
                                    finditems.Add(u);
                                }
                            }

                            Console.SetCursorPosition(0, 15);
                            foreach (var i in finditems)
                            {
                                Console.WriteLine(string.Join("\t\t", i));
                            }
                        }
                        else if ((int)k.Key == (int)keys.key_Esc)
                        {
                            ConsoleUi.drawUsersWin1(lname);
                            break;
                        }
                    }
                }
                else if ((int)key.Key == (int)keys.key_Esc)
                {
                    ConsoleUi.drawAuthWin();
                    MainSys.AuthL();
                    break;
                }
            }
        }
        public static void initAdminFuncs(string name)
        {
            ConsoleUi.drawUsersWin1(name);
            keysProcessing(name);
        }
    }
    public class HRmanager
    {
        public int id;
        public string login;
        public string passwd;
        public int role;
        public HRmanager(int _id, string _login, string _passwd)
        {
            id = _id;
            login = _login;
            passwd = _passwd;
            role = 1;
        }
        public static void initHrFuncs(string name)
        {
            ConsoleUi.drawHrMainWin(name);
        }

    }
    public class CargoManager
    {
        public int id;
        public string login;
        public string passwd;
        public int role;
        public CargoManager(int _id, string _login, string _passwd)
        {
            id = _id;
            login = _login;
            passwd = _passwd;
            role = 2;
        }
        public static void initCargoFuncs()
        {
            Console.Clear();
            Console.WriteLine("Функционал склада");

        }

    }
    public class Cashier
    {
        public int id;
        public string login;
        public string passwd;
        public int role;
        public Cashier(int _id, string _login, string _passwd)
        {
            id = _id;
            login = _login;
            passwd = _passwd;
            role = 3;
        }
        public static void initCashFuncs()
        {
            Console.Clear();
            Console.WriteLine("Функционал кассира");

        }
    }
    public class Accountant
    {
        public int id;
        public string login;
        public string passwd;
        public int role;
        public Accountant(int _id, string _login, string _passwd)
        {
            id = _id;
            login = _login;
            passwd = _passwd;
            role = 4;
        }
        public static void initAccntntFuncs()
        {
            Console.Clear();
            Console.WriteLine("Функционал Бухгалтера");

        }
    }
    public class Worker
    {
        public int id, paspdata, salary, specid;
        public string name, lastname, patronymic, spec;
        public string birthdate;

        public Worker(int _id, int _paspdata, int _salary, int _specid, string _name, string _lastname, string _patronymic, string _spec, string _birthdate)
        {
            id = _id;
            paspdata = _paspdata;
            salary = _salary;
            specid = _specid;
            name = _name;
            lastname = _lastname;
            patronymic = _patronymic;
            spec = _spec;
            birthdate = _birthdate;
        }
    }
}
