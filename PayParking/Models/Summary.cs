using System;
using PayParking.Vehicles;

namespace PayParking.Model
{
    public class Summary
    {
        public Vehicle Vehicle { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime LeaveTime { get; set; }
        public decimal TotalPayment { get; set; }
    }
}
