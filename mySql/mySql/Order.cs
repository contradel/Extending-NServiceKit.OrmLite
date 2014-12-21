using System;
using NServiceKit.DataAnnotations;

namespace mySql
{
	public class Order
	{
		//Try to add or comment out properties here, SQL will be generated automatic
		[AutoIncrement]
		public int Id { get; set; }	//pk
		public DateTime? OrderDate { get; set; }

		//[References(typeof(Customer))]      //Creates Foreign Key
		//public int CustomerId { get; set; }


		//public DateTime? RequiredDate { get; set; }
		public DateTime? ShippedDate { get; set; }
		public int? ShipVia { get; set; }
		public decimal Freight { get; set; }
		public decimal Total { get; set; }
		public int? Hue { get; set; }
		public int? Oppo { get; set; }
		public DateTime? Mange { get; set; }
		//public double Yksikok { get; set; }
		public string Doge { get; set; }
		public string hansi { get; set; }
		public decimal Decimal { get; set; }

	}

	public class Customer
	{
		[AutoIncrement]
		public int Id { get; set; }

		public string Name { get; set; }
		public string Company { get; set; }
		public int Age { get; set; }
	}
}