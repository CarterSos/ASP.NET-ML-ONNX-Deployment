using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zoo.Models
{
    public class Animal
    {
        [Key]
        public string animal_name { get; set; }
        public int Hair { get; set; }
        public int Feathers { get; set; }
        public int Eggs { get; set; }
        public int Milk { get; set; }
        public int Airborne { get; set; }
        public int Aquatic { get; set; }
        public int Predator { get; set; }
        public int Toothed { get; set; }
        public int Backbone { get; set; }
        public int Breathes { get; set; }
        public int Venomous { get; set; }
        public int Fins { get; set; }
        public int Legs { get; set; }
        public int Tail { get; set; }
        public int Domestic { get; set; }
        public int Catsize { get; set; }
        public int class_type { get; set; }
    }
}
