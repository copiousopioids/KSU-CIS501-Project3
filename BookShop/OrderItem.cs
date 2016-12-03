using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
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

        public override string ToString()
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
    }
}
