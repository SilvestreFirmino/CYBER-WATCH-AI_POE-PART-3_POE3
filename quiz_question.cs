using System;
using System.Collections.Generic;

namespace CYBER_WATCH_AI_POE_PART_2
{
   

    //this class just holds the details for ONE quiz question
    //think of it like a little box that carries everything about one question
    public class quiz_question
    {
       
        // The text of the quiz question
        public string Text { get; set; }

        // The correct answer to the question
        public string correctAnswer { get; set; }

        // A list of wrong answer options for the question
        public List<string> wrongAnswer { get; set; }
    
}
}