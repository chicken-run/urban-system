using System;
using System.Web.Http;
using RestApi.Interfaces;
using RestApi.Models;
using System.Linq;

namespace RestApi.Controllers
{
    public class PatientsController : ApiController
    {
        private readonly PatientContext _patientContext = new PatientContext();

        [HttpGet]
        public Patient Get(int patientId)
        {
            var requestedPatient = _patientContext.Patients.Find(patientId);

            requestedPatient.Episodes = _patientContext.Episodes.Where(e => e.PatientId == requestedPatient.PatientId);

            return requestedPatient;
        }
    }
}