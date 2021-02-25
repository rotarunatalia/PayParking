using PayParking.Vehicles;

namespace PayParking
{
    public interface IParkingSpot
    {
        Vehicle Vehicle { get; }
        bool IsFree { get; }
    }
}