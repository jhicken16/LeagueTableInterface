using System;
using System.IO;

namespace LeagueTableInterface.Models.MediaFetcher
{
    public class FetchTeamMedia
    {
        protected string[] GetFileNames(string teamId)
        {

            string directoryPath = $".\\wwwroot\\TeamMidia\\{teamId}\\";


            return Directory.GetFiles(directoryPath, "*");

        }
        protected void TurnRootsToURLs(string[] fileNames)
        {

            string temp = "";
            char check = '\\';
            for (int i = 0; i < fileNames.Length; i++)
            {

                if (!string.IsNullOrEmpty(fileNames[i]))
                {

                    for (int x = 0; x < fileNames[i].Length; x++)
                    {
                        char c = (char)fileNames[i][x];
                        if (x == 0)
                        {
                            //remove wwwroot from start of string.
                            x = fileNames[i].IndexOf("wwwroot") + "wwwroot".Length - 1;
                        }
                        else if (c.Equals(check))
                        {
                            //swaps backslashes to forward slashes
                            temp += "/";
                        }
                        else
                        {
                            temp += fileNames[i][x];
                        }
                    }
                }
                fileNames[i] = temp;
                temp = "";
            }
           
        }
    }
}
