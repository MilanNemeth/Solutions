#define DEBUG
#undef DEBUG

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cinemas
{
    class PresentationLayer //Sorry, but it became an ugly freak...     8|
    {
        #region Members
        static int index = 0;
        static int cinemaCounter = 0;
        static Cinema activeCinema;
        static Auditorium activeAuditorium;
        static Projection activeProjection;
        static string[] mostViewedProjectionData = new string[2];
        static int offset = 40;
        static int sleepTime = 1000;
        static string appName = AppDomain.CurrentDomain.FriendlyName.ToString().Replace(".exe","");
        struct MenuTitle
        {
            string titleName;
            public int Length;
            public static implicit operator MenuTitle(string value)
            {
                return new MenuTitle() { titleName = value.ToUpper(), Length = value.Length };
            }
            public override string ToString()
            {
                return titleName;
            }
        }
        struct MenuItem
        {
            string itemName;
            public int Length;
            public static implicit operator MenuItem(string value)
            {
                return new MenuItem() { itemName = value, Length = value.Length };
            }
            public override string ToString()
            {
                return itemName;
            }
        }
        delegate void MenuMethod();
        static Dictionary<int, MenuMethod> MainMenuMethods = new Dictionary<int, MenuMethod>();
        static Dictionary<int, MenuMethod> CinemasMenuMethods = new Dictionary<int, MenuMethod>();
        static Dictionary<int, MenuMethod> MostViewedProjectionMenuMethods = new Dictionary<int, MenuMethod>();
        static Dictionary<int, MenuMethod> InCinemaMenuMethods = new Dictionary<int, MenuMethod>();
        static Dictionary<int, MenuMethod> InAuditoriumMenuMethods = new Dictionary<int, MenuMethod>();
        static Dictionary<int, MenuMethod> InProjectionMenuMethods = new Dictionary<int, MenuMethod>();
        static Dictionary<int, MenuMethod> ReservationMenuMethods = new Dictionary<int, MenuMethod>();
        static Dictionary<string, Dictionary<int, MenuMethod>> DelegateDictionaries = new Dictionary<string, Dictionary<int, MenuMethod>>();
        #endregion

        public PresentationLayer()
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.WindowWidth = 2 * offset;
            Console.Title = appName;

            MainMenuMethods[0] = NewCinemaMenu;
            MainMenuMethods[1] = CinemasMenu;
            MainMenuMethods[2] = MostViewedProjectionMenu;
            MostViewedProjectionMenuMethods[0] = delegate () { IO_Handler.SaveToFile(mostViewedProjectionData); };//Western technique

            DelegateDictionaries["MainMenu"] = MainMenuMethods;
            DelegateDictionaries["CinemasMenu"] = CinemasMenuMethods;
            DelegateDictionaries["MostViewedProjectionMenu"] = MostViewedProjectionMenuMethods;
            DelegateDictionaries["InCinemaMenu"] = InCinemaMenuMethods;
            DelegateDictionaries["InAuditoriumMenu"] = InAuditoriumMenuMethods;
            DelegateDictionaries["InProjectionMenu"] = InProjectionMenuMethods;
            DelegateDictionaries["ReservationMenu"] = ReservationMenuMethods;
            InfoMenu();
            MainMenu();
        }

        #region Menus
        void MainMenu()
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            MenuTitle mainMenu = "MAIN MENU";
            MenuItem newCinema = "-Create New Cinema-";
            MenuItem cinemas = "Cinemas Menu";
            MenuItem mostProjection = "Most Viewed Projection";
            MenuItem[] MainMenuItems = { newCinema, cinemas, mostProjection };
            KeepDoingMenu(mainMenu, MainMenuItems);
        }
        //In Main:
        void NewCinemaMenu()
        {
            MenuTitle newCinemaMenu = "New Cinema Menu";
            MenuItem[] newCinemaMenuItems = { };
            ShowMenu(newCinemaMenu, newCinemaMenuItems);
            string cinemaName = IO_Handler.EnterString("Please, enter the name of the new cinema: ");
            if (ObjectContainer.CDB.Contains(ObjectContainer.CDB.Find(i => i.Name == cinemaName.ToUpper())))
            {
                throw new Exception("This Cinema has already exists!");
            }
            byte auditoriumCount = IO_Handler.EnterByte("Please, enter the number of auditoriums: ");
            new Cinema(cinemaName, auditoriumCount);
            CinemasMenuMethods[cinemaCounter++] = InCinemaMenu;
            IO_Handler.SuccessMessage($"New Cinema: \"{cinemaName}\" has been created with {auditoriumCount} Auditorium{(auditoriumCount>1?"s":"")} in it.");
            Thread.Sleep(sleepTime);
        }
        void CinemasMenu()
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            MenuTitle cinemasMenu = "Cinemas";
            MenuItem[] cinemasMenuItems = ListToMenuItemArray(ObjectContainer.CDB);
            KeepDoingMenu(cinemasMenu, cinemasMenuItems);
        }
        void MostViewedProjectionMenu()
        {
            MenuTitle mostViewedProjectionMenu = "Most Viewed Projection";
            MenuItem[] mostViewedProjectionMenuItems = { "Save to file" };
            try
            {
                if (ObjectContainer.FindProjectionMostViewed().ReservedSeatsCount < 1)
                {
                    throw new Exception("Nothing to show");
                }
                Projection mostViewed = ObjectContainer.FindProjectionMostViewed();
                string[] mostViewedData = {
                $"{mostViewed.OwnMovie} ({mostViewed.OwnMovie.MinutesOfLength})",
                $"On schedule: \n{mostViewed.OwnerAuditorium} in {mostViewed.OwnerAuditorium.OwnerCinema}"
                };
                Console.WriteLine(mostViewedData[0]);
                Console.WriteLine(mostViewedData[1]);
                mostViewedProjectionData = mostViewedData;
                KeepDoingMenu(mostViewedProjectionMenu, mostViewedProjectionMenuItems);
            }
            catch (Exception)
            {
                Console.WriteLine("\n\n");
                Console.WriteLine("Nothing to show yet.".PadLeft(offset + 10));
                Thread.Sleep(1500);
            }
        }
        //In Subs:
        void InCinemaMenu()
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
            Thread.Sleep(500);
#endif
            #endregion
            MenuTitle inCinemaMenu = "In Cinema: ";
            MenuItem[] inCinemaMenuItems = DictToMenuItemArray(ObjectContainer.CDB[index].OwnAuditoriums);
            string currentCinema = inCinemaMenu+ObjectContainer.CDB[index].Name;
            activeCinema = ObjectContainer.CDB[index];
            for (int i = 0; i < activeCinema.OwnAuditoriums.Count; i++)
            {
                InCinemaMenuMethods[i] = InAuditoriumMenu;
            }
            KeepDoingMenu(currentCinema, inCinemaMenuItems);
        }
        void InAuditoriumMenu()
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
            Thread.Sleep(500);
#endif
            #endregion
            MenuTitle inAuditoriumMenu = "In ";
            MenuItem[] inAuditoriumMenuItems = { "-Add Nem Projection-", "Projections" };
            InAuditoriumMenuMethods[0] = activeAuditorium.AddNewProjection;
            InAuditoriumMenuMethods[1] = InProjectionMenu;
            string currentAuditorium = inAuditoriumMenu + activeCinema.OwnAuditoriums[(byte)(index+1)].ToString()+$"@{ activeCinema}";
            KeepDoingMenu(currentAuditorium, inAuditoriumMenuItems);
        }
        void InProjectionMenu()
        {
            MenuTitle inProjectionMenu = $"Projections of {activeAuditorium}@{activeCinema}";
            //MenuTitle current = inProjectionMenu+$"{activeAuditorium}@{activeCinema}";
            MenuItem[] inProjectionMenuItems = DictToMenuItemArray(activeAuditorium.OwnProjections);
            for (int i = 0; i < inProjectionMenuItems.Length; i++)
            {
                InProjectionMenuMethods[i] = ReservationMenu;
            }
            KeepDoingMenu(inProjectionMenu, inProjectionMenuItems);
        }
        void ReservationMenu()
        {
            MenuTitle inProjectionMenu = $"Reservations for {activeProjection.OwnMovie}({activeProjection.OwnMovie.MinutesOfLength})";
            MenuItem[] inProjectionMenuItems = { "-Reserve Seat-", "-Free Seat-", "-Check Seat Availability-" };
            ReservationMenuMethods[0] = activeProjection.ReserveSeat;
            ReservationMenuMethods[1] = activeProjection.FreeSeat;
            ReservationMenuMethods[2] = delegate () { activeProjection.PrintOwnSeatsByAvailability(); Console.WriteLine("\n\nPress any key to return..."); };
            KeepDoingMenu(inProjectionMenu, inProjectionMenuItems);
        }
        #endregion

        #region control+helper functions
        void InfoMenu()
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Contorols:");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter menu: EnterKey \nQuit menu: EscapeKey \nNavigate up: UpArrow \nNavigate down: DownArrow \nFirst Menu Item: PageUp \nLast Menu Item: PageDown \n\n");
            Console.ResetColor();
            Console.WriteLine("\tPress any key to continue!");
            Console.ReadKey(false);
            Console.Clear();
        }
        void KeepDoingMenu(MenuTitle menuTitle, MenuItem[] MenuItems)
        {
            index = 0;
            bool run = true;
            while (run)
            {
                ShowMenu(menuTitle, MenuItems);
                run = ControlMenu(MenuItems);
                Console.Clear();
            }
        }
        void ShowMenu(MenuTitle menuTitle, MenuItem[] MenuItems)
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(menuTitle.ToString().PadLeft(offset + menuTitle.Length / 2));
            Console.WriteLine();
            Console.ResetColor();
            for (int i = 0; i < MenuItems.Length; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(MenuItems[i].ToString().PadLeft(offset + MenuItems[i].Length / 2));
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(MenuItems[i].ToString().PadLeft(offset + MenuItems[i].Length / 2));
                }
            }
        }
        bool ControlMenu(MenuItem[] MenuItems)
        {
            #region debug message
            #if DEBUG
            IO_Handler.LogItsCaller();
            #endif
            #endregion
            bool invalid = true;
            while (invalid)
            {
                ConsoleKey ck = Console.ReadKey(false).Key;
                switch (ck)
                {
                    case ConsoleKey.UpArrow:
                        if (--index == -1) index = MenuItems.Length - 1;
                        goto case ConsoleKey.Clear;
                    case ConsoleKey.DownArrow:
                        if (++index == MenuItems.Length) index = 0;
                        goto case ConsoleKey.Clear;
                    case ConsoleKey.PageDown:
                        index = MenuItems.Length - 1;
                        goto case ConsoleKey.Clear;
                    case ConsoleKey.PageUp:
                        index = 0;
                        goto case ConsoleKey.Clear;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        try
                        {
                            StackFrame sf = new StackFrame(2);
                            string caller_lvl2 = sf.GetMethod().Name;
                            if (caller_lvl2.Equals("InCinemaMenu"))
                            {
                                try
                                {
                                    activeAuditorium = activeCinema.OwnAuditoriums[(byte)(index + 1)];
                                    #region DEBUG
#if DEBUG
                                    IO_Handler.SuccessMessage("ACTIVE AUDITORIUM GOT SET NOW!");
                                    Thread.Sleep(800);
#endif
                                    #endregion
                                }
                                catch (Exception e)
                                {
                                    IO_Handler.ErrorMessage($"active auditorium not set:\nLOC-{(new StackFrame(1, true)).GetFileLineNumber()}\n" + e.Message);
                                    Thread.Sleep(sleepTime);
                                }
                            }
                            if (caller_lvl2.Equals("InProjectionMenu"))
                            {
                                try
                                {
                                    activeProjection = activeAuditorium.OwnProjections.Skip(index).First().Value;
                                }
                                catch (Exception e)
                                {
                                    IO_Handler.ErrorMessage($"active projection not set:\nLOC-{(new StackFrame(1, true)).GetFileLineNumber()}\n" + e.Message);
                                    Thread.Sleep(sleepTime);
                                }
                            }
                            GetThisMenuMethods()[index]();
                        }
                        catch (Exception e)
                        {
                            IO_Handler.ErrorMessage($"Error while opening this menu!\nLOC-{(new StackFrame(1, true)).GetFileLineNumber()}\n" + e.Message);
                            Thread.Sleep(sleepTime);
                        }
                        goto case ConsoleKey.Clear;
                    case ConsoleKey.Escape:
                        index = 0;
                        return false;
                    case ConsoleKey.Clear:
                        return true;
                    default:
                        invalid = true;
                        break;
                }
            }
            return true;
        }
        Dictionary<int, MenuMethod> GetThisMenuMethods()
        {
            StackFrame frame = new StackFrame(3);
            try
            {
                Dictionary<int, MenuMethod> ResultDict = DelegateDictionaries[frame.GetMethod().Name];
                return ResultDict;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        MenuItem[] ListToMenuItemArray<T>(List<T> Collection)
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            MenuItem[] MenuItems = new MenuItem[Collection.Count];
            for (int i = 0; i < Collection.Count; i++)
            {
                MenuItems[i] = Collection[i].ToString();
            }
            return MenuItems;
        }
        MenuItem[] DictToMenuItemArray<K, V>(Dictionary<K, V> Collection)
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            List<V> Helper = new List<V>();
            foreach (var item in Collection)
            {
                Helper.Add(item.Value);
            }
            return ListToMenuItemArray(Helper);
        }
        #endregion
    }
}
