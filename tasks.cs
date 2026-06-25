using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace CYBER_WATCH_AI_POE_PART_2
{
    public class tasks
    {
        // Global connection string mapped to LocalDB master or custom db instancing
        private readonly string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

        // Auto creates the table structure safely on startup if it doesn't exist
        public void CreateTableIfNotExists()
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                try
                {
                    connect.Open();
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='demo_tasks' AND xtype='U')
                        BEGIN
                            CREATE TABLE demo_tasks (
                                task_id INT IDENTITY(1,1) PRIMARY KEY,
                                task_name NVARCHAR(100) NOT NULL,
                                task_description NVARCHAR(255),
                                task_due_date NVARCHAR(50),
                                task_status NVARCHAR(20)
                            )
                        END";

                    using (SqlCommand createCommand = new SqlCommand(createTableQuery, connect))
                    {
                        createCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error creating table: " + error.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Test the database availability
        public void test_connection()
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                try
                {
                    connect.Open();
                    MessageBox.Show("Database Connection Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception error)
                {
                    MessageBox.Show("Connection Failed: " + error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Safe Insert: Fixed vulnerability by implementing Parameterized SQL Commands
        public void insert_task(string name, string description, string dueDate, string status)
        {
            using (SqlConnection connects = new SqlConnection(connection))
            {
                try
                {
                    connects.Open();
                    string query = "INSERT INTO demo_tasks (task_name, task_description, task_due_date, task_status) VALUES (@name, @desc, @due, @status);";

                    using (SqlCommand run_query = new SqlCommand(query, connects))
                    {
                        run_query.Parameters.AddWithValue("@name", name);
                        run_query.Parameters.AddWithValue("@desc", description);
                        run_query.Parameters.AddWithValue("@due", dueDate);
                        run_query.Parameters.AddWithValue("@status", status);

                        run_query.ExecuteNonQuery();
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("Insert failed: " + error.Message);
                }
            }
        }

        // Load Tasks directly into WPF ListView UI element
        public void load_tasks(ListView view_task)
        {
            view_task.Items.Clear(); // Clear old items

            using (SqlConnection connects = new SqlConnection(connection))
            {
                try
                {
                    connects.Open();
                    string query = "SELECT * FROM demo_tasks;";

                    using (SqlCommand run_query = new SqlCommand(query, connects))
                    using (SqlDataReader data_collect = run_query.ExecuteReader())
                    {
                        bool data_found = false;

                        while (data_collect.Read())
                        {
                            data_found = true;
                            string task_id = data_collect["task_id"].ToString();
                            string task_name = data_collect["task_name"].ToString();
                            string task_description = data_collect["task_description"].ToString();
                            // Fixed Typo alignment to match 'task_due_date' columns in schema
                            string task_dueDate = data_collect["task_due_date"].ToString();
                            string task_status = data_collect["task_status"].ToString();

                            view_task.Items.Add($"{task_id} | {task_name} - {task_description} (Due: {task_dueDate}) [{task_status}]");
                        }

                        if (!data_found)
                        {
                            view_task.Items.Add("No active tasks found.");
                        }
                    }
                }
                catch (Exception error)
                {
                    view_task.Items.Add("Error loading tasks: " + error.Message);
                }
            }
        }

        // Update task status flag safely
        public void update_taskStatus(int id)
        {
            using (SqlConnection connects = new SqlConnection(connection))
            {
                connects.Open();
                string query = "UPDATE demo_tasks SET task_status = 'done' WHERE task_id = @id";

                using (SqlCommand run_query = new SqlCommand(query, connects))
                {
                    run_query.Parameters.AddWithValue("@id", id);
                    run_query.ExecuteNonQuery();
                }
            }
        }

        // Delete item record from system
        public void delete_task(int id)
        {
            using (SqlConnection connects = new SqlConnection(connection))
            {
                connects.Open();
                string query = "DELETE FROM demo_tasks WHERE task_id = @id";

                using (SqlCommand run_query = new SqlCommand(query, connects))
                {
                    run_query.Parameters.AddWithValue("@id", id);
                    run_query.ExecuteNonQuery();
                }
            }
        }
    }
}