using System;
using Praxis.Game.Users;
using Tekly.Common.Utils;
using Tekly.DataModels.Models;
using Tekly.Injectors;
using Tekly.Logging;
using Tekly.TreeState;
using Tekly.Webster;
using UnityEngine;

namespace Praxis
{
	public class AppCore : MonoBehaviour, IInjectionProvider
	{
		[Inject] private RootModel _rootModel;
		
		public void Provide(InjectorContainer container)
		{
			container.Register(this);
			container.Register(RootModel.Instance);
			
			container.Singleton<IUserService, UserService>();
			container.Factory<UsersModel>();
			
			Debug.Log("Crash Detected: " + CrashCanary.Instance.CrashDetected);
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Application.Quit();
				return;
			}

			ModelManager.Instance.Tick();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Initialize()
		{
			TkLogger.Initialize();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void InitializeDebug()
		{
			WebsterServer.Start(true);
			
			TreeStateRegistry.Instance.ActivityModeChanged.Subscribe(evt => {
				var type = evt.IsState ? "Tree State" : "Tree Activity";
				switch (evt.Mode) {
					case ActivityMode.Inactive:
						Frameline.EndEvent($"{evt.State} Unloading", type);
						break;
					case ActivityMode.Loading:
						Frameline.BeginEvent($"{evt.State} Loading", type);
						break;
					case ActivityMode.ReadyToActivate:
						Frameline.EndEvent($"{evt.State} Loading", type);
						break;
					case ActivityMode.Active:
						Frameline.BeginEvent($"{evt.State} Active", type);
						break;
					case ActivityMode.Unloading:
						Frameline.BeginEvent($"{evt.State} Unloading", type);
						Frameline.EndEvent($"{evt.State} Active", type);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			});
		}
	}
}