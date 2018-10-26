using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEKe
{
    public interface IEatable : ICollideable
    {
        Food.FoodType Type { get; }
    }
}
