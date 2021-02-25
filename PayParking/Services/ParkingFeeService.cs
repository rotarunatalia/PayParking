using System;
using PayParking.Helpers;

namespace PayParking.Services
{
    public class ParkingFeeService : IParkingFeeService
    {
        public decimal CalculateFee(DateTime entryTime, DateTime leaveTime)
        {
            if (leaveTime < entryTime)
                throw new ArgumentException("LeaveTime is less than EntryTime");

            var totalMinutes = leaveTime.Subtract(entryTime).TotalMinutes;
            var payedHours = totalMinutes % 60 < 1
                ? (int)(totalMinutes / 60)
                : (decimal)Math.Ceiling(totalMinutes / 60);
            return payedHours >= 1
                ? payedHours * Constants.HourPrice + Constants.FirstHourPrice - Constants.HourPrice
                : 0;
        }
    }
}