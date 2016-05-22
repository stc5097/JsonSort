using System;
using System.Collections.Generic;

namespace JsonSort.Models
{
    public class Customer 
    {
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
    }
    public class CustomerList
    {
        public IEnumerable<Customer> customers { get; set; }
    }

    public class CustomerSortOrder
    {
        public IEnumerable<char> sort_order { get; set; }
    }

    public class CustomerJoinSort
    {
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public int sortOrderLast { get; set; }
        public int sortOrderFirst { get; set; }
        public int sortOrderMiddle { get; set; }
    }

    public class CustomerJoinSortList
    {
        public IEnumerable<CustomerJoinSort> CustomerListWithSort { get; set; }
    }

    public class SemiNumericComparer : IComparer<String>
    {
        public int Compare(string x, string y)
        {
            String[] valueA = x.Split('/');
            String[] valueB = y.Split('/');

            if (valueA.Length <= valueB.Length)
            {
                int count = 0;
                foreach (String letterNumVal in valueA)
                {
                    if (count < (valueA.Length - 1))
                    {
                        //A should be sorted before B
                        if (int.Parse(valueA[count]) < int.Parse(valueB[count]))
                        {
                            return -1;
                        }
                        //A should be sorted after B
                        if (int.Parse(valueA[count]) > int.Parse(valueB[count]))
                        {
                            return 1;
                        }
                        //A and B are equal so look at next value
                        if (int.Parse(valueA[count]) == int.Parse(valueB[count]))
                        {
                            count++;
                        }
                    }
                }
            }
            if (valueA.Length > valueB.Length)
            {
                int count = 0;
                foreach (String letterNumVal in valueB)
                {
                    if (count < (valueB.Length - 1))
                    {
                        //B should be sorted before A
                        if (int.Parse(valueB[count]) < int.Parse(valueA[count]))
                        {
                            return 1;
                        }
                        //B Should be sorted after A
                        if (int.Parse(valueB[count]) > int.Parse(valueA[count]))
                        {
                            return -1;
                        }
                        //B and A are equal so look at next value
                        if (int.Parse(valueB[count]) == int.Parse(valueA[count]))
                        {
                            count++;
                        }
                    }
                }
            }
            return 0;

        }
    }
}