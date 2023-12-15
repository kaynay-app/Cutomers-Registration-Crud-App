using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Cutomers_Registration_Crud_App.Pages.Customers
{
	public class CreateModel : PageModel
	{
		public CustomerInfo customerInfo = new CustomerInfo();
		public String errorMessage = "";
		public String successMessage = "";


		public void OnGet()
		{
		}

		public void OnPost()
		{
			try
			{
				customerInfo = new CustomerInfo
				{
					name = Request.Form["name"],
					address = Request.Form["address"],
					city = Request.Form["city"],
					state = Request.Form["state"],
					zip = Request.Form["zip"]
				};

				if (string.IsNullOrEmpty(customerInfo.name) || string.IsNullOrEmpty(customerInfo.address) ||
					string.IsNullOrEmpty(customerInfo.city) || string.IsNullOrEmpty(customerInfo.state) ||
					string.IsNullOrEmpty(customerInfo.zip))
				{
					errorMessage = "Please fill in all fields.";
					return;
				}

				// Save the new customer into the database
				try
				{
					String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=CustomersDb;Integrated Security=True";
					using(SqlConnection connection = new SqlConnection(connectionString))
					{
						connection.Open();
						String sql = "INSERT INTO CustomersDb " +
									 "(name, address, city, state, zip) VALUES " +
									 "(@name, @address, @city, @state, @zip);";


						using(SqlCommand command = new SqlCommand(sql, connection))
						{
							command.Parameters.AddWithValue("@name", customerInfo.name);
							command.Parameters.AddWithValue("@address", customerInfo.address);
							command.Parameters.AddWithValue("@city", customerInfo.city);
							command.Parameters.AddWithValue("@state", customerInfo.state);
							command.Parameters.AddWithValue("@zip", customerInfo.zip);

							command.ExecuteNonQuery();
						}
					}
				}
				catch (Exception ex)
				{
					errorMessage = ex.Message;
					return;

				}

				customerInfo.name = ""; customerInfo.address = "";
				customerInfo.city = ""; customerInfo.state = "";customerInfo.zip = "";

				successMessage = "New Customer added successfully";

				Response.Redirect("/Customers/Index");
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
			}

		}
	}
}
