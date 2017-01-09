using Jitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JitterUser
{
    class StringParser : IParser<string>
    {
        public int Check(byte[] objdata)
        {
            if (objdata[0] == ';')
                return -1;

            int semiindex0 = 0;
            int semiindex1 = 0;
            for (int i = 1; i < objdata.Length; i++)
            {
                if (objdata[i] == ';')
                {
                    if (semiindex0 == 0)
                        semiindex0 = i;
                    else
                    {
                        semiindex1 = i;
                        break;
                    }
                }
            }

            if (semiindex1 == 0)
                return -1;

            if ((semiindex1 - semiindex0) - 1 == 0)
                return -1;

            var indexstring = Encoding.UTF8.GetString(objdata, 0, semiindex0);
            var stringstring = Encoding.UTF8.GetString(objdata, semiindex0 + 1, (semiindex1 - semiindex0)-1);

            int index;
            if (!int.TryParse(indexstring, out index))
                return -1;

            return semiindex1+1;
        }

        public string Parse(byte[] objdata, out int index)
        {
            int semiindex0 = 0;
            int semiindex1 = 0;
            for (int i = 1; i < objdata.Length; i++)
            {
                if (objdata[i] == ';')
                {
                    if (semiindex0 == 0)
                        semiindex0 = i;
                    else
                        semiindex1 = i;
                }
            }

            var indexstring = Encoding.UTF8.GetString(objdata, 0, semiindex0);
            var stringstring = Encoding.UTF8.GetString(objdata, semiindex0 + 1, (semiindex1 - semiindex0) - 1);

            index = int.Parse(indexstring);

            return stringstring;
        }
    }
}
