using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDiator.Tests.Events
{
    public class MyEvent : IMDiatorEvent
    {
        public int Id { get; set; } = 1;
    }
}
