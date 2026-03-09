using Shared.Entities;

namespace Shared.Data;

public static partial class DbSeeder
{
    private static void SeedChallenges(ApplicationDbContext context)
    {
        // Easy Challenges (10)
        var easyChallenges = new[]
        {
            // Easy 1: Hello World
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Hello World",
                Description = "Write a program that prints 'Hello, World!' to the console.",
                Difficulty = Difficulty.Easy,
                StarterCode = @"using System;

class Program
{
    static void Main()
    {
        // Write your code here
    }
}"
            },
            
            // Easy 2: String Reverse
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Reverse a String",
                Description = "Write a method that takes a string and returns it reversed.",
                Difficulty = Difficulty.Easy,
                StarterCode = @"using System;

class Program
{
    static string ReverseString(string input)
    {
        // Write your code here
        return """";
    }
    
    static void Main()
    {
        Console.WriteLine(ReverseString(Console.ReadLine()));
    }
}"
            },
            
            // Easy 3: Sum of Array
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Sum of Array",
                Description = "Calculate the sum of all elements in an integer array.",
                Difficulty = Difficulty.Easy,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static int SumArray(int[] numbers)
    {
        // Write your code here
        return 0;
    }
    
    static void Main()
    {
        var input = Console.ReadLine().Split(',').Select(int.Parse).ToArray();
        Console.WriteLine(SumArray(input));
    }
}"
            },
            
            // Easy 4: Find Maximum
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Find Maximum Number",
                Description = "Find and return the maximum number in an array of integers.",
                Difficulty = Difficulty.Easy,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static int FindMax(int[] numbers)
    {
        // Write your code here
        return 0;
    }
    
    static void Main()
    {
        var input = Console.ReadLine().Split(',').Select(int.Parse).ToArray();
        Console.WriteLine(FindMax(input));
    }
}"
            },
            
            // Easy 5: Count Vowels
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Count Vowels",
                Description = "Count the number of vowels (a, e, i, o, u) in a given string (case-insensitive).",
                Difficulty = Difficulty.Easy,
                StarterCode = @"using System;

class Program
{
    static int CountVowels(string text)
    {
        // Write your code here
        return 0;
    }
    
    static void Main()
    {
        Console.WriteLine(CountVowels(Console.ReadLine()));
    }
}"
            },
            
            // Easy 6: Is Palindrome
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Check Palindrome",
                Description = "Determine if a given string is a palindrome (reads the same forwards and backwards).",
                Difficulty = Difficulty.Easy,
                StarterCode = @"using System;

class Program
{
    static bool IsPalindrome(string text)
    {
        // Write your code here
        return false;
    }
    
    static void Main()
    {
        Console.WriteLine(IsPalindrome(Console.ReadLine()));
    }
}"
            },
            
            // Easy 7: Factorial
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Calculate Factorial",
                Description = "Calculate the factorial of a non-negative integer n (n!).",
                Difficulty = Difficulty.Easy,
                StarterCode = @"using System;

class Program
{
    static long Factorial(int n)
    {
        // Write your code here
        return 0;
    }
    
    static void Main()
    {
        Console.WriteLine(Factorial(int.Parse(Console.ReadLine())));
    }
}"
            },
            
            // Easy 8: FizzBuzz
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "FizzBuzz",
                Description = "Print numbers from 1 to n. For multiples of 3 print 'Fizz', for multiples of 5 print 'Buzz', for multiples of both print 'FizzBuzz'.",
                Difficulty = Difficulty.Easy,
                StarterCode = @"using System;

class Program
{
    static void FizzBuzz(int n)
    {
        // Write your code here
    }
    
    static void Main()
    {
        FizzBuzz(int.Parse(Console.ReadLine()));
    }
}"
            },
            
            // Easy 9: Remove Duplicates
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Remove Duplicates from Array",
                Description = "Remove duplicate elements from an integer array and return the unique elements in order.",
                Difficulty = Difficulty.Easy,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static int[] RemoveDuplicates(int[] numbers)
    {
        // Write your code here
        return new int[0];
    }
    
    static void Main()
    {
        var input = Console.ReadLine().Split(',').Select(int.Parse).ToArray();
        Console.WriteLine(string.Join("","", RemoveDuplicates(input)));
    }
}"
            },
            
            // Easy 10: String to Uppercase
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Convert to Uppercase",
                Description = "Convert all characters in a string to uppercase without using built-in ToUpper() method.",
                Difficulty = Difficulty.Easy,
                StarterCode = @"using System;

class Program
{
    static string ToUpperCase(string text)
    {
        // Write your code here
        return """";
    }
    
    static void Main()
    {
        Console.WriteLine(ToUpperCase(Console.ReadLine()));
    }
}"
            }
        };

        
        // Medium Challenges (10)
        var mediumChallenges = new[]
        {
            // Medium 1: Binary Search
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Binary Search",
                Description = "Implement binary search algorithm to find the index of a target value in a sorted array. Return -1 if not found.",
                Difficulty = Difficulty.Medium,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static int BinarySearch(int[] sortedArray, int target)
    {
        // Write your code here
        return -1;
    }
    
    static void Main()
    {
        var parts = Console.ReadLine().Split('|');
        var array = parts[0].Split(',').Select(int.Parse).ToArray();
        var target = int.Parse(parts[1]);
        Console.WriteLine(BinarySearch(array, target));
    }
}"
            },
            
            // Medium 2: Two Sum
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Two Sum",
                Description = "Find two numbers in an array that add up to a target sum. Return their indices as 'index1,index2'.",
                Difficulty = Difficulty.Medium,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static string TwoSum(int[] numbers, int target)
    {
        // Write your code here
        return """";
    }
    
    static void Main()
    {
        var parts = Console.ReadLine().Split('|');
        var numbers = parts[0].Split(',').Select(int.Parse).ToArray();
        var target = int.Parse(parts[1]);
        Console.WriteLine(TwoSum(numbers, target));
    }
}"
            },
            
            // Medium 3: Valid Parentheses
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Valid Parentheses",
                Description = "Determine if a string containing only '(', ')', '{', '}', '[', ']' has valid matching pairs and order.",
                Difficulty = Difficulty.Medium,
                StarterCode = @"using System;

class Program
{
    static bool IsValidParentheses(string s)
    {
        // Write your code here
        return false;
    }
    
    static void Main()
    {
        Console.WriteLine(IsValidParentheses(Console.ReadLine()));
    }
}"
            },
            
            // Medium 4: LINQ Group By
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Group and Count with LINQ",
                Description = "Given a list of words, group them by their first letter and count how many words start with each letter. Output as 'Letter:Count' per line.",
                Difficulty = Difficulty.Medium,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static void GroupAndCount(string[] words)
    {
        // Write your code here using LINQ
    }
    
    static void Main()
    {
        var words = Console.ReadLine().Split(',');
        GroupAndCount(words);
    }
}"
            },
            
            // Medium 5: Merge Sorted Arrays
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Merge Two Sorted Arrays",
                Description = "Merge two sorted arrays into one sorted array.",
                Difficulty = Difficulty.Medium,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static int[] MergeSortedArrays(int[] arr1, int[] arr2)
    {
        // Write your code here
        return new int[0];
    }
    
    static void Main()
    {
        var parts = Console.ReadLine().Split('|');
        var arr1 = parts[0].Split(',').Select(int.Parse).ToArray();
        var arr2 = parts[1].Split(',').Select(int.Parse).ToArray();
        Console.WriteLine(string.Join("","", MergeSortedArrays(arr1, arr2)));
    }
}"
            },
            
            // Medium 6: Anagram Check
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Check Anagrams",
                Description = "Determine if two strings are anagrams (contain the same characters in different order).",
                Difficulty = Difficulty.Medium,
                StarterCode = @"using System;

class Program
{
    static bool AreAnagrams(string str1, string str2)
    {
        // Write your code here
        return false;
    }
    
    static void Main()
    {
        var parts = Console.ReadLine().Split('|');
        Console.WriteLine(AreAnagrams(parts[0], parts[1]));
    }
}"
            },
            
            // Medium 7: Longest Common Prefix
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Longest Common Prefix",
                Description = "Find the longest common prefix string amongst an array of strings. Return empty string if there is no common prefix.",
                Difficulty = Difficulty.Medium,
                StarterCode = @"using System;

class Program
{
    static string LongestCommonPrefix(string[] strings)
    {
        // Write your code here
        return """";
    }
    
    static void Main()
    {
        var strings = Console.ReadLine().Split(',');
        Console.WriteLine(LongestCommonPrefix(strings));
    }
}"
            },
            
            // Medium 8: Rotate Array
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Rotate Array",
                Description = "Rotate an array to the right by k steps. For example, [1,2,3,4,5] rotated by 2 becomes [4,5,1,2,3].",
                Difficulty = Difficulty.Medium,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static int[] RotateArray(int[] numbers, int k)
    {
        // Write your code here
        return new int[0];
    }
    
    static void Main()
    {
        var parts = Console.ReadLine().Split('|');
        var numbers = parts[0].Split(',').Select(int.Parse).ToArray();
        var k = int.Parse(parts[1]);
        Console.WriteLine(string.Join("","", RotateArray(numbers, k)));
    }
}"
            },
            
            // Medium 9: Find Missing Number
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Find Missing Number",
                Description = "Given an array containing n distinct numbers from 0 to n, find the one number that is missing.",
                Difficulty = Difficulty.Medium,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static int FindMissingNumber(int[] numbers)
    {
        // Write your code here
        return 0;
    }
    
    static void Main()
    {
        var numbers = Console.ReadLine().Split(',').Select(int.Parse).ToArray();
        Console.WriteLine(FindMissingNumber(numbers));
    }
}"
            },
            
            // Medium 10: Product Except Self
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Product of Array Except Self",
                Description = "Return an array where each element is the product of all elements in the input array except itself. Do not use division.",
                Difficulty = Difficulty.Medium,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static int[] ProductExceptSelf(int[] numbers)
    {
        // Write your code here
        return new int[0];
    }
    
    static void Main()
    {
        var numbers = Console.ReadLine().Split(',').Select(int.Parse).ToArray();
        Console.WriteLine(string.Join("","", ProductExceptSelf(numbers)));
    }
}"
            }
        };

        
        // Hard Challenges (10)
        var hardChallenges = new[]
        {
            // Hard 1: Longest Substring Without Repeating
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Longest Substring Without Repeating Characters",
                Description = "Find the length of the longest substring without repeating characters.",
                Difficulty = Difficulty.Hard,
                StarterCode = @"using System;

class Program
{
    static int LengthOfLongestSubstring(string s)
    {
        // Write your code here
        return 0;
    }
    
    static void Main()
    {
        Console.WriteLine(LengthOfLongestSubstring(Console.ReadLine()));
    }
}"
            },
            
            // Hard 2: Median of Two Sorted Arrays
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Median of Two Sorted Arrays",
                Description = "Find the median of two sorted arrays. The overall run time complexity should be O(log(m+n)).",
                Difficulty = Difficulty.Hard,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static double FindMedianSortedArrays(int[] nums1, int[] nums2)
    {
        // Write your code here
        return 0.0;
    }
    
    static void Main()
    {
        var parts = Console.ReadLine().Split('|');
        var nums1 = parts[0].Split(',').Select(int.Parse).ToArray();
        var nums2 = parts[1].Split(',').Select(int.Parse).ToArray();
        Console.WriteLine(FindMedianSortedArrays(nums1, nums2));
    }
}"
            },
            
            // Hard 3: Implement LRU Cache
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "LRU Cache Implementation",
                Description = "Design and implement a Least Recently Used (LRU) cache with Get and Put operations in O(1) time complexity.",
                Difficulty = Difficulty.Hard,
                StarterCode = @"using System;
using System.Collections.Generic;

class LRUCache
{
    public LRUCache(int capacity)
    {
        // Initialize your data structure here
    }
    
    public int Get(int key)
    {
        // Write your code here
        return -1;
    }
    
    public void Put(int key, int value)
    {
        // Write your code here
    }
}

class Program
{
    static void Main()
    {
        var cache = new LRUCache(2);
        var operations = Console.ReadLine().Split(';');
        foreach (var op in operations)
        {
            var parts = op.Split(',');
            if (parts[0] == ""PUT"")
                cache.Put(int.Parse(parts[1]), int.Parse(parts[2]));
            else
                Console.WriteLine(cache.Get(int.Parse(parts[1])));
        }
    }
}"
            },
            
            // Hard 4: Regular Expression Matching
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Regular Expression Matching",
                Description = "Implement regular expression matching with support for '.' (matches any single character) and '*' (matches zero or more of the preceding element).",
                Difficulty = Difficulty.Hard,
                StarterCode = @"using System;

class Program
{
    static bool IsMatch(string text, string pattern)
    {
        // Write your code here
        return false;
    }
    
    static void Main()
    {
        var parts = Console.ReadLine().Split('|');
        Console.WriteLine(IsMatch(parts[0], parts[1]));
    }
}"
            },
            
            // Hard 5: N-Queens Problem
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "N-Queens Problem",
                Description = "Place N queens on an NxN chessboard so that no two queens attack each other. Return the number of distinct solutions.",
                Difficulty = Difficulty.Hard,
                StarterCode = @"using System;

class Program
{
    static int SolveNQueens(int n)
    {
        // Write your code here
        return 0;
    }
    
    static void Main()
    {
        Console.WriteLine(SolveNQueens(int.Parse(Console.ReadLine())));
    }
}"
            },
            
            // Hard 6: Word Ladder
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Word Ladder",
                Description = "Find the shortest transformation sequence from beginWord to endWord, changing only one letter at a time. Each transformed word must exist in the word list.",
                Difficulty = Difficulty.Hard,
                StarterCode = @"using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static int LadderLength(string beginWord, string endWord, string[] wordList)
    {
        // Write your code here
        return 0;
    }
    
    static void Main()
    {
        var parts = Console.ReadLine().Split('|');
        var beginWord = parts[0];
        var endWord = parts[1];
        var wordList = parts[2].Split(',');
        Console.WriteLine(LadderLength(beginWord, endWord, wordList));
    }
}"
            },
            
            // Hard 7: Serialize and Deserialize Binary Tree
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Serialize and Deserialize Binary Tree",
                Description = "Design an algorithm to serialize and deserialize a binary tree. Serialization converts a tree to a string, deserialization converts it back.",
                Difficulty = Difficulty.Hard,
                StarterCode = @"using System;
using System.Collections.Generic;

class TreeNode
{
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int x) { val = x; }
}

class Codec
{
    public string Serialize(TreeNode root)
    {
        // Write your code here
        return """";
    }
    
    public TreeNode Deserialize(string data)
    {
        // Write your code here
        return null;
    }
}

class Program
{
    static void Main()
    {
        var codec = new Codec();
        var serialized = Console.ReadLine();
        var tree = codec.Deserialize(serialized);
        Console.WriteLine(codec.Serialize(tree));
    }
}"
            },
            
            // Hard 8: Trapping Rain Water
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Trapping Rain Water",
                Description = "Given n non-negative integers representing an elevation map where the width of each bar is 1, compute how much water it can trap after raining.",
                Difficulty = Difficulty.Hard,
                StarterCode = @"using System;
using System.Linq;

class Program
{
    static int Trap(int[] height)
    {
        // Write your code here
        return 0;
    }
    
    static void Main()
    {
        var height = Console.ReadLine().Split(',').Select(int.Parse).ToArray();
        Console.WriteLine(Trap(height));
    }
}"
            },
            
            // Hard 9: Wildcard Matching
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Wildcard Pattern Matching",
                Description = "Implement wildcard pattern matching with support for '?' (matches any single character) and '*' (matches any sequence of characters including empty).",
                Difficulty = Difficulty.Hard,
                StarterCode = @"using System;

class Program
{
    static bool IsMatch(string text, string pattern)
    {
        // Write your code here
        return false;
    }
    
    static void Main()
    {
        var parts = Console.ReadLine().Split('|');
        Console.WriteLine(IsMatch(parts[0], parts[1]));
    }
}"
            },
            
            // Hard 10: Minimum Window Substring
            new Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Minimum Window Substring",
                Description = "Find the minimum window substring in string s that contains all characters from string t. Return empty string if no such window exists.",
                Difficulty = Difficulty.Hard,
                StarterCode = @"using System;

class Program
{
    static string MinWindow(string s, string t)
    {
        // Write your code here
        return """";
    }
    
    static void Main()
    {
        var parts = Console.ReadLine().Split('|');
        Console.WriteLine(MinWindow(parts[0], parts[1]));
    }
}"
            }
        };

        
        // Add all challenges to context
        var allChallenges = easyChallenges.Concat(mediumChallenges).Concat(hardChallenges).ToList();
        context.Challenges.AddRange(allChallenges);
        context.SaveChanges();
        
        // Create test cases for each challenge
        SeedTestCases(context, easyChallenges, mediumChallenges, hardChallenges);
    }

    
    private static void SeedTestCases(ApplicationDbContext context, Challenge[] easy, Challenge[] medium, Challenge[] hard)
    {
        var testCases = new List<TestCase>();
        
        // Easy Challenge Test Cases
        
        // Easy 1: Hello World
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = easy[0].Id, Input = "", ExpectedOutput = "Hello, World!", OrderIndex = 1, IsHidden = false }
        });
        
        // Easy 2: Reverse String
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = easy[1].Id, Input = "hello", ExpectedOutput = "olleh", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = easy[1].Id, Input = "world", ExpectedOutput = "dlrow", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = easy[1].Id, Input = "a", ExpectedOutput = "a", OrderIndex = 3, IsHidden = true }
        });
        
        // Easy 3: Sum of Array
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = easy[2].Id, Input = "1,2,3,4,5", ExpectedOutput = "15", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = easy[2].Id, Input = "10,20,30", ExpectedOutput = "60", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = easy[2].Id, Input = "0", ExpectedOutput = "0", OrderIndex = 3, IsHidden = true }
        });
        
        // Easy 4: Find Maximum
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = easy[3].Id, Input = "1,5,3,9,2", ExpectedOutput = "9", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = easy[3].Id, Input = "-5,-1,-10", ExpectedOutput = "-1", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = easy[3].Id, Input = "42", ExpectedOutput = "42", OrderIndex = 3, IsHidden = true }
        });
        
        // Easy 5: Count Vowels
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = easy[4].Id, Input = "hello", ExpectedOutput = "2", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = easy[4].Id, Input = "AEIOU", ExpectedOutput = "5", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = easy[4].Id, Input = "xyz", ExpectedOutput = "0", OrderIndex = 3, IsHidden = true }
        });
        
        // Easy 6: Is Palindrome
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = easy[5].Id, Input = "racecar", ExpectedOutput = "True", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = easy[5].Id, Input = "hello", ExpectedOutput = "False", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = easy[5].Id, Input = "a", ExpectedOutput = "True", OrderIndex = 3, IsHidden = true }
        });
        
        // Easy 7: Factorial
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = easy[6].Id, Input = "5", ExpectedOutput = "120", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = easy[6].Id, Input = "0", ExpectedOutput = "1", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = easy[6].Id, Input = "10", ExpectedOutput = "3628800", OrderIndex = 3, IsHidden = true }
        });
        
        // Easy 8: FizzBuzz
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = easy[7].Id, Input = "15", ExpectedOutput = "1\n2\nFizz\n4\nBuzz\nFizz\n7\n8\nFizz\nBuzz\n11\nFizz\n13\n14\nFizzBuzz", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = easy[7].Id, Input = "5", ExpectedOutput = "1\n2\nFizz\n4\nBuzz", OrderIndex = 2, IsHidden = false }
        });
        
        // Easy 9: Remove Duplicates
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = easy[8].Id, Input = "1,2,2,3,4,4,5", ExpectedOutput = "1,2,3,4,5", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = easy[8].Id, Input = "1,1,1,1", ExpectedOutput = "1", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = easy[8].Id, Input = "1,2,3", ExpectedOutput = "1,2,3", OrderIndex = 3, IsHidden = true }
        });
        
        // Easy 10: String to Uppercase
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = easy[9].Id, Input = "hello", ExpectedOutput = "HELLO", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = easy[9].Id, Input = "World123", ExpectedOutput = "WORLD123", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = easy[9].Id, Input = "ABC", ExpectedOutput = "ABC", OrderIndex = 3, IsHidden = true }
        });
        
        // Medium Challenge Test Cases
        
        // Medium 1: Binary Search
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = medium[0].Id, Input = "1,3,5,7,9|5", ExpectedOutput = "2", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = medium[0].Id, Input = "1,3,5,7,9|6", ExpectedOutput = "-1", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = medium[0].Id, Input = "2,4,6,8,10,12|12", ExpectedOutput = "5", OrderIndex = 3, IsHidden = true }
        });
        
        // Medium 2: Two Sum
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = medium[1].Id, Input = "2,7,11,15|9", ExpectedOutput = "0,1", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = medium[1].Id, Input = "3,2,4|6", ExpectedOutput = "1,2", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = medium[1].Id, Input = "3,3|6", ExpectedOutput = "0,1", OrderIndex = 3, IsHidden = true }
        });
        
        // Medium 3: Valid Parentheses
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = medium[2].Id, Input = "()", ExpectedOutput = "True", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = medium[2].Id, Input = "()[]{}", ExpectedOutput = "True", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = medium[2].Id, Input = "(]", ExpectedOutput = "False", OrderIndex = 3, IsHidden = false },
            new TestCase { ChallengeId = medium[2].Id, Input = "([)]", ExpectedOutput = "False", OrderIndex = 4, IsHidden = true }
        });
        
        // Medium 4: LINQ Group By
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = medium[3].Id, Input = "apple,apricot,banana,blueberry,cherry", ExpectedOutput = "a:2\nb:2\nc:1", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = medium[3].Id, Input = "dog,cat,duck,cow", ExpectedOutput = "d:2\nc:2", OrderIndex = 2, IsHidden = false }
        });
        
        // Medium 5: Merge Sorted Arrays
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = medium[4].Id, Input = "1,3,5|2,4,6", ExpectedOutput = "1,2,3,4,5,6", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = medium[4].Id, Input = "1,2,3|4,5,6", ExpectedOutput = "1,2,3,4,5,6", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = medium[4].Id, Input = "1|2", ExpectedOutput = "1,2", OrderIndex = 3, IsHidden = true }
        });
        
        // Medium 6: Anagram Check
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = medium[5].Id, Input = "listen|silent", ExpectedOutput = "True", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = medium[5].Id, Input = "hello|world", ExpectedOutput = "False", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = medium[5].Id, Input = "anagram|nagaram", ExpectedOutput = "True", OrderIndex = 3, IsHidden = true }
        });
        
        // Medium 7: Longest Common Prefix
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = medium[6].Id, Input = "flower,flow,flight", ExpectedOutput = "fl", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = medium[6].Id, Input = "dog,racecar,car", ExpectedOutput = "", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = medium[6].Id, Input = "test,test,test", ExpectedOutput = "test", OrderIndex = 3, IsHidden = true }
        });
        
        // Medium 8: Rotate Array
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = medium[7].Id, Input = "1,2,3,4,5|2", ExpectedOutput = "4,5,1,2,3", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = medium[7].Id, Input = "1,2,3|1", ExpectedOutput = "3,1,2", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = medium[7].Id, Input = "1,2|3", ExpectedOutput = "2,1", OrderIndex = 3, IsHidden = true }
        });
        
        // Medium 9: Find Missing Number
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = medium[8].Id, Input = "0,1,3", ExpectedOutput = "2", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = medium[8].Id, Input = "0,1,2,3,4,6", ExpectedOutput = "5", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = medium[8].Id, Input = "1", ExpectedOutput = "0", OrderIndex = 3, IsHidden = true }
        });
        
        // Medium 10: Product Except Self
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = medium[9].Id, Input = "1,2,3,4", ExpectedOutput = "24,12,8,6", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = medium[9].Id, Input = "2,3,4,5", ExpectedOutput = "60,40,30,24", OrderIndex = 2, IsHidden = false }
        });
        
        // Hard Challenge Test Cases
        
        // Hard 1: Longest Substring Without Repeating
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = hard[0].Id, Input = "abcabcbb", ExpectedOutput = "3", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = hard[0].Id, Input = "bbbbb", ExpectedOutput = "1", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = hard[0].Id, Input = "pwwkew", ExpectedOutput = "3", OrderIndex = 3, IsHidden = true }
        });
        
        // Hard 2: Median of Two Sorted Arrays
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = hard[1].Id, Input = "1,3|2", ExpectedOutput = "2", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = hard[1].Id, Input = "1,2|3,4", ExpectedOutput = "2.5", OrderIndex = 2, IsHidden = false }
        });
        
        // Hard 3: LRU Cache
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = hard[2].Id, Input = "PUT,1,1;PUT,2,2;GET,1;PUT,3,3;GET,2", ExpectedOutput = "1\n-1", OrderIndex = 1, IsHidden = false }
        });
        
        // Hard 4: Regular Expression Matching
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = hard[3].Id, Input = "aa|a", ExpectedOutput = "False", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = hard[3].Id, Input = "aa|a*", ExpectedOutput = "True", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = hard[3].Id, Input = "ab|.*", ExpectedOutput = "True", OrderIndex = 3, IsHidden = true }
        });
        
        // Hard 5: N-Queens
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = hard[4].Id, Input = "4", ExpectedOutput = "2", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = hard[4].Id, Input = "8", ExpectedOutput = "92", OrderIndex = 2, IsHidden = true }
        });
        
        // Hard 6: Word Ladder
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = hard[5].Id, Input = "hit|cog|hot,dot,dog,lot,log,cog", ExpectedOutput = "5", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = hard[5].Id, Input = "hit|cog|hot,dot,dog,lot,log", ExpectedOutput = "0", OrderIndex = 2, IsHidden = false }
        });
        
        // Hard 7: Serialize Binary Tree
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = hard[6].Id, Input = "1,2,3,null,null,4,5", ExpectedOutput = "1,2,3,null,null,4,5", OrderIndex = 1, IsHidden = false }
        });
        
        // Hard 8: Trapping Rain Water
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = hard[7].Id, Input = "0,1,0,2,1,0,1,3,2,1,2,1", ExpectedOutput = "6", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = hard[7].Id, Input = "4,2,0,3,2,5", ExpectedOutput = "9", OrderIndex = 2, IsHidden = false }
        });
        
        // Hard 9: Wildcard Matching
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = hard[8].Id, Input = "aa|a", ExpectedOutput = "False", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = hard[8].Id, Input = "aa|*", ExpectedOutput = "True", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = hard[8].Id, Input = "cb|?a", ExpectedOutput = "False", OrderIndex = 3, IsHidden = false }
        });
        
        // Hard 10: Minimum Window Substring
        testCases.AddRange(new[]
        {
            new TestCase { ChallengeId = hard[9].Id, Input = "ADOBECODEBANC|ABC", ExpectedOutput = "BANC", OrderIndex = 1, IsHidden = false },
            new TestCase { ChallengeId = hard[9].Id, Input = "a|a", ExpectedOutput = "a", OrderIndex = 2, IsHidden = false },
            new TestCase { ChallengeId = hard[9].Id, Input = "a|aa", ExpectedOutput = "", OrderIndex = 3, IsHidden = true }
        });
        
        // Add all test cases to context
        context.TestCases.AddRange(testCases);
        context.SaveChanges();
    }
}
