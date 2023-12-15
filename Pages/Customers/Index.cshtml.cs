using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace Cutomers_Registration_Crud_App.Pages.Customers
{
	public class IndexModel : PageModel
	{
		//to save data of all clients we create a list that stores the data
		public List<CustomerInfo> listCustomers = new List<CustomerInfo>();

		public void OnGet()
		{
			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=CustomersDb;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM CustomersDb";
					using(SqlCommand command = new SqlCommand(sql, connection))
					{
						using(SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								CustomerInfo customerInfo = new CustomerInfo();
								customerInfo.id = "" + reader.GetInt32(0);
								customerInfo.name = reader.GetString(1);
								customerInfo.address = reader.GetString(2);
								customerInfo.city = reader.GetString(3);
								customerInfo.state = reader.GetString(4);
								customerInfo.zip = reader.GetString(5);

								listCustomers.Add(customerInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.ToString());
			}
		}
	}

	public class CustomerInfo 
	{
		//this class allows to store data of only one client
		public string id;
		public string name;
		public string address;
		public string city;
		public string state;
		public string zip;
	}
}
