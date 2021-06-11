using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo2.Model
{
    public static class UtilityExtensions
    {
        public static void RemoveAllOfType<T>(this ICollection<Vehicle> vehicles) where T : Vehicle
        {
            var vehiclesToRemove = vehicles.ToList().Where(x => x is T);
            foreach (var vehicle in vehiclesToRemove)
            {
                vehicles.Remove(vehicle);
            }
        }

        public static void PrintAllOfType<T>(this ICollection<Vehicle> vehicles, out IEnumerable<Vehicle> vehiclesToPrint) where T : Vehicle
        {
            vehiclesToPrint = vehicles.ToList().Where(x => x is T);
            if (vehiclesToPrint.Any())
            {
                foreach (var vehicle in vehiclesToPrint)
                {
                    Console.WriteLine(vehicle.ToString());
                }
            }
        }
    }
}