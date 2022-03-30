using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviorTree : MonoBehaviour
{

    public GameObject treasure;
    public Door door;
    public FinishScript finish;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ITask build = CreateTree();
            build.run();
        }
    }

    public ITask CreateTree()
    {
        List <ITask> taskList = new List<ITask>();

        ITask isDoorOpen = new IsTrue(door.IsOpen);
        ITask OpenDoor = new OpenDoor(door.gameObject);
        taskList.Add(isDoorOpen);
        taskList.Add(OpenDoor);
        Sequence OpenedDoor = new Sequence(taskList);

        //If door is not locked, open it
        taskList = new List<ITask>();
        ITask isDoorNotLocked = new IsFalse(door.IsLocked);
        //ITask Wait = new Wait();
        //taskList.Add(Wait);
        taskList.Add(isDoorNotLocked);
        //taskList.Add(Wait);
        taskList.Add(OpenDoor);
        Sequence openUnlockedDoor = new Sequence(taskList);


        //barge a closed door
        taskList = new List<ITask>();
        ITask isDoorClosed = new IsFalse(door.IsOpen);
        ITask bargeDoor = new BargeDoor(door.rb);
        //taskList.Add(Wait);
        taskList.Add(isDoorClosed);
        //taskList.Add(Wait);
        taskList.Add(bargeDoor);
        Sequence bargeClosedDoor = new Sequence(taskList);

        //open a closed door, one way or another
        taskList = new List<ITask>();
        taskList.Add(OpenDoor);
        taskList.Add(openUnlockedDoor);
        taskList.Add(bargeClosedDoor);
        Selector openTheDoor = new Selector(taskList);

        //get the treasure when the door is closed
        taskList = new List<ITask>();
        ITask moveToDoor = new MoveToTarget(this.GetComponent<Kinematic>(), door.gameObject, finish.IsAngry);
        ITask moveToTreasure = new MoveToTarget(this.GetComponent<Kinematic>(), treasure.gameObject, finish.IsAngry);
        ITask ChangeToGreen = new ChangeGreen(finish.gameObject);
        taskList.Add(ChangeToGreen);
        taskList.Add(moveToDoor);
        taskList.Add(openTheDoor);
        taskList.Add(moveToTreasure);
        Sequence getTreasureBehindClosedDoor = new Sequence(taskList);

        //get the treaseure when the door is open
        taskList = new List<ITask>();
        taskList.Add(ChangeToGreen);
        taskList.Add(isDoorOpen);
        taskList.Add(moveToTreasure);
        Sequence getTreasureWithOpenedDoor = new Sequence(taskList);

        //run from the treasure
        taskList = new List<ITask>();
        ITask IsFinishAngry = new IsTrue(finish.IsAngry);
        ITask RunFromTreasure = new MoveToTarget(this.GetComponent<Kinematic>(), treasure.gameObject, finish.IsAngry);
        ITask ChangeToRed = new ChangeRed(finish.gameObject);
        taskList.Add(IsFinishAngry);
        taskList.Add(ChangeToRed);
        taskList.Add(RunFromTreasure);
        Sequence RunFromAngryTreasure = new Sequence(taskList);


        //Get the treasure
        taskList = new List<ITask>();
        taskList.Add(getTreasureWithOpenedDoor);
        taskList.Add(getTreasureBehindClosedDoor);
        Selector getTreasure = new Selector(taskList);

        //Check if thing is angry or not
        taskList = new List<ITask>();
        taskList.Add(RunFromAngryTreasure);
        taskList.Add(getTreasure);
        Selector LookToGetTreasure = new Selector(taskList);

        return LookToGetTreasure;


    }
}
