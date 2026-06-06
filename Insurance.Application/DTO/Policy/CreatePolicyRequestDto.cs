using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Application.DTOs.Policy
{
    public class CreatePolicyRequestDto
    {
        //public int QuoteId { get; set; }

        public DateTime PolicyStartDate { get; set; }
    }
}