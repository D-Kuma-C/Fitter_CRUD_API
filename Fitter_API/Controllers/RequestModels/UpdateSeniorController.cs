﻿using System.ComponentModel.DataAnnotations;

namespace Fitter_API.Controllers.RequestModels
{
    public class UpdateSeniorController
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        public IEnumerable<int> FitterId { get; set; } = new List<int>();
    }
}
