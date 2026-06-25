using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace CYBER_WATCH_AI_POE_PART_2
{//start of namespace

    public class tasks
    {//start of the class

        //global connection string , with variable declaration
        string connection = @"Data source=(localdb)\MSSQLLocalDB;Database=master";



        // AUTO CREATE TABLE METHOD
        public void CreateTableIfNotExists()
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                try
                {
                    connect.Open();

                    // Check if table exists and create if not
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='demo_tasks' AND xtype='U')
                        BEGIN
                            CREATE TABLE demo_tasks (
                                task_id INT IDENTITY(1,1) PRIMARY KEY,
                                task_name NVARCHAR(100) NOT NULL,
                                task_description NVARCHAR(255),
                                task_dueDate NVARCHAR(50),
                                task_status NVARCHAR(20)
                            )
                        END";

                    SqlCommand createCommand = new SqlCommand(createTableQuery, connect);
                    createCommand.ExecuteNonQuery();

                    connect.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error creating table: " + error.Message);
                }
            }
        }

        //creating method to test the connection
        public void test_connection()
        {//start of test connection method

            /*
             * SqlConnection -used to make conneciton with Database
             * SqlCommand - used to run queries , all of them
             * SqlDataReader -Used to read what is collected by
             *                 the SqlCommand , and show the user data
             */


            //connect to the database
            SqlConnection connect = new SqlConnection(connection);

            //try and catch any error that it will throw
            try
            {
                //Open the connection and close the connection
                connect.Open();
                //put the database query and run it
                MessageBox.Show("connected..");
                //then close it after you are done
                connect.Close();

            }
            catch (Exception error)
            {
                //show message error
                MessageBox.Show(error.Message);
            }

        }//end of conneciton method






        //method to insert or store the tasks 
        public void insert_task(string name, string description, string dueDate, string status)
        {//start of insert method

            //create the connection instance
            // SqlConnection connects = new SqlConnection(connection);

            //you must use try and catch 

            //make sure the using is covered by the try and catch
            using (SqlConnection connects = new SqlConnection(connection))
            {//start of using


                //open the connection
                connects.Open();


                //do the query
                string query = $"insert into demo_tasks values('{name}','{description}','{dueDate}','{status}');";

                //then use the SqlCOmmand to run the query
                SqlCommand run_query = new SqlCommand(query, connects);
                //then run the query as a nonExecuteQuery()
                run_query.ExecuteNonQuery();


                connects.Close();


            }//end of using


        }//end of insert method



        //method to auto load all the user's tasks
        public void load_tasks(ListView view_task)
        {//start of the load task method


            /*
 * SqlConnection -used to make conneciton with Database
 * SqlCommand - used to run queries , all of them
 * SqlDataReader -Used to read what is collected by
 *                 the SqlCommand , and show the user data
 */

            //create an instance for the connection
            SqlConnection connects = new SqlConnection(connection);

            //open connection
            connects.Open();

            //temp variable , to hold the query
            string query = $"select * from demo_tasks;";

            //use sqlCommand to run this query
            SqlCommand run_query = new SqlCommand(query, connects);

            //reading what the command found and show/display , using SqlDataReader
            SqlDataReader data_collect = run_query.ExecuteReader();
            //temp variable for boolean, to get the status of data found or not found , Not found mean false but if Found then it is true
            bool data_found = false;


            //use the while Loop to get all the columns
            while (data_collect.Read())
            {//start while loop

                //data found must be true
                data_found = true;


                //getting all the columns by their names
                string task_id = data_collect["task_id"].ToString();
                string task_name = data_collect["task_name"].ToString();
                string task_description = data_collect["task_description"].ToString();
                string task_dueDate = data_collect["task_dueDate"].ToString();
                string task_status = data_collect["task_status"].ToString();

                //add the found tasks to the ListView
                view_task.Items.Add(task_id + " " + task_name + " with " + task_description + " due on " + task_dueDate + " and is " + task_status);

            }//end of while loop


            //display error message
            if (!data_found)
            {//start of if
                //display the message in a listView
                view_task.Items.Add("No task found");
            }//end of if

            //close the connection
            connects.Close();




        }//end of the load task method

        //method to update tasks
        public void update_taskStatus(int id)
        {//start of update_taskStatus method

            //create connection
            SqlConnection connects = new SqlConnection(connection);

            //then open the connection
            connects.Open();

            //then use SqlCommand to run the query
            //temp variable to hold the query
            string query = $"update demo_tasks set task_status='done' where task_id={id}";

            //then run the query
            SqlCommand run_query = new SqlCommand(query, connects);
            run_query.ExecuteNonQuery();



            //and close the conneciton once done
            connects.Close();

        }//end of update_taskStatus method



        //method to delete tasks
        public void delete_task(int id)
        {//start of delete_task method


            //connect
            SqlConnection connects = new SqlConnection(connection);

            //then open the connection
            connects.Open();

            //temp variable to hold the query
            string query = $"delete from demo_tasks where task_id={id}";

            //run the query 
            SqlCommand run_query = new SqlCommand(query, connects);

            //run
            run_query.ExecuteNonQuery();


            //close the connection after using it
            connects.Close();



        }//end of delete_task method


    }//end of class
}//end of namespace