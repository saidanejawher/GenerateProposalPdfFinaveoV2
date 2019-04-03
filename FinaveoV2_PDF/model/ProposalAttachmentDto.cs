using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaveoV2_PDF.model
{
    public class ProposalAttachmentDto
    {
        public string DestinationPath { get; set; }

        public byte[] Content { get; set; }
    }
}
