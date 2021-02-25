using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using PayParking.Helpers;
using PayParking.Services;
using PayParking.Vehicles;

namespace PayParking.Tests
{
    [TestFixture]
    class ParkingLotTests
    {
        private Mock<IParkingFeeService> _parkingFeeServiceMock;
        private Mock<IDateTimeHelper> _dateTimeHelperMock;
        private Random _randomNumber;
        private ParkingLot _parkingLot;

        [SetUp]
        public void SetUp()
        {
            _parkingFeeServiceMock = new Mock<IParkingFeeService>();
            _dateTimeHelperMock = new Mock<IDateTimeHelper>();
            _parkingLot = new ParkingLot(_parkingFeeServiceMock.Object, _dateTimeHelperMock.Object);
            _randomNumber = new Random();
        }
        
        [Test]
        public void Park_ThrowException_WhenVehicleIsNull()
        {
            //Act
            Assert.Throws<ArgumentException>(() => _parkingLot.Park(null));
        }

        [Test]
        public void Park_ThrowException_WhenVehicleAlreadyExists()
        {
            //Arrange
            var vehicle = new Car(Guid.NewGuid().ToString());

            //Act
            _parkingLot.Park(vehicle);

            Assert.Throws<ArgumentException>(() => _parkingLot.Park(vehicle));
        }


        [Test]
        public void Park_ReturnTrue_WhenParkOneCar()
        {
            //Arrange
            var vehicle = new Car(Guid.NewGuid().ToString());
            var dateTimeNow = DateTime.Now;

            _dateTimeHelperMock.Setup(m => m.GetDateTimeNow()).Returns(dateTimeNow);

            //Act
            var result = _parkingLot.Park(vehicle);

            //Assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(1, _parkingLot.ParkedVehicles.ToList().Count);
            Assert.AreEqual(Constants.SpotsCount - 1, _parkingLot.FreeSlotsCount);
            Assert.AreEqual(dateTimeNow, vehicle.EntryTime);
        }

        [Test]
        public void Park_ReturnFalse_WhenAllParkingSpotsAreOccupied()
        {
            //Arrange
            for (var i = 0; i < Constants.SpotsCount; i++)
            {
                _parkingLot.Park(new Car(Guid.NewGuid().ToString()));
            }

            //Act
            var result = _parkingLot.Park(new Car(Guid.NewGuid().ToString()));

            //Assert
            Assert.AreEqual(false, result);
            Assert.AreEqual(0, _parkingLot.FreeSlotsCount);
            Assert.AreEqual(Constants.SpotsCount, _parkingLot.ParkedVehicles.ToList().Count);
        }

        [Test]
        public void Leave_ThrowException_WhenVehicleIsNull()
        {
            //Act
            Assert.Throws<ArgumentException>(() => _parkingLot.Leave(null));
        }

        [Test]
        public void Leave_ThrowException_WhenVehicleIsNotParked()
        {
            //Arrange
            var result = new Car(Guid.NewGuid().ToString());

            //Act
            Assert.Throws<ArgumentException>(() => _parkingLot.Leave(result));
        }

        [Test]
        public void Park_ReturnSummary_WhenVehicleLeave()
        {
            //Arrange
            var vehicle = new Car(Guid.NewGuid().ToString());
            var entryTime = DateTime.Now;
            var leaveTime = entryTime.AddHours(1).AddMinutes(1);
            var parkingFee = _randomNumber.Next();

            _dateTimeHelperMock.Setup(m => m.GetDateTimeNow()).Returns(entryTime);

            _parkingLot.Park(vehicle);

            _dateTimeHelperMock.Setup(m => m.GetDateTimeNow()).Returns(leaveTime);
            _parkingFeeServiceMock.Setup(m => m.CalculateFee(entryTime, leaveTime)).Returns(parkingFee);

            //Act
            var summary = _parkingLot.Leave(vehicle);

            //Assert
            Assert.AreEqual(entryTime, summary.EntryTime);
            Assert.AreEqual(leaveTime, summary.LeaveTime);
            Assert.AreEqual(parkingFee, summary.TotalPayment);
            Assert.AreEqual(vehicle, summary.Vehicle);
            Assert.AreEqual(0, _parkingLot.ParkedVehicles.ToList().Count);
            Assert.AreEqual(Constants.SpotsCount, _parkingLot.FreeSlotsCount);
        }
    }
}
