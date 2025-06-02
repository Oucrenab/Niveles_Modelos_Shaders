using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{

    public static ScreenManager Instance;

    [SerializeField] Stack<BaseScreen> _screens = new();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool ScreenActive { get { return _screens.Count > 0; } }

    public void ActivateScreen(BaseScreen screen)
    {
        screen.Activate();

        _screens.Push(screen);
    }

    public void DeactivateScreen()
    {
        if (_screens.Count <= 0) return;

        _screens.Pop().Deactivate();
    }
}
