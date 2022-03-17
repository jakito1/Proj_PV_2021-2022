﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class Client
    {

        [DisplayName("ID do Cliente")]
        public int ClientId { get; set; }

        [DisplayName("Primeiro Nome")]
        public string? ClientFirstName { get; set; }

        [DisplayName("Último Nome")]
        public string? ClientLastName { get; set; }


        [DisplayName("Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ClientBirthday { get; set; }

        [DisplayName("Peso")]
        public double? Weight { get; set; }

        [DisplayName("Altura")]
        public double? Height { get; set; }

        [DisplayName("Ginásio")]
        public Gym? Gym { get; set; }

        [DisplayName("Nutricionista")]
        public Nutritionist? Nutritionist { get; set; }

        [DisplayName("Treinador")]
        public Trainer? Trainer { get; set; }

        public UserAccountModel? UserAccountModel { get; set; }
    }
}