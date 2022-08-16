using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace MovieMatch.People
{
    public class Person:Entity<int>
    {
        public string Name { get; set; }
        public string ProfilePath { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime? DeathDay { get; set; }
        public string Biography { get; set; }

        private Person()
        {

        }

        public Person(int id,string name,string profilePath,DateTime birthDay,DateTime? deathDay,string biography)
        {
            Id = id;
            Name = name;
            ProfilePath = profilePath;
            BirthDay = birthDay;
            DeathDay = deathDay;
            Biography = biography;
        }
    }
}
