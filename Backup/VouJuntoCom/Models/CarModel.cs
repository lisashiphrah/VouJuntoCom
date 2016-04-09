using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VouJuntoCom.Models
{
    public class CarModel
    {
        public Guid ID { get; set; }

        [Display(Name="Marca")]
        public string Make { get; set; }

        [Display(Name = "Modelo")]
        public string Model { get; set; }

        [Display(Name = "Cor")]
        public string Color { get; set; }

        [Display(Name = "RENAVAM")]
        public string RENAVAM { get; set; }

		[Display(Name = "Dois últimos dígitos da placa")]
		public int? Digits { get; set; }

        [Display(Name = "Ar Condicionado")]
        public bool ArConditioning { get; set; }

        [Display(Name = "Rádio")]
        public bool Radio { get; set; }

        [Display(Name = "Fumante")]
        public bool Smoke { get; set; }

        [Display(Name = "Animais")]
        public bool Pet { get; set; }

    }
}