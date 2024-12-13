
using DVLD.Properties;  
using DVLD_Buisness;   
using System;  
using System.ComponentModel;  
using System.Windows.Forms;  
using System.IO;  
using DVLD.People; 

namespace DVLD.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson _Person;

        private int  _PersonID = -1;

        public int PersonID   
        {
            get { return _PersonID; }   
        }

        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person=clsPerson.Find(PersonID);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with PersonID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
                _FillPersonInfo();
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with National No. = " + NationalNo.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
             _FillPersonInfo();
        }

        private void _LoadPersonImage()
        {
            if (_Person.Gendor == 0)
                pbPersonImage.Image = Resources.icons8_user_male_512;
            else
                pbPersonImage.Image = Resources.icons8_female_user_512;

            string ImagePath = _Person.ImagePath;
            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPersonImage.ImageLocation= ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void _FillPersonInfo()
        {
            btnEditPersonInfo.Enabled = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text=_Person.PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblFullName.Text = _Person.FullName;
            lblGendor.Text = _Person.Gendor == 0 ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblPhone.Text = _Person.Phone;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblCountry.Text= clsCountry.Find( _Person.NationalityCountryID).CountryName ;
            lblAddress.Text= _Person.Address;
            _LoadPersonImage();        
        }

        public void ResetPersonInfo()
        {
            _PersonID = -1;
            lblPersonID.Text = "N/A";
            lblNationalNo.Text = "N/A";
            lblFullName.Text = "N/A";
            lblGendor.Text = "N/A";
            lblEmail.Text = "N/A";
            lblPhone.Text = "N/A";
            lblDateOfBirth.Text = "N/A";
            lblCountry.Text = "N/A";
            lblAddress.Text = "N/A";
            pbPersonImage.Image = Resources.icons8_user_male_512;
        
        }

        private void btnEditPersonInfo_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
            frm.ShowDialog();
            LoadPersonInfo(_PersonID);
        }
    }
}
