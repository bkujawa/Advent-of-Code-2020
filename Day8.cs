using System;
using System.Linq;
using System.Collections.Generic;

namespace Advent_of_Code_2020
{
/*
    --- Day 8: Handheld Halting ---
    Your flight to the major airline hub reaches cruising altitude without incident. 
    While you consider checking the in-flight menu for one of those drinks that come with a little umbrella, you are interrupted by the kid sitting next to you.

    Their handheld game console won't turn on! They ask if you can take a look.
    You narrow the problem down to a strange infinite loop in the boot code (your puzzle input) of the device. 
    You should be able to fix it, but first you need to be able to run the code in isolation.

    The boot code is represented as a text file with one instruction per line of text. 
    Each instruction consists of an operation (acc, jmp, or nop) and an argument (a signed number like +4 or -20).

    acc increases or decreases a single global value called the accumulator by the value given in the argument. For example, acc +7 would increase the accumulator by 7. 
        The accumulator starts at 0. After an acc instruction, the instruction immediately below it is executed next.
    jmp jumps to a new instruction relative to itself. The next instruction to execute is found using the argument as an offset from the jmp instruction; 
        for example, jmp +2 would skip the next instruction, jmp +1 would continue to the instruction immediately below it, and jmp -20 would cause the instruction 20 lines above to be executed next.
    nop stands for No OPeration - it does nothing. The instruction immediately below it is executed next.

    For example, consider the following program:

    nop +0
    acc +1
    jmp +4
    acc +3
    jmp -3
    acc -99
    acc +1
    jmp -4
    acc +6
    These instructions are visited in this order:

    nop +0  | 1
    acc +1  | 2, 8(!)
    jmp +4  | 3
    acc +3  | 6
    jmp -3  | 7
    acc -99 |
    acc +1  | 4
    jmp -4  | 5
    acc +6  |
    First, the nop +0 does nothing. 
    Then, the accumulator is increased from 0 to 1 (acc +1) and jmp +4 sets the next instruction to the other acc +1 near the bottom. 
    After it increases the accumulator from 1 to 2, jmp -4 executes, setting the next instruction to the only acc +3. 
    It sets the accumulator to 5, and jmp -3 causes the program to continue back at the first acc +1.

    This is an infinite loop: with this sequence of jumps, the program will run forever. The moment the program tries to run any instruction a second time, you know it will never terminate.
    Immediately before the program would run an instruction a second time, the value in the accumulator is 5.
    Run your copy of the boot code. Immediately before any instruction is executed a second time, what value is in the accumulator?

    To begin, get your puzzle input.

        --- Part Two ---
    After some careful analysis, you believe that exactly one instruction is corrupted.
    Somewhere in the program, either a jmp is supposed to be a nop, or a nop is supposed to be a jmp. 
    (No acc instructions were harmed in the corruption of this boot code.)

    The program is supposed to terminate by attempting to execute an instruction immediately after the last instruction in the file. 
    By changing exactly one jmp or nop, you can repair the boot code and make it terminate correctly.

    For example, consider the same program from above:

    nop +0
    acc +1
    jmp +4
    acc +3
    jmp -3
    acc -99
    acc +1
    jmp -4
    acc +6
    If you change the first instruction from nop +0 to jmp +0, it would create a single-instruction infinite loop, never leaving that instruction. 
    If you change almost any of the jmp instructions, the program will still eventually find another jmp instruction and loop forever.
    However, if you change the second-to-last instruction (from jmp -4 to nop -4), the program terminates! The instructions are visited in this order:

    nop +0  | 1
    acc +1  | 2
    jmp +4  | 3
    acc +3  |
    jmp -3  |
    acc -99 |
    acc +1  | 4
    nop -4  | 5
    acc +6  | 6
    After the last instruction (acc +6), the program terminates by attempting to run the instruction below the last instruction in the file. 
    With this change, after the program terminates, the accumulator contains the value 8 (acc +1, acc +1, acc +6).

    Fix the program so that it terminates normally by changing exactly one jmp (to nop) or nop (to jmp). What is the value of the accumulator after the program terminates?
*/
    public class Day8 : PuzzleSolver
    {
        private int accumulator = 0;
        public Day8(string inputString)
            : base(inputString)
        {
            Name = "Day Eight";
        }

        protected override void SolvePuzzleOne()
        {
            var visitedActions = new List<int>();
            int i = 0;
            while (true)
            {
                // Found loop
                if (visitedActions.Contains(i))
                {
                    break;
                }
                visitedActions.Add(i);
                i += FindNextIndex(this.input[i]);
            }

            Console.WriteLine(this.accumulator);
        }

        private int FindNextIndex(string action)
        {
            var indexChange = 1;
            var actionDelta = Convert.ToInt16(action.Substring(5, action.Length - 5));

            if (action.Substring(0, 3).Equals("jmp"))
            {
                if (action.Contains("+"))
                {
                    return actionDelta;
                }
                else if (action.Contains("-"))
                {
                    return -actionDelta;
                }
            }
            if (action.Substring(0, 3).Equals("acc"))
            {
                if (action.Contains("+"))
                {
                    this.accumulator += actionDelta;
                }
                else if (action.Contains("-"))
                {
                    this.accumulator -= actionDelta;
                }
            }

            return indexChange;
        }

        protected override void SolvePuzzleTwo()
        {
            // Find actions with JMP and NOP (together with their indexes)
            var jmpAndNopActions = FindJmpAndNopActions();

            while (true)
            {
                this.accumulator = 0;
                bool infiniteLoop = false;
                int i = 0;
                // Replace one JMP/NOP action with other, and return previous action
                KeyValuePair<int, string> previousAction = ReplaceJmpAndNopAction(jmpAndNopActions.First().Key, jmpAndNopActions.First().Value);
                var visitedActions = new List<int>();

                while (true)
                {
                    // If we got to end of file
                    if (i >= this.input.Length)
                    {
                        break;
                    }

                    // If we got infinite loop
                    if (visitedActions.Contains(i))
                    {
                        infiniteLoop = true;
                        break;
                    }

                    visitedActions.Add(i);
                    i += FindNextIndex(this.input[i]);
                }

                if (infiniteLoop)
                {
                    // If we got infinite loop, we need to revert previous replacement, do another one and start over, with next action to replace
                    RevertJmpAndNopAction(previousAction.Key, previousAction.Value);
                    jmpAndNopActions.Remove(jmpAndNopActions.First().Key);
                }
                else
                {
                    Console.WriteLine(this.accumulator);
                    break;
                }
            }
        }

        private Dictionary<int, string> FindJmpAndNopActions()
        {
            var actions = new Dictionary<int, string>();
            for (int i = 0; i < this.input.Length; ++i)
            {
                if (this.input[i].Contains("jmp") || this.input[i].Contains("nop"))
                {
                    actions.Add(i, this.input[i]);
                }
            }

            return actions;
        }

        private KeyValuePair<int, string> ReplaceJmpAndNopAction(int index, string action)
        {
            var previousAction = new KeyValuePair<int, string>(index, this.input[index]);

            if (action.Contains("nop"))
            {
                this.input[index] = this.input[index].Replace("nop", "jmp");
            }
            else if (action.Contains("jmp"))
            {
                this.input[index] = this.input[index].Replace("jmp", "nop");
            }

            return previousAction;
        }

        private void RevertJmpAndNopAction(int index, string action)
        {
            this.input[index] = action;
        }
    }
}