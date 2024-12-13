using DVLD_Buisness;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD.User
{
    public partial class frmListUsers : Form
    {
        private static DataTable _dtAllUsers ;

        int _UserID = -1;
        public frmListUsers()
        {
            InitializeComponent();
        }
        private bool _checkUserID()
        {
            if (_UserID == -1)
            {
                MessageBox.Show("Select a User from the table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }
            return false;
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _dtAllUsers = clsUser.GetAllUsers();
            dgvPeople.DataSource = _dtAllUsers;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();  
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible= false;
                cbIsActive.Visible = true;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
            } 
            
            else

            {
                
                txtFilterValue.Visible = (cbFilterBy.Text !="None") ;
                cbIsActive.Visible = false;

                if (cbFilterBy.Text == "None")
                {
                    txtFilterValue.Enabled = false;
                }
                else
                    txtFilterValue.Enabled = true;

                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

           
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "UserName":
                    FilterColumn = "UserName";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

        
                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
                return;
            }


            if (FilterColumn != "FullName" && FilterColumn != "UserName")
                //in this case we deal with numbers not string.
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {

             
          string FilterColumn = "IsActive";
          string FilterValue =cbIsActive.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0"; 
                    break;
            }


            if (FilterValue == "All")
                _dtAllUsers.DefaultView.RowFilter = "";  
            else
                //in this case we deal with numbers not string.
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString();


        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser Frm1 = new frmAddUpdateUser ();
            Frm1.ShowDialog();
            frmListUsers_Load(null, null);  
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_checkUserID())
                return;

            frmAddUpdateUser Frm1 = new frmAddUpdateUser(_UserID);
            Frm1.ShowDialog();
            frmListUsers_Load(null, null);

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_checkUserID())
                return;

            frmUserInfo Frm1 = new frmUserInfo(_UserID);
            Frm1.ShowDialog();
           
        }

        private void ChangePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_checkUserID())
                return;

            frmChangePassword Frm1 = new frmChangePassword(_UserID);
            Frm1.ShowDialog();

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id or user id is selected.
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_checkUserID())
                return;

            if (MessageBox.Show($"are you sure you want delete this {_UserID}", "delete user", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (clsUser.DeleteUser(_UserID))
                {
                    MessageBox.Show("User has been deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListUsers_Load(null, null);
                }

                else
                    MessageBox.Show("User is not delted due to data connected to it.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void dgvPeople_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            _UserID = (int)dgvPeople.CurrentRow.Cells[0].Value;
        }
    }
}
