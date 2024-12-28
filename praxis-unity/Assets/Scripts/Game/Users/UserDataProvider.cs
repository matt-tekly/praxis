using Tekly.Common.Utils;
using Tekly.DataModels.Models;
using Tekly.Extensions.DataProviders;
using Tekly.Injectors;
using Tekly.Logging;
using UnityEngine;

namespace Praxis.Game.Users
{
	public class UserDataProvider : RootModelDataProvider
	{
		[Inject] private IUserService m_userService;
		[Inject] private UsersModel m_usersModel;

		protected override void OnBind()
		{
			AddModel("users", m_usersModel);
		}
	}

	public class UsersModel : DisposableObjectModel
	{
		private readonly StringValueModel m_selected;
		
		public UsersModel(IUserService userService)
		{
			var all = AddObject("all");
			
			for (var index = 0; index < userService.Users.Count; index++) {
				var user = userService.Users[index];
				all.Add(index, new UserModel(user, this, index));
			}
			
			m_selected = Add("selected", "0");
			
			Add("count", userService.Users.Count);
		}

		public void SetSelect(string id)
		{
			m_selected.Value = id;
		}
	}

	public class UserModel : DisposableObjectModel, ITickable
	{
		private readonly User m_user;
		private readonly NumberValueModel m_progress;
		private readonly ButtonModel m_button;
		
		private readonly TkLogger m_logger = TkLogger.Get<UserModel>();
		
		public UserModel(User user, UsersModel usersModel, int index)
		{
			m_user = user;
			
			Add("name_first", user.NameFirst);
			Add("name_last", user.NameLast);
			Add("age", user.Age);

			m_button = AddButton("select");
			
			m_button.Activated.Subscribe(x => {
				usersModel.SetSelect(index.ToString());
				m_logger.Debug($"Press Button [{user.NameFirst}]");
			}).AddTo(m_disposables);
			
			m_progress = Add("progress", 0);
		}

		protected override void OnTick()
		{
			m_progress.Value = m_user.Progress / m_user.Duration;
			m_button.IsInteractable = m_progress.Value > 0.5f;
		}
	}
}