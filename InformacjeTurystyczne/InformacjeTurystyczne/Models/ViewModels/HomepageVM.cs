using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.ViewModels
{
    public class HomepageVM
    {
        private Random rnd = new Random();
        public int RandomInt(int max)
        {
            return rnd.Next(max);
        }
    }
}
