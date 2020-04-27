using System.Collections.Generic;
using Kolokwium1.Models;

namespace Kolokwium1.DTOs.Responses
{
    public class GetMedicamentResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<Presciption> PrescitionList { get; set; } 
    }
}