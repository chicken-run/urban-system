using System;
using System.Web.Http;
using RestApi.Interfaces;
using RestApi.Models;

namespace RestApi.Controllers
{
    public class PatientsController : ApiController
    {
        [HttpGet]
        public Patient Get(int patientId)
        {
            throw new NotImplementedException();
        }
    }
}