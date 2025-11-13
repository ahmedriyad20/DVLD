using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Project.Classes
{
    public class clsFormat
    {
        public static string DateToShort(DateTime dt)
        {
            return dt.ToString("dd/MMM/yyyy");
        }
    }
}
