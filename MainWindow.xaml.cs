using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CYBER_WATCH_AI_POE_PART_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
        public partial class MainWindow : Window
        {// start of main window class


            //creating an instance for the class Array
            ArrayList reply = new ArrayList();
            ArrayList ignore = new ArrayList();



            public MainWindow()
            {
                InitializeComponent();

                // creating an instance for the class respond without a object name, 
                new respond(reply, ignore) { };
            }

            private void send(object sender, RoutedEventArgs e)
            { // start of send method

                //get the question from the design
                //user input

                string questions = question.Text.ToString();

                //if statement to check if user extend a question or not
                if (questions == "")
                {

                    //call the error method();
                    error_method();

                }
                else
                {//start of else block

                    //temp variables and arrays
                    string[] words = questions.Split(' ');

                    bool found = false;
                    string message = string.Empty;

                    Random indexer = new Random();

                    ArrayList per_word = new ArrayList();
                    ArrayList answers_found = new ArrayList();

                    //alterate per word from the words array
                    foreach (String word in words)
                    {// start of the main foreach 

                        //check if the word is allowed or not

                        if (!ignore.Contains(word.ToLower()))
                        {//start of  check word if  statement 

                            //MessageBox.Show( word + " allowed");
                            per_word.Clear();


                            //foreach to search for the answer of the word allowed
                            foreach (string answer in reply)
                            {//start answer loop

                                if (answer.Contains(word.ToLower()))
                                { // start of if statement


                                    found = true;

                                    //store all answers for the word
                                    per_word.Add(answer);



                                }// end of if statement



                            }//end of answer loop

                            //then check if found is true and store
                            //per random
                            if (found)
                            { // start of found if statement


                                //get the random indexer
                                int indexing = indexer.Next(0, per_word.Count);


                                // store one answer per word now
                                answers_found.Add(per_word[indexing]);



                            }// end of found if statement

                        }// end of check word if statement

                    }// end of the main foreach

                    //Check and show the user the answers
                    if (found)
                    {// start of found if tue

                        //get all of answers and show to the user

                        foreach (string per_answer in answers_found)
                        {// start of show answer loop
                            message += per_answer + "\n";
                        }// end of show answer loop

                        //add the message to answers to the list view
                        chats.Items.Add(message);

                        //auto scroll to the end of the list view

                        chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);


                    }// end of found if tue

                }// end of else block

            }// end of send method

            //error method
            private void error_method()
            { //start of error method

                //call the chats which is a listview

                chats.Items.Add(
                    new TextBlock
                    {
                        Inlines = {
                    new Run{

                        Text = "Xs Ai : " ,
                        Foreground = Brushes.Blue,

                    },

                    new Run{
                    Text = "Please enter a question !! ",
                     Foreground = Brushes.Red

                    }

                        }

                    }

                    );

            }// end of error method

            private void submit_name(object sender, RoutedEventArgs e)
            {// start of submit name method


                //temp variables
                string filename = "user_names.txt";

                //check if the name exists or not ,then auto create
                if (!File.Exists(filename))
                {// start of if statement

                    //AUTO CREATE THE FILE USING AppendAllText() function
                    File.AppendAllText(filename, "auto_create\n");

                }// end of if statement

                //temp variable to store the name
                string name = user_name.Text.ToString();
                bool found = check_name(name);

                if (!found)
                {// start of if statement

                    //store username into the  a text file
                    MessageBox.Show("Welcome " + name + " to Xs Ai !!");
                    File.AppendAllText(filename, name + "\n");
                    //hide the name input and submit button

                    name_grid.Visibility = Visibility.Hidden;
                    Chat_Grid.Visibility = Visibility.Visible;


                }// end of if statement

                else
                { //start of else statement

                    //welcome back the user

                    MessageBox.Show("Welcome back " + name + " to Xs Ai !!");
                    name_grid.Visibility = Visibility.Hidden;
                    Chat_Grid.Visibility = Visibility.Visible;

                }// end of else statement

            }// end of submit name method

            //check_name method to check name if exists or not

            private Boolean check_name(string name)
            { // start of check name method


                //temporary for the text file part

                string filename = "user_names.txt";

                bool name_found = false;

                //one dimension array to read all names from the text file

                string[] names = File.ReadAllLines(filename);
                //for each to look through the one dimension array to search fo current user name

                foreach (string search_name in names)
                {// start of for each loop

                    //if statement to check if the user name is found or not

                    if (search_name.ToLower() == name.ToLower())
                    {// start of if statement

                        // name_found but be true
                        name_found = true;



                    }// end of if statement

                }// end of for each loop

                //returning status if the user is found or not
                return name_found;

            }// end of check name method
        }
}
