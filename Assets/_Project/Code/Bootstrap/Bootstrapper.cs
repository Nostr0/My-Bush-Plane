using MaintGames.Bush.Contracts.Events;
using MaintGames.Bush.Foundation.Events;
using MaintGames.Bush.Foundation.Logging;
using MaintGames.Bush.Foundation.Services;
using MaintGames.Bush.Infrastructure.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MaintGames.Bush.Bootstrap
{
    /// <summary>
    /// Thin MonoBehaviour shell around <see cref="GameComposition"/>. Lives in Bootstrap.unity, the only start
    /// scene. The Bootstrap scene is never unloaded; content scenes are loaded additively.
    /// </summary>
    [DefaultExecutionOrder(-10000)]
    public sealed class Bootstrapper : MonoBehaviour
    {
        [Tooltip("Build index of the scene to load additively after startup. -1 loads nothing.")]
        [SerializeField] private int _nextSceneBuildIndex = -1;

        private ServiceRegistry _services;
        private BootstrapReporter _reporter;

        private void Awake()
        {
            if (FindObjectsByType<Bootstrapper>(FindObjectsSortMode.None).Length > 1)
            {
                new UnityLog().Error("Bootstrap", "More than one Bootstrapper is loaded. Disabling " + name + ".");
                enabled = false;
                return;
            }

            ILog log = new UnityLog();
            _services = GameComposition.Build(log);

            IEventBus events = _services.Get<IEventBus>();
            _reporter = new BootstrapReporter(events, log);
            _reporter.Enable();

            events.Publish(new GameBootstrapped(_services.Count, Time.realtimeSinceStartupAsDouble));
        }

        private void Start()
        {
            if (_services == null || _nextSceneBuildIndex < 0)
            {
                return;
            }

            AsyncOperation load = SceneManager.LoadSceneAsync(_nextSceneBuildIndex, LoadSceneMode.Additive);
            load.completed += _ => ActivateLoadedScene();
        }

        private void OnDestroy()
        {
            _reporter?.Disable();
        }

        private void ActivateLoadedScene()
        {
            // Lighting settings come from the active scene, so the content scene must become active.
            Scene loaded = SceneManager.GetSceneByBuildIndex(_nextSceneBuildIndex);
            if (loaded.IsValid())
            {
                SceneManager.SetActiveScene(loaded);
            }
        }
    }
}
