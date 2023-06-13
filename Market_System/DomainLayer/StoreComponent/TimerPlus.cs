using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.DomainLayer.StoreComponent
{
    public class TimerPlus : System.Timers.Timer
    {
        private DateTime DueDate;

        public TimerPlus(double interval, DateTime dueDate) : base(interval)
        {
            this.DueDate = dueDate;
        }

        public double MinutesRemains()
        {
            return (this.DueDate - DateTime.Now).TotalMinutes;
        }
    }
}