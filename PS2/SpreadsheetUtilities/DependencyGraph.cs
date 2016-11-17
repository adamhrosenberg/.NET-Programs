// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// The meat was added to the skeleton by Adam Rosenberg
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Adam Rosenberg
//CS 3500
//PS2
//u0751643
//9/17/2015
namespace SpreadsheetUtilities
{
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// s1 depends on t1 --> t1 must be evaluated before s1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// (Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.)
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Creates an empty DependencyGraph and int size.
        /// </summary>
        private Dictionary<String, HashSet<String>> dictionary;
        private int size;
        /// <summary>
        /// Dependency Graph is our object. Initial size is set to 0 and the dictionary is set up
        /// to have a string, and a HashSet(string) as its paramters.
        /// </summary>
        public DependencyGraph()
        {
            dictionary = new Dictionary<string, HashSet<string>>();
            size = 0;
        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// Its private so we have the get.
        /// Must make sure if we remove a dependency and there's no more values in the key, to also remove
        /// the key to keep the size correct (orphan case)
        /// </summary>
        public int Size
        {
            get { return size; } //use get to access private variable
        }

        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get { return GetDependees(s).Count(); }//returns size of dependees
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            try
            {
                return dictionary[s].Count > 0;
            }
            catch (KeyNotFoundException)//no other way to check count without this error. Catches it if there are no dependents for s
            {
                return false;
            }
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            return (GetDependees(s).Count() > 0); //get count from dependees list and if the lenght of that list is > 0 that it has dependees.
        }


        /// <summary>
        /// Enumerates dependents(s). Return as a new list so outside world cannot
        /// access private data within this class. 
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            HashSet<string> dentList = new HashSet<string>();//create new hashset
            if (dictionary.ContainsKey(s))//see if it contains the key S
            {
                foreach (string dent in dictionary[s])
                {
                    dentList.Add(dent);//build the list
                }
                return dentList;
            }
            HashSet<string> dentsEmpty = new HashSet<string>();
            return dentsEmpty;//return empty list to pass the test.
        }

        /// <summary>
        /// Enumerates dependees(s). Return new hashset so outside cannot access
        /// private data within my class.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            HashSet<string> dees = new HashSet<string>();
            foreach (KeyValuePair<string, HashSet<string>> pairs in dictionary){
                if (pairs.Value.Contains(s))
                {
                    dees.Add(pairs.Key);//add key to list 
                }
            }     
                return dees;
        }

        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   s depends on t
        ///
        /// </summary>
        /// <param name="s"> s cannot be evaluated until t is</param>
        /// <param name="t"> t must be evaluated first.  S depends on T</param>
        public void AddDependency(string s, string t)
        {
            if (s.Length > 0 && t.Length >0)
            {
                if (dictionary.ContainsKey(s))//make sure key s is in the dictionary
                {
                    if (!dictionary[s].Contains(t))//if the value t is not already in the key s than add it. Makes sure you dont add duplicates.
                    {
                        dictionary[s].Add(t);
                        size++;
                    }
                }
                else if (!dictionary.ContainsKey(s)) //if s is not a dependee at all. (first time add)
                {
                    dictionary.Add(s, new HashSet<string>());//new hashset for new key
                    dictionary[s].Add(t);
                    size++;
                }
            }
        }//end add                 
            
        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (dictionary.ContainsKey(s) && dictionary[s].Contains(t))//if dictionary has key s, with value t. remove ordered pair.
            {
                dictionary[s].Remove(t);
                if (dictionary[s].Count == 0)//if there are no more values within the key, the get rid of the key.
                {
                    dictionary.Remove(s);
                }
                size--;
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            foreach (string r in GetDependents(s))
                RemoveDependency(s, r);
            foreach (string t in newDependents)
                AddDependency(s, t);
        }
        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            foreach (KeyValuePair<string, HashSet<string>> pairs in dictionary)
            {
                if (pairs.Value.Contains(s))//if the pair has the string
                {
                    pairs.Value.Remove(s);//remove
                    size--;
                }
            }
            foreach (string newDee in newDependees)//add the new dependees. also works if the original key wasn't there. so it acts as an adding function still
            {
                AddDependency(newDee,s);
            }
        }//end replacedependees
    }
}
