using System;
using System.ComponentModel.DataAnnotations;

namespace Course.Ui.Resources
{
	public class GroupCreateResource
	{
        [Required]
        [MaxLength(5)]
        public string No { get; set; }

        [Required]
        public int Limit { get; set; }
       
    }
}

