using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Курсач_сайко_1125;

namespace BD
{
    public class DB
    {
        static Dayn1Context peremennaya;
        public static Dayn1Context GetInstance()
        {
            if (peremennaya == null)
                peremennaya = new Dayn1Context();
            return peremennaya;
        }

        static Dayn1Context instance;
        public static Dayn1Context Instance
        {
            get
            {
                if (instance == null)
                    instance = new Dayn1Context();
                return instance;
            }

        }


    }
}
