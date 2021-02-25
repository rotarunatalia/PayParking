using PayParking.Vehicles;

namespace PayParking
{
    public class ParkingSpot : IParkingSpot
    {
        public ParkingSpot(Vehicle vehicle)
        {
            Vehicle = vehicle;
        }
        public Vehicle Vehicle { get; }

        public bool IsFree => Vehicle == null;
    }
}