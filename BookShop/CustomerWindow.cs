using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace edu.ksu.cis.masaaki
{
    public partial class CustomerWindow : Form
    {
        // XXX add more fields if necessary

        Controller _attachedControl;

        CustomerDialog customerDialog;
        LoginDialog loginDialog;
        ListBooksDialog listBooksDialog;
        BookInformationDialog bookInformationDialog;
        CartDialog cartDialog;
        WishListDialog wishListDialog;
        BookInWishListDialog bookInWishListDialog;
        ListTransactionHistoryDialog listTransactionHistoryDialog;
        ShowTransactionDialog showTransactionDialog;

        public CustomerWindow()
        {
            InitializeComponent();
        }

        public CustomerWindow(Controller ac) : this()
        {
            _attachedControl = ac;
        }

        // XXX You may add overriding constructors (constructors with different set of arguments).
        // If you do so, make sure to call :this()
        // public CustomerWindow(XXX xxx): this() { }
        // Without :this(), InitializeComponent() is not called


        private void CustomerWindow_Load(object sender, EventArgs e)
        {
            customerDialog = new CustomerDialog();
            loginDialog = new LoginDialog();
            listBooksDialog = new ListBooksDialog();
            bookInformationDialog = new BookInformationDialog();
            cartDialog = new CartDialog();
            wishListDialog = new WishListDialog();
            bookInWishListDialog = new BookInWishListDialog();
            listTransactionHistoryDialog = new ListTransactionHistoryDialog();
            showTransactionDialog = new ShowTransactionDialog();
        }

        private void bnLogin_Click(object sender, EventArgs e)
        {
            try
            {  // throw exception if the customer is not found
                // XXX Login Button event handler
                // First, you may want to check if anyone is logged in

                
                if (_attachedControl.CurrentCustomer != null)
                {
                    throw new BookShopException("A customer is already logged in.");
                }

                switch(loginDialog.Display())
                {
                    case DialogReturn.Cancel:
                        return;
                    // XXX Login button is pressed
                    case DialogReturn.Login:
                        if (_attachedControl.LoginCustomer(loginDialog.UserName, loginDialog.Password))
                            UpdateCustomerText();
                        break;
                    default:
                        return;
                }

                //if (loginDialog.Display() == DialogReturn.Cancel) return;

                //// XXX Login Button is pressed
                //if (loginDialog.Display() == DialogReturn.Login)
                //    _attachedControl.LoginCustomer(loginDialog.UserName, loginDialog.Password);

            }
            catch(BookShopException bsex)
            {
                MessageBox.Show(this, bsex.ErrorMessage);
            }
}

        private void bnAddCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                // throw exception if the customer id is already registered
                // ZTM : Throws exception inside Control
                // XXX Register Button event handler
                
                customerDialog.ClearDisplayItems();

                switch(customerDialog.Display())
                {
                    case DialogReturn.Cancel:
                        return;
                    case DialogReturn.Done:
                        _attachedControl.RegisterCustomer(customerDialog.FirstName, customerDialog.LastName, customerDialog.UserName, customerDialog.Password, customerDialog.EMailAddress, customerDialog.Address, customerDialog.TelephoneNumber);
                        break;
                    default:
                        return;
                }

                //if (customerDialog.Display() == DialogReturn.Cancel) return;
                //// XXX pick up information from customerDialog by calling its properties
                //// and register a new customer
                //if (customerDialog.Display() == DialogReturn.Done)
                //{
                //    _attachedControl.RegisterCustomer(customerDialog.FirstName, customerDialog.LastName, customerDialog.UserName, customerDialog.Password, customerDialog.EMailAddress, customerDialog.Address, Convert.ToInt32(customerDialog.TelephoneNumber));
                //}
                

            }
            catch (BookShopException bsex)
            {
                MessageBox.Show(this, bsex.ErrorMessage);
            }
        }

        private void bnEditSelfInfo_Click(object sender, EventArgs e)
        {
            // XXX Edit Self Info button event handler
            try {
                if (_attachedControl.IsLoggedIn())
                {
                    switch (customerDialog.Display())
                    {
                        case DialogReturn.Cancel:
                            return;
                        case DialogReturn.Done:
                            _attachedControl.EditCurrentCustomer(customerDialog.FirstName, customerDialog.LastName, customerDialog.UserName, customerDialog.Password, customerDialog.EMailAddress, customerDialog.Address, customerDialog.TelephoneNumber);
                            break;
                        default:
                            return;

                    }
                }
                else throw new BookShopException("Customer not logged in"); 
            }
            catch(BookShopException bsex)
            {
                MessageBox.Show(this, bsex.ErrorMessage);
            }

            //if (customerDialog.Display() == DialogReturn.Cancel) return;
            //// XXX Done button is pressed
            //if (customerDialog.Display() == DialogReturn.Done)
            //    _attachedControl.EditCurrentCustomer(customerDialog.FirstName, customerDialog.LastName, customerDialog.UserName, customerDialog.Password, customerDialog.EMailAddress, customerDialog.Address, Convert.ToInt32(customerDialog.TelephoneNumber));

        }

        private void bnBook_Click(object sender, EventArgs e)
        {
            // XXX List Books buton is pressed
            
            while (true)
            { 
                try
                {  // to capture an exception from SelectedItem/SelectedIndex of listBooksDialog
                    listBooksDialog.ClearDisplayItems();
                    listBooksDialog.AddDisplayItems(_attachedControl.BookList.ToArray()); // XXX null is a dummy argument
                    if (listBooksDialog.Display() == DialogReturn.Done) return;
                    // select is pressed
                    Book selectedBook = (Book)listBooksDialog.SelectedItem;
                    _attachedControl.PopulateBookDialog(selectedBook, bookInformationDialog);
                    switch (bookInformationDialog.Display())
                    {
                        case DialogReturn.AddToCart: // Add to Cart
                                                     // XXX
                            _attachedControl.CurrentCustomer.AddBookToCart(selectedBook);
                            continue;

                        case DialogReturn.AddToWishList: // Add to Wishlist
                            // XXX
                            _attachedControl.CurrentCustomer.AddToWishList(selectedBook);
                            continue;

                        case DialogReturn.Done: // cancel
                            continue;
                        default: return;
                    }
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }

        private void bnShowWishlist_Click(object sender, EventArgs e)
        {
            // XXX Show WishList Button event handler
          
            while (true)
            {
                try
                { // to capture an excepton by SelectedItem/SelectedIndex of wishListDialog
                    if (!_attachedControl.IsLoggedIn())
                    {
                        throw new BookShopException("Customer not logged in.");
                    }

                    wishListDialog.ClearDisplayItems();
                    wishListDialog.AddDisplayItems(_attachedControl.CurrentCustomer.WishList.ToArray());  // XXX null is a dummy argument
                    if (wishListDialog.Display() == DialogReturn.Done) return;
                    //// select is pressed
                    WishListItem selectedWishListItem = (WishListItem)wishListDialog.SelectedItem;
                    _attachedControl.PopulateBookDialog(selectedWishListItem.AttachedBook, bookInWishListDialog);
                    //XXX 
                    switch (bookInWishListDialog.Display())
                    {
                        case DialogReturn.AddToCart:
                            // XXX 
                            _attachedControl.CurrentCustomer.AddToCartFromWishList(selectedWishListItem);

                            continue;
                        case DialogReturn.Remove:
                            // XXX
                            _attachedControl.CurrentCustomer.RemoveFromWishList(selectedWishListItem);

                            continue;
                        case DialogReturn.Done: // Done
                            continue;
                    }
                }
                catch(BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }

        private void bnShowCart_Click(object sender, EventArgs e)
        {
           // XXX Show Cart Button event handler
            while (true)
            {
                try
                {  // to capture an exception from SelectedIndex/SelectedItem of carDisplay
                    cartDialog.ClearDisplayItems();
                    cartDialog.AddDisplayItems(_attachedControl.CurrentCustomer.Cart.BookList.ToArray()); // null is a dummy argument
                    cartDialog.AddDisplayItems(_attachedControl.CurrentCustomer.Cart.CartTotalArray());

                    //foreach (OrderItem oi in _attachedControl.CurrentCustomer.Cart.BookList)
                    //{
                    //    if (oi == selectedObject)
                    //    {
                    //        targetObject = oi;
                    //    }
                    //}

                    switch (cartDialog.Display())
                    {
                        case DialogReturn.CheckOut:  // check out
                            // XXX
                            _attachedControl.CheckOutCurrentCustomer();
                            return;
                        case DialogReturn.ReturnBook: // remove a book
                            // XXX
                            object selectedObject = cartDialog.SelectedItem;  // picked up a string or an OrderItem object
                            if (!(selectedObject is OrderItem))
                            {
                                MessageBox.Show(this, "an extra line was selected");
                                continue;
                            }
                            OrderItem targetObject = (OrderItem)selectedObject;
                            _attachedControl.ReturnBook(targetObject);
                            continue;
                        
                        case DialogReturn.Done: // cancel
                            return;
                    }
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }

        private void bnTransactionHistory_Click(object sender, EventArgs e)
        {
            // XXX Transaction History button handler
            while (true)
            {
                
                try
                {  // to capture an exception from SelectedIndex/SelectedItem of listTransactionHistoryDialog
                    if (_attachedControl.IsLoggedIn())
                    {

                        listTransactionHistoryDialog.ClearDisplayItems();
                        listTransactionHistoryDialog.AddDisplayItems(_attachedControl.CurrentCustomer.OrderHistory.ToArray()); // null is a dummy argument
                        if (listTransactionHistoryDialog.Display() == DialogReturn.Done) return;
                        // Select is pressed
                        Transaction selectedTransaction = 

                        showTransactionDialog.ClearDisplayItems();
                        showTransactionDialog.AddDisplayItems(null); // null is a dummy argument
                        showTransactionDialog.ShowDialog();
                    }
                    else throw new BookShopException("Customer not logged in.");
                }
                catch(BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                }
            }
        }

        private void bnLogout_Click(object sender, EventArgs e)
        {
            // XXX Logout  button event handler
            _attachedControl.LogOutCustomer();
        }

        public void UpdateCustomerText()
        {
            if (_attachedControl.IsLoggedIn())
                lbLoggedinCustomer.Text = ("Loggedin Customer: " + _attachedControl.CurrentCustomer.UserName);
            else
                lbLoggedinCustomer.Text = ("Loggedin Customer: (none)");
        }


    }
}
