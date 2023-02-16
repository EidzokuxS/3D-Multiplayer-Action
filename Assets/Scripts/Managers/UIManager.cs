using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcade3D
{
    public enum CanvasType
    {
        MainMenu,
        GameUI,
        Lobby,
        EndScreen
    }

    public class UIManager : SingletonBase<UIManager>
    {
        #region Properties
        private List<UICanvasController> _canvasControllerList;
        private UICanvasController _lastActiveCanvas;
        #endregion

        #region Unity Events
        protected override void Awake()
        {
            base.Awake();
            InitializeManager();
        }

        #endregion

        #region Public API

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SetMousePointerState(bool visibility, CursorLockMode mode)
        {
            Cursor.lockState = mode;
            Cursor.visible = visibility;
        }
        public void SwitchCanvas(CanvasType _type)
        {
            if (_lastActiveCanvas != null)
                _lastActiveCanvas.gameObject.SetActive(false);

            UICanvasController desiredCanvas = _canvasControllerList.Find(x => x.canvasType == _type);
            if (desiredCanvas != null)
            {
                desiredCanvas.gameObject.SetActive(true);
                _lastActiveCanvas = desiredCanvas;
            }
            else
                Debug.LogWarning("The desired canvas was not found!");
        }
        #endregion

        #region Private API
        private void InitializeManager()
        {
            _canvasControllerList = GetComponentsInChildren<UICanvasController>().ToList();
            _canvasControllerList.ForEach(controller => controller.gameObject.SetActive(false));
            SetMousePointerState(true, CursorLockMode.Confined);
            SwitchCanvas(CanvasType.MainMenu);
        }

        #endregion
    }
}
