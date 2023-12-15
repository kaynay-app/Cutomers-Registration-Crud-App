using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Cutomers_Registration_Crud_App.Pages.Customers
{
	public class EditModel : PageModel
	{
		public CustomerInfo customerInfo = new CustomerInfo();
		public String errorMessage = "";
		public String successMessage = "";

		public void OnGet()
		{
			String id = Request.Query["id"];

			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=CustomersDb;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM CustomersDb WHERE id=@id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								//customerInfo = new CustomerInfo(); //must be deleted
								customerInfo.id = "" + reader.GetInt32(0);
								customerInfo.name = reader.GetString(1);
								customerInfo.address = reader.GetString(2);
								customerInfo.city = reader.GetString(3);
								customerInfo.state = reader.GetString(4);
								customerInfo.zip = reader.GetString(5);
							}
						}
					}
				}

			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
			}
		}

		public void OnPost()
		{
			customerInfo.id = Request.Form["id"];
			customerInfo.name = Request.Form["name"];
			customerInfo.address = Request.Form["address"];
			customerInfo.city = Request.Form["city"];
			customerInfo.state = Request.Form["state"];
			customerInfo.zip = Request.Form["zip"];

			if (customerInfo.id.Length == 0 || customerInfo.name.Length == 0 ||
				customerInfo.address.Length == 0 || customerInfo.city.Length == 0 ||
				customerInfo.state.Length == 0 || customerInfo.zip.Length == 0)
			{
				errorMessage = "Please all fields must be filled";
				return;
			}

			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=CustomersDb;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE CustomersDb SET name=@name, address=@address, city=@city, state=@state, zip=@zip WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", customerInfo.id);
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

			Response.Redirect("/Customers/Index");
		}
	}
}
