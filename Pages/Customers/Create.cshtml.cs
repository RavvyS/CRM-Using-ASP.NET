using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;  // Add this for SqlConnection and SqlCommand

namespace dotnetProjects.Pages.Customers
{
    public class Create : PageModel
    {
        [BindProperty]
        public CustomerModel Customer { get; set; } = new CustomerModel();  // Initialize to fix CS8618
        
        public string ErrorMessage { get; set; } = "";  // Added for your Razor view

        public void OnGet()
        {
            // Initialize empty customer object
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fix the errors in the form.";
                return Page();
            }

            if (Customer.Phone == null) Customer.Phone = "";
            if (Customer.Address == null) Customer.Address = "";
            if (Customer.Notes == null) Customer.Notes = "";
            
            try
            {
                string connectionString = "Server=localhost;Database=testdb;User Id=SA;Password=password1234;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string sql = "INSERT INTO customers " +
                                "(Firstname, Lastname, Email, Phone, Address, Company, Notes) VALUES " +
                                "(@Firstname, @Lastname, @Email, @Phone, @Address, @Company, @Notes);";
                    
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Firstname", Customer.Firstname);
                        command.Parameters.AddWithValue("@Lastname", Customer.Lastname);
                        command.Parameters.AddWithValue("@Email", Customer.Email);
                        command.Parameters.AddWithValue("@Phone", Customer.Phone);
                        command.Parameters.AddWithValue("@Address", Customer.Address);
                        command.Parameters.AddWithValue("@Company", Customer.Company);
                        command.Parameters.AddWithValue("@Notes", Customer.Notes);
                        command.Parameters.AddWithValue("@CreatedAt", Customer.CreatedAt);
                        
                        command.ExecuteNonQuery();
                    }
                }
                
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Handle exception
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                ErrorMessage = $"Error creating customer: {ex.Message}";
                return Page();
            }
        }
    }

    public class CustomerModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string Firstname { get; set; } = "";

        [Required(ErrorMessage = "Last name is required")]
        public string Lastname { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        public string? Phone { get; set; } = "";
        public string? Address { get; set; } = "";
        
        [Required]
        public string Company { get; set; } = "";
        
        public string? Notes { get; set; } = "";
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}