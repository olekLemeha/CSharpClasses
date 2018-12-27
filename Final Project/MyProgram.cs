using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FinalProject
{
    class MyProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter you name: ");
            string name = Console.ReadLine();

            Console.WriteLine($"Greetings, {name} we are going to play a Trivia game today!Are you excited? \n\nThere will be 5 questions. Please provide answer for each question by entering a / b / c / d and pressing 'Enter' key.\nLet's go!");
            string pathToFile = @"c:\Class9\questions.txt";
            string pathToOutFile = @"c:\Class9\score.txt";
            List<Question> questions = new List<Question>();

            int correctNum = 0;

            //if statement checks if the file "pathToFile" exists 

            if (!File.Exists(pathToFile))
            {
                Console.WriteLine("Could not find file " + pathToFile);
                return;
            }
            //Load the questions from the file using a method
            questions = LoadQuestionsFromFile(pathToFile);

            //Randomize the questions list using a method
            RandomizeQuestions(questions);

            //Finds the number of correct answers using a method
            correctNum = AskQuestion(questions, correctNum);

            //Calculate the final score using a method
            CalculateScore(pathToOutFile, correctNum, name);

        }

        private static int AskQuestion(List<Question> questions, int correctNum)
        {
            for (int i = 0; i < questions.Count - 5; i++)
            {
                Console.WriteLine($"\n{i + 1}.{questions[i].question} \n A-{questions[i].answers[0].answer} \n B-{questions[i].answers[1].answer}  \n C-{questions[i].answers[2].answer} \n D-{questions[i].answers[3].answer}");


                Console.WriteLine("\nEnter the answer");
                var answer = Console.ReadLine().ToUpper();
                if (questions[i].answers[answer[0] - 65].isCorrect)
                {
                    correctNum++;
                    Console.WriteLine("\nCorrect!");
                }
                else
                {
                    Console.WriteLine("\nIncorrect!");
                }

            }
            return correctNum;
        }

        private static List<Question> LoadQuestionsFromFile(string pathToFile)
        {
            string[] lines = File.ReadAllLines(pathToFile);

            var questions = new List<Question>();
            //fill the questions list with the questions and answers
            for (int i = 0; i < lines.Length; i = i + 5)
            {
                Question ques = new Question();
                ques.question = lines[i];
                //create the answers and check for the right answer
                for (int j = 0; j < 4; j++)
                {
                    Answer correct = new Answer();
                    correct.answer = lines[i + j + 1];

                    if (correct.answer.StartsWith("+"))
                    {
                        correct.answer = correct.answer.Substring(1);
                        correct.isCorrect = true;

                    }
                    ques.answers[j] = correct;
                }
                questions.Add(ques);
            }
            return questions;
        }
        
        private static void RandomizeQuestions(List<Question> questions)
        {
            Random rand = new Random();
            for (int k = 0; k < questions.Count; k++)
            {
                var current = questions[k];
                var randomIndex = rand.Next(questions.Count);
                questions[k] = questions[randomIndex];
                questions[randomIndex] = current;
            }
        }
       
        private static void CalculateScore(string pathToOutFile, int correctNum,string name)
        {
            var score = ((decimal)correctNum / 5) * 100;
            //Save the time, name, and score in file
            using (StreamWriter sw = new StreamWriter(pathToOutFile, true))
            {
                sw.WriteLine($"{DateTime.Now} {name} {score}");
            }

            Console.WriteLine($"Your total score is {correctNum} out of 5. Congratulations, that is {score}");
        }
    }
}

