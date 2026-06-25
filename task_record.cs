namespace CYBER_WATCH_AI_POE_PART_2
{
    //a small container that holds one row from the demo_tasks table
    //used when we read tasks back out of the database to show in the GUI
    public class task_record
    {
        public int task_id;
        public string task_name;
        public string task_description;
        public string task_due_date;
        public string task_status;
    }
}