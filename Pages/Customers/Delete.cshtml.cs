using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace dotnetProjects.Pages.Customers
{
    public class Delete : PageModel
    {

        public void OnGet()
        {
     
        }

        public void OnPost(int id){
            deleteCustomer(id);
            Response.Redirect("/Customers/Index");
        }

        private void deleteCustomer(int id)
        {
            string connectionString = "Server=localhost;Database=testdb;User Id=SA;Password=password1234;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM customers WHERE Id=@Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public string ErrorMessage { get; set; } = "";
    }
}
