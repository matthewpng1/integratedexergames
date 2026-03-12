using Platformer.Mechanics;
using Platformer.UI;
using UnityEngine;

namespace Platformer.UI
{
    /// <summary>
    /// The MetaGameController is responsible for switching control between the high level
    /// contexts of the application, eg the Main Menu and Gameplay systems.
    /// </summary>
    public class MetaGameController : MonoBehaviour
    {
        /// <summary>
        /// The main UI object which used for the menu.
        /// </summary>
        public MainUIController mainMenu;

        /// <summary>
        /// A list of canvas objects which are used during gameplay (when the main ui is turned off)
        /// </summary>
        public Canvas[] gamePlayCanvasii;

        /// <summary>
        /// The game controller.
        /// </summary>
        public GameController gameController;

    // Key to toggle the main menu. Use Escape by default. This avoids relying on
    // an Input Manager axis/button named "Menu" which may not exist and throws
    // an ArgumentException at runtime.
    public KeyCode menuKey = KeyCode.Escape;
        bool showMainCanvas = false;

        // Pause menu canvas. When active the game is paused.
        public Canvas pauseMenuCanvas;
        bool isPaused = false;

        void OnEnable()
        {
            _ToggleMainMenu(showMainCanvas);
        }

        /// <summary>
        /// Turn the main menu on or off.
        /// </summary>
        /// <param name="show"></param>
        public void ToggleMainMenu(bool show)
        {
            if (this.showMainCanvas != show)
            {
                _ToggleMainMenu(show);
            }
        }

        void _ToggleMainMenu(bool show)
        {
            if (show)
            {
                Time.timeScale = 0;
                mainMenu.gameObject.SetActive(true);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                mainMenu.gameObject.SetActive(false);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(true);
            }
            this.showMainCanvas = show;
        }

        void Update()
        {
            // Use the configured key to toggle pause menu. If the main menu is
            // open we prioritize closing it first.
            if (Input.GetKeyDown(menuKey))
            {
                if (showMainCanvas)
                {
                    ToggleMainMenu(show: false);
                }
                else
                {
                    TogglePauseMenu(!isPaused);
                }
            }
        }

        /// <summary>
        /// Turn the pause menu on or off. When on, timeScale=0 to pause the game.
        /// </summary>
        /// <param name="pause"></param>
        public void TogglePauseMenu(bool pause)
        {
            if (isPaused == pause) return;
            _TogglePauseMenu(pause);
        }

        void _TogglePauseMenu(bool pause)
        {
            isPaused = pause;
            if (pause)
            {
                Time.timeScale = 0;
                if (pauseMenuCanvas != null) pauseMenuCanvas.gameObject.SetActive(true);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                if (pauseMenuCanvas != null) pauseMenuCanvas.gameObject.SetActive(false);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(true);
            }
        }

    }
}
