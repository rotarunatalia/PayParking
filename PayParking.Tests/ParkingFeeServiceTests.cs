using System;
using NUnit.Framework;
using PayParking.Helpers;
using PayParking.Services;

namespace PayParking.Tests
{
    [TestFixture]
    class ParkingFeeServiceTests
    {
        private IParkingFeeService _parkingFeeService;

        [SetUp]
        public void SetUp()
        {
            _parkingFeeService = new ParkingFeeService();
        }


        [Test]
        public void CalculateFee_ThrowException_WhenLeaveTimeIsLessThatEntryTime()
        {
            //Arrange
            var entryTime = DateTime.Now;
            var leaveTime = entryTime.AddMinutes(-1);

            //Act
            Assert.Throws<ArgumentException>(() => _parkingFeeService.CalculateFee(entryTime, leaveTime));
        }


        [Test]
        public void CalculateFee_WhenVehicleParksUnderOneHour()
        {
            //Arrange
            var entryTime = DateTime.Now;
            var leaveTime = entryTime.AddMinutes(10);

            //Act
            var fee = _parkingFeeService.CalculateFee(entryTime, leaveTime);

            //Asserts
            Assert.AreEqual(Constants.FirstHourPrice, fee);
        }

        [Test]
        public void CalculateFee_WhenVehicleParksOneHourAndOneMinute()
        {
            //Arrange
            var entryTime = DateTime.Now;
            var leaveTime = entryTime.AddMinutes(61);

            //Act
            var fee = _parkingFeeService.CalculateFee(entryTime, leaveTime);

            //Asserts
            Assert.AreEqual(Constants.FirstHourPrice + Constants.HourPrice, fee);
        }

        [Test]
        public void CalculateFee_WhenVehicleParksOneHourAnd59Seconds()
        {
            //Arrange
            var entryTime = DateTime.Now;
            var leaveTime = entryTime.AddHours(1).AddSeconds(59);

            //Act
            var fee = _parkingFeeService.CalculateFee(entryTime, leaveTime);

            //Asserts
            Assert.AreEqual(Constants.FirstHourPrice, fee);
        }

        [Test]
        public void CalculateFee_WhenVehicleUnderOneMinute()
        {
            //Arrange
            var entryTime = DateTime.Now;
            var leaveTime = entryTime.AddSeconds(59);

            //Act
            var fee = _parkingFeeService.CalculateFee(entryTime, leaveTime);

            //Asserts
            Assert.AreEqual(0, fee);
        }
    }
}
