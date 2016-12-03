using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    public class Controller
    {
        private List<Customer> _customers;
        public List<Customer> CustomerList { get { return _customers; } set { _customers = value; } }
        private Customer _currentCustomer;
        public Customer CurrentCustomer { get { return _currentCustomer; } set { _currentCustomer = value; } }
        private List<Book> _books;
        public List<Book> BookList { get { return _books;  } set { _books = value; } }
        public List<Transaction> _completeOrders;
        public List<Transaction> _pendingOrders;

        

        public Controller()
        {
            _customers = new List<Customer>();
            _books = new List<Book>();
            _pendingOrders = new List<Transaction>();
            _completeOrders = new List<Transaction>();
        }


        public void RegisterCustomer(string fn, string ln, string un, string pw, string email, string add, string tn)
        {
            foreach (Customer c in _customers) {
                if (c.IsSameUsername(un))
                {
                    throw new BookShopException("That username already exists");
                }
            }

            Customer toRegister = new Customer(fn, ln, un, pw, email, add, tn);

            _customers.Add(toRegister);

        }

        public bool LoginCustomer(string username, string pw)
        {
            //Handled in CustomerWindow for now

            //if (_currentCustomer != null)
            //{
            //    throw new BookShopException("A customer is already logged in.");
            //}
                
            bool loginSuccess = false;
            foreach (Customer c in _customers)
            {
                if (c.LoginOK(username, pw))
                {
                    loginSuccess = true;
                    _currentCustomer = c;
                    return true;
                }
                //if(c.LoginOK(username, pw))
                //{
                //    _currentCustomer = c;
                //    loginSuccess = true;
                //}
            }

            if (!loginSuccess)
                throw new BookShopException("User not found.");

            return false;      
        }
        
        // ZTM: Possibly just access directly through the CustomerWindow class instead of passing through like this...
        public void EditCurrentCustomer(string fn, string ln, string un, string pw, string email, string add, string tn)
        {
            _currentCustomer.EditInfo(fn, ln, un, pw, email, add, tn);
        }
        

        public void AddBookToWishListByISBN(string isbn)
        {
            foreach (Book b in _books)
            {
                if (b.ISBNMatch(isbn))
                {
                    _currentCustomer.AddToWishList(b);
                    break;
                }
            }
        }

        public void AddBookToCartByISBN(string isbn)
        {
            foreach (Book b in _books)
            {
                if (b.ISBNMatch(isbn))
                {
                    _currentCustomer.AddBookToCart(b);
                }
            }
        }

        public void CheckOutCurrentCustomer()
        {
            _pendingOrders.Add(_currentCustomer.CheckOut());
        }

        public void ProcessPendingTransaction(int index)
        {
            if (index >= 0 && index < _pendingOrders.Count())
            {
                _completeOrders.Add(_pendingOrders.ElementAt(index));
                _pendingOrders.RemoveAt(index);
            }
            else throw new BookShopException("Index is outside range of pending transactions.");
        }

        public void LogOutCustomer()
        {
            _currentCustomer = null;
        }

    }
}
