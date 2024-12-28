using System.Collections.Generic;
using Tekly.Common.Collections;
using UnityEngine;

namespace Praxis.Game.Users
{
	public class User
	{
		public string NameFirst;
		public string NameLast;
		
		public int Age;

		public float Duration;
		public float Progress;

		public void Tick()
		{
			Progress += Time.deltaTime;

			if (Progress >= Duration) {
				Progress -= Duration;
			}
		}
	}

	public interface IUserService
	{
		public IList<User> Users { get; }
		void Tick();
	}

	public class UserService : IUserService
	{
		private static readonly string[] NAMES_FIRST = { "Sophia", "Olivia", "Liam", "Emma", "James", "Ethan", "Mia", "Noah", "Isabella", "William" };
		private static readonly string[] NAMES_LAST = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Martinez", "Lopez" };

		public IList<User> Users => m_users;

		private readonly List<User> m_users = new List<User>();

		public UserService()
		{
			var count = Random.Range(9, 15);
			
			for (int i = 0; i < count; i++) {
				var user = new User();

				user.NameFirst = NAMES_FIRST.Random();
				user.NameLast = NAMES_LAST.Random();
				user.Age = Random.Range(10, 67);
				user.Duration = Random.Range(1, 5);

				m_users.Add(user);
			}
		}

		public void Tick()
		{
			foreach (var user in m_users) {
				user.Tick();
			}
		}
	}
}