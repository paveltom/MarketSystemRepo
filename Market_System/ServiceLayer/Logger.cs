﻿using System;
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


        private Logger ()
        {
            string combine_me = "\\logger\\event_logger.txt";
            string combine_me2 = "\\logger\\error_logger.txt";
            this.log_event_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + combine_me;
            this.log_errors_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + combine_me2;
            this.log_event_writer = new StreamWriter(log_event_path);
            this.log_error_writer = new StreamWriter(log_errors_path);

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

                this.log_event_writer.WriteLine(DateTime.Now.ToLongDateString() + " : " + new_event);

     
                

            }
         }


        public void record_error(string new_error)
        {
            lock (this)
            {

                this.log_error_writer.WriteLine(DateTime.Now.ToLongDateString() + " : " + new_error);

     
                

            }
        }




    }
}