using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaveoV2_PDF.model
{
    public class ManagementFeesDto
    {

        public double? MinimumFees { get; set; }

        public double? MaximumFees { get; set; }

        public string MinimumFeesUnit { get; set; }

        public string MaximumFeesUnit { get; set; }
    }
}
