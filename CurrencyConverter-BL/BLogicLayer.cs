using CurrencyConverter_BL_DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConverter_BL_BLogic
{
    public class BLogicLayer
    {
        private string _filePath;
        private DataLayer dLayer;
        private Dictionary<string, float> dictRates;

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
        public BLogicLayer(string filePath)
        {
            this._filePath = filePath;
            dLayer = new DataLayer(FilePath);
            dictRates = new Dictionary<string, float>();
        }

        /// <summary>
        /// Method to check whether a file exist on not (at FilePath)
        /// </summary>
        /// <returns></returns>
        public bool CheckIfExist()
        {
            return dLayer.CheckIfAlreadyExist();
        }

        /// <summary>
        /// Method to get Rate list form DateLayer
        /// </summary>
        public void GetRateList()
        {
            //Dictionary<string, float> dictRate = new Dictionary<string, float>();
            this.dictRates = dLayer.LoadRateList();
        }

        /// <summary>
        /// Method to check if the symbol is present in rate list or not
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public bool ContainsSymbol(string symbol)
        {
            return dictRates.ContainsKey(symbol);
        }

        /// <summary>
        /// Method to Add new symbol and rate to the rateList
        /// </summary>
        /// <param name="symbol">string symbol</param>
        /// <param name="rate">float rate</param>
        /// <param name="createNew">true if you want to create a new list , false will append new values to existing list</param>
        /// <returns> true - if successfully added <br/> false - if there is already a symbol present with same name </returns>
        public void AddNewSymbolAndRate(string symbol, float rate, bool createNew)
        {
            if (createNew)
            {
                dictRates.Clear();
            }

            dLayer.WriteRateToFile(symbol, rate, createNew);
            dictRates.Add(symbol, rate);
        }

        /// <summary>
        /// Method to calculate conversion on the basis of the rate List present
        /// </summary>
        /// <param name="symbol">symbol to use</param>
        /// <param name="amount">amount to convert</param>
        /// <param name="result">out variable to store result</param>
        /// <returns> true - if conversion is successfull <br/> false - if symbol is not present in the rateList</returns>
        public bool CalculateConversion(string symbol, double amount, out double result)
        {
            float rate;

            if (dictRates.TryGetValue(symbol, out rate))
            {
                result = amount * rate;
            }
            else
            {
                result = 0;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method to get all Symbols present
        /// </summary>
        /// <returns>list of string symbols</returns>
        public List<string> GetCurrencySymbolList()
        {
            return dictRates.Keys.ToList<string>();
        }
    }
}