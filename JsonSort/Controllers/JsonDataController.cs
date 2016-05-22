using JsonSort.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace JsonSort.Controllers
{

    public class JsonDataController : Controller
    {
        private string JsonDataURL = "https://efptest.sungardk12demo.com/data/data.json";
        private string JsonSortURL = "https://efptest.sungardk12demo.com/data/sort.json";
        private Models.CustomerJoinSort CustomerInfoAll = new Models.CustomerJoinSort();

        public List<String> ConvertToNumber(Dictionary<string,int> orderOverload, Models.CustomerList CustomerList, String PartOfName)
        {
            if (PartOfName == "first_name")
            {
                //Convert first name too number values assigned to sort order data
                //Dictionary to store cust info as a number (key) and cust info as chars (value)
                var firstNameAsNum = new List<string>();
                foreach (Models.Customer cust in CustomerList.customers)
                {
                    //stores customer first name as number with "/" serperating each value
                    String infoToNum = ""; 
                    char[] firstName;
                    //store customer's first name in char array
                    firstName = cust.first_name.ToCharArray(); 
                    /*Create string of numbers seperated by "/"
                      Look up char value in sort order and get numeric val for char
                      Add string of nums to list*/
                    foreach (char letter in firstName)  
                    {
                        if (orderOverload.ContainsKey(letter.ToString()))
                        {
                            infoToNum += orderOverload[letter.ToString()].ToString() + "/";
                        }
                        else
                        {
                            infoToNum = 0.ToString();
                        }
                    }

                    if (!firstNameAsNum.Contains(infoToNum))
                    {
                        firstNameAsNum.Add(infoToNum);
                    }
                }
                return firstNameAsNum;
            }
            if (PartOfName == "last_name")
            {
                //Convert last name too number values assigned to sort order data
                //Dictionary to store cust info as a number (key) and cust info as chars (value)
                var lastNameAsNum = new List<string>();
                foreach (Models.Customer cust in CustomerList.customers)
                {
                    String infoToNum = "";
                    char[] lastName;
                    lastName = cust.last_name.ToCharArray();
                    foreach (char letter in lastName)
                    {
                        if (orderOverload.ContainsKey(letter.ToString()))
                        {
                            infoToNum += orderOverload[letter.ToString()].ToString() + "/";
                        }
                        else
                        {
                            infoToNum = 0.ToString();
                        }
                    }

                    if (!lastNameAsNum.Contains(infoToNum))
                    {
                        lastNameAsNum.Add(infoToNum);
                    }
                }
                return lastNameAsNum;
            }
            if (PartOfName == "middle_name")
            {
                //Convert middle name too number values assigned to sort order data
                //Dictionary to store cust info as a number (key) and cust info as chars (value)
                var middleNameAsNum = new List<string>();
                foreach (Models.Customer cust in CustomerList.customers)
                {
                    String infoToNum = "";
                    char[] middleName;
                    if (cust.middle_name != null)
                    {
                        middleName = cust.middle_name.ToCharArray();
                        foreach (char letter in middleName)
                        {
                            if (orderOverload.ContainsKey(letter.ToString()))
                            {
                                infoToNum += orderOverload[letter.ToString()].ToString() + "/";
                            }
                            else
                            {
                                infoToNum = 0.ToString();
                            }
                        }
                        if (!middleNameAsNum.Contains(infoToNum))
                        {
                            middleNameAsNum.Add(infoToNum);
                        }
                    }
                    else
                    {
                        if (!middleNameAsNum.Contains("0"))
                        {
                            middleNameAsNum.Add("0");
                        }
                    }
                }
                return middleNameAsNum;
            }
            return null;
        }

        public Dictionary<string, int> ConvertToString(Dictionary<string, int> orderOverload,List<String> CustNameAsNums)
        {
            //Convert last name from numbers back to chars and add to dictionary (last name val + sort order)
            int countEmp = 1;
            var NameWithSortVal = new Dictionary<string, int>();
            //Loop through each numeric string representing customer info
            foreach (String employee in CustNameAsNums)
            {
                String[] nameNum = employee.Split('/');
                String name = "";
                //Loop thorugh each number in number string
                foreach (String letter in nameNum)
                {
                    if (letter != "")
                    {
                        //Get char value for number and store in var
                        string numCharVal = orderOverload.FirstOrDefault(z => z.Value == int.Parse(letter)).Key;
                        name = name + numCharVal;
                    }
                }
                if (!NameWithSortVal.ContainsKey(name))
                {
                    //Add name to list
                    NameWithSortVal.Add(name, countEmp);
                    countEmp++;
                }
            }
            return NameWithSortVal;
        }

        public List<String> CustInfoSort(List<String> CustNameAsNums)
        {
            //Sort Customer Info list using custom sort
            return CustNameAsNums.OrderBy(emp => emp, new SemiNumericComparer()).ToList();
        }

        public ActionResult Index()
        {
            using (WebClient webClient = new WebClient())
            {
                //Get Json data (Custoemr info:  First Name, LAst Name, Middle Init)
                WebClient WCdata = new WebClient();
                var CustomerInfoDataSource = WCdata.DownloadString(JsonDataURL);
                var CustomerList = JsonConvert.DeserializeObject<Models.CustomerList>(CustomerInfoDataSource);

                //Get Json data (Sort order for characters)
                WebClient WCsort = new WebClient();
                var SortDataSource = WCsort.DownloadString(JsonSortURL);
                var SortDataList = JsonConvert.DeserializeObject<Models.CustomerSortOrder>(SortDataSource);

                //Assign order number to each value in sort order data
                var orderOverload = new Dictionary<string, int>();
                int sortNum = new int();
                sortNum = 1;

                foreach (Char sortVal in SortDataList.sort_order)
                {
                    orderOverload.Add(sortVal.ToString(), sortNum);
                    sortNum++;
                }

                var firstNameAsNum = ConvertToNumber(orderOverload, CustomerList, "first_name");
                var middleNameAsNum = ConvertToNumber(orderOverload, CustomerList, "middle_name");
                var lastNameAsNum = ConvertToNumber(orderOverload, CustomerList, "last_name");

                var firstNameSorted = CustInfoSort(firstNameAsNum);
                var middleNameSorted = CustInfoSort(middleNameAsNum);
                var lastNameSorted = CustInfoSort(lastNameAsNum);

                var firstNameAsString = ConvertToString(orderOverload, firstNameSorted);
                var middleNameAsString = ConvertToString(orderOverload, middleNameSorted);
                var lastNameAsString = ConvertToString(orderOverload, lastNameSorted);



                /*Use Custoemr list and join on firstNameAsString,LastNameAsString and Middle Name as String.
                  By joining on original customer list, gerts  "order" values assigned to each first,last,middle name.
                 Sort by last name, first name, middle init and store in IList to pass to view
                 Sort on values assigned when converting back to chars from nums*/
                var CustomerInfoSorted = (from cust in CustomerList.customers
                                          join sortValLast in lastNameAsString on cust.last_name equals sortValLast.Key into a
                                          from b in a.DefaultIfEmpty()
                                          join sortValFirst in firstNameAsString on cust.first_name equals sortValFirst.Key into c
                                          from d in c.DefaultIfEmpty()
                                          join sortValMid in middleNameAsString on cust.middle_name equals sortValMid.Key into e
                                          from f in e.DefaultIfEmpty()
                                          orderby b.Value, d.Value, f.Value
                                          select new Models.CustomerJoinSort { first_name = cust.first_name, middle_name = cust.middle_name, last_name = cust.last_name, sortOrderLast = b.Value, sortOrderFirst = d.Value, sortOrderMiddle = f.Value }).ToList();

                return View(CustomerInfoSorted);
            }
        }

    }
}