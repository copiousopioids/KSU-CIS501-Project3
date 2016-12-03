using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    public class WishListItem
    {
        public Book AttachedBook { get; set; }


        public WishListItem(Book b)
        {
            AttachedBook = b;
        }

        public override string ToString()
        {
            return AttachedBook.Title + " BY " + AttachedBook.Author;
        }
    }
}
