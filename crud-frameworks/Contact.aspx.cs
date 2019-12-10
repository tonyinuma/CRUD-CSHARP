using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crud_frameworks
{
    public partial class Contact : System.Web.UI.Page
    {

        SqlConnection sqlCon = new SqlConnection(@"Data Source= TONYID;Initial Catalog = CrudWeb; Integrated Security=true;");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDelete.Enabled = false;
                fillGridView();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            hfContactID.Value = "";
            txtName.Text = txtCellPhone.Text = txtAddrss.Text = "";
            lblSuccessMessage.Text = lblErrorMessage.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("ContactCreateOrUpdate", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@ContactID", (hfContactID.Value == "" ? 0 : Convert.ToInt32(hfContactID.Value)));
                sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Cellphone", txtCellPhone.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Addrss", txtAddrss.Text.Trim());
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
                Clear();
                if (hfContactID.Value == "")
                {
                    lblSuccessMessage.Text = "Saved Successfully";
                }
                else
                {
                    lblSuccessMessage.Text = "Upsated Successfully";
                }
                fillGridView();
            }            
        }

        void fillGridView()
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
                SqlDataAdapter sqlAda = new SqlDataAdapter("ContactViewAll", sqlCon);
                sqlAda.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtbl = new DataTable();
                sqlAda.Fill(dtbl);
                sqlCon.Close();
                gvContact.DataSource = dtbl;
                gvContact.DataBind();
            }

        }

        protected void lnkView_Click(object sender, EventArgs e)
        {
            int contactID = Convert.ToInt32((sender as LinkButton).CommandArgument);

            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();

                SqlDataAdapter sqlAda = new SqlDataAdapter("ContactViewByID", sqlCon);
                sqlAda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlAda.SelectCommand.Parameters.AddWithValue("@ContactID", contactID);
                DataTable dtbl = new DataTable();
                sqlAda.Fill(dtbl);
                sqlCon.Close();

                hfContactID.Value = contactID.ToString();

                txtName.Text = dtbl.Rows[0]["Name"].ToString();
                txtCellPhone.Text = dtbl.Rows[0]["CellPhone"].ToString();
                txtAddrss.Text = dtbl.Rows[0]["Addrss"].ToString();

                btnSave.Text = "Update";
                btnDelete.Enabled = true;
        }
    }
}