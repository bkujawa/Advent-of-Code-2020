using System;
using System.Linq;
using System.Collections.Generic;

namespace Advent_of_Code_2020
{
/*
    --- Day 7: Handy Haversacks ---
    You land at the regional airport in time for your next flight. 
    In fact, it looks like you'll even have time to grab some food: all flights are currently delayed due to issues in luggage processing.

    Due to recent aviation regulations, many rules (your puzzle input) are being enforced about bags and their contents; 
    bags must be color-coded and must contain specific quantities of other color-coded bags. 
    Apparently, nobody responsible for these regulations considered how long they would take to enforce!

    For example, consider the following rules:
    light red bags contain 1 bright white bag, 2 muted yellow bags.
    dark orange bags contain 3 bright white bags, 4 muted yellow bags.
    bright white bags contain 1 shiny gold bag.
    muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
    shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
    dark olive bags contain 3 faded blue bags, 4 dotted black bags.
    vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
    faded blue bags contain no other bags.
    dotted black bags contain no other bags.

    These rules specify the required contents for 9 bag types. 
    In this example, every faded blue bag is empty, every vibrant plum bag contains 11 bags (5 faded blue and 6 dotted black), and so on.

    You have a shiny gold bag. If you wanted to carry it in at least one other bag, how many different bag colors would be valid for the outermost bag? 
    (In other words: how many colors can, eventually, contain at least one shiny gold bag?)

    In the above rules, the following options would be available to you:

    A bright white bag, which can hold your shiny gold bag directly.
    A muted yellow bag, which can hold your shiny gold bag directly, plus some other bags.
    A dark orange bag, which can hold bright white and muted yellow bags, either of which could then hold your shiny gold bag.
    A light red bag, which can hold bright white and muted yellow bags, either of which could then hold your shiny gold bag.
    So, in this example, the number of bag colors that can eventually contain at least one shiny gold bag is 4.

    How many bag colors can eventually contain at least one shiny gold bag? (The list of rules is quite long; make sure you get all of it.)

    --- Part Two ---
    It's getting pretty expensive to fly these days - not because of ticket prices, but because of the ridiculous number of bags you need to buy!

    Consider again your shiny gold bag and the rules from the above example:

    faded blue bags contain 0 other bags.
    dotted black bags contain 0 other bags.
    vibrant plum bags contain 11 other bags: 5 faded blue bags and 6 dotted black bags.
    dark olive bags contain 7 other bags: 3 faded blue bags and 4 dotted black bags.
    So, a single shiny gold bag must contain 1 dark olive bag (and the 7 bags within it) plus 2 vibrant plum bags (and the 11 bags within each of those): 1 + 1*7 + 2 + 2*11 = 32 bags!

    Of course, the actual rules have a small chance of going several levels deeper than this example; be sure to count all of the bags, even if the nesting becomes topologically impractical!

    Here's another example:

    shiny gold bags contain 2 dark red bags.
    dark red bags contain 2 dark orange bags.
    dark orange bags contain 2 dark yellow bags.
    dark yellow bags contain 2 dark green bags.
    dark green bags contain 2 dark blue bags.
    dark blue bags contain 2 dark violet bags.
    dark violet bags contain no other bags.

    In this example, a single shiny gold bag must contain 126 other bags.

    How many individual bags are required inside your single shiny gold bag?
*/
    public class Day7 : PuzzleSolver
    {
        private List<string> bagsThatCanContainGoldBags = new List<string>();
        private string shinyGold = "shiny gold";
        public Day7(string inputString)
            : base(inputString)
        {
            Name = "Day Seven";
        }

        protected override void SolvePuzzleOne()
        {
            // Find bags that can directly contain gold bags
            var goldBags = this.input.Where(x => x.Contains(this.shinyGold)).Select(x => GetBagName(x)).Where(x => x != this.shinyGold).ToList();
            // Add them to list of all bags that can contain gold bags (directly or indirectly)
            this.bagsThatCanContainGoldBags.AddRange(goldBags);

            // Find rest of the bags
            FindAllBagsThatCanContainGoldBags(goldBags);

            // Count only distinct bags
            Console.WriteLine(this.bagsThatCanContainGoldBags.Distinct().Count());
        }

        private void FindAllBagsThatCanContainGoldBags(List<string> nextBagsToCheck)
        {
            var nextBagsThatCanContaingGoldBags = new List<string>();
            foreach (var bag in nextBagsToCheck)
            {
                // Find bags that can directly contain bags in parameter (so indirectly contain gold bags)
                var tempBags = this.input.Where(x => x.Contains(bag)).Select(x => GetBagName(x)).Where(x => x != bag);
                nextBagsThatCanContaingGoldBags.AddRange(tempBags);
            }

            if (nextBagsThatCanContaingGoldBags.Count == 0)
            {
                return;
            }
            else 
            {
                // If any bags found, add them to list of all bags, and start again
                this.bagsThatCanContainGoldBags.AddRange(nextBagsThatCanContaingGoldBags);
                FindAllBagsThatCanContainGoldBags(nextBagsThatCanContaingGoldBags);
            }
        }

        private string GetBagName(string rule)
        {
            // First two words of rule are bags name
            var temp = rule.Split(" ", 3);
            var bagName = temp[0] + " " + temp[1];
            return bagName;
        }

        protected override void SolvePuzzleTwo()
        {
            var bagsToCheck = new Dictionary<string, int>();

            // Find gold bag rule 
            var goldBagRule = this.input.Where(x => x.Substring(0, this.shinyGold.Length) == this.shinyGold).ToList().First();
            var goldBagList = TransformBagRule(goldBagRule);

            for (int i = 4; i < goldBagList.Count; i += 4)
            {
                // Find bags in gold bag rule
                var bagToCheck = goldBagList[i + 1] + " " + goldBagList[i + 2];
                bagsToCheck.Add(bagToCheck, Convert.ToInt16(goldBagList[i]));
            }

            // Find other rules and calculate bags
            int requiredBags = CalculateBags(bagsToCheck);
            Console.WriteLine(requiredBags);
        }

        private int CalculateBags(Dictionary<string, int> bagsToCheck)
        {
            int bagsRequired = 0;
            // i.e parent bag can contain 3 black 2 red bags
            foreach (var bag in bagsToCheck)
            {
                // Find next bags rule 
                // i.e find black rule
                var nextBagsToCheck = FindNextBagsToCheck(bag);
                if (nextBagsToCheck.Count != 0)
                {
                    // Multiply amount of bags in rule
                    // i.e 3 black * rules of black bag
                    bagsRequired += bag.Value * CalculateBags(nextBagsToCheck);
                }
            }

            foreach (var bag in bagsToCheck)
            {
                bagsRequired += bag.Value;
            }

            return bagsRequired;
        }

        private Dictionary<string, int> FindNextBagsToCheck(KeyValuePair<string, int> bag)
        {
            var nextBagsToCheck = new Dictionary<string, int>();

            // Find bag rule
            var bagRule = this.input.Where(x => x.Substring(0, bag.Key.Length) == bag.Key).ToList().First();
            var bagList = TransformBagRule(bagRule);

            for (int i = 4; i < bagList.Count; i += 4)
            {
                // If this is not a 1-5 integer, there are no more bags to check
                if (bagList[i].Length > 1)
                {
                    break;
                }

                var bagToCheck = bagList[i + 1] + " " + bagList[i + 2];
                nextBagsToCheck.Add(bagToCheck, Convert.ToInt16(bagList[i]));
            }

            return nextBagsToCheck;
        }

        private List<string> TransformBagRule(string rule)
        {
            // Transforms string with bag rule into list of strings with every word of rule
            var bags = rule.Split(" ").ToList();
            return bags;
        }
    }
}