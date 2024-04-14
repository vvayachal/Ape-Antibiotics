using UnityEngine;

public class CursorManager : MonoBehaviour
{
    //this is so we can see the value in the editor
    [SerializeField] private bool lockCursor = true;

    //keep track of the cursor locked stated
    private bool cursorIsLocked = true;

    // Update is called once per frame
    void Update()
    {
        UpdateCursorLock();
    }

    private void UpdateCursorLock()
    {
        //this is so we can disable unlocking the mouse
        if (lockCursor)
        {
            InternalLockUpdate();
        }
    }

    //used to enable/disable the cursor lock
    private void SetCursorLock(bool value)
    {
        //set the lockCursor value to our input
        lockCursor = value;
        //if we're disabling the cursorlock
        if (!lockCursor)
        {
            //make the cursor unlocked
            Cursor.lockState = CursorLockMode.None;
            //set the cursor to be visible
            Cursor.visible = true;
        }

    }

    private void InternalLockUpdate()
    {
        //if the user presses the esc key
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //unlock our cursor
            cursorIsLocked = false;
            //else if the user pushes the left mouse button
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //lock our cursor
            cursorIsLocked = true;
        }

        //if we're going to lock the cursor
        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        //if we're unlocking the cursor
        else if (!cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //this is for external scripts to disable locking the mouse
    public void DisableMouseLock()
    {
        SetCursorLock(false);
    }

    //this is for external scripts to enable locking the mouse
    public void EnableMouseLock()
    {
        SetCursorLock(true);
    }


}
