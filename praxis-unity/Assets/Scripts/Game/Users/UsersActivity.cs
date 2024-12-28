using Tekly.Injectors;
using Tekly.TreeState.StandardActivities;

namespace Praxis.Game.Users
{
	public class UsersActivity : InjectableActivity
	{
		[Inject] private IUserService m_usersService;

		protected override void ActiveUpdate()
		{
			m_usersService.Tick();
		}
	}
}