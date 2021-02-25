using System;
using System.Collections.Generic;
using System.Linq;
using PayParking.Helpers;
using PayParking.Model;
using PayParking.Services;
using PayParking.Vehicles;

namespace PayParking
{
    public class ParkingLot
    {
        private readonly IParkingFeeService _parkingFeeService;
        private readonly IDateTimeHelper _dateTimeHelper;

        private const int _spotsCount = Constants.SpotsCount;
        private readonly List<IParkingSpot> _occupiedSpots = new List<IParkingSpot>();
        
        public ParkingLot(IParkingFeeService parkingFeeService, IDateTimeHelper dateTimeHelper)
        {
            _parkingFeeService = parkingFeeService;
            _dateTimeHelper = dateTimeHelper;
        }

        public int FreeSlotsCount => _spotsCount - _occupiedSpots.Count;
        public IEnumerable<Vehicle> ParkedVehicles => _occupiedSpots.Where(m => !m.IsFree).Select(m => m.Vehicle);

        public bool Park(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentException("Invalid vehicle");
            if (_occupiedSpots.Any(m => m.Vehicle != null && m.Vehicle.RegistrationPlate == vehicle.RegistrationPlate))
                throw new ArgumentException("Duplicate vehicle");

            if(_occupiedSpots.Count >= _spotsCount)
                return false;

            vehicle.EntryTime = _dateTimeHelper.GetDateTimeNow();
            _occupiedSpots.Add(new ParkingSpot(vehicle));

            return true;
        }

        public Summary Leave(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentException("Invalid Parking spot");

            var parkingSpot = _occupiedSpots.FirstOrDefault(m => m.Vehicle == vehicle);
            if(parkingSpot == null)
                throw new ArgumentException("Invalid Parking spot");

            _occupiedSpots.Remove(parkingSpot);

            var dateTimeNow = _dateTimeHelper.GetDateTimeNow();
            var summary = new Summary
            {
                EntryTime = parkingSpot.Vehicle.EntryTime,
                LeaveTime = dateTimeNow,
                TotalPayment = _parkingFeeService.CalculateFee(parkingSpot.Vehicle.EntryTime, dateTimeNow),
                Vehicle = parkingSpot.Vehicle
            };
            return summary;
        }
    }
}