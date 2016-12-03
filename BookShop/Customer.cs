using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    public class Customer
    {
        private string _firstName;
        private string _lastName;
        private string _userName;
        public string UserName { get { return _userName; } }
        private string _password;
        private string _email;
        private string _address;
        private string _teleNum;

        private Transaction _cart;
        private List<Transaction> _orderHistory;
        private List<Book> _wishList;

        public Customer(string fn, string ln, string un, string pw, string email, string add, string tn)
        {
            _firstName = fn;
            _lastName = ln;
            _userName = un;
            _password = pw;
            _email = email;
            _address = add;
            _teleNum = tn;

            _orderHistory = new List<Transaction>();
            _wishList = new List<Book>();
            _cart = new Transaction(this, new List<OrderItem>());
        }

        public bool IsSameUsername(string username)
        {
            if (username == _userName)
                return true;
            else
                return false;
        }

        public bool LoginOK(string un, string pw)
        {
            if (un == _userName & pw == _password)
            {
                return true;
            }
            else return false;
        }

        public void EditInfo(string fn, string ln, string un, string pw, string email, string add, string tn)
        {
            if (fn != null)
                _firstName = fn;
            if (ln != null)
                _lastName = ln;
            if (un != null)
                _userName = un;
            if (pw != null)
                _password = pw;
            if (email != null)
                _email = email;
            if (add != null)
                _address = add;

            _teleNum = tn;
        }

        public void AddBookToCart(Book b)
        {
            if (b.TryGetBook())
            {
                _cart.AddBook(b);
            }
        }

        public void AddToWishList(Book bookToAdd)
        {
            foreach (Book b in _wishList)
            {
                if (bookToAdd == b)
                {
                    throw new BookShopException("Book is already on your wishlist.");
                }
            }
            _wishList.Add(bookToAdd);
        }

        public Transaction CheckOut()
        {
            Transaction toReturn = _cart;
            _orderHistory.Add(_cart);
            _cart = new Transaction(this, new List<OrderItem>());
            return toReturn;
        }
    }
}
