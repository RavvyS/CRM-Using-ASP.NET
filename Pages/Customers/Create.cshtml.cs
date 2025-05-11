using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations; // Add this namespace for [Required] attribute

namespace dotnetProjects.Pages.Customers
{
    public class Create : PageModel
    {
        [BindProperty]
        public CustomerModel Customer { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Your form submission handling code here

            return RedirectToPage("./Index");
        }
    }

    public class CustomerModel
    {
        public int Id { get; set; }

        [BindProperty, Required(ErrorMessage = "First name is required")]
        public string Firstname { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Last name is required")]
        public string Lastname { get; set; } = "";

        [BindProperty,Required, EmailAddress]
        
        public string Email { get; set; } = "";

[BindProperty, Required]
        public string? Phone { get; set; } = "";

        [BindProperty, Required]
        public string? Address { get; set; } = "";

        [BindProperty, Required]
        public string Company { get; set; } = "";

 

        [Required(ErrorMessage = "Notes are required")]
        public string? Notes { get; set; } = "";

       
       
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public void OnPost (){
        if(!ModelState.IsValid)
        {
            // Handle invalid model state
            return;
        }

        if (Phone ==null)Phone = "";
        if (Address == null) Address = "";
        if (Notes == null) Notes = "";
        try{
            string connectionString = "Server=localhost;Database=testdb;User Id=SA;Password=password1234;TrustServerCertificate=True;";
            using (Sql)
        }catch(Exception ex)
        {
            // Handle exception
            ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            return;

        }
    }
    
    }
}