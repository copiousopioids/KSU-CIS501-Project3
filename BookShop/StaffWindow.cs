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
    public partial class StaffWindow : Form
    {
        // XXX add more fields if necessary

        ListCustomersDialog listCustomersDialog;
        CustomerDialog customerDialog;
        ListBooksDialog listBooksDialog;
        BookDialog bookDialog;
        ListCompleteTransactionsDialog listCompleteTransactionsDialog;
        ShowCompleteTransactionDialog showCompleteTransactionDialog;
        ListPendingTransactionsDialog listPendingTransactionsDialog;
        ShowPendingTransactionDialog showPendingTransactionDialog;

        Controller _attachedControl;

        public StaffWindow()
        {
            InitializeComponent();
        }

        // XXX You may add overriding constructors (constructors with different set of arguments).
        // If you do so, make sure to call :this()
        // public StaffWindow(XXX xxx): this() { }
        // Without :this(), InitializeComponent() is not called

        public StaffWindow(Controller ac) : this()
        {
            _attachedControl = ac;
        }

        private void StaffWindow_Load(object sender, EventArgs e)
        {
            listCustomersDialog = new ListCustomersDialog();
            customerDialog = new CustomerDialog();
            listBooksDialog = new ListBooksDialog();
            bookDialog = new BookDialog();
            listCompleteTransactionsDialog = new ListCompleteTransactionsDialog();
            showCompleteTransactionDialog = new ShowCompleteTransactionDialog();
            listPendingTransactionsDialog = new ListPendingTransactionsDialog();
            showPendingTransactionDialog = new ShowPendingTransactionDialog();
        }

        private void bnListCustomers_Click(object sender, EventArgs e)
        {
            // XXX List Customers button event handler
            
            while (true)
            {
               
                try
                { // to capture an exception from SelectedIndex/SelectedItem of listCustomersDialog
                    listCustomersDialog.ClearDisplayItems();
                    listCustomersDialog.AddDisplayItems(_attachedControl.CustomerList.ToArray()); // null is a dummy argument
                    if (listCustomersDialog.Display() == DialogReturn.Done) return;
                    // select button is pressed
                    Customer selectedCustomer = (Customer)listCustomersDialog.SelectedItem;
                    _attachedControl.PopulateCustomerDialog(selectedCustomer, customerDialog);
                    if (customerDialog.Display() == DialogReturn.Cancel) continue;
                    // XXX Edit Done button is pressed
                    _attachedControl.EditCustomer(selectedCustomer, customerDialog.FirstName, customerDialog.LastName, customerDialog.UserName, customerDialog.Password, customerDialog.EMailAddress, customerDialog.Address, customerDialog.TelephoneNumber);
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }

        private void bnAddBook_Click(object sender, EventArgs e)
        {
            // XXX Add Book button event handler
            while (true)
            {
                try
                { // to capture an exception from Price/Stock of bookDialog
                    // also throw an exception if the ISBN is already registered
                    bookDialog.ClearDisplayItems();
                    if (bookDialog.ShowDialog() == DialogResult.Cancel) return;
                    // Edit Done button is pressed
                    _attachedControl.AddBook(bookDialog.BookTitle, bookDialog.Author, bookDialog.Publisher, bookDialog.ISBN, bookDialog.Date, bookDialog.Price, bookDialog.Stock);
                    return;
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }

        private void bnListBooks_Click(object sender, EventArgs e)
        {
            // XXX List Books button event handler
           
            while (true)
            {
            
                try
                {   // to capture an exception from SelectedItem/SelectedIndex of listBooksDialog
                    listBooksDialog.ClearDisplayItems();
                    listBooksDialog.AddDisplayItems(_attachedControl.BookList.ToArray()); //null is a dummy argument
                    if (listBooksDialog.Display() == DialogReturn.Done) return;
                    // select is pressed
                    Book selectedBook = (Book)listBooksDialog.SelectedItem;
                    _attachedControl.PopulateBookDialog(selectedBook, bookDialog);
                    while (true)
                    {
  
                        try
                        { // to capture an exception from Price/Stock of bookDialog
                            if (bookDialog.Display() == DialogReturn.Cancel) break;
                            // XXX Edit Done is Pressed
                            selectedBook.EditBook(bookDialog.BookTitle, bookDialog.Author, bookDialog.Publisher, bookDialog.ISBN, bookDialog.Date, bookDialog.Price, bookDialog.Stock);
                            break;
                        }
                        catch (BookShopException bsex)
                        {
                            MessageBox.Show(this, bsex.ErrorMessage);
                            continue;
                        }
                    }
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }
        private void bnPendingTransactions_Click(object sender, EventArgs e)
        {
            // XXX List Pending Transactions button event handl

            while (true)
            {
               
                try
                {  // to capture an exception from SelectedIndex/SelectedItem of listPendingTransactionsDialog
                    listPendingTransactionsDialog.ClearDisplayItems();
                    listPendingTransactionsDialog.AddDisplayItems(_attachedControl.PendingOrders.ToArray());  // null is a dummy argument
                    if (listPendingTransactionsDialog.Display() == DialogReturn.Done) return;
                    // select button is pressed
                    Transaction selectedTransaction = (Transaction)listPendingTransactionsDialog.SelectedItem;
                    while (true)
                    {
                        try
                        {  // to capture an exception from SelectedItem/SelectedTransaction of showPendingTransactionDialog
                            showPendingTransactionDialog.ClearDisplayItems();
                            showPendingTransactionDialog.AddDisplayItems(selectedTransaction.BookList.ToArray()); // null is a dummy argument
                            showPendingTransactionDialog.AddDisplayItems(selectedTransaction.CartTotalArray());
                            switch (showPendingTransactionDialog.Display())
                            {
                                case DialogReturn.Approve:  // Transaction Processed
                                    // XXX
                                    _attachedControl.ProcessPendingTransaction(listPendingTransactionsDialog.SelectedIndex);
                                    break;
                                case DialogReturn.ReturnBook: // Return Book
                                    // XXX
                                    object selectedObject = showPendingTransactionDialog.SelectedItem;  // picked up a string or an OrderItem object
                                    if (!(selectedObject is OrderItem))
                                    {
                                        MessageBox.Show(this, "an extra line was selected");
                                        continue;
                                    }
                                    OrderItem targetObject = (OrderItem)selectedObject;
                                    _attachedControl.ReturnBook(selectedTransaction.Customer, selectedTransaction, targetObject);
                                    continue;
                                case DialogReturn.Remove: // Remove transaction
                                                          // XXX
                                    _attachedControl.PendingOrders.Remove(selectedTransaction);
                                    break;
                            }
                            break; //for "transaction processed"
                        }
                        catch (BookShopException bsex)
                        {
                            MessageBox.Show(this, bsex.ErrorMessage);
                            continue;
                        }
                    }
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }

        private void bnCompleteTransactions_Click(object sender, EventArgs e)
        {
            // XXX List Complete Transactions button event handler
            
            while (true)
            {           
                try
                { // to capture an exception from SelectedItem/SelectedIndex of listCompleteTransactionsDialog
                    listCompleteTransactionsDialog.ClearDisplayItems();
                    listCompleteTransactionsDialog.AddDisplayItems(_attachedControl.CompleteOrders.ToArray()); // XXX null is a dummy argument
                    if (listCompleteTransactionsDialog.Display() == DialogReturn.Done) return;
                    // select button is pressed
                    Transaction selectedTransaction = (Transaction)listCompleteTransactionsDialog.SelectedItem;
                    showCompleteTransactionDialog.ClearDisplayItems();
                    showCompleteTransactionDialog.AddDisplayItems(selectedTransaction.BookList.ToArray()); // XXX null is a dummy argument
                    showCompleteTransactionDialog.AddDisplayItems(selectedTransaction.CartTotalArray());
                    switch (showCompleteTransactionDialog.Display())
                    {
                        case DialogReturn.Remove: // transaction Remove
                            // XXX
                            _attachedControl.CompleteOrders.Remove(selectedTransaction);
                            continue;
                        case DialogReturn.Done:
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

        private void bnSave_Click(object sender, EventArgs e)
        {
            // XXX Save button handler
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "VRS Files|*.vrs";
                saveFileDialog.AddExtension = true;
                saveFileDialog.InitialDirectory = Application.StartupPath;
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                // XXX
                _attachedControl.SaveState(saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Serialization Failed" + ex.ToString());
            }
        }

        private void bnRestore_Click(object sender, EventArgs e)
        {
            // XXX Restore button handler
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "VRS Files|*.vrs";
                openFileDialog.InitialDirectory = Application.StartupPath;
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                // XXX
                _attachedControl.RestoreState(openFileDialog.FileName);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Serialization Failed" + ex.ToString());
            }
        }

        private void bnDone_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
}
