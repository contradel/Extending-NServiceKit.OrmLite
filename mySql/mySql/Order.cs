using System;
using NServiceKit.DataAnnotations;
using NServiceKit.OrmLite;

namespace mySql
{
	public class Order
	{
		//Try to add or comment out properties here, SQL will be generated automatic
		[AutoIncrement]
		public int Id { get; set; }
		public DateTime? OrderDate { get; set; }
		public DateTime? RequiredDate { get; set; }
		public DateTime? ShippedDate { get; set; }
		//public int? ShipVia { get; set; }
		public decimal Freight { get; set; }
		//public decimal Total { get; set; }
		public int Doge { get; set; }

		[ForeignKey(typeof(Customer), OnDelete = "NO ACTION", OnUpdate = "NO ACTION")]
		public int? CustomerId { get; set; }

	}

	public class Customer
	{
		[AutoIncrement]
		public int Id { get; set; }

		public string Name { get; set; }
		public string Company { get; set; }
		public DateTime Birthday { get; set; }
		//public DateTime topdoge { get; set; }
	}





}