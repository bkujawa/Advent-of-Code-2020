using System;
using System.Linq;
using System.Collections.Generic;

namespace Advent_of_Code_2020
{
/*
    --- Day 6: Custom Customs ---
    As your flight approaches the regional airport where you'll switch to a much larger plane, customs declaration forms are distributed to the passengers.
    The form asks a series of 26 yes-or-no questions marked a through z. 
    All you need to do is identify the questions for which anyone in your group answers "yes". Since your group is just you, this doesn't take very long.

    However, the person sitting next to you seems to be experiencing a language barrier and asks if you can help. 
    For each of the people in their group, you write down the questions for which they answer "yes", one per line. For example:

    abcx
    abcy
    abcz
    In this group, there are 6 questions to which anyone answered "yes": a, b, c, x, y, and z. 
    (Duplicate answers to the same question don't count extra; each question counts at most once.)

    Another group asks for your help, then another, and eventually you've collected answers from every group on the plane (your puzzle input). 
    Each group's answers are separated by a blank line, and within each group, each person's answers are on a single line. For example:
    abc

    a
    b
    c

    ab
    ac

    a
    a
    a
    a

    b
    This list represents answers from five groups:

    The first group contains one person who answered "yes" to 3 questions: a, b, and c.
    The second group contains three people; combined, they answered "yes" to 3 questions: a, b, and c.
    The third group contains two people; combined, they answered "yes" to 3 questions: a, b, and c.
    The fourth group contains four people; combined, they answered "yes" to only 1 question, a.
    The last group contains one person who answered "yes" to only 1 question, b.
    In this example, the sum of these counts is 3 + 3 + 3 + 1 + 1 = 11.

    For each group, count the number of questions to which anyone answered "yes". What is the sum of those counts?

--- Part Two ---
    As you finish the last group's customs declaration, you notice that you misread one word in the instructions:

    You don't need to identify the questions to which anyone answered "yes"; you need to identify the questions to which everyone answered "yes"!
    Using the same example as above:

    abc

    a
    b
    c

    ab
    ac

    a
    a
    a
    a

    b
    This list represents answers from five groups:

    In the first group, everyone (all 1 person) answered "yes" to 3 questions: a, b, and c.
    In the second group, there is no question to which everyone answered "yes".
    In the third group, everyone answered yes to only 1 question, a. Since some people did not answer "yes" to b or c, they don't count.
    In the fourth group, everyone answered yes to only 1 question, a.
    In the fifth group, everyone (all 1 person) answered "yes" to 1 question, b.
    In this example, the sum of these counts is 3 + 0 + 1 + 1 + 1 = 6.

    For each group, count the number of questions to which everyone answered "yes". What is the sum of those counts?
*/
    public class Day6 : PuzzleSolver
    {
        private Dictionary<char, bool> questionsAnswered = new Dictionary<char, bool>();

        public Day6(string inputString) 
            : base(inputString)
        {
            Name = "Day Six";
        }

        protected override void SolvePuzzleOne()
        {
            PrepareQuestionsAnswered();
            var sumOfYesAnswers = 0;
            foreach (var personAnswers in this.input)
            {
                if (string.IsNullOrWhiteSpace(personAnswers))
                {
                    sumOfYesAnswers += CountYesAnswers();
                    ResetQuestionsAnswered();
                    continue;
                }

                for (int i = 0; i < personAnswers.Length; ++i)
                {
                    this.questionsAnswered[personAnswers[i]] = true;
                }
            }

            Console.WriteLine(sumOfYesAnswers);
        }

        private int CountYesAnswers()
        {
            var answers = 0;
            foreach (var answer in this.questionsAnswered)
            {
                if (answer.Value == true)
                {
                    answers++;
                }
            }

            return answers;
        }

        private void PrepareQuestionsAnswered()
        {
            for (int i = 0; i < 26; ++i)
            {
                this.questionsAnswered.Add(Convert.ToChar(i + 'a'), false);   
            }
        }

        private void ResetQuestionsAnswered()
        {
            for (int i = 0; i < 26; ++i)
            {
                this.questionsAnswered[Convert.ToChar(i + 'a')] = false;
            }
        }

        protected override void SolvePuzzleTwo()
        {
            var sumOfYesAnswers = 0;
            var groupOfAnswers = new List<string>();
            foreach (var personAnswers in this.input)
            {
                if (string.IsNullOrWhiteSpace(personAnswers))
                {
                    var longestAnswer = FindLongestAnswer(groupOfAnswers);

                    sumOfYesAnswers += CountAllYesAnswers(longestAnswer, groupOfAnswers);

                    groupOfAnswers.Clear();
                    continue;
                }

                groupOfAnswers.Add(personAnswers);
            }

            Console.WriteLine(sumOfYesAnswers);
        }

        private string FindLongestAnswer(List<string> groupOfAnswers)
        {
            var longestAnswer = "";
            for (int i = 0; i < groupOfAnswers.Count; ++i)
            {
                if (groupOfAnswers[i].Length > longestAnswer.Length)
                {
                    longestAnswer = groupOfAnswers[i];
                }
            }

            return longestAnswer;
        }

        private int CountAllYesAnswers(string longestAnswer, List<string> groupOfAnswers)
        {
            var answers = 0;
            foreach(var character in longestAnswer)
            {
                if (groupOfAnswers.All(x => x.Contains(character)))
                {
                    answers++;
                }
            }

            return answers;
        }
    }
}