using System;
using System.Collections.Generic;

namespace PasswordEncryption
{
    public class App
    {
        public void Run()
        {
            var usernameDict = new Dictionary<string, string>();
            MainMenu(usernameDict);
        }

        private static void MainMenu(Dictionary<string, string> usernameDict)
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------\n");
            Console.WriteLine("PASSWORD AUTHENTICATION SYSTEM\n");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Establish an account");
            Console.WriteLine("2. Authenticate a user");
            Console.WriteLine("3. Exit the system");
            Console.Write("\nEnter a selection: ");
            var menuOption = Console.ReadLine();
            Console.WriteLine("\n\n--------------------------------------------------------------------\n");
            if (menuOption == "1")
            {
                MakeAccount(usernameDict);
            }
            else if (menuOption == "2")
            {
                Authenticate(usernameDict);
            }
            else if (menuOption == "3")
            {
                Exit();
            }
            else
            {
                Console.WriteLine("Not a valid choice.  Press any key to continue...");
                Console.ReadKey();
                MainMenu(usernameDict);
            }
        }

        private static void MakeAccount(Dictionary<string,string> usernameDict)
        {

            try
            {
                Console.Clear();
                Console.WriteLine("--------------------------------------------------------------------\n");
                Console.WriteLine("CREATE A NEW USER\n");
                Console.Write("Enter a username: ");
                var username = Console.ReadLine();
                Console.Write("Enter a password: ");
                var password = Console.ReadLine();
                password = GetHash(password);
                usernameDict.Add(username, password);
                Console.WriteLine("\n\n--------------------------------------------------------------------\n");

                Console.WriteLine($"The following user was added:");
                Console.WriteLine($"Username: {username}");
                Console.WriteLine($"Password (SHA256 Hash): {usernameDict[username]}");
                Console.WriteLine("\n");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                MainMenu(usernameDict);
            }
            catch(ArgumentException)
            {
                Console.WriteLine("\nThat username is already in the data store.  Please try again.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            finally
            {
                MainMenu(usernameDict);
            }

        }
        private static void Authenticate(Dictionary<string, string> usernameDict)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--------------------------------------------------------------------\n");
                Console.WriteLine("AUTHENTICATE A USER\n");
                Console.Write("Enter a username: ");
                var username = Console.ReadLine();

                var passCorrect = false;
                var count = 3;
                do
                {
                    Console.Write("Enter a password: ");
                    var password = Console.ReadLine();
                    password = GetHash(password);
                    var hashedPassword = usernameDict[username];
                    if (hashedPassword == password)
                    {
                        Console.WriteLine("You have successfully authenticated; the password matched.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        passCorrect = true;
                    }
                    else
                    {
                        count--;
                        Console.WriteLine("\nIncorrect password.");
                        Console.WriteLine($"You have {count} attempts remaining...");
                    }
                } while (passCorrect == false && count != 0);

                Console.WriteLine("\n\n--------------------------------------------------------------------\n");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("\nThat username is not in the data store.  Please try again.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            finally
            {
                MainMenu(usernameDict);
            }
        }
        private static void Exit()
        {
            Console.WriteLine("All usernames and passwords have been deleted. Goodbye.");
            Environment.Exit(0);
        }
        internal static string GetHash(string password)
        {
            if (String.IsNullOrEmpty(password))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}