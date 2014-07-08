using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugGUI : MonoBehaviour {

    private static int yOffset = 15;
    private int msgLength = 300;
    private int msgHeight = 20;
    private int loopCount = 0;
    private static Dictionary<int, string> messageList = new Dictionary<int, string>();
    
    //private static List<string> messages = new List<string>();

    void Start() {
        //StartCoroutine(ClearMessages());
    }

    public static void UpdateMessage(int ID, string message) {
        if (messageList != null) {
            if (!messageList.ContainsKey(ID)) {
                messageList.Add(ID, message);
            } else {
                messageList[ID] = message;
            }
        }

    }

    /*public static void Output(string message) {
        if (messages != null  && message != null) {
            messages.Add(message);
        }
    }*/

    public static void RemoveMessage(int ID) {
        if (messageList != null) {
            messageList.Remove(ID);
        }
    }

    void OnGUI() {
        if (messageList != null && messageList.Count != 0) {
            loopCount = 0;
            foreach(KeyValuePair<int, string> msg in messageList) {
                if (msg.Value != "") {
                    GUI.Label(new Rect(0, (loopCount++ * yOffset), msgLength, msgHeight), msg.Value);
                }
            }
        }
    }

    /*void OnGUI() {
        if (messageList !=null) {
            for (int i=0; i<messages.Count; ++i) {
                if (messages[i] == null) break; //assume a null entry means all others are empty
                //print(i*yOffset);
                GUI.Label(new Rect(0, (i * yOffset), msgLength, msgHeight), messages[i]);
            }
            GUI.Label(new Rect(200, 0, msgLength, msgHeight), messages.Count.ToString());
        }
    }*/

    /*IEnumerator ClearMessages() {
        while(true) {
            yield return new WaitForEndOfFrame();
            messages.Clear();
        }
    }*/
}
