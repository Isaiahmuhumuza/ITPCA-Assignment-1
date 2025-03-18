using System;
using System.Data.SqlClient;


internal class Program
{
    static void Main()
    {
        string connectionString = @"Data Source = ASUS\SQLEXPRESS; Initial Catalog = FluffyBakes; Integrated Security = True; Connect Timeout = 30; Encrypt = False;";
        DatabaseHelper dbHelper = new DatabaseHelper(connectionString);

        bool state = true;

        do {
            
            Console.WriteLine("Welcome to FluffyBakes \nPick an option:\n1.Add Product\n2.Search for product\n3.Update Product Details\n4.Delete Product\n5.Exit");
            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
                    Console.WriteLine("Enter Product Name: ");
                    string productName = Console.ReadLine();

                    Console.WriteLine("Enter Product Category: ");
                    string category = Console.ReadLine();

                    Console.WriteLine("Enter Stock of Product: ");
                    int stock = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Product Price: ");
                    int price = int.Parse(Console.ReadLine());

                    if (dbHelper.ProductExists(productName))
                    {
                        Console.WriteLine("A product with this name already exists. Please re-enter the product's name.");
                        return;
                    }

                    bool success = dbHelper.InsertProduct(productName, category, stock, price);
                    Console.WriteLine(success ? "Product added successfully!\n" : "Failed to add product.\n");
                    break;
                case 2:
                    Console.WriteLine("Enter Product to search: ");
                    string productSearch = Console.ReadLine();
                    dbHelper.Search(productSearch);
                    break;
                case 3:
                    Console.WriteLine("Enter product name to update:");
                    string productUpdate = Console.ReadLine();

                    Console.WriteLine("Enter new category:");
                    string newCategory = Console.ReadLine();

                    Console.WriteLine("Enter new stock quantity:");
                    int newStock = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter new price:");
                    int newPrice = int.Parse(Console.ReadLine());

                    bool updated = dbHelper.UpdateProduct(productUpdate, newCategory, newStock, newPrice);

                    if (!updated)
                    {
                        Console.WriteLine("Update operation failed. Please check product name.\n");
                    }
                    break;
                case 4:
                    Console.WriteLine("Enter product name to delete:");
                    string productDelete = Console.ReadLine();

                    bool deleted = dbHelper.DeleteProduct(productDelete);

                    if (!deleted)
                    {
                        Console.WriteLine("Delete operation failed. Please check product name.\n");
                    }

                    break;
                case 5:
                    Console.WriteLine("Exiting Program. Goodbye!");
                    state = false;
                    break;
            }
        } while (state);
    }
}

public class DatabaseHelper
{
    private string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public bool ProductExists(string productName)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            string checkQuery = "SELECT COUNT(*) FROM Products WHERE productName = @productName";
            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@productName", productName);
                int count = (int)checkCmd.ExecuteScalar();
                return count > 0;
            }
        }
    }

    public bool InsertProduct(string productName, string category, int stock, int price)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            string insertQuery = "INSERT INTO Products (productName, category, quantityInStock, price) VALUES (@productName, @category, @quantityInStock, @price)";
            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@productName", productName);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@quantityInStock", stock);
                cmd.Parameters.AddWithValue("@price", price);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool Search(string productName)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            string searchQuery = "SELECT productName, category, quantityInStock, price FROM Products WHERE productName = @productName";

            using (SqlCommand cmd = new SqlCommand(searchQuery, conn))
            {
                cmd.Parameters.AddWithValue("@productName", productName);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"PRODUCT DETAILS \n\nProdcut Name: {reader.GetString(0)}\nCategory: {reader.GetString(1)}\nQuantity: {reader.GetInt32(2)}\nPrice: {reader.GetDecimal(3)}\n");
                        }
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Product not found");
                        return false;
                    }
                }
            }
        }
    }

    public bool UpdateProduct(string productName, string newCategory, int newStock, int newPrice)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            string checkQuery = "SELECT COUNT(*) FROM Products WHERE productName = @productName";
            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@productName", productName);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    Console.WriteLine("Product not found. Update failed.");
                    return false;  // Product does not exist
                }
            }

            string updateQuery = "UPDATE Products SET category = @category, quantityInStock = @stock, price = @price WHERE productName = @productName";

            using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
            {
                cmd.Parameters.AddWithValue("@productName", productName);
                cmd.Parameters.AddWithValue("@category", newCategory);
                cmd.Parameters.AddWithValue("@stock", newStock);
                cmd.Parameters.AddWithValue("@price", newPrice);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Product updated successfully!\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to update product.");
                    return false;
                }
            }
        }
    }

    public bool DeleteProduct(string productName)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            string checkQuery = "SELECT COUNT(*) FROM Products WHERE productName = @productName";
            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@productName", productName);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    Console.WriteLine("Product not found. Delete failed.");
                    return false;  
                }
            }

            
            string deleteQuery = "DELETE FROM Products WHERE productName = @productName";

            using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
            {
                cmd.Parameters.AddWithValue("@productName", productName);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Product deleted successfully!\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to delete product.");
                    return false;
                }
            }
        }
    }
}