using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    [Serializable]
    public class Transaction
    {
        private Customer _attachedCust;
        public Customer Customer { get { return _attachedCust; } }
        private List<OrderItem> _bookList;
        public List<OrderItem> BookList { get { return _bookList; } }

        
        public Transaction(Customer attachedCust, List<OrderItem> bookList)
        {
            _attachedCust = attachedCust;
            _bookList = bookList;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (OrderItem oi in _bookList)
            {
                sb.Append(_attachedCust.UserName + ": ");
                sb.Append(oi.ToTransactionString());
                sb.Append(",  ");
            }

            return sb.ToString();
        }

        public void AddBook(Book b)
        {
            foreach (OrderItem oi in _bookList)
            {
                if (oi.TryAddBook(b))
                {
                    return;
                }
                
            }
            _bookList.Add(new OrderItem(1, b));
        }

        public void ReturnBook(OrderItem oi)
        {
            if (oi.ReturnBook() == false)
            {
                _bookList.Remove(oi);
            }
        }

        public string[] CartTotalArray()
        {
            string[] toReturn = new string[2];
            decimal totalPrice = 0;
            foreach (OrderItem oi in _bookList)
            {
                totalPrice += oi.GetCost();
            }
            toReturn[0] = "====================================";
            toReturn[1] = "Total Price: " + totalPrice.ToString("C");

            return toReturn;
        }
    }
}
