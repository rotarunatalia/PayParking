using System;

namespace PayParking.Vehicles
{
    public abstract class Vehicle
    {
        protected Vehicle(string registrationPlate)
        {
            RegistrationPlate = registrationPlate;
        }
        public string RegistrationPlate { get; }
        public DateTime EntryTime { get; set; }
    }
}