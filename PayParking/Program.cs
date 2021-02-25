using System;
using System.Collections.Generic;
using PayParking.Helpers;
using PayParking.Model;
using PayParking.Services;
using PayParking.Vehicles;

namespace PayParking
{
    class Program
    {
        static void Main()
        {
            var parkingFeeService = new ParkingFeeService();
            var dateTimeHelper = new DateTimeHelper();


            var parkingLot = new ParkingLot(parkingFeeService, dateTimeHelper);


            DisplayFreeSlotsCount(parkingLot.FreeSlotsCount);
            DisplayParkedVehicles(parkingLot.ParkedVehicles);
            
            var vehicle1 = new Car("IS 25 MB");
            var vehicle2 = new Car("B 99 MBT");
            var vehicle3 = new Car("CT 58 DTT");


            Console.WriteLine("----------------------------------------------------");
            parkingLot.Park(vehicle1);
            Console.WriteLine($"Park car: {vehicle1.RegistrationPlate}");

            parkingLot.Park(vehicle2);
            Console.WriteLine($"Park car: {vehicle2.RegistrationPlate}");

            parkingLot.Park(vehicle3);
            Console.WriteLine($"Park car: {vehicle3.RegistrationPlate}");


            Console.WriteLine("----------------------------------------------------");
            DisplayFreeSlotsCount(parkingLot.FreeSlotsCount);
            DisplayParkedVehicles(parkingLot.ParkedVehicles);

            Console.WriteLine("----------------------------------------------------");
            var summary = parkingLot.Leave(vehicle2);
            Console.WriteLine($"Leave car: {vehicle2.RegistrationPlate}");
            DisplaySummary(summary);
        }

        public static void DisplaySummary(Summary summary)
        {
            Console.WriteLine($"Summary for {summary.Vehicle.RegistrationPlate}");
            Console.WriteLine($"Entry time: {summary.EntryTime}");
            Console.WriteLine($"Entry leave: {summary.LeaveTime}");
            Console.WriteLine($"Total payment: {summary.TotalPayment}");
        }

        public static void DisplayFreeSlotsCount(int freeSlotsCount)
        {
            Console.WriteLine($"Parking free slots: {freeSlotsCount}");
        }

        public static void DisplayParkedVehicles(IEnumerable<Vehicle> parkedVehicles)
        {
            Console.WriteLine($"List of parkedVehicle:");
            var i = 1;
            foreach (var parkedVehicle in parkedVehicles)
            {
                Console.WriteLine($"{i++}: {parkedVehicle.RegistrationPlate}");
            }
        }
    }
}
