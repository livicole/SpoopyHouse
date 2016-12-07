using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class DoorTracker : MonoBehaviour {

    public List<ObjectInfo> doors, rooms;
    private Transform child;
    public int index = 0;

    void Start()
    {
        doors = new List<ObjectInfo>();
        rooms = new List<ObjectInfo>();
    }

    void Update()
    {
       
        InitDoorTracker(false);
    }

    public void InitDoorTracker(bool overrideCheck)
    {
        if (doors.Count == 0 || overrideCheck)
        {
            //Collect all relevant gameobjects in arrays.
            GameObject[] allDoors = GameObject.FindGameObjectsWithTag("Door");
            GameObject[] allRooms = GameObject.FindGameObjectsWithTag("Room");
            
            //Check if the doors or rooms list are empty
            if(doors.Count > 0)
            {
                doors.Clear();
                rooms.Clear();
            }

            //Add all the collected doors to the arrays.
            for(int i = 0; i < allDoors.Length; i++)
            {
                if (allDoors[i].GetComponent<DoorScript>().isLive)
                {
                    ObjectInfo temp = new ObjectInfo(false, allDoors[i].transform);
                    doors.Add(temp);
                }
              
            }

            for(int i =0; i < allRooms.Length; i++)
            {
                ObjectInfo temp = new ObjectInfo(false, allRooms[i].transform);
                rooms.Add(temp);
            }

            child = GameObject.Find("ChildPlayer").transform;
        }
    }

    //Disable all doors on the room about to be moved.
    public void DisableRoomDoors(Transform room)
    {
        foreach(Transform door in room.GetComponent<GridLocker>().doors)
        {   
            door.GetComponent<DoorScript>().isLive = false;
            door.GetComponent<DoorScript>().ResetDoors();
        }
    }

    //Enable all doors on this room that has been moved/checked.
    public void EnableRoomDoors(Transform room)
    {
        foreach (Transform door in room.GetComponent<GridLocker>().doors)
        {
            door.GetComponent<DoorScript>().isLive = true;
//door.GetComponent<DoorScript>().ResetDoors();
        }
    }

    public void ConnectAllDoors()
    {
        foreach(ObjectInfo objInfo in doors)
        {
            objInfo.obj.GetComponent<DoorScript>().ConnectDoors();
        }
    }

    //
    // Summary:
    //     ///
    //     Returns bool, true if all rooms are connected and reachable by the human player. 
    //     ///
    public bool AreAllRoomsConnected()
    {
        ObjectInfo temp = new ObjectInfo(false, child.GetComponent<RoomParenter>().currentRoom);
        //InitDoorTracker(true);
        CheckDoorsInRoom(temp.obj);
        return CheckAllRooms();

    }

    public void ReplaceRoomInData(Transform realRoom, Transform tempRoom)
    {
        foreach (ObjectInfo objInfo in rooms)
        {
            if (objInfo.obj.Equals(realRoom))
            {
                //Debug.Log("Replacing");
                objInfo.obj = tempRoom;
            }
        }

        int i = 0;
        foreach (ObjectInfo objInfo in doors)
        {
            foreach (Transform door in realRoom.GetComponent<GridLocker>().doors)
            {
                if (objInfo.obj.Equals(door))
                {
                    //Debug.Log("Replacing door...");
                    objInfo.obj = tempRoom.GetComponent<GridLocker>().doors[i];
                    i++;
                }
            }
        }
    }

    public void CheckDoorsInRoom(Transform room)
    {
        foreach(Transform door in room.GetComponent<GridLocker>().doors)
        {
            ObjectInfo objinfo =  GetDoor(door);
            if (!objinfo.check)
            {   
                //Debug.Log("Checking door: " + door.GetComponent<DoorScript>().room);
                objinfo.SetCheck(true);
                if (door.GetComponent<DoorScript>().otherDoor != null)
                {
                    CheckDoorsInRoom(door.GetComponent<DoorScript>().otherDoor.GetComponent<DoorScript>().room);
                }
            }
        }
        GetRoom(room).SetCheck(true);
    }

    public ObjectInfo GetRoom(Transform room)
    {
        foreach (ObjectInfo objInfo in rooms)
        {
            if (objInfo.obj.Equals(room))
            {
                return objInfo;
            }
        }
        return null;
    }

    public ObjectInfo GetDoor(Transform door)
    {
        foreach(ObjectInfo objInfo in doors)
        {
            if (objInfo.obj.Equals(door))
            {
                return objInfo;
            }
        }
        return null;
    }

    public bool CheckAllRooms()
    {
        foreach (ObjectInfo objInfo in rooms)
        {
            if (!objInfo.check)
            {
                return false;
            }
        }
        return true;
    }

    public void ResetAllBools()
    {
        foreach(ObjectInfo objInfo in rooms)
        {
            objInfo.check = false;
        }

        foreach(ObjectInfo objInfo in doors)
        {
            objInfo.check = false;
        }
    }
}

[System.Serializable]
public class ObjectInfo
{
    public bool check = false;
    public Transform obj = null;

    public ObjectInfo(bool check, Transform obj)
    {
        this.check = check;
        this.obj = obj;
    }

    public bool Equals(ObjectInfo temp)
    {
        if (this.check == temp.check && this.obj.Equals(temp.obj))
        {
            return true;
        }
        return false;
    }

    public void SetCheck(bool boolean)
    {
        check = boolean;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ObjectInfo))]
public class ObjectInfoEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        label.text = "Object Info";
        position.height = 20;
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Get rid of indent among child labels
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate the rectangles
        Rect objRect = new Rect(position.x, position.y, 200, position.height);
        Rect checkRect = new Rect(position.x + 200, position.y, 200, position.height);

        //Change the height of the property


        // Draw the fields
        EditorGUI.PropertyField(objRect, property.FindPropertyRelative("obj"), GUIContent.none);
        EditorGUI.PropertyField(checkRect, property.FindPropertyRelative("check"), GUIContent.none);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float extraHeight = 0.0f;
        return base.GetPropertyHeight(property, label) + extraHeight;
    }
}
#endif
