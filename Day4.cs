using System;
using System.Collections.Generic;

namespace Advent_of_Code_2020
{
/*
--- Day 4: Passport Processing ---
    You arrive at the airport only to realize that you grabbed your North Pole Credentials instead of your passport. 
    While these documents are extremely similar, North Pole Credentials aren't issued by a country and therefore aren't actually valid documentation for travel in most of the world.

    It seems like you're not the only one having problems, though; a very long line has formed for the automatic passport scanners, and the delay could upset your travel itinerary.
    Due to some questionable network security, you realize you might be able to solve both of these problems at the same time.

    The automatic passport scanners are slow because they're having trouble detecting which passports have all required fields. The expected fields are as follows:
    byr (Birth Year)
    iyr (Issue Year)
    eyr (Expiration Year)
    hgt (Height)
    hcl (Hair Color)
    ecl (Eye Color)
    pid (Passport ID)
    cid (Country ID)
    Passport data is validated in batch files (your puzzle input). Each passport is represented as a sequence of key:value pairs separated by spaces or newlines. Passports are separated by blank lines.

    Here is an example batch file containing four passports:
    ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
    byr:1937 iyr:2017 cid:147 hgt:183cm

    iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
    hcl:#cfa07d byr:1929

    hcl:#ae17e1 iyr:2013
    eyr:2024
    ecl:brn pid:760753108 byr:1931
    hgt:179cm

    hcl:#cfa07d eyr:2025 pid:166559648
    iyr:2011 ecl:brn hgt:59in
    The first passport is valid - all eight fields are present. The second passport is invalid - it is missing hgt (the Height field).
    The third passport is interesting; the only missing field is cid, so it looks like data from North Pole Credentials, not a passport at all! 
    Surely, nobody would mind if you made the system temporarily ignore missing cid fields. Treat this "passport" as valid.
    The fourth passport is missing two fields, cid and byr. Missing cid is fine, but missing any other field is not, so this passport is invalid.
    According to the above rules, your improved system would report 2 valid passports.

    Count the number of valid passports - those that have all required fields. Treat cid as optional. In your batch file, how many passports are valid?

--- Part Two ---
    The line is moving more quickly now, but you overhear airport security talking about how passports with invalid data are getting through. Better add some data validation, quick!

    You can continue to ignore the cid field, but each other field has strict rules about what values are valid for automatic validation:
    byr (Birth Year) - four digits; at least 1920 and at most 2002.
    iyr (Issue Year) - four digits; at least 2010 and at most 2020.
    eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
    hgt (Height) - a number followed by either cm or in:
    If cm, the number must be at least 150 and at most 193.
    If in, the number must be at least 59 and at most 76.
    hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
    ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
    pid (Passport ID) - a nine-digit number, including leading zeroes.
    cid (Country ID) - ignored, missing or not.
    Your job is to count the passports where all required fields are both present and valid according to the above rules. Here are some example values:

    byr valid:   2002
    byr invalid: 2003

    hgt valid:   60in
    hgt valid:   190cm
    hgt invalid: 190in
    hgt invalid: 190

    hcl valid:   #123abc
    hcl invalid: #123abz
    hcl invalid: 123abc

    ecl valid:   brn
    ecl invalid: wat

    pid valid:   000000001
    pid invalid: 0123456789
    Here are some invalid passports:

    eyr:1972 cid:100
    hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

    iyr:2019
    hcl:#602927 eyr:1967 hgt:170cm
    ecl:grn pid:012533040 byr:1946

    hcl:dab227 iyr:2012
    ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

    hgt:59cm ecl:zzz
    eyr:2038 hcl:74454a iyr:2023
    pid:3556412378 byr:2007
    Here are some valid passports:

    pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
    hcl:#623a2f

    eyr:2029 ecl:blu cid:129 byr:1989
    iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

    hcl:#888785
    hgt:164cm byr:2001 iyr:2015 cid:88
    pid:545766238 ecl:hzl
    eyr:2022

    iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719
    Count the number of valid passports - those that have all required fields and valid values. Continue to treat cid as optional. In your batch file, how many passports are valid?
*/
    public class Day4 : PuzzleSolver
    {

        private string[] passportFields = new[] 
        {
            "byr",
            "iyr",
            "eyr",
            "hgt",
            "hcl",
            "ecl",
            "pid",
            "cid"
        };

        private Dictionary<string, bool> passportData = new Dictionary<string, bool>
        {
            { "byr", false },
            { "iyr", false },
            { "eyr", false },
            { "hgt", false },
            { "hcl", false },
            { "ecl", false },
            { "pid", false },
            { "cid", true }
        };

        public Day4(string inputString) 
            : base(inputString)
        {
            Name = "Day Four";
        }

        protected override void SolvePuzzleOne()
        {
            int validPassports = 0;
            foreach (var line in this.input)
            {
                // Take fields from current line
                ValidatePassportLine(line);
                // Blank line - separation of passports OR last line in file
                if (string.IsNullOrWhiteSpace(line) || this.input[this.input.Length - 1] == line)
                {
                    if (CheckIfPassportDataIsValid())
                    {
                        validPassports++;
                    }
                    ResetPassportData();
                }
            }
            Console.WriteLine(validPassports);
        }

        protected override void SolvePuzzleTwo()
        {
            int validPassports = 0;
            var isAnyFieldInvalid = false;
            foreach (var line in this.input)
            {
                if (!isAnyFieldInvalid)
                {
                    // Take fields from current line
                    ValidatePassportLineExtended(line, out isAnyFieldInvalid);
                }
                
                // If any field is invalid 
                if (isAnyFieldInvalid)
                {
                    // If it is, no reason to check other fields in passport
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        ResetPassportData();
                        isAnyFieldInvalid = false;
                    }
                    continue;
                }

                // Blank line - separation of passports OR last line in file
                if (string.IsNullOrWhiteSpace(line) || this.input[this.input.Length - 1] == line)
                {
                    if (CheckIfPassportDataIsValid())
                    {
                        validPassports++;
                    }
                    ResetPassportData();
                }
            }
            Console.WriteLine(validPassports);
        }

        private void ValidatePassportLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            int i = 0;
            bool takeField = true;
            while (i < line.Length)
            {
                if (takeField)
                {
                    // Take field from line
                    var data = line.Substring(0, 3);
                    // Set passport field as found
                    this.passportData[data] = true;

                    // Remove field from line (along with ':' sign)
                    line = line.Remove(0, 4);

                    // Set takeField to false untill next field is found
                    takeField = false;
                }

                // Next field is separated by whitespace
                if (char.IsWhiteSpace(line[i]))
                {
                    // Remove previous data from line
                    line = line.Remove(0, i + 1);
                    // Set index to 0
                    i = 0;
                    // Next field is found
                    takeField = true;
                    // Start over
                    continue;
                }
                i++;
            }
        }

        private void ValidatePassportLineExtended(string line, out bool isAnyFieldInvalid)
        {
            isAnyFieldInvalid = false;
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            // Added whitespace to end of line so last field will be also "separated by whitespace"
            line += " ";

            int i = 0;
            while (i < line.Length)
            {
                // Next field is separated by whitespace
                if (char.IsWhiteSpace(line[i]))
                {
                    var field = line.Substring(0, 3);
                    var value = line.Substring(4, i - 4);
                    isAnyFieldInvalid = !ValidateField(field, value);

                    if (isAnyFieldInvalid)
                    {
                        // If one field is invalid, no need to check others
                        return;
                    }
                    else 
                    {
                        this.passportData[field] = true;
                    }

                    // Remove previous data from line
                    line = line.Remove(0, i + 1);
                    // Set index to 0
                    i = 0;
                }
                i++;
            }
        }

        private bool ValidateField(string field, string value)
        {
            switch (field)
            {
                // byr (Birth Year) - four digits; at least 1920 and at most 2002.
                case "byr":
                {
                    var byr = Convert.ToInt16(value);
                    if (byr >= 1920 && byr <= 2002)
                    {
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                }
                // iyr (Issue Year) - four digits; at least 2010 and at most 2020.
                case "iyr":
                {
                    var iyr = Convert.ToInt16(value);
                    if (iyr >= 2010 && iyr <= 2020)
                    {
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                }
                // eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
                case "eyr":
                {
                    var eyr = Convert.ToInt16(value);
                    if (eyr >= 2020 && eyr <= 2030)
                    {
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                }
                // hgt (Height) - a number followed by either cm or in:
                // If cm, the number must be at least 150 and at most 193.
                // If in, the number must be at least 59 and at most 76.
                case "hgt":
                {
                    for (int i = 0; i < value.Length; ++i)
                    {
                        if (!char.IsDigit(value[i]))
                        {
                            // cm / in
                            var heightType = value.Substring(i, 2);
                            // Numeric value
                            var hgt = Convert.ToInt16(value.Substring(0, i));
                            if (heightType.Equals("cm"))
                            {
                                if (hgt >= 150 && hgt <= 193)
                                {
                                    return true;
                                }
                                else 
                                {
                                    return false;
                                }
                            }
                            else if (heightType.Equals("in"))
                            {
                                if (hgt >= 59 && hgt <= 76)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return false;
                }
                // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                case "hcl":
                {
                    if (!value[0].Equals('#'))
                    {
                        return false;
                    }

                    for (int i = 1; i < 7; ++i)
                    {
                        if (!char.IsDigit(value[i]) && value[i] < 'a' && value[i] > 'f')
                        {
                            return false;
                        }
                    }
                    return true;
                }
                // ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                case "ecl":
                {
                    if (value.Equals("amb") || value.Equals("blu") || value.Equals("brn") || value.Equals("gry") || value.Equals("grn") || value.Equals("hzl") || value.Equals("oth"))
                    {
                        return true;
                    }
                    return false;
                }
                // pid (Passport ID) - a nine-digit number, including leading zeroes.
                case "pid":
                {
                    if (value.Length != 9)
                    {
                        return false;
                    }
                    for (int i = 0; i < 9; ++i)
                    {
                        if (!char.IsDigit(value[i]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                // cid (Country ID) - ignored, missing or not.
                case "cid":
                    return true;
            }
            return false;
        }

        private void ResetPassportData()
        {
            foreach (var field in this.passportFields)
            {
                // Passport without cid field is valid
                if (field == "cid")
                {
                    continue;
                }
                this.passportData[field] = false;
            }
        }

        private bool CheckIfPassportDataIsValid()
        {
            foreach (var field in this.passportData)
            {
                if (!field.Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}