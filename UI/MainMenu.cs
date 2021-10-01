using System;
using Models;
using BL;
using DL;

namespace UI
{
    public class MainMenu : IMenu
    {
        public void Start(){
            Console.WriteLine("Welcome to the Shopping App! We hope you have the best experience.");
            
            bool exit = false;
            string input = "";

            do{
                Console.WriteLine("\n[1] Have an account? Type 0 to Login.");
                Console.WriteLine("[2] No account? Type 1 to Sign up.");
                Console.WriteLine("[3] Or you can type x to Exit.");

                input = Console.ReadLine();

                switch(input){
                    case "0":
                        MenuFactory.GetMenu("login").Start();
                        break;
                    case "1":
                        MenuFactory.GetMenu("signup").Start();
                        break;
                    case "x":
                        Console.WriteLine("Exiting...\n");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Please type the right input\n");
                        break;
                }

            }while (!exit);

        }
        
    }
}