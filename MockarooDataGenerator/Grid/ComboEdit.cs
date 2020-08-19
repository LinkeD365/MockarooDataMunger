using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkeD365.MockDataGen
{
    public class ComboItem
    {
        public string Text { get; set; }
        public string LogicalName { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}