using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Datas
{
    public struct DataTower
    {
        public string name;
        public string spritePath;
        public int price;
        public float attaqueSpeed;
        public float range;
        public int damage;
        public float fearChance;
        public float missileSpeed;
        public float slowPercentage;
        public float slowTime;
        public bool splachEffect;
        public float stunChance;
        public float stunTime;
        public string comment;

        public override string ToString()
        {
            String temp = comment.Replace("\\n", "\n");
            String temp2 = name.Substring(0, 1).ToUpper() + name.Substring(1, name.Length - 2);
            String str = temp2 + " | Price:" + price + " | Damage:" + damage + " | Speed:" + attaqueSpeed + "\nSpeciality : " + temp;

            return str;
        }
    }
}
