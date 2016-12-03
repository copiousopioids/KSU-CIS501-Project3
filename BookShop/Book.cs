using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    public class Book
    {
        private string _isbn;
        private string _title;
        public string Title { get { return _title; } }
        private string _author;
        private int _stock;
        private string _publisher;
        private decimal _price;
        private string _date;


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

        public string ToWishlistString()
        {
            return (_title + " BY " + _author);
        }

        public string ToCartString()
        {
            return (ToWishlistString() + " : " + _stock + "      " + _price.ToString("C"));
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
    }
}
