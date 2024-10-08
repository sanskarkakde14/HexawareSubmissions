using NUnit.Framework;
using System;
using CarRentalSystem.Exceptions;
using CarRentalSystem.dao;
using CarRentalSystem.dao.Interfaces;
using CarRentalSystem.Model;
using CarRentalSystem.Util;
using CarRentalSystem.Repository;
using NUnit.Framework.Legacy;

namespace CarRentalSystem.Tests
{
    [TestFixture]
    public class CarRentalSystemTests
    {
        private string connectionString;
        private IVehicleDataAccess vehicleDataAccess;
        private ICustomerDataAccess customerDataAccess;
        private ILeaseDataAccess leaseDataAccess;
        private IPaymentDataAccess paymentDataAccess;

        [SetUp]
        public void Setup()
        {
            connectionString = DbConnUtil.GetConnString();
            vehicleDataAccess = new VehicleDataAccess(connectionString);
            customerDataAccess = new CustomerDataAccess(connectionString);
            leaseDataAccess = new LeaseDataAccess(connectionString);
            paymentDataAccess = new PaymentDataAccess(connectionString);
        }

        [Test]
        public void GetAllVehicles_ShouldReturnVehicles()
        {
            // Act
            var allVehicles = vehicleDataAccess.GetAvailableVehicles();

            // Assert
            ClassicAssert.IsNotNull(allVehicles);
            ClassicAssert.GreaterOrEqual(allVehicles.Count, 0);

            if (allVehicles.Count > 0)
            {
                ClassicAssert.IsNotNull(allVehicles[0].VehicleID);
                ClassicAssert.IsNotNull(allVehicles[0].Model);
            }
        }

        [Test]
        public void AddVehicle_ShouldAddNewVehicle()
        {
            // Arrange
            var newVehicle = new Vehicle(
                vehicleID: 0, 
                make: "Lexus",
                model: "RX",
                year: 2012,
                dailyRate: 400m,
                status: "Available",
                passengerCapacity: 3,
                engineCapacity: "2L"
            );

            // Act
            vehicleDataAccess.AddVehicle(newVehicle);

            // Assert
            var allVehicles = vehicleDataAccess.GetAvailableVehicles();
            ClassicAssert.IsNotNull(allVehicles, "The vehicle list should not be null.");
            ClassicAssert.IsTrue(allVehicles.Any(v =>
                v.Make == newVehicle.Make &&
                v.Model == newVehicle.Model &&
                v.Year == newVehicle.Year),
                "The newly added vehicle should be in the list.");
        }

        [Test]
        public void CreateLease_ShouldCreateNewLease()
        {
            // Arrange
            int customerId = 6; 
            int vehicleId = 12;  
            DateTime startDate = new DateTime(2024, 10, 15); 
            DateTime endDate = new DateTime(2024, 11, 15);  
            string type = "dailyLease"; 

            // Act
            Lease newLease = leaseDataAccess.CreateLease(customerId, vehicleId, startDate, endDate, type);

            // Assert
            ClassicAssert.IsNotNull(newLease, "The new lease should not be null.");
            ClassicAssert.AreEqual(customerId, newLease.CustomerID, "The Customer ID should match.");
            ClassicAssert.AreEqual(vehicleId, newLease.VehicleID, "The Vehicle ID should match.");
            ClassicAssert.AreEqual(startDate, newLease.StartDate, "The Start Date should match.");
            ClassicAssert.AreEqual(endDate, newLease.EndDate, "The End Date should match.");
            ClassicAssert.AreEqual(type, newLease.Type, "The Lease type should match.");
        }



        [Test]
        public void GetLeaseById_ShouldRetrieveLeaseById()
        {
            // Arrange
            int leaseId = 23;

            // Act
            var leaseInfo = leaseDataAccess.GetLeaseById(leaseId);

            // Assert
            ClassicAssert.IsNotNull(leaseInfo);
            Assert.That(leaseInfo.LeaseID, Is.EqualTo(leaseId));
        }

        [Test]
        public void GetVehicleById_ShouldThrowExceptionForInvalidId()
        {
            // Arrange
            int invalidCarId = 999;

            // Act & Assert
            Assert.Throws<CarNotFoundException>(() => vehicleDataAccess.GetVehicleById(invalidCarId));
        }

        [Test]
        public void GetCustomerById_ShouldThrowExceptionForInvalidId()
        {
            // Arrange
            int invalidCustomerId = 99;

            // Act & Assert
            Assert.Throws<CustomerNotFoundException>(() => customerDataAccess.GetCustomerById(invalidCustomerId));
        }

        [Test]
        public void GetLeaseById_ShouldThrowExceptionForInvalidId()
        {
            // Arrange
            int invalidLeaseId = 999;

            // Act & Assert
            Assert.Throws<LeaseNotFoundException>(() => leaseDataAccess.GetLeaseById(invalidLeaseId));
        }
    }
}