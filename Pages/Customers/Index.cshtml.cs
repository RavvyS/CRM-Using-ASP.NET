using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace dotnetProjects.Pages.Customers
{
    public class Index : PageModel
    {
        public List<CustomerInfo> CustomersList { get; set; } = [];

        public void OnGet()
        {
            try
            {
                // Update connection string for Mac environment
                // Replace with your actual server, username, and password
                string connectionString = "Server=localhost;Database=testdb;User Id=SA;Password=password1234;TrustServerCertificate=True;";
                //string connectionString = "Server=.;Database=testdb;Trusted_connection=True;TrustServerCertificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM customers ORDER BY Id DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerInfo customerInfo = new CustomerInfo();
                                customerInfo.Id = reader.GetInt32(0);
                                customerInfo.Firstname = reader.GetString(1);
                                customerInfo.Lastname = reader.GetString(2);
                                customerInfo.Email = reader.GetString(3);
                                customerInfo.Phone = reader.GetString(4);
                                customerInfo.Address = reader.GetString(5);
                                customerInfo.Company = reader.GetString(6);
                                customerInfo.Notes = reader.GetString(7);
                                customerInfo.CreatedAt = reader.GetDateTime(8).ToString("MM/dd/yyyy");

                                CustomersList.Add(customerInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Error fetching customer data: {ex.Message}");
                // Make error visible in the UI
                ModelState.AddModelError(string.Empty, $"Error fetching data: {ex.Message}");
            }
        }
    }

    public class CustomerInfo
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string Company { get; set; } = "";
        public string Notes { get; set; } = "";
        public string CreatedAt { get; set; } = "";
    }
}