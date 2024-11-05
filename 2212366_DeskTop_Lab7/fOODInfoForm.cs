using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2212366_DeskTop_Lab7
{
    public partial class fOODInfoForm : Form
    {
        public fOODInfoForm()
        {
            InitializeComponent();
        }

        private void fOODInfoForm_Load(object sender, EventArgs e)
        {
            this.InitValues();
        }

        private void InitValues()
        {
            string connectionString = "server = PC727; database = RestaurantManagement1; Integrated Security = true;";

            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT ID, Name FROM Category";

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);  
            DataSet ds = new DataSet();

            conn.Open();

            adapter.Fill(ds, "Category");

            cbbCategoryName.DataSource = ds.Tables["Category"];
            cbbCategoryName.DisplayMember = "Name";
            cbbCategoryName.ValueMember = "ID";

            conn.Close();
            conn.Dispose();
        }

        private void ResetText()
        {
            txtFoodID.ResetText();
            txtNameFood.ResetText();
            txtNotes.ResetText();
            txtUnit.ResetText();
            cbbCategoryName.ResetText();
            txtPrice.ResetText();
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "server = PC727; database = RestaurantManagement1; Integrated Security = true;";

                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "EXECUTE InsertFood @id OUTPUT, @name, @unit, @foodCategoryID, @price, @notes";

                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 1000);
                cmd.Parameters.Add("@unit", SqlDbType.NVarChar, 100);
                cmd.Parameters.Add("@foodCategoryId", SqlDbType.Int);
                cmd.Parameters.Add("@price", SqlDbType.Int);
                cmd.Parameters.Add("@notes", SqlDbType.NVarChar, 3000);

                cmd.Parameters["@id"].Direction = ParameterDirection.Output;

                cmd.Parameters["@name"].Value = txtNameFood.Text;
                cmd.Parameters["@unit"].Value = txtUnit.Text;
                cmd.Parameters["@foodCategoryId"].Value = cbbCategoryName.SelectedValue;
                cmd.Parameters["@price"].Value = txtPrice.Text;
                cmd.Parameters["@notes"].Value = txtNotes.Text;

                conn.Open();

                int numRowAffected = cmd.ExecuteNonQuery();

                if (numRowAffected > 0)
                {
                    string foodID = cmd.Parameters["@id"].Value.ToString();
                    MessageBox.Show("Sucessfully add food, Food ID= " +  foodID, "Message");

                    this.ResetText();
                }
                else
                {
                    MessageBox.Show("Failed");
                }

                conn.Close();
                conn.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "SQL Error");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Error");

            }



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdateFood_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "server = PC727; database = RestaurantManagement1; Integrated Security = true;";

                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "EXECUTE UpdateFood @id , @name, @unit, @foodCategoryID, @price, @notes";

                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 1000);
                cmd.Parameters.Add("@unit", SqlDbType.NVarChar, 100);
                cmd.Parameters.Add("@foodCategoryId", SqlDbType.Int);
                cmd.Parameters.Add("@price", SqlDbType.Int);
                cmd.Parameters.Add("@notes", SqlDbType.NVarChar, 3000);


                cmd.Parameters["@id"].Value = int.Parse(txtFoodID.Text);
                cmd.Parameters["@name"].Value = txtNameFood.Text;
                cmd.Parameters["@unit"].Value = txtUnit.Text;
                cmd.Parameters["@foodCategoryId"].Value = cbbCategoryName.SelectedValue;
                cmd.Parameters["@price"].Value = txtPrice.Text;
                cmd.Parameters["@notes"].Value = txtNotes.Text;

                conn.Open();

                int numRowAffected = cmd.ExecuteNonQuery();

                if (numRowAffected > 0)
                {
                    ;
                    MessageBox.Show("Sucessfully update food", "Message");

                    this.ResetText();
                }
                else
                {
                    MessageBox.Show("Failed");
                }

                conn.Close();
                conn.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "SQL Error");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Error");

            }
        }

        public void DisplayFoodInfo(DataRowView rowView)
        {
            try
            {
                txtFoodID.Text = rowView["ID"].ToString();
                txtNameFood.Text = rowView["Name"].ToString();
                txtUnit.Text = rowView["Unit"].ToString();
                txtNotes.Text = rowView["Notes"].ToString();
                txtPrice.Text = rowView["Price"].ToString();

                cbbCategoryName.SelectedIndex = -1;

                for (int index = 0; index < cbbCategoryName.Items.Count; index++)
                {
                    DataRowView cat = cbbCategoryName.Items[index] as DataRowView;
                    if (cat["ID"].ToString() == rowView["FoodCategoryID"].ToString()) 
                    {
                        cbbCategoryName.SelectedIndex = index;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Error");
                this.Close();
            }
        }
    }
}
