using System;
using System.Web.Http;
using RestApi.Interfaces;
using RestApi.Models;
using System.Linq;

namespace RestApi.Controllers
{
    public class PatientsController : ApiController
    {
        private readonly IDatabaseContext _iDatabaseContext;

        public PatientsController(IDatabaseContext iDatabaseContext)
        {
            _iDatabaseContext = iDatabaseContext;
        }

        [HttpGet]
        public Patient Get(int patientId)
        {
            var requestedPatient = _iDatabaseContext.Patients.Find(patientId);

            requestedPatient.Episodes = _iDatabaseContext.Episodes.Where(e => e.PatientId == requestedPatient.PatientId);

            return requestedPatient;
        }
    }
}