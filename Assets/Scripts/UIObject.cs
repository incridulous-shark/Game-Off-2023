using System;
using System.Threading.Tasks;
using UnityEngine;

public class UIObject : MonoBehaviour
{
    [Flags]
    public enum UIObjType
    {
        None = 0,
        Move = 1,
        Scale = 2,
        Rotate = 4
    }

    [SerializeField] private UIObjType uIType;
    [SerializeField] private UIGroup myUiGroup;
    [SerializeField] private Vector3 defaultPos;            // When not in active group
    [SerializeField] private Vector3 activePos;             // When is in active group
    [SerializeField] private Vector3 defaultScale;          // When not in active group
    [SerializeField] private Vector3 activeScale;           // When is in active group
    [SerializeField] private float duration;                // Animation duration
    [SerializeField] private float delay;                   // Animation delay

    [Tooltip("Uncheck if you don't want UI to respond")]
    public bool isActive;
    [Tooltip("Which UI Controller to respond to, leave blank if not needed")]
    [SerializeField] private UIController UIParent;

    private void Awake()
    {
        // Subcribe to changes in the UIGroup
        if (UIParent != null)
            UIParent.onUIGroupChanged += OnUIGroupChanged;
        else
        {
            // If no UIController, do a default in-animation
            AnimateUIAsync(myUiGroup);
        }
    }

    private void OnDestroy()
    {
        if (UIParent != null)
            UIParent.onUIGroupChanged -= OnUIGroupChanged;
    }

    private void OnUIGroupChanged(UIGroup uIGroup)
    {
        if(isActive)
            AnimateUIAsync(uIGroup);
    }

    private async void AnimateUIAsync(UIGroup uIGroup)
    {
        if(LeanTween.isTweening(gameObject))
            LeanTween.cancel(gameObject);

        if (delay > 0)
            await Task.Delay(Mathf.RoundToInt(delay * 1000));

        if((myUiGroup & uIGroup) == UIGroup.Zero)       // If no matches
        {
            if ((uIType & UIObjType.Move) != UIObjType.None)
            {
                LeanTween.moveLocal(gameObject, defaultPos, duration).setEaseOutCubic();
            }
            if ((uIType & UIObjType.Scale) != UIObjType.None)
            {
                LeanTween.moveLocal(gameObject, defaultScale, duration).setEaseOutCubic();
            }
            if ((uIType & UIObjType.Rotate) != UIObjType.None)
            {

            }
        }
        else                                            // If there are matches
        {
            if((uIType & UIObjType.Move) != UIObjType.None)
            {
                LeanTween.moveLocal(gameObject, activePos, duration).setEaseOutCubic();
            }
            if ((uIType & UIObjType.Scale) != UIObjType.None)
            {
                LeanTween.moveLocal(gameObject, activeScale, duration).setEaseOutCubic();
            }
            if ((uIType & UIObjType.Rotate) != UIObjType.None)
            {
            }
        }
    }

#if UNITY_EDITOR
    [Header("Debugging")]
    public bool isDefault = false;

    [ContextMenu("Set Position")]
    public void SetDefaultPosition()
    {
        if (isDefault)
            defaultPos = transform.localPosition;
        else
            activePos = transform.localPosition;
    }

    [ContextMenu("Go To Position")]
    public void GoToPosition()
    {
        if (isDefault)
            transform.localPosition = defaultPos;
        else
            transform.localPosition = activePos;
    }
#endif
}
