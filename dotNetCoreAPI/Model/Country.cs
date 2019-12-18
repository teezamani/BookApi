﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetCoreAPI.Model
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "The Country cannot be more than 50 characters")]
        public string Name { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }
}
