using Moq;
using NUnit.Framework;
using RestApi.Controllers;
using RestApi.Interfaces;
using RestApi.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace RestApi.UnitTests
{
    [TestFixture]
    public class GetPatientTests
    {
        [Test]
        public void PatientsController_Get_passesId()
        {
            // Arrange
            var input = 5;
            var expectedPatient = new Patient()
            {
                DateOfBirth = DateTime.UtcNow.Date.AddYears(-25),
                FirstName = "Felix",
                LastName = "Milton",
                NhsNumber = "0268GD54",
                PatientId = input
            };
            var expectedEpisode = new Episode()
            {
                PatientId = expectedPatient.PatientId,
                EpisodeId = 7,
                Diagnosis = "Concussion",
                AdmissionDate = DateTime.UtcNow.AddDays(-17),
                DischargeDate = DateTime.UtcNow.AddDays(-13),
            };
            var notToNeReturnedEpisode = new Episode()
            {
                PatientId = expectedPatient.PatientId + 1,
                EpisodeId = 11,
                Diagnosis = "Fractured Spine",
                AdmissionDate = DateTime.UtcNow.AddDays(-29),
                DischargeDate = DateTime.UtcNow.AddDays(-23),
            };

            var mockPatients = new Mock<IDbSet<Patient>>();
            mockPatients.Setup(p => p.Find(It.Is<int>(i => input.Equals(i)))).Returns(expectedPatient);

            var episodes = new InMemoryDbSet<Episode>();
            episodes.Add(expectedEpisode);
            episodes.Add(notToNeReturnedEpisode);
            
            // Mocked Dependency
            var mockedDbContext = new Mock<IDatabaseContext>();
            mockedDbContext.Setup(c => c.Patients).Returns(mockPatients.Object);
            mockedDbContext.Setup(c => c.Episodes).Returns(episodes);

            var objectUnderTest = new PatientsController(mockedDbContext.Object);

            // Act
            var actual = objectUnderTest.Get(input);

            // Assert
            mockPatients.VerifyAll();
            // Not asserting every property - don't want to test Moq functionality 
            Assert.AreEqual(expectedPatient.PatientId, actual.PatientId);
            Assert.AreEqual(1, actual.Episodes.Count());
            Assert.AreEqual(expectedEpisode.PatientId, actual.Episodes.First().PatientId);
        }
    }
}