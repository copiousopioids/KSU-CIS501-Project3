using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    public class Transaction
    {
        private Customer _attachedCust;
        private List<OrderItem> _bookList;

        
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
                sb.Append(oi.ToString());
                sb.Append(",   ");
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
    }
}
