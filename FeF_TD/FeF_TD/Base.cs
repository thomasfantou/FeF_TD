using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datas;

namespace FeF_TD
{
    public class Base
    {

        static Base instance = null;
        static readonly object padlock = new object();

        private DataBase _dataBase;

        public DataBase DataBase
        {
            get { return _dataBase; }
            set { _dataBase = value; }
        }
        private int[] _basePrice;

        public int[] BasePrice
        {
            get { return _basePrice; }
            set { _basePrice = value; }
        }

        private Base()
        {
        }

        public void Init()
        {
            _basePrice = new int[6];
            _basePrice[0] = _dataBase.lvl2Price;
            _basePrice[1] = _dataBase.lvl3Price;
            _basePrice[2] = _dataBase.lvl4Price;
            _basePrice[3] = _dataBase.lvl5Price;
            _basePrice[4] = _dataBase.lvl6Price;
            _basePrice[5] = 0;
        }

        public static Base GetInstance()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Base();
                }
                return instance;
            }

        }
    }
}
