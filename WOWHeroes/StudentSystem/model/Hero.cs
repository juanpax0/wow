using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOWHeroes.model
{
    public class Hero
    {
        public string name;
        public string race;
        public string sex;
        public string faction;

        public Hero(string name, string race, string sex, string faction)
        {
            this.name = name;
            this.race = race;
            this.sex = sex;
            this.faction = faction;
        }
    }
}
