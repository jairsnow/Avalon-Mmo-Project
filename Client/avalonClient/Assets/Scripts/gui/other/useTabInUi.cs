using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class useTabInUi : MonoBehaviour {

    private string[] tabOrder = { "usernameValue", "passwordValue", "loginButtons" };

    void Start() {
        Selectable newFocusElement = GameObject.Find(tabOrder[0]).GetComponent<Selectable>();
        newFocusElement.Select();
    }

    void Update () {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
            EventSystem myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            bool nextActive = false, focussedSomething = false;

            if (myEventSystem.currentSelectedGameObject)
            {
                foreach (string tabbable in tabOrder)
                {

                    if (nextActive)
                    {
                        Selectable newFocusElement = GameObject.Find(tabbable).GetComponent<Selectable>();
                        newFocusElement.Select();
                        nextActive = false;
                        break;
                    }

                    if (tabbable == myEventSystem.currentSelectedGameObject.name) { nextActive = true; focussedSomething = true; }

                }
                
                // Set the first active
                if (nextActive == true || focussedSomething == false)
                {
                    Selectable newFocusElement = GameObject.Find(tabOrder[0]).GetComponent<Selectable>();
                    newFocusElement.Select();
                }

            }
        }
    }

}
