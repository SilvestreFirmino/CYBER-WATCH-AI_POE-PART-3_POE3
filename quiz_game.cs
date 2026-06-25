using System;
using System.Collections.Generic;

namespace CYBER_WATCH_AI_POE_PART_2
{
    //this class manages the whole quiz: the questions, the score, and which question we are on
    public class quiz_game
    {//start of class

        //the full list of questions for this quiz
        private List<quiz_question> questions;

        //keeps track of which question the user is currently on
        private int current_index;

        //how many the user has gotten right so far
        private int score;

        //true while a quiz is currently in progress
        //MainWindow checks this to know whether the user's next message
        //should be treated as a quiz answer instead of a normal chat question
        public bool is_active;


        //constructor - runs once when the quiz_game object is first created
        public quiz_game()
        {
            questions = load_questions();
            current_index = 0;
            score = 0;
            is_active = false;
        }


        //this is where all 10+ cybersecurity questions live
        //feel free to add more, or change the wording, as long as you keep the same pattern
        private List<quiz_question> load_questions()
        {//start of load_questions

            List<quiz_question> list = new List<quiz_question>();

            list.Add(new quiz_question
            {
                question_text = "What should you do if you receive an email asking for your password?",
                type = question_type.multiple_choice,
                options = new string[] {
                    "A) Reply with your password",
                    "B) Delete the email",
                    "C) Report the email as phishing",
                    "D) Ignore it"
                },
                correct_answer = "C",
                explanation = "Reporting phishing emails helps prevent scams and protects others too."
            });

            list.Add(new quiz_question
            {
                question_text = "True or False: Using the same password on multiple accounts is safe.",
                type = question_type.true_false,
                options = null,
                correct_answer = "false",
                explanation = "Reusing passwords means one leaked password can unlock all your accounts."
            });

            list.Add(new quiz_question
            {
                question_text = "Which of these is the strongest password?",
                type = question_type.multiple_choice,
                options = new string[] {
                    "A) password123",
                    "B) MyDog2024",
                    "C) Tr#9!vLp$2qZ",
                    "D) 123456"
                },
                correct_answer = "C",
                explanation = "Strong passwords mix upper and lower case letters, numbers, and symbols, and are not predictable."
            });

            list.Add(new quiz_question
            {
                question_text = "True or False: Public Wi-Fi networks are always safe for online banking.",
                type = question_type.true_false,
                options = null,
                correct_answer = "false",
                explanation = "Public Wi-Fi can be intercepted by attackers, so sensitive activity like banking should be avoided on it."
            });

            list.Add(new quiz_question
            {
                question_text = "What is social engineering?",
                type = question_type.multiple_choice,
                options = new string[] {
                    "A) A type of computer virus",
                    "B) Tricking people into giving up confidential information",
                    "C) A method of encrypting data",
                    "D) A firewall configuration"
                },
                correct_answer = "B",
                explanation = "Social engineering relies on manipulating people rather than hacking systems directly."
            });

            list.Add(new quiz_question
            {
                question_text = "True or False: Two-factor authentication adds an extra layer of security beyond your password.",
                type = question_type.true_false,
                options = null,
                correct_answer = "true",
                explanation = "Two-factor authentication requires a second proof of identity, making accounts much harder to break into."
            });

            list.Add(new quiz_question
            {
                question_text = "Which of these is a sign of a phishing website?",
                type = question_type.multiple_choice,
                options = new string[] {
                    "A) A misspelled domain name",
                    "B) A valid security certificate",
                    "C) A familiar logo",
                    "D) A fast loading speed"
                },
                correct_answer = "A",
                explanation = "Phishing sites often use slightly misspelled URLs to impersonate trusted brands."
            });

            list.Add(new quiz_question
            {
                question_text = "True or False: You should update your software and apps regularly.",
                type = question_type.true_false,
                options = null,
                correct_answer = "true",
                explanation = "Updates often patch security flaws that attackers could otherwise exploit."
            });

            list.Add(new quiz_question
            {
                question_text = "What should you do before clicking a link in an unexpected message?",
                type = question_type.multiple_choice,
                options = new string[] {
                    "A) Click it immediately to see where it goes",
                    "B) Hover over it or verify the sender first",
                    "C) Forward it to your friends",
                    "D) Reply asking what it is"
                },
                correct_answer = "B",
                explanation = "Checking the link or sender first helps you spot scams before any damage is done."
            });

            list.Add(new quiz_question
            {
                question_text = "True or False: A locked screen on your phone or laptop is a basic but effective security measure.",
                type = question_type.true_false,
                options = null,
                correct_answer = "true",
                explanation = "Screen locks prevent anyone who picks up your device from accessing it instantly."
            });

            list.Add(new quiz_question
            {
                question_text = "Which of these is the safest way to back up important files?",
                type = question_type.multiple_choice,
                options = new string[] {
                    "A) Only keep one copy on your laptop",
                    "B) Email them to yourself",
                    "C) Use a reputable cloud service or external drive with regular backups",
                    "D) Save them on a public computer"
                },
                correct_answer = "C",
                explanation = "Reliable backups protect your data if your device is lost, stolen, or infected with malware."
            });

            return list;

        }//end of load_questions


        //called when the user types something like "start quiz"
        //resets everything back to the beginning
        public void start_quiz()
        {
            current_index = 0;
            score = 0;
            is_active = true;
        }


        //builds the text the chatbot should show for the question we are currently on
        public string get_current_question_text()
        {//start

            quiz_question q = questions[current_index];

            string display = "Question " + (current_index + 1) + " of " + questions.Count + ":\n" + q.question_text;

            if (q.type == question_type.multiple_choice)
            {
                foreach (string option in q.options)
                {
                    display += "\n" + option;
                }
            }
            else
            {
                display += "\n(answer true or false)";
            }

            return display;

        }//end


        //checks the user's typed answer against the correct answer for the current question
        //returns the feedback message to show in the chat
        public string check_answer(string user_answer)
        {//start

            quiz_question q = questions[current_index];

            string clean_answer = user_answer.Trim().ToUpper();

            if (q.type == question_type.true_false)
            {
                //accept "t"/"f" shorthand as well as the full word
                if (clean_answer == "T")
                    clean_answer = "TRUE";
                else if (clean_answer == "F")
                    clean_answer = "FALSE";
            }
            else
            {
                //for multiple choice, only look at the first letter the user typed
                //this way "C", "c)", or "C) Report the email as phishing" all work
                if (clean_answer.Length > 0)
                    clean_answer = clean_answer.Substring(0, 1);
            }

            bool is_correct = clean_answer == q.correct_answer.ToUpper();

            string feedback;

            if (is_correct)
            {
                score = score + 1;
                feedback = "Correct! " + q.explanation;
            }
            else
            {
                feedback = "Not quite. The correct answer was " + q.correct_answer + ". " + q.explanation;
            }

            return feedback;

        }//end


        //true if there is another question left after the current one
        public bool has_next_question()
        {
            return current_index < questions.Count - 1;
        }


        //moves on to the next question
        public void move_next()
        {
            current_index = current_index + 1;
        }


        //called once the last question has been answered
        //builds the final score message and ends the quiz
        public string get_final_score_message()
        {//start

            is_active = false;

            string message = "Quiz complete! You scored " + score + " out of " + questions.Count + ".\n";

            double percentage = (double)score / questions.Count * 100;

            if (percentage >= 80)
                message += "Great job! You're a cybersecurity pro!";
            else if (percentage >= 50)
                message += "Good effort! Keep learning to stay safe online!";
            else
                message += "Keep practicing - review the explanations above and try again!";

            return message;

        }//end

    }//end of class
}