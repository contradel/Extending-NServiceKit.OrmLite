namespace OrmLitePehjExtensions
{
	public class SqlModifier
	{
		public string RemoveFirstAndLastCharacter(string target)
		{
			target = target.Remove(0, 1);
			target = target.Remove(target.Length - 1, 1);

			return target;
		}
	}
}