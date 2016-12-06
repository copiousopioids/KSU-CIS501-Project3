using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    [Serializable]
    public  class OrderItem
    {
        private int _numBooks;
        private Book _book;
        public Book Book { get { return _book; } }

        public OrderItem(int numBooks, Book book)
        {
            _numBooks = numBooks;
            _book = book;
        }

        public string ToTransactionString()
        {
            return (_book.Title + "(" + _numBooks + ")");
        }

        public bool TryAddBook(Book b)
        {
            if (b == _book)
            {
                _numBooks++;
                return true;
            }
            else return false;
        }

        public override string ToString()
        {
            return _book.ToWishListString() + " : " + _numBooks + " " + GetCost().ToString("C");
        }

        public decimal GetCost()
        {
            return _book.Price * _numBooks;
        }

        /// <summary>
        /// Returns true if there is more books left.
        /// Returns false if there is no more books left.
        /// </summary>
        /// <returns</returns>
        public bool ReturnBook()
        {
            _numBooks--;
            if (_numBooks <= 0) return false;
            else return true;
        }
    }
}
