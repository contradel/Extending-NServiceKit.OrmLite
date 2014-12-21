using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mySql;
using NUnit.Framework;

namespace mySqlTests
{
	[TestFixture]
	class SqlModifierTest
	{
		[Test]
		public void RemoveFirstAndLastCharacter_Letters_StartAndEndCharIsRemoved()
		{
			//Arrange
			var mod = new SqlModifier();

			//Act
			var t = mod.RemoveFirstAndLastCharacter("eaglebrain");

			//Assert
			Assert.AreEqual("aglebrai", t);
		}

		[Test]
		public void RemoveFirstAndLastCharacter_ManyDifferentCharacters_StartAndEndCharIsRemoved()
		{
			//Arrange
			var mod = new SqlModifier();

			//Act
			var t = mod.RemoveFirstAndLastCharacter("P_b[c2cpS7~K+c=r9n4vwMYZX/g_7MVkCJ6t6bb{9?UW21hG,o");

			//Assert
			Assert.AreEqual("_b[c2cpS7~K+c=r9n4vwMYZX/g_7MVkCJ6t6bb{9?UW21hG,", t);
		}
	}
}
