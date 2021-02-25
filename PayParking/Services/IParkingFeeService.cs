using System;

namespace PayParking.Services
{
    public interface IParkingFeeService
    {
        decimal CalculateFee(DateTime entryTime, DateTime leaveTime);
    }
}