using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CurrencyConverter_BL_DataLayer
{
    public class DataLayer
    {
        private string _filePath;

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath">path to the file that contains rateList</param>
        public DataLayer(string filePath)
        {
            this._filePath = filePath;
        }

        /// <summary>
        /// Method to check whether a file at {FilePath} exist or not
        /// it will return false in case of empty file
        /// </summary>
        /// <returns>boolean value <br/>
        /// true - if file is present <br/>
        /// false - if file is not there</returns>
        public bool CheckIfAlreadyExist()
        {
            if (File.Exists(FilePath) && (new FileInfo(FilePath)).Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to write a pair of (rate and symbol) to a file at location {FilePath}
        /// </summary>
        /// <param name="symbol">Currency Symbol</param>
        /// <param name="rate">Currency Rate</param>
        /// <param name="createNew">a boolean arg. set - <br/>
        /// true - if you want to create a new file (it will delete the previous existing file) <br>
        /// false - if you want to append into already existing file</param>
        public void WriteRateToFile(string symbol, float rate, bool createNew)
        {
            try
            {
                StreamWriter sw = new StreamWriter(FilePath, !createNew);
                sw.WriteLine($"{symbol},{rate}");
                sw.Close();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to load conversion list from text file
        /// it will return a dictionary with key value pair <currency name, currency rate equivalent to 1 INR>
        /// </summary>
        /// <returns>Dictionary<string,float></returns> the rate List in form of dictionary<symbol, rate>
        /// <exception cref="FormatException">Thrown while parsing the rate value into float</exception>
        /// <exception cref="FileNotFoundException">Thrown when file at provided path does not exist</exception>
        public Dictionary<string, float> LoadRateList()
        {
            Dictionary<string, float> rateList = new Dictionary<string, float>();
            try
            {
                rateList = File.ReadAllLines(FilePath)
                                .Select(x =>
                                        new KeyValuePair<string, float>(x.Split(',')[0], float.Parse(x.Split(',')[1])))
                                .ToDictionary(t => t.Key, t => t.Value);
            }
            catch
            {
                throw;
            }

            return rateList;
        }
    }
}