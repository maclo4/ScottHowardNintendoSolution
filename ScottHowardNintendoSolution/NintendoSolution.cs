using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ScottHowardNintendoSolution
{
    class NintendoSolution
    {
        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.White;

            // declare variables
            StreamReader newFileStreamReader = null;
            StreamReader oldFileStreamReader = null;
            string newShaTxt;
            string oldShaTxt;
            string newFilePath = "";
            bool validFilesGiven = true;

            // the following 3 blocks of code are just error checking to ensure valid paths are given
            do
            {
                validFilesGiven = true;
                // get user inputs for file locations
                Console.WriteLine("Enter path to Old.sha.txt: ");

                oldShaTxt = Console.ReadLine();

                try
                {
                    oldFileStreamReader = new StreamReader(@oldShaTxt);
                }
                catch (Exception e)
                {
                    validFilesGiven = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                Console.WriteLine();
            } while (validFilesGiven == false);


            do
            {

                validFilesGiven = true;
                Console.WriteLine("Enter path to New.sha.txt: ");

                newShaTxt = Console.ReadLine();

                // create StreamReaders to read fileNames
                try
                {
                    newFileStreamReader = new StreamReader(@newShaTxt);

                }
                catch (Exception e)
                {
                    validFilesGiven = false;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                Console.WriteLine();
            } while (validFilesGiven == false);

            do
            {

                validFilesGiven = true;
                Console.WriteLine("Enter the path to the location in which you would like to create new files \n" +
                    "ex. C:/User/Documents (leaving blank will create them in current directory): ");
                newFilePath = Console.ReadLine();


                // create StreamReaders to read fileNames
                try
                {
                    File.WriteAllText(@newFilePath + "OldNotInNew.txt", "");
                    if (newFilePath.EndsWith(@"\") == false && newFilePath.EndsWith(@"/") == false
                        && newFilePath.Equals("") == false)
                    {
                        newFilePath = "";
                        throw new Exception(@"File path must end in \ or /");
                    }
                }
                catch (Exception e)
                {
                    validFilesGiven = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                Console.WriteLine();
            } while (validFilesGiven == false);



            // variables used to store each line of the files
            List<string> newFiles = new List<string>();
            List<string> oldFiles = new List<string>();
            string line;

            // read files into each list
            line = newFileStreamReader.ReadLine();
            while (line != null)
            {
                newFiles.Add(line);
                line = newFileStreamReader.ReadLine();
            }

            line = oldFileStreamReader.ReadLine();
            while (line != null)
            {
                oldFiles.Add(line);
                line = oldFileStreamReader.ReadLine();
            }

            // call funtion to find newnotinold and oldnotinnew
            List<string> newNotInOld = findMatchingFiles(oldFiles, newFiles);
            List<string> oldNotInNew = findMatchingFiles(newFiles, oldFiles);

            // write results to new files
            File.WriteAllLinesAsync(@newFilePath + "OldNotInNew.txt", oldNotInNew);
            File.WriteAllLinesAsync(@newFilePath + "NewNotInOld.txt", newNotInOld);

            // print results for easy viewing
            Console.WriteLine("\nNewNotInOld contains: ");
            foreach (string currLine in newNotInOld)
            {
                Console.WriteLine(currLine);

            }


            Console.WriteLine("\nOldNotInNew contains: ");
            foreach (string currLine in oldNotInNew)
            {
                Console.WriteLine(currLine);
            }
            Console.WriteLine();
        }


        // function to search for files from fileList1 that are not in fileList2
        static List<string> findMatchingFiles(List<string> fileList1, List<string> fileList2)
        {
            // this is the variable to be returned, will contain all files from fileList1 that do not appear in fileList2
            List<string> filesNotInList2 = new List<string>();

            // flag to test if file was find in fileList2
            bool fileFoundInList2 = false;

            // compare sha values for equivalency
            string list1ShaSubstring;
            string list2ShaSubstring;

            // loop through each line in fileList1
            for (int i = 0; i < fileList1.Count; i++)
            {
                // take the substring of only the sha value
                list1ShaSubstring = fileList1[i].Substring(0, fileList1[i].IndexOf(" "));

                // compare each sha value in fileList2 to the current shaSubstring
                foreach (string newFileName in fileList2)
                {
                    // get sha substring
                    list2ShaSubstring = newFileName.Substring(0, fileList1[i].IndexOf(" "));

                    // compare sha strings, if equal break loop and set flag to true
                    if (list1ShaSubstring.Equals(list2ShaSubstring))
                    {
                        fileFoundInList2 = true;
                        break;
                    }
                }

                // check flag to see if file was found in list 2
                if (fileFoundInList2 == false)
                {
                    // if not in list 2, add it to filesNotInList2
                    filesNotInList2.Add(fileList1[i].Substring(fileList1[i].IndexOf(" ") + 1));
                }
                else
                {
                    // reset flag
                    fileFoundInList2 = false;
                }
            }

            return filesNotInList2;
        }
    }
}

