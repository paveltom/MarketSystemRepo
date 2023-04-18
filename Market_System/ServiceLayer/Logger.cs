using System;
using System.Collections.Generic;
using System.IO;

namespace Market_System.ServiceLayer
{
    public class Logger
    {
        public static Logger instance;

        private string log_event_path;
        private string log_errors_path;
        private StreamWriter log_event_writer;
        private StreamWriter log_error_writer;
        private StreamReader log_event_reader;
        private StreamReader log_error_reader;

        private Logger ()
        {
            string combine_me = "\\logger\\event_logger.txt";
            string combine_me2 = "\\logger\\error_logger.txt";
            this.log_event_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + combine_me;
            this.log_errors_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + combine_me2;
        
        }

        public static Logger get_instance()
        {
            if(instance==null)
            {
                instance = new Logger();

            }
            return instance;
        }

        public void record_event(string new_event)
        {
            lock (this)
            {
                this.log_event_writer = new StreamWriter(log_event_path, true); //append = true - instead of overwriting it.
                
                this.log_event_writer.WriteLine(DateTime.Now.ToLongDateString() + " : " + new_event);
                this.log_event_writer.Close();
            }
         }

        public void record_error(string new_error)
        {
            lock (this)
            {
                
                this.log_error_writer = new StreamWriter(log_errors_path);

                this.log_error_writer.WriteLine(DateTime.Now.ToLongDateString() + " : " + new_error);
                this.log_error_writer.Close();
            }
        }

        public string Read_Events_Record()
        {
            
            
            this.log_event_reader = new StreamReader(log_event_path);
            string return_me= this.log_event_reader.ReadToEnd();
            this.log_event_reader.Close();
            return return_me;
        }

        public string Read_Errors_Record()
        {

            this.log_error_reader = new StreamReader(log_errors_path);
            string return_me= this.log_error_reader.ReadToEnd();
            this.log_error_reader.Close();
            return return_me;
        }
    }
}