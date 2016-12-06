using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    [Serializable]
    public class Book
    {
        private string _isbn;
        public string ISBN { get { return _isbn; } }
        private string _title;
        public string Title { get { return _title; } }
        private string _author;
        public string Author { get { return _author; } }
        private int _stock;
        public int Stock { get { return _stock; } }
        private string _publisher;
        public string Publisher { get { return _publisher; } }
        private decimal _price;
        public decimal Price { get { return _price; } }
        private string _date;
        public string Date { get { return _date; } }


        public Book(string title, string author, string publisher, string isbn, string date, decimal price, int stock)
        {
            _isbn = isbn;
            _title = title;
            _author = author;
            _stock = stock;
            _publisher = publisher;
            _price = price;
            _date = date;

        }

        public override string ToString()
        {
            return (_title + "  " + _author + "  " + _publisher + "  " + _isbn + "  " + _price.ToString("F") + "  " + _stock);
        }

        public string ToWishListString()
        {
            return (_title + " BY " +  _author);
        }

        public bool TryGetBook()
        {
            if (_stock > 0)
            {
                _stock--;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddStock(int numToAdd)
        {
            _stock += numToAdd;
        }


        public bool ISBNMatch(string isbn)
        {
            if (isbn == _isbn)
                return true;
            else return false;
        }

        public void EditBook(string title, string author, string publisher, string isbn, string date, decimal price, int stock)
        {
            if (stock < 0) throw new BookShopException("Stock can't be less than 0");
            if (price < 0) throw new BookShopException("Price can't be less than 0");

            _isbn = isbn;
            _title = title;
            _author = author;
            _stock = stock;
            _publisher = publisher;
            _price = price;
            _date = date;
        }
    }
}
