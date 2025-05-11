using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations; // Add this namespace for validation attributes

namespace dotnetProjects.Pages.Customers
{
    public class Edit : PageModel
    {
        [BindProperty]
        public CustomerEditModel Customer { get; set; } = new CustomerEditModel();
        
        public string ErrorMessage { get; set; } = "";
        
        // Method to handle initial page load with ID parameter
        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }
            
            try
            {
                string connectionString = "Server=localhost;Database=testdb;User Id=SA;Password=password1234;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM customers WHERE Id=@Id";
                    
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Customer.Id = reader.GetInt32(0);
                                Customer.Firstname = reader.GetString(1);
                                Customer.Lastname = reader.GetString(2);
                                Customer.Email = reader.GetString(3);
                                Customer.Phone = reader.GetString(4);
                                Customer.Address = reader.GetString(5);
                                Customer.Company = reader.GetString(6);
                                Customer.Notes = reader.GetString(7);
                                // No need to read CreatedAt as we don't typically update it
                            }
                            else
                            {
                                return RedirectToPage("./Index");
                            }
                        }
                    }
                }
                
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading customer: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }
        
        // Method to handle form submission
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            try
            {
                string connectionString = "Server=localhost;Database=testdb;User Id=SA;Password=password1234;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string sql = "UPDATE customers SET " +
                                "Firstname = @Firstname, " +
                                "Lastname = @Lastname, " +
                                "Email = @Email, " +
                                "Phone = @Phone, " +
                                "Address = @Address, " +
                                "Company = @Company, " +
                                "Notes = @Notes " +
                                "WHERE Id = @Id";
                    
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Customer.Id);
                        command.Parameters.AddWithValue("@Firstname", Customer.Firstname);
                        command.Parameters.AddWithValue("@Lastname", Customer.Lastname);
                        command.Parameters.AddWithValue("@Email", Customer.Email);
                        command.Parameters.AddWithValue("@Phone", Customer.Phone ?? "");
                        command.Parameters.AddWithValue("@Address", Customer.Address ?? "");
                        command.Parameters.AddWithValue("@Company", Customer.Company);
                        command.Parameters.AddWithValue("@Notes", Customer.Notes ?? "");
                        
                        command.ExecuteNonQuery();
                    }
                }
                
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                ErrorMessage = $"Error updating customer: {ex.Message}";
                return Page();
            }
        }
    }
    
    public class CustomerEditModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "First name is required")]
        public string Firstname { get; set; } = "";
        
        [Required(ErrorMessage = "Last name is required")]
        public string Lastname { get; set; } = "";
        
        [Required, EmailAddress]
        public string Email { get; set; } = "";
        
        public string? Phone { get; set; }
        public string? Address { get; set; }
        
        [Required]
        public string Company { get; set; } = "";
        
        public string? Notes { get; set; }
    }
}