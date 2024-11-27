using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using ProductsApp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProductsApp.Services
{
    public class ProductDAO : IProductDAO
    {
        private readonly string _connectionString;

        public ProductDAO(IConfiguration configuration)
        {
            // Get connection string from appsettings.json
            _connectionString = configuration.GetConnectionString("ProductsDatabase");
        }

        public async Task<int> AddProduct(ProductModel product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Products (Name, Price, Description, CreatedAt, ImageURL) 
                         OUTPUT INSERTED.Id 
                         VALUES (@Name, @Price, @Description, @CreatedAt, @ImageURL)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedAt", product.CreatedAt.HasValue ? product.CreatedAt.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@ImageURL", product.ImageURL ?? (object)DBNull.Value);

                conn.Open();
                int newId = (int)(await cmd.ExecuteScalarAsync() ?? 0);
                return newId;
            }
        }

        // Delete Product by ID
        public async Task DeleteProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Products WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                await cmd.ExecuteNonQueryAsync(); // Execute the delete command
            }
        }

        // Get Product by ID
        public async Task<ProductModel> GetProductById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Products WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return new ProductModel
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Price = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                        Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        CreatedAt = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4),
                        ImageURL = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                    };
                }
            }
            return null; // Return null if no product is found
        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            List<ProductModel> products = new List<ProductModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Products", conn);
                conn.Open();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    products.Add(new ProductModel
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Price = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                        Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        CreatedAt = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4),
                        ImageURL = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                    });
                }
            }
            return products;
        }

        public async Task<IEnumerable<ProductModel>> SearchForProductsByDescription(string searchTerm)
        {
            List<ProductModel> products = new List<ProductModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Products WHERE Description LIKE @SearchTerm";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                conn.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    products.Add(new ProductModel
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Price = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                        Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        CreatedAt = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4),
                        ImageURL = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                    });
                }
            }

            return products;
        }

        public async Task<IEnumerable<ProductModel>> SearchForProductsByName(string searchTerm)
        {
            List<ProductModel> products = new List<ProductModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Products WHERE Name LIKE @SearchTerm";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                conn.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    products.Add(new ProductModel
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Price = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                        Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        CreatedAt = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4),
                        ImageURL = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                    });
                }
            }

            return products;
        }

        public async Task UpdateProduct(ProductModel product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Products 
                         SET Name = @Name, Price = @Price, Description = @Description, CreatedAt = @CreatedAt, ImageURL = @ImageURL 
                         WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", product.Id);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedAt", product.CreatedAt.HasValue ? product.CreatedAt.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@ImageURL", product.ImageURL ?? (object)DBNull.Value);

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
