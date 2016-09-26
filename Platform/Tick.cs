using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform
{
    public enum Exchange
    {
        SPBFUT, MICEX, OPTION
    }

    public class Tick
    {
        public double priceTick;
        public double deltaTick;
        public Byte buy;
        public DateTime dateTimeTick;
        public int volumeTick;
        public string paperCode;
        public Exchange exchangeTick;
    }
}
