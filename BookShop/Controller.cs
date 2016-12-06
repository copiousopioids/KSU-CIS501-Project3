using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        private List<Transaction> _completeOrders;
        public List<Transaction> CompleteOrders { get { return _completeOrders; } }
        private List<Transaction> _pendingOrders;
        public List<Transaction> PendingOrders { get { return _pendingOrders; } }

        

        public Controller()
        {
            _customers = new List<Customer>();
            _books = new List<Book>();
            _pendingOrders = new List<Transaction>();
            _completeOrders = new List<Transaction>();
        }

        public bool IsLoggedIn()
        {
            if (_currentCustomer != null)
                return true;
            else return false;
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
        public void EditCustomer(Customer c, string fn, string ln, string un, string pw, string email, string add, string tn)
        {
            c.EditInfo(fn, ln, un, pw, email, add, tn);
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

        public void AddBook(string title, string author, string publisher, string isbn, string date, decimal price, int stock)
        {
            foreach (Book b in _books)
            {
                if (b.ISBN == isbn)
                {
                    throw new BookShopException("That ISBN has already been registered.");
                }
            }
            _books.Add(new Book(title, author, publisher, isbn, date, price, stock));
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

        public void AddToPendingTransactions(Transaction t)
        {
            _pendingOrders.Add(t);
        }

        public void CompleteTransactionAt(int index)
        {
            if (index >= 0 && index < _pendingOrders.Count())
            {
                _completeOrders.Add(_pendingOrders.ElementAt(index));
                _pendingOrders.RemoveAt(index);
            }
        }
        
        public void ReturnBook(Customer c, Transaction t, OrderItem oi)
        {
            c.ReturnFromTransaction(t, oi);
        }

        public void SaveState(string filename)
        {
            BinaryFormatter fo = new BinaryFormatter();
            using (FileStream f = new FileStream(filename, FileMode.Create))
            {
                Tuple<List<Customer>, List<Book>, List<Transaction>, List<Transaction>> tuple = new Tuple<List<Customer>, List<Book>, List<Transaction>, List<Transaction>>(_customers, _books, _pendingOrders, _completeOrders);
                fo.Serialize(f, tuple);
            }

        } 
        
        public void RestoreState(string filename)
        {
            BinaryFormatter fo = new BinaryFormatter();
            using (FileStream f = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read))
            {
                Tuple<List<Customer>, List<Book>, List<Transaction>, List<Transaction>> tuple = (Tuple<List<Customer>, List<Book>, List<Transaction>, List<Transaction>>) new BinaryFormatter().Deserialize(f);
                _customers = tuple.Item1;
                _books = tuple.Item2;
                _pendingOrders = tuple.Item3;
                _completeOrders = tuple.Item4;
            }
        }

        public void PopulateCustomerDialog(Customer c, CustomerDialog cd)
        {
            cd.FirstName = c.FirstName;
            cd.LastName = c.LastName;
            cd.UserName = c.UserName;
            cd.Password = c.Password;
            cd.EMailAddress = c.Email;
            cd.Address = c.Address;
            cd.TelephoneNumber = c.Telephone;
        }

        public void PopulateBookDialog(Book b, BookDialog bd)
        {
            bd.BookTitle = b.Title;
            bd.Author = b.Author;
            bd.Publisher = b.Publisher;
            bd.ISBN = b.ISBN;
            bd.Date = b.Date;
            bd.Price = b.Price;
            bd.Stock = b.Stock;
        }
    }
}
