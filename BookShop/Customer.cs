using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    [Serializable]
    public class Customer
    {
        private string _firstName;
        public string FirstName { get { return _firstName; } }
        private string _lastName;
        public string LastName { get { return _lastName; } }
        private string _userName;
        public string UserName { get { return _userName; } }
        private string _password;
        public string Password { get { return _password; } }
        private string _email;
        public string Email { get { return _email; } }
        private string _address;
        public string Address { get { return _address; } }
        private string _teleNum;
        public string Telephone { get { return _teleNum; } }

        private Transaction _cart;
        public Transaction Cart { get { return _cart; } }
        private List<Transaction> _orderHistory;
        public List<Transaction> OrderHistory { get { return _orderHistory; } }
        private List<WishListItem> _wishList;
        public List<WishListItem> WishList { get { return _wishList; } }

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
            _wishList = new List<WishListItem>();
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
            _firstName = fn;
            _lastName = ln;
            _userName = un;
            _password = pw;
            _email = email;
            _address = add;
            _teleNum = tn;
        }

        public void AddBookToCart(Book b)
        {
            if (b.TryGetBook())
            {
                _cart.AddBook(b);
            }
            else throw new BookShopException("There is no stock left for the selected book.");
        }

        public void AddToWishList(Book bookToAdd)
        {
            foreach (WishListItem wli in _wishList)
            {
                if (bookToAdd == wli.AttachedBook)
                {
                    throw new BookShopException("Book is already on your wishlist.");
                }
            }
            _wishList.Add(new WishListItem(bookToAdd));
        }

        public Transaction CheckOut()
        {
            Transaction toReturn = _cart;
            _orderHistory.Add(_cart);
            _cart = new Transaction(this, new List<OrderItem>());
            return toReturn;
        }

        public void AddToCartFromWishList(WishListItem wli)
        {
            AddBookToCart(wli.AttachedBook);
        }

        public void RemoveFromWishList(WishListItem wli)
        {
                _wishList.Remove(wli);
        }

        public void ReturnFromTransaction(Transaction t, OrderItem oi)
        {
            t.ReturnBook(oi);
        }

        public override string ToString()
        {
            return _lastName + " " + _firstName + " " + _userName + " " + _email + " " + _address + " " + _teleNum;
        }
    }
}
