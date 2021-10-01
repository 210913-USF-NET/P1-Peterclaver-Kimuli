using System;
using Models;
using BL;
using System.Collections.Generic;
using Serilog;

namespace UI
{
    public class ManagerInterfaceMenu : IMenu
    {
        private Manager _manager;

        private IBL _bl;
        public ManagerInterfaceMenu(){}
        public ManagerInterfaceMenu(IBL bl, Manager manager){
            this._manager = manager;
            _bl = bl;
        }
        public void Start()
        {
            Console.WriteLine($"\nWelcome {_manager.Name}. Please use the menu below to navigate through the App.");

            bool exit = false;
            do
            {
                Console.WriteLine("\n[1] Type 1 to select a store location");
                Console.WriteLine("[2] Type 2 to create a new location... or type x to exit");

                switch(Console.ReadLine())
                {
                    case "1":
                        SelectStore();
                        break;
                    case "2":
                        CreateStore();
                        break;
                    case "x":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Please type the correct input");
                        break;
                }
            }while(!exit);
        }

        private void CreateStore(){
            Console.WriteLine("\n[Create a new Store.]");
            
            //Capturing store number
            Store newStore = new Store();
            newStore.ManagerPhone = _manager.Phonenumber;
            number:
            Console.WriteLine("Store number:");
            string storeNumber = Console.ReadLine();
            try{
                newStore.Number = storeNumber;
            }
            catch(InputInvalidException e){
                Console.WriteLine(e.Message);
                goto number;
            }

            //Capturing store location
            location:
            Console.WriteLine("Store Location:");
            string storeLocation = Console.ReadLine();
            try{
                newStore.Location = storeLocation;
            }
            catch(InputInvalidException e){
                Console.WriteLine(e.Message);
                goto location;
            }

            //Capturing store zipcode
            zip:
            Console.WriteLine("Store Zipcode:");
            string storeZip = Console.ReadLine();
            try{
                newStore.Zipcode = storeZip;
            }
            catch(InputInvalidException e){
                Console.WriteLine(e.Message);
                goto zip;
            }

            Store addedStore = _bl.AddStore(newStore);

            Log.Information("Store created successfully");
            Console.WriteLine($"You have successfully created a store:\n{addedStore.ToString()}");
        }

        private void SelectStore()
        {
            menu:
            List<Store> storesToSelect = _bl.GetManagerStores(_manager.Phonenumber);
            Console.WriteLine("\nPlease select from the stores below:");
            if(storesToSelect.Count == 0 || storesToSelect == null)
            {
                Console.WriteLine("There are no stores currently\n");
                return;
            }
            for(int i=0; i<storesToSelect.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {storesToSelect[i].Number}, {storesToSelect[i].Location} {storesToSelect[i].Zipcode}");
            }

            string input = Console.ReadLine();
            int parsedInput;
            bool parseSuccess = Int32.TryParse(input, out parsedInput);
            if (parseSuccess && parsedInput >= 0 && parsedInput <= storesToSelect.Count)
            {
                int actualInput = parsedInput - 1;
                Log.Information("Logging into the store manager menu");
                MenuFactory.GetMenu("storemanagermenu", storesToSelect[actualInput]).Start();
            }
            else
            {
                Console.WriteLine("Invalid input.");
                goto menu;
            }
        }
    }
}