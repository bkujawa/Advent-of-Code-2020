using System;

namespace Advent_of_Code_2020
{
/*
    --- Day 2: Password Philosophy ---
    Your flight departs in a few days from the coastal airport; the easiest way down to the coast from here is via toboggan.
    The shopkeeper at the North Pole Toboggan Rental Shop is having a bad day. "Something's wrong with our computers; we can't log in!" You ask if you can take a look.

    Their password database seems to be a little corrupted: some of the passwords wouldn't have been allowed by the Official Toboggan Corporate Policy that was in effect when they were chosen.

    To try to debug the problem, they have created a list (your puzzle input) of passwords (according to the corrupted database) and the corporate policy when that password was set.
    For example, suppose you have the following list:

    1-3 a: abcde
    1-3 b: cdefg
    2-9 c: ccccccccc
    Each line gives the password policy and then the password. 
    The password policy indicates the lowest and highest number of times a given letter must appear for the password to be valid. 
    For example, 1-3 a means that the password must contain a at least 1 time and at most 3 times.

    In the above example, 2 passwords are valid. 
    The middle password, cdefg, is not; it contains no instances of b, but needs at least 1. 
    The first and third passwords are valid: they contain one a or nine c, both within the limits of their respective policies.

    How many passwords are valid according to their policies?

    --- Part Two ---
    While it appears you validated the passwords correctly, they don't seem to be what the Official Toboggan Corporate Authentication System is expecting.

    The shopkeeper suddenly realizes that he just accidentally explained the password policy rules from his old job at the sled rental place down the street! 
    The Official Toboggan Corporate Policy actually works a little differently.

    Each policy actually describes two positions in the password, where 1 means the first character, 2 means the second character, and so on. 
    (Be careful; Toboggan Corporate Policies have no concept of "index zero"!) 
    Exactly one of these positions must contain the given letter. Other occurrences of the letter are irrelevant for the purposes of policy enforcement.

    Given the same example list from above:

    1-3 a: abcde is valid: position 1 contains a and position 3 does not.
    1-3 b: cdefg is invalid: neither position 1 nor position 3 contains b.
    2-9 c: ccccccccc is invalid: both position 2 and position 9 contain c.
    How many passwords are valid according to the new interpretation of the policies?
*/
    public class Day2 : PuzzleSolver
    {
        public Day2(string inputString) : base(inputString)
        {
            Name = "Day Two";
        }

        protected override void SolvePuzzleOne()
        {
            int validPasswords = 0;
            for (int i = 0; i < this.input.Length; ++i)
            {
                if (CheckIfPasswordIsValid(this.input[i]))
                {
                    validPasswords++;
                }
            }
            Console.WriteLine(validPasswords);
        }

        protected override void SolvePuzzleTwo()
        {
            int validPasswords = 0;
            for (int i = 0; i < this.input.Length; ++i)
            {
                if (CheckIfPasswordIsValidForExtendedPolicy(this.input[i]))
                {
                    validPasswords++;
                }
            }
            Console.WriteLine(validPasswords);
        }

        private bool CheckIfPasswordIsValid(string policyAndPassword)
        {
            var policy = TakePolicyAndPassword(policyAndPassword);
            var occurrence = 0;

            for (int i = 0; i < policy.password.Length; ++i)
            {
                if (policy.password[i].Equals(policy.character))
                {
                    occurrence++;
                }
                if (occurrence > policy.maximal)
                {
                    return false;
                }
            }

            if (occurrence < policy.minimal)
            {
                return false;
            }
            
            return true;
        }

        private bool CheckIfPasswordIsValidForExtendedPolicy(string policyAndPassword)
        {
            var policy = TakePolicyAndPassword(policyAndPassword);

            if (policy.password[policy.minimal - 1] == policy.character && policy.password[policy.maximal - 1] == policy.character)
            {
                return false;
            }

            if (policy.password[policy.minimal - 1] != policy.character && policy.password[policy.maximal - 1] != policy.character)
            {
                return false;
            }

            return true;
        }

        private (int minimal, int maximal, char character, string password) TakePolicyAndPassword(string policyAndPassword)
        {
            string minimalPolicy = "";
            string maximalPolicy = "";

            minimalPolicy += policyAndPassword[0];
            if (char.IsDigit(policyAndPassword[1]))
            {
                minimalPolicy += policyAndPassword[1];
                policyAndPassword = policyAndPassword.Remove(0, 3);
            }
            else 
            {
                policyAndPassword = policyAndPassword.Remove(0, 2);
            }
            var min = Convert.ToInt32(minimalPolicy);

            maximalPolicy += policyAndPassword[0];
            if (char.IsDigit(policyAndPassword[1]))
            {
                maximalPolicy += policyAndPassword[1];
                policyAndPassword = policyAndPassword.Remove(0, 3);
            }
            else 
            {
                policyAndPassword = policyAndPassword.Remove(0, 2);
            }
            var max = Convert.ToInt32(maximalPolicy);

            var policyCharacter = policyAndPassword[0];
            var password = policyAndPassword.Remove(0, 3);

            return new (min, max, policyCharacter, password);
        }
    }
}