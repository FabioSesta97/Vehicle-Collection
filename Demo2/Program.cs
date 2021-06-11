using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo2
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // Load from XML file
            //var vehicles = LoadFromXML();
            // Load from JSON file
            var vehicles = await LoadToJSONAsync();
            while (true)
            {
                var choice = MenuLoop();
                switch (choice)
                {
                    case '0':
                        return;
                    case '1':
                        AddVehicle(vehicles);
                        break;
                    case '2':
                        PrintAllVehicles(vehicles);
                        break;
                    case '3':
                        PrintSpecificVehicles(vehicles);
                        break;
                    case '4':
                        ModifyVehicle(vehicles);
                        break;
                    case '5':
                        DeleteVehicle(vehicles);
                        break;
                    case '6':
                        SaveToXML(vehicles);
                        break;
                    case '7':
                        await SaveToJsonAsync(vehicles);
                        break;
                    default:
                        break;
                }
            }
        }

        private static async Task<ICollection<Vehicle>> LoadToJSONAsync()
        {
            if (!File.Exists("vehicles.json"))
            {
                ErrorMessage("\n! ERROR: The JSON file was not found!");
                return Enumerable.Empty<Vehicle>().ToList();
            }
            var jsonString = await File.ReadAllTextAsync("vehicles.json");
            var formattedItems = JsonSerializer.Deserialize<Items>(jsonString);
            var cars = formattedItems.Cars;
            var trucks = formattedItems.Trucks;
            var bikes = formattedItems.Bikes;
            var vehicles = new List<Vehicle>();
            Console.WriteLine("....LOADING VEHICLES.....");
            SuccessMessage("\n+ Vehicles successfully loaded +");
            vehicles.AddRange(cars);
            vehicles.AddRange(trucks);
            vehicles.AddRange(bikes);
            return vehicles.OrderBy(x => x.ID).ToList();
        }

        private static async Task SaveToJsonAsync(ICollection<Vehicle> vehicles)
        {
            var items = new Items
            {
                Cars = vehicles.OfType<Car>().ToList(),
                Trucks = vehicles.OfType<Truck>().ToList(),
                Bikes = vehicles.OfType<Bike>().ToList()
            };
            //string jsonString = JsonConvert.SerializeObject(items, Formatting.Indented);
            var jsonString = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync("vehicles.json", jsonString);
            SuccessMessage("\n+ Saved successfully in JSON file +");
        }

        private static ICollection<Vehicle> LoadFromXML()
        {
            if (!File.Exists("vehicles.xml"))
            {
                ErrorMessage("\n! ERROR: The XML file was not found!");
                return Enumerable.Empty<Vehicle>().ToList();
            }
            var doc = XDocument.Load("vehicles.xml");
            var cars = doc.Root.Element("cars").Elements("car").
                Select(x => new Car
                {
                    ID = Convert.ToInt32(x.Element("id").Value),
                    Maker = x.Element("maker")?.Value,
                    Plate = x.Element("plate")?.Value,
                    Seats = Convert.ToInt32(x.Element("seats").Value)
                }).ToList();
            var trucks = doc.Root.Element("trucks").Elements("truck").
                Select(x => new Truck
                {
                    ID = Convert.ToInt32(x.Element("id").Value),
                    Maker = x.Element("maker")?.Value,
                    Plate = x.Element("plate")?.Value,
                    Axes = Convert.ToInt32(x.Element("axes").Value)
                }).ToList();
            var bikes = doc.Root.Element("bikes").Elements("bike").
                Select(x => new Bike
                {
                    ID = Convert.ToInt32(x.Element("id").Value),
                    Maker = x.Element("maker")?.Value,
                    Plate = x.Element("plate")?.Value,
                    Gears = Convert.ToInt32(x.Element("gears").Value)
                }).ToList();
            Console.WriteLine("....LOADING VEHICLES.....");
            SuccessMessage("\n+ Vehicles successfully loaded +");
            var vehicles = new List<Vehicle>();
            vehicles.AddRange(cars);
            vehicles.AddRange(trucks);
            vehicles.AddRange(bikes);
            return vehicles.OrderBy(x => x.ID).ToList();
        }

        private static void SaveToXML(ICollection<Vehicle> vehicles)
        {
            var cars = vehicles.OfType<Car>().Select(
                x => new XElement("car",
                        new XElement("id", x.ID),
                        new XElement("maker", x.Maker),
                        new XElement("plate", x.Plate),
                        new XElement("seats", x.Seats))).ToList();
            var trucks = vehicles.OfType<Truck>().Select(
                x => new XElement("truck",
                        new XElement("id", x.ID),
                        new XElement("maker", x.Maker),
                        new XElement("plate", x.Plate),
                        new XElement("axes", x.Axes))).ToList();
            var bikes = vehicles.OfType<Bike>().Select(
                x => new XElement("bike",
                        new XElement("id", x.ID),
                        new XElement("maker", x.Maker),
                        new XElement("plate", x.Plate),
                        new XElement("gears", x.Gears))).ToList();
            var doc = new XDocument(
                        new XElement("vehicles",
                            new XElement("cars", cars),
                            new XElement("trucks", trucks),
                            new XElement("bikes", bikes)));
            doc.Save("vehicles.xml");
            SuccessMessage("\n+ Saved successfully in XML file +");
        }

        private static void DeleteVehicle(ICollection<Vehicle> vehicles)
        {
            Console.WriteLine("\nInsert the ID of the vehicle you want to delete:");
            var stringID = Console.ReadLine();
            if (!Int32.TryParse(stringID, out int id))
            {
                ErrorMessage("! ERROR: ID must be an int value!");
                return;
            }
            var vehicleToRemove = vehicles.Where(x => x.ID == id).FirstOrDefault();
            if (vehicleToRemove == null)
            {
                ErrorMessage("! ERROR: No vehicles with ID: " + id);
                return;
            }
            vehicles.Remove(vehicleToRemove);
            SuccessMessage("+ Vehicle successfully removed +");
        }

        private static void ModifyVehicle(ICollection<Vehicle> vehicles)
        {
            Console.WriteLine("\nInsert the vehicle's ID that you want to modify: ");
            var stringID = Console.ReadLine();
            if (!Int32.TryParse(stringID, out int id))
            {
                ErrorMessage("! ERROR: ID must be an int value!");
                return;
            }
            var vehicle = vehicles.Where(x => x.ID == id).FirstOrDefault();
            if (vehicle == null)
            {
                ErrorMessage("! ERROR: No vehicle with the ID: " + id);
            }
            else
            {
                switch (vehicle)
                {
                    case Car:
                        ModifyCar(vehicle);
                        break;
                    case Truck:
                        ModifyTruck(vehicle);
                        break;
                    case Bike:
                        ModifyBike(vehicle);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ModifyBike(Vehicle v)
        {
            Console.WriteLine("\n********* Modifying a BIKE *********");
            Console.WriteLine("Insert the maker:");
            var maker = Console.ReadLine().Trim();
            Console.WriteLine("Insert the plate:");
            var plate = Console.ReadLine().Trim();
            Console.WriteLine("Insert the number of gears:");
            var stringGears = Console.ReadLine();
            if (!Int32.TryParse(stringGears, out int gears))
            {
                ErrorMessage("! ERROR:  must be an int value!");
                return;
            }
            var bike = (Bike)v;
            bike.Maker = maker;
            bike.Plate = plate;
            bike.Gears = gears;
            SuccessMessage("+ Bike modified correctly +");
        }

        private static void ModifyTruck(Vehicle v)
        {
            Console.WriteLine("\n********* Modifying a TRUCK *********");
            Console.WriteLine("Insert the maker:");
            var maker = Console.ReadLine().Trim();
            Console.WriteLine("Insert the plate:");
            var plate = Console.ReadLine().Trim();
            Console.WriteLine("Insert the number of axes:");
            var stringAxes = Console.ReadLine();
            if (!Int32.TryParse(stringAxes, out int axes))
            {
                ErrorMessage("! ERROR:  must be an int value!");
                return;
            }
            var truck = (Truck)v;
            truck.Maker = maker;
            truck.Plate = plate;
            truck.Axes = axes;
            SuccessMessage("+ Truck modified correctly +");

        }

        private static void ModifyCar(Vehicle v)
        {
            Console.WriteLine("\n********* Modifying a CAR *********");
            Console.WriteLine("Insert the maker:");
            var maker = Console.ReadLine().Trim();
            Console.WriteLine("Insert the plate:");
            var plate = Console.ReadLine().Trim();
            Console.WriteLine("Insert the number of seats:");
            var stringSeats = Console.ReadLine();
            if (!Int32.TryParse(stringSeats, out int seats))
            {
                ErrorMessage("! ERROR:  must be an int value!");
                return;
            }
            var car = (Car)v;
            car.Maker = maker;
            car.Plate = plate;
            car.Seats = seats;
            SuccessMessage("+ Car modified correctly +");
        }

        private static void PrintSpecificVehicles(ICollection<Vehicle> vehicles)
        {
            Console.WriteLine("\nPress 1 to print CARS, 2 to print TRUCKS, 3 to print BIKES:");
            var choice = Console.ReadKey();
            switch (choice.KeyChar)
            {
                case '1':
                    PrintCars(vehicles);
                    break;
                case '2':
                    PrintTrucks(vehicles);
                    break;
                case '3':
                    PrintBikes(vehicles);
                    break;
                default:
                    break;
            }
        }

        private static void PrintBikes(ICollection<Vehicle> vehicles)
        {
            Console.WriteLine("");
            var bikes = vehicles.Where(x => x is Bike);
            if (!bikes.Any())
            {
                ErrorMessage("! NO Bikes Found");
            }
            else
            {
                Console.WriteLine("\n------------LIST OF BIKES-----------\n");
                foreach (var item in bikes)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }

        private static void PrintTrucks(ICollection<Vehicle> vehicles)
        {
            Console.WriteLine("");
            var trucks = vehicles.Where(x => x is Truck);
            if (!trucks.Any())
            {
                ErrorMessage("! NO Trucks Found");
            }
            else
            {
                Console.WriteLine("\n------------LIST OF TRUCKS-----------\n");
                foreach (var item in trucks)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }

        private static void PrintCars(ICollection<Vehicle> vehicles)
        {
            Console.WriteLine("");
            var cars = vehicles.Where(x => x is Car);
            if (!cars.Any())
            {
                ErrorMessage("! NO Cars Found");
            }
            else
            {
                Console.WriteLine("\n------------LIST OF CARS-----------\n");
                foreach (var item in cars)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }

        private static void PrintAllVehicles(ICollection<Vehicle> vehicles)
        {
            Console.WriteLine("");
            if (!vehicles.Any())
            {
                ErrorMessage("! NO Vehicles Found");
                return;
            }
            Console.WriteLine("\n------------LIST OF VEHICLES-----------\n");
            foreach (var item in vehicles)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static void AddVehicle(ICollection<Vehicle> vehicles)
        {
            Console.WriteLine("\nPress 1 to add a CAR, 2 to add a TRUCK, 3 to add a BIKE:");
            var typeOfVehicle = Console.ReadKey();
            switch (typeOfVehicle.KeyChar)
            {
                case '1':
                    AddCar(vehicles);
                    break;
                case '2':
                    AddTruck(vehicles);
                    break;
                case '3':
                    AddBike(vehicles);
                    break;
                default:
                    break;
            }
        }

        private static void AddBike(ICollection<Vehicle> vehicles)
        {
            var id = GetNewID(vehicles);
            Console.WriteLine("\nInsert the maker:");
            var maker = Console.ReadLine().Trim();
            Console.WriteLine("Insert the plate:");
            var plate = Console.ReadLine().Trim();
            Console.WriteLine("Insert the number of gears:");
            var stringGears = Console.ReadLine();
            if (!Int32.TryParse(stringGears, out int gears))
            {
                ErrorMessage("! ERROR: Gears must be an int value!");
                return;
            }
            var bike = new Bike(id, maker, plate, gears);
            vehicles.Add(bike);
            SuccessMessage("+ Added Correctly +");
        }

        private static void AddTruck(ICollection<Vehicle> vehicles)
        {
            var id = GetNewID(vehicles);
            Console.WriteLine("\nInsert the maker:");
            var maker = Console.ReadLine().Trim();
            Console.WriteLine("Insert the plate:");
            var plate = Console.ReadLine().Trim();
            Console.WriteLine("Insert the number of axes:");
            var stringAxes = Console.ReadLine();
            if (!Int32.TryParse(stringAxes, out int axes))
            {
                ErrorMessage("! ERROR: Axes must be an int value!");
                return;
            }
            var truck = new Truck(id, maker, plate, axes);
            vehicles.Add(truck);
            SuccessMessage("+ Added Correctly +");
        }

        private static void AddCar(ICollection<Vehicle> vehicles)
        {
            var id = GetNewID(vehicles);
            Console.WriteLine("\nInsert the maker:");
            var maker = Console.ReadLine().Trim();
            Console.WriteLine("Insert the plate:");
            var plate = Console.ReadLine().Trim();
            Console.WriteLine("Insert the number of seats:");
            var stringSeats = Console.ReadLine();
            if (!Int32.TryParse(stringSeats, out int seats))
            {
                ErrorMessage("! ERROR: Seats must be an int value!");
                return;
            }
            var car = new Car(id, maker, plate, seats);
            vehicles.Add(car);
            SuccessMessage("+ Added Correctly +");
        }

        private static int GetNewID(ICollection<Vehicle> vehicles)
        {
            return vehicles?.Count == 0 ? 1 : vehicles.Max(x => x.ID) + 1;
        }

        private static void SuccessMessage(string s)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        private static void ErrorMessage(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();
        }

        private static int MenuLoop()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n*---------------------MENU---------------------*");
            Console.WriteLine("* Press 1 to ADD a vehicle                     *");
            Console.WriteLine("* Press 2 to PRINT all vehicles                *");
            Console.WriteLine("* Press 3 to PRINT a specific type of vehicle  *");
            Console.WriteLine("* Press 4 to MODIFY a vehicle                  *");
            Console.WriteLine("* Press 5 to REMOVE a vehicle                  *");
            Console.WriteLine("* Press 6 to SAVE the list on XML file         *");
            Console.WriteLine("* Press 7 to SAVE the list on JSON file        *");
            Console.WriteLine("* Press 0 to EXIT                              *");
            Console.WriteLine("*----------------------------------------------*");
            Console.Write("Choice --->\t");
            var choice = Console.ReadKey().KeyChar;
            Console.ResetColor();
            return choice;
        }
    }
}