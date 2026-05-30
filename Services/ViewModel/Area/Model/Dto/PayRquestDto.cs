using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModel.Area.Model.Dto
{
    public class PayRquestDto
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public DateTime PayDate { get; set; }

        public long RefId { get; set; }

        public bool IsPay { get; set; }

    }
}
