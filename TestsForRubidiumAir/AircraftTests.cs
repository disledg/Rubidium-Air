using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rubidium;
using System;

namespace TestsForRubidiumAir
{
    [TestClass]
    public class AircraftTests
    {
        [TestMethod]
        public void TestAircraftCreation()
        {
            // Arrange
            var aircraft = new Aircraft
            {
                Id = 1,
                registration_number = "RA-12345",
                model = "Boeing 737",
                year_of_manufacture = 2020,
                passenger_capacity = 180,
                cargo_capacity = 2000
            };

            // Assert
            Assert.AreEqual(1, aircraft.Id);
            Assert.AreEqual("RA-12345", aircraft.registration_number);
            Assert.AreEqual("Boeing 737", aircraft.model);
            Assert.AreEqual(2020, aircraft.year_of_manufacture);
            Assert.AreEqual(180, aircraft.passenger_capacity);
            Assert.AreEqual(2000, aircraft.cargo_capacity);
        }

        [TestMethod]
        public void TestAircraftMetricsCollection()
        {
            // Arrange
            var aircraft = new Aircraft();

            // Assert
            Assert.IsNotNull(aircraft.AircraftMetrics);
            Assert.IsInstanceOfType(aircraft.AircraftMetrics, typeof(System.Collections.Generic.ICollection<AircraftMetric>));
        }

        [TestMethod]
        public void TestAircraftMetricCreation()
        {
            // Arrange
            var metric = new AircraftMetric
            {
                Id = 1,
                aircraft_id = 1,
                date = new DateTime(2024, 3, 20),
                fuel_consumption = 1000.5m,
                passenger_load = 150,
                cargo_load = 1500.0m,
                water_level = 95.5m,
                oil_level = 85.0m,
                technical_condition = 98.5m
            };

            // Assert
            Assert.AreEqual(1, metric.Id);
            Assert.AreEqual(1, metric.aircraft_id);
            Assert.AreEqual(new DateTime(2024, 3, 20), metric.date);
            Assert.AreEqual(1000.5m, metric.fuel_consumption);
            Assert.AreEqual(150, metric.passenger_load);
            Assert.AreEqual(1500.0m, metric.cargo_load);
            Assert.AreEqual(95.5m, metric.water_level);
            Assert.AreEqual(85.0m, metric.oil_level);
            Assert.AreEqual(98.5m, metric.technical_condition);
        }

        [TestMethod]
        public void TestAircraftCapacityValidation()
        {
            // Arrange
            var aircraft = new Aircraft
            {
                passenger_capacity = 200,
                cargo_capacity = 2500
            };

            var metric = new AircraftMetric
            {
                passenger_load = 180,
                cargo_load = 2000
            };

            // Assert
            Assert.IsTrue(aircraft.passenger_capacity >= metric.passenger_load);
            Assert.IsTrue(aircraft.cargo_capacity >= metric.cargo_load);
        }

        [TestMethod]
        public void TestMetricTechnicalParameters()
        {
            // Arrange
            var metric = new AircraftMetric
            {
                water_level = 95.5m,
                oil_level = 85.0m,
                technical_condition = 98.5m
            };

            // Assert
            Assert.IsTrue(metric.water_level >= 0 && metric.water_level <= 100);
            Assert.IsTrue(metric.oil_level >= 0 && metric.oil_level <= 100);
            Assert.IsTrue(metric.technical_condition >= 0 && metric.technical_condition <= 100);
        }
    }
} 