using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Market_System.ServiceLayer
{
    public class Logger
    {
        public static Logger instance;

        private string log_event_path;
        private string log_errors_path;
        private string tests_log_events_path;
        private string tests_log_errors_path;
        private string regular_log_events_path;
        private string regular_log_errors_path;
        private StreamWriter log_event_writer;
        private StreamWriter log_error_writer;
        private StreamReader log_event_reader;
        private StreamReader log_error_reader;

        private Logger()
        {
            string combine_me = "\\logger\\event_logger.txt";
            string combine_me2 = "\\logger\\error_logger.txt";
            string combine_me_tests1 = "\\logger\\tests_error_logger.txt";
            string combine_me_tests2 = "\\logger\\tests_events_logger.txt";

            var temp_path = Directory.GetParent(Environment.CurrentDirectory).FullName;
            if (temp_path.Equals("C:\\Program Files (x86)")) //Meaning that we're running the project.
            {
                string hosting_path = HostingEnvironment.ApplicationPhysicalPath;
                int slice_me = HostingEnvironment.ApplicationPhysicalPath.LastIndexOf('\\');
                string current_path = hosting_path.Substring(0, slice_me);
                slice_me = current_path.LastIndexOf('\\');
                this.regular_log_events_path = current_path.Substring(0, slice_me) + combine_me;
                this.regular_log_errors_path = current_path.Substring(0, slice_me) + combine_me2;
                log_event_path = regular_log_events_path;
                log_errors_path = regular_log_errors_path;
                tests_log_events_path = current_path.Substring(0, slice_me) + combine_me_tests2;
                tests_log_errors_path = current_path.Substring(0, slice_me) + combine_me_tests1;
            }

            else //Meaning that we're running the tests.
            {
                int slice_me = temp_path.LastIndexOf('\\');
                while (!temp_path.Substring(slice_me).Equals("\\MarketSystemRepo"))
                {
                    temp_path = temp_path.Substring(0, slice_me);
                    slice_me = temp_path.LastIndexOf('\\');
                }

                this.regular_log_events_path = temp_path + combine_me;
                this.regular_log_errors_path = temp_path + combine_me2;
                this.tests_log_events_path = temp_path + combine_me_tests2;
                this.tests_log_errors_path = temp_path + combine_me_tests1;

                log_event_path = tests_log_events_path;
                log_errors_path = tests_log_errors_path;
            }
        }

        public static Logger get_instance()
        {
            if(instance==null)
            {
                instance = new Logger();

            }
            return instance;
        }

        public void change_logger_path_to_tests()
        {
            this.log_event_path = tests_log_events_path;
            this.log_errors_path = tests_log_errors_path;
        }

        public void change_logger_path_to_regular()
        {
            this.log_event_path = regular_log_events_path;
            this.log_errors_path = regular_log_errors_path;
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
                
                this.log_error_writer = new StreamWriter(log_errors_path, true); //append = true - instead of overwriting it.

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