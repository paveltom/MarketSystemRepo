using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market_System.ServiceLayer
{
    public class Logger
    {

        public static Logger instance;
        private Dictionary<string, string> event_log;
        private Dictionary<string, string> error_log;

        private Logger ()
        {
           
        }

        public static Logger get_instance()
        {
            if(instance==null)
            {
                instance = new Logger();
            }
            return instance;
        }


        public Dictionary<string, string> get_event_log()
        {

            return this.event_log;
        }
        public Dictionary<string,string> get_error_log()
        {
            return this.error_log;
        }

        public void record_event(string new_event)
        {
            lock (this)
            {
                this.event_log.Add(DateTime.Now.ToString(), new_event);
            }
         }


        public void record_error(string new_error)
        {
            lock (this)
            {
                this.event_log.Add(DateTime.Now.ToString(), new_error);
            }
        }




    }
}