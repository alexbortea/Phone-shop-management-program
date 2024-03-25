using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProiectBD
{
    public partial class Form2 : Form
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-ON8J59B;Initial Catalog=Magazin;Integrated Security=True");
        private int selectedProductId;
        private int selectedReviewId;

        public Form2()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("C:/Users/Alex/source/repos/ProiectBD/pisica.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            FILLDGV();
            dataGridViewProduse.SelectionChanged += dataGridViewProduse_SelectionChanged;
            dataGridViewRecenzii.SelectionChanged += dataGridViewRecenzii_SelectionChanged;
        }

        private void FILLDGV()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                string query = "Select * From Produse";
                String query2 = "Select * From Recenzii";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                SqlDataAdapter da2 = new SqlDataAdapter(query2, con);
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                da.Fill(dt);
                da2.Fill(dt2);
                dataGridViewProduse.DataSource = dt;
                dataGridViewRecenzii.DataSource = dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        private void ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Form prompt = new Form();
            prompt.Width = 500; // Ajustează lățimea conform preferințelor tale
            prompt.Height = 200; // Ajustează înălțimea conform preferințelor tale
            prompt.Text = title;

            Label textLabel = new Label() { Left = 50, Top = 20, Text = message };
            prompt.Controls.Add(textLabel);

            Button confirmation = new Button() { Text = "OK", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);

            prompt.ShowDialog();
        }
        private void dataGridViewProduse_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewProduse.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewProduse.SelectedRows[0];
                textBox2.Text = selectedRow.Cells["Nume_Produs"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Descriere"].Value.ToString();
                textBox4.Text = selectedRow.Cells["pret"].Value.ToString();
                textBox5.Text = selectedRow.Cells["Stoc_Disponibil"].Value.ToString();
                textBox6.Text = selectedRow.Cells["Specificatii"].Value.ToString();
                textBox7.Text = selectedRow.Cells["Id_Categorie"].Value.ToString();
                selectedProductId = Convert.ToInt32(selectedRow.Cells["Id_Produs"].Value);
            }
        }

        private void dataGridViewRecenzii_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewRecenzii.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewRecenzii.SelectedRows[0];
                textBox8.Text = selectedRow.Cells["Id_Produs"].Value.ToString();
                textBox9.Text = selectedRow.Cells["Id_Client"].Value.ToString();
                textBox10.Text = selectedRow.Cells["Nota"].Value.ToString();
                textBox11.Text = selectedRow.Cells["Comentariu"].Value.ToString();
                textBox12.Text = selectedRow.Cells["Data_Recenziei"].Value.ToString();
                selectedReviewId = Convert.ToInt32(selectedRow.Cells["Id_Recenzie"].Value);
            }
        }

        private void btnInsertProduse_Click_1(object sender, EventArgs e)
        {
            InsertProduse();
        }

        private void InsertProduse()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                string Nume_Produs = textBox2.Text;
                string Descriere = textBox3.Text;
                int pret = Convert.ToInt32(textBox4.Text);
                int Stoc_Disponibil = Convert.ToInt32(textBox5.Text);
                string Specificatii = textBox6.Text;
                int Id_Categorie = Convert.ToInt32(textBox7.Text);

                string insertQuery = "INSERT INTO Produse (Nume_Produs, Descriere, pret, Stoc_Disponibil, Specificatii, Id_Categorie) VALUES (@Nume_Produs, @Descriere, @Pret, @Stoc_Disponibil, @Specificatii, @Id_Categorie)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Nume_Produs", Nume_Produs);
                    cmd.Parameters.AddWithValue("@Descriere", Descriere);
                    cmd.Parameters.AddWithValue("@Pret", pret);
                    cmd.Parameters.AddWithValue("@Stoc_Disponibil", Stoc_Disponibil);
                    cmd.Parameters.AddWithValue("@Specificatii", Specificatii);
                    cmd.Parameters.AddWithValue("@Id_Categorie", Id_Categorie);

                    cmd.ExecuteNonQuery();
                }

                FILLDGV();

                MessageBox.Show("Product inserted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnModifyProduse_Click(object sender, EventArgs e)
        {
            ModifyProduse();
        }

        private void ModifyProduse()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                string modifiedProductName = textBox2.Text;
                string modifiedDescription = textBox3.Text;
                int modifiedPrice = Convert.ToInt32(textBox4.Text);
                int modifiedStock = Convert.ToInt32(textBox5.Text);
                string modifiedSpecifications = textBox6.Text;
                int modifiedCategoryId = Convert.ToInt32(textBox7.Text);

                string updateQuery = "UPDATE Produse SET Nume_Produs = @ModifiedProductName, Descriere = @ModifiedDescription, pret = @ModifiedPrice, Stoc_Disponibil = @ModifiedStock, Specificatii = @ModifiedSpecifications, Id_Categorie = @ModifiedCategoryId WHERE Id_Produs = @ProductIdToModify";

                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {
                    cmd.Parameters.AddWithValue("@ModifiedProductName", modifiedProductName);
                    cmd.Parameters.AddWithValue("@ModifiedDescription", modifiedDescription);
                    cmd.Parameters.AddWithValue("@ModifiedPrice", modifiedPrice);
                    cmd.Parameters.AddWithValue("@ModifiedStock", modifiedStock);
                    cmd.Parameters.AddWithValue("@ModifiedSpecifications", modifiedSpecifications);
                    cmd.Parameters.AddWithValue("@ModifiedCategoryId", modifiedCategoryId);
                    cmd.Parameters.AddWithValue("@ProductIdToModify", selectedProductId);

                    cmd.ExecuteNonQuery();
                }

                FILLDGV();

                MessageBox.Show("Product modified successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnInsertRecenzii_Click(object sender, EventArgs e)
        {
            InsertRecenzii();
        }

        private void InsertRecenzii()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                int Id_Produs = Convert.ToInt32(textBox8.Text);
                int Id_Client = Convert.ToInt32(textBox9.Text);
                int Nota = Convert.ToInt32(textBox10.Text);
                string Comentariu = textBox11.Text;
                DateTime Data_Recenziei = DateTime.Parse(textBox12.Text);

                string insertQuery = "INSERT INTO Recenzii (Id_Produs, Id_Client, Nota, Comentariu, Data_Recenziei) VALUES (@Id_Produs, @Id_Client, @Nota, @Comentariu, @Data_Recenziei)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Id_Produs", Id_Produs);
                    cmd.Parameters.AddWithValue("@Id_Client", Id_Client);
                    cmd.Parameters.AddWithValue("@Nota", Nota);
                    cmd.Parameters.AddWithValue("@Comentariu", Comentariu);
                    cmd.Parameters.AddWithValue("@Data_Recenziei", Data_Recenziei);

                    cmd.ExecuteNonQuery();
                }

                FILLDGV();

                MessageBox.Show("Review inserted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnModifyRecenzii_Click(object sender, EventArgs e)
        {
            ModifyRecenzii();
        }

        private void ModifyRecenzii()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                int modifiedProductId = Convert.ToInt32(textBox8.Text);
                int modifiedClientId = Convert.ToInt32(textBox9.Text);
                int modifiedRating = Convert.ToInt32(textBox10.Text);
                string modifiedComment = textBox11.Text;
                DateTime modifiedReviewDate = DateTime.Parse(textBox12.Text);

                string updateQuery = "UPDATE Recenzii SET Id_Produs = @ModifiedProductId, Id_Client = @ModifiedClientId, Nota = @ModifiedRating, Comentariu = @ModifiedComment, Data_Recenziei = @ModifiedReviewDate WHERE Id_Recenzie = @ReviewIdToModify";

                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {
                    cmd.Parameters.AddWithValue("@ModifiedProductId", modifiedProductId);
                    cmd.Parameters.AddWithValue("@ModifiedClientId", modifiedClientId);
                    cmd.Parameters.AddWithValue("@ModifiedRating", modifiedRating);
                    cmd.Parameters.AddWithValue("@ModifiedComment", modifiedComment);
                    cmd.Parameters.AddWithValue("@ModifiedReviewDate", modifiedReviewDate);
                    cmd.Parameters.AddWithValue("@ReviewIdToModify", selectedReviewId);

                    cmd.ExecuteNonQuery();
                }

                FILLDGV();

                MessageBox.Show("Review modified successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnDeleteProduse_Click(object sender, EventArgs e)
        {
            DeleteProduse();
        }

        private void DeleteProduse()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                if (selectedProductId != 0)
                {
                    // First, delete associated reviews
                    string deleteReviewsQuery = "DELETE FROM Recenzii WHERE Id_Produs = @ProductIdToDelete";

                    using (SqlCommand cmdReviews = new SqlCommand(deleteReviewsQuery, con))
                    {
                        cmdReviews.Parameters.AddWithValue("@ProductIdToDelete", selectedProductId);
                        cmdReviews.ExecuteNonQuery();
                    }

                    // Second, delete associated orders details
                    string deleteDetaliiComandaQuery = "DELETE FROM Detalii_comanda WHERE Id_Produs = @ProductIdToDelete";

                    using (SqlCommand cmdDetaliiComanda = new SqlCommand(deleteDetaliiComandaQuery, con))
                    {
                        cmdDetaliiComanda.Parameters.AddWithValue("@ProductIdToDelete", selectedProductId);
                        cmdDetaliiComanda.ExecuteNonQuery();
                    }

                    // Third, delete the product
                    string deleteProductQuery = "DELETE FROM Produse WHERE Id_Produs = @ProductIdToDelete";

                    using (SqlCommand cmdProduct = new SqlCommand(deleteProductQuery, con))
                    {
                        cmdProduct.Parameters.AddWithValue("@ProductIdToDelete", selectedProductId);
                        cmdProduct.ExecuteNonQuery();
                    }

                    FILLDGV();

                    MessageBox.Show("Product, associated reviews, and order details deleted successfully!");
                }
                else
                {
                    MessageBox.Show("Please select a product to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnDeleteRecenzii_Click(object sender, EventArgs e)
        {
            DeleteRecenzii();
        }

        private void DeleteRecenzii()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                if (selectedReviewId != 0)
                {
                    // Delete the review
                    string deleteReviewQuery = "DELETE FROM Recenzii WHERE Id_Recenzie = @ReviewIdToDelete";

                    using (SqlCommand cmdReview = new SqlCommand(deleteReviewQuery, con))
                    {
                        cmdReview.Parameters.AddWithValue("@ReviewIdToDelete", selectedReviewId);
                        cmdReview.ExecuteNonQuery();
                    }

                    FILLDGV();

                    MessageBox.Show("Review deleted successfully!");
                }
                else
                {
                    MessageBox.Show("Please select a review to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnExecuteQuery_Click(object sender, EventArgs e)
        {
            ExecuteQueryAndShowResults();
        }

        private void ExecuteQueryAndShowResults()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Your query
                string query = "SELECT Categorii_de_Produse.Nume_Categorie, COUNT(Produse.Id_Produs) AS Total_Products " +
                                "FROM Categorii_de_Produse " +
                                "LEFT JOIN Produse ON Categorii_de_Produse.Id_Categorie = Produse.Id_Categorie " +
                                "GROUP BY Categorii_de_Produse.Nume_Categorie";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display results in a MessageBox
                string message = "Numar Produse Pe Categorii\n";
                foreach (DataRow row in dt.Rows)
                {
                    message += $"{row["Nume_Categorie"]}\t{row["Total_Products"]}\n";
                }

                MessageBox.Show(message, "Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnExecuteQuery2_Click(object sender, EventArgs e)
        {
            ExecuteQuery2AndShowResults();
        }

        private void ExecuteQuery2AndShowResults()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Your query
                string query = "SELECT Clienti.Id_Client, Clienti.Nume, Clienti.Prenume, COUNT(Comanda.Id_Comanda) AS Total_Orders " +
                                "FROM Clienti " +
                                "LEFT JOIN Comanda ON Clienti.Id_Client = Comanda.Id_Client " +
                                "GROUP BY Clienti.Id_Client, Clienti.Nume, Clienti.Prenume";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display results in a MessageBox
                string message = "Client ID\tName\tPrenume\tNr. Comenzi\n";
                foreach (DataRow row in dt.Rows)
                {
                    message += $"{row["Id_Client"]}\t{row["Nume"]}\t{row["Prenume"]}\t{row["Total_Orders"]}\n";
                }

                MessageBox.Show(message, "Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnExecuteQuery3_Click(object sender, EventArgs e)
        {
            ExecuteQuery3AndShowResults();
        }

        private void ExecuteQuery3AndShowResults()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Your query
                string query = "SELECT Produse.Nume_Produs, COALESCE(AVG(CAST(Recenzii.Nota AS FLOAT)), 0) AS Average_Rating " +
                                "FROM Produse " +
                                "LEFT JOIN Recenzii ON Produse.Id_Produs = Recenzii.Id_Produs " +
                                "GROUP BY Produse.Nume_Produs";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display results in a MessageBox with each product and its rating vertically
                StringBuilder message = new StringBuilder();
                foreach (DataRow row in dt.Rows)
                {
                    string productName = row["Nume_Produs"].ToString();
                    string averageRating = row["Average_Rating"].ToString();
                    message.AppendLine($"{productName}: {averageRating}");
                }

                MessageBox.Show(message.ToString(), "Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnExecuteQuery4_Click(object sender, EventArgs e)
        {
            ExecuteQuery4AndShowResults();
        }

        private void ExecuteQuery4AndShowResults()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Your query
                string query = "SELECT Clienti.Id_Client, Clienti.Nume, Clienti.Prenume, SUM(Comanda.Total_Comanda) AS Total_Spent " +
                                "FROM Clienti " +
                                "JOIN Comanda ON Clienti.Id_Client = Comanda.Id_Client " +
                                "GROUP BY Clienti.Id_Client, Clienti.Nume, Clienti.Prenume";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display results in a MessageBox
                StringBuilder message = new StringBuilder();
                foreach (DataRow row in dt.Rows)
                {
                    string clientId = row["Id_Client"].ToString();
                    string clientName = row["Nume"].ToString();
                    string clientSurname = row["Prenume"].ToString();
                    string totalSpent = row["Total_Spent"].ToString();
                    message.AppendLine($"{clientId}\t{clientName}\t{clientSurname}\t{totalSpent}");
                }

                MessageBox.Show(message.ToString(), "Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnExecuteQuery5_Click(object sender, EventArgs e)
        {
            ExecuteQuery5AndShowResults();
        }

        private void ExecuteQuery5AndShowResults()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Your query
                string query = "SELECT TOP 5 Produse.Nume_Produs, COALESCE(AVG(CAST(Recenzii.Nota AS FLOAT)), 0) AS Average_Rating " +
                                "FROM Produse " +
                                "LEFT JOIN Recenzii ON Produse.Id_Produs = Recenzii.Id_Produs " +
                                "GROUP BY Produse.Nume_Produs " +
                                "ORDER BY Average_Rating DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display results in a MessageBox with each product and its rating vertically
                StringBuilder message = new StringBuilder();
                foreach (DataRow row in dt.Rows)
                {
                    string productName = row["Nume_Produs"].ToString();
                    string averageRating = row["Average_Rating"].ToString();
                    message.AppendLine($"{productName}: {averageRating}");
                }

                MessageBox.Show(message.ToString(), "Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnExecuteQuery6_Click(object sender, EventArgs e)
        {
            ExecuteQuery6AndShowResults();
        }

        private void ExecuteQuery6AndShowResults()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Your query
                string query = "WITH LatestReviews AS (" +
                                "   SELECT Id_Produs, MAX(Data_Recenziei) AS LatestReviewDate " +
                                "   FROM Recenzii " +
                                "   GROUP BY Id_Produs" +
                                ") " +
                                "SELECT Produse.Nume_Produs, Clienti.Nume AS Nume_Client, Clienti.Prenume AS Prenume_Client, Recenzii.Nota, Recenzii.Comentariu, Recenzii.Data_Recenziei " +
                                "FROM LatestReviews " +
                                "JOIN Recenzii ON LatestReviews.Id_Produs = Recenzii.Id_Produs AND LatestReviews.LatestReviewDate = Recenzii.Data_Recenziei " +
                                "JOIN Produse ON Recenzii.Id_Produs = Produse.Id_Produs " +
                                "JOIN Clienti ON Recenzii.Id_Client = Clienti.Id_Client";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display results in a MessageBox
                if (dt.Rows.Count > 0)
                {
                    StringBuilder message = new StringBuilder("Ultimul review despre fiecare produs:\n");
                    foreach (DataRow row in dt.Rows)
                    {
                        string productName = row["Nume_Produs"].ToString();
                        string clientName = row["Nume_Client"].ToString();
                        string clientSurname = row["Prenume_Client"].ToString();
                        string rating = row["Nota"].ToString();
                        string comment = row["Comentariu"].ToString();
                        string reviewDate = row["Data_Recenziei"].ToString();

                        message.AppendLine($"Product: {productName}\nCustomer: {clientName} {clientSurname}\nRating: {rating}\nComment: {comment}\nDate: {reviewDate}\n");
                    }

                    MessageBox.Show(message.ToString(), "Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No reviews found.", "Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnExecuteComplexQuery1_Click(object sender, EventArgs e)
        {
            ExecuteComplexQuery1AndShowResults();
        }

        private void ExecuteComplexQuery1AndShowResults()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Your query
                string query = "SELECT Produse.Nume_Produs " +
                                "FROM Produse " +
                                "WHERE NOT EXISTS (SELECT 1 FROM Recenzii WHERE Produse.Id_Produs = Recenzii.Id_Produs)";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display results in a MessageBox
                StringBuilder message = new StringBuilder();
                foreach (DataRow row in dt.Rows)
                {
                    string productName = row["Nume_Produs"].ToString();
                    message.AppendLine($"{productName}");
                }

                MessageBox.Show(message.ToString(), "Complex Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnExecuteComplexQuery2_Click(object sender, EventArgs e)
        {
            ExecuteComplexQuery2AndShowResults();
        }

        private void ExecuteComplexQuery2AndShowResults()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Your query
                string query = "SELECT Comanda.Id_Comanda, Comanda.Data_Comenzii, Comanda.Stare_Comanda, Comanda.Total_Comanda " +
                                "FROM Comanda " +
                                "JOIN Clienti ON Comanda.Id_Client = Clienti.Id_Client " +
                                "WHERE Clienti.Id_Client IN (SELECT TOP 1 Id_Client FROM Recenzii GROUP BY Id_Client ORDER BY AVG(CAST(Nota AS FLOAT)) DESC)";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Create a new form to display results
                Form resultForm = new Form();
                resultForm.Text = "Query Results";
                resultForm.Size = new Size(600, 400);

                RichTextBox richTextBoxResults = new RichTextBox();
                richTextBoxResults.Dock = DockStyle.Fill;
                resultForm.Controls.Add(richTextBoxResults);

                // Display results in a RichTextBox
                StringBuilder message = new StringBuilder();
                foreach (DataRow row in dt.Rows)
                {
                    string orderId = row["Id_Comanda"].ToString();
                    string orderDate = row["Data_Comenzii"].ToString();
                    string orderStatus = row["Stare_Comanda"].ToString();
                    string orderTotal = row["Total_Comanda"].ToString();
                    message.AppendLine($"Order ID: {orderId}\tDate: {orderDate}\tStatus: {orderStatus}\tTotal: {orderTotal}");
                }

                richTextBoxResults.Text = message.ToString();

                resultForm.ShowDialog(); // Show the form as a dialog
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        private string GetDataTableAsString(DataTable dataTable)
{
    StringBuilder result = new StringBuilder();

    // Append column names
    foreach (DataColumn column in dataTable.Columns)
    {
        result.Append(column.ColumnName).Append("\t");
    }
    result.AppendLine();

    // Append data rows
    foreach (DataRow row in dataTable.Rows)
    {
        foreach (var item in row.ItemArray)
        {
            result.Append(item).Append("\t");
        }
        result.AppendLine();
    }

    return result.ToString();
}

        private void btnExecuteComplexQuery3_Click(object sender, EventArgs e)
        {
            ExecuteComplexQuery3AndShowResults();
        }

        private void ExecuteComplexQuery3AndShowResults()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Your modified query with a subquery
                string query = "SELECT Nume_Produs, AverageRating " +
                               "FROM (" +
                               "   SELECT Produse.Nume_Produs, AVG(Recenzii.Nota) AS AverageRating " +
                               "   FROM Produse " +
                               "   LEFT JOIN Recenzii ON Produse.Id_Produs = Recenzii.Id_Produs " +
                               "   GROUP BY Produse.Nume_Produs" +
                               ") AS SubqueryResult " +
                               "WHERE AverageRating > 4.5";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display results in a RichTextBox
                if (dt.Rows.Count > 0)
                {
                    StringBuilder message = new StringBuilder("Products with Average Rating > 4.5:\n");
                    foreach (DataRow row in dt.Rows)
                    {
                        string productName = row["Nume_Produs"].ToString();
                        string averageRating = row["AverageRating"].ToString();
                        message.AppendLine($"{productName}: {averageRating}");
                    }

                    MessageBox.Show(message.ToString(), "Complex Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No products found with an average rating greater than 4.5.", "Complex Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void btnExecuteComplexQuery4_Click(object sender, EventArgs e)
        {
            ExecuteComplexQuery4AndShowResults();
        }

        private void ExecuteComplexQuery4AndShowResults()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Your query
                string query = "SELECT Id_Client, Nume, Prenume " +
                               "FROM Clienti " +
                               "WHERE Id_Client IN (" +
                               "    SELECT DISTINCT c.Id_Client " +
                               "    FROM Comanda c " +
                               "    JOIN Detalii_comanda dc ON c.Id_Comanda = dc.Id_Comanda " +
                               "    JOIN Produse p ON dc.Id_Produs = p.Id_Produs " +
                               "    LEFT JOIN Recenzii r ON p.Id_Produs = r.Id_Produs " +
                               "    GROUP BY c.Id_Client " +
                               "    HAVING AVG(CAST(r.Nota AS FLOAT)) > 4" +
                               ")";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display results in a RichTextBox
                if (dt.Rows.Count > 0)
                {
                    StringBuilder message = new StringBuilder("Clients with Average Rating > 4:\n");
                    foreach (DataRow row in dt.Rows)
                    {
                        string clientId = row["Id_Client"].ToString();
                        string clientName = row["Nume"].ToString();
                        string clientSurname = row["Prenume"].ToString();
                        message.AppendLine($"Client ID: {clientId}\tName: {clientName}\tSurname: {clientSurname}");
                    }

                    MessageBox.Show(message.ToString(), "Complex Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No clients found with an average rating greater than 4.", "Complex Query Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

    }
}
