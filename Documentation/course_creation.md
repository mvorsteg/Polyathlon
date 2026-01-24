# Race Course Creation

#### To create a new race course, simply do the following:

1. Navigate to `Assets\Scenes`.
2. Duplicate `Course 0 (template).unity` and rename the duplicated scene appropriately. Be careful not to edit anything in `Course 0 (template).unity`.
3. Here's what you'll find in your new scene's hierarchy:
  - RaceManager
    - This keeps track of the race itself. You generally shouldn't need to mess with this unless you are making a training course (in which case "Is Training Course" should be selected).
  - Racers
    - This is where the Player Input Manager component resides. You generally shouldn't need to touch this.
  - Directional Light
    - Default lighting
  - StartingPointsParent
    - The child gameObjects of this gameObject represent the locations where the racers will spawn at the start of the race. You can reposition them and/or their parent as needed.
  - Checkpoints
    - The children of this gameObject are instances of the Checkpoint prefab, which keep track of the current place that each player is in. Distance is calculated from each player to the next checkpoint to determine their current placement value.
    - The order of the checkpoints in the hierarchy listing matters, as the one at the top of the list is the first checkpoint encountered in the race, and the one at the bottom of the list is the final checkpoint encountered in the race.
  - Waypoints
    - The children of this gameObject are Waypoint Chains (just one to start), with the children of Waypoint Chains being various types of Waypoint prefabs. Waypoints are what tells the computer players where to go next. Each waypoint can have a fork that may divide between continuing in the current Waypoint Chain or instead following a different Waypoint Chain.
  - AudioManager
    - The AudioManager gameObject contains the `AudioManager` component. This component contains a `Songs` list, which is where all the tracks for music should be added.
    - The child of AudioManager is a MusicSwitch gameObject, which has two children, each containing a trigger and a `Music Switch` component. The value of `Music Index` on that component corresponds to the index of the song in the AudioManager's `Songs` list. When the player enters the trigger, the AudioManager will switch to the song whose index matches the `Music Index` of the corresponding `Music Switch` component.
  - Navigation
    - This is where the NavMesh is managed, on the `NavMesh Surface` component. There is no NavMesh Data when you initially open your duplicate of `Course 0 (template).unity`, so you will need to click `Bake` in order for the computer players to be able to move.
      - We don't have a baked NavMesh in the template scene by default because when the scene is duplicated, the duplicates retain the same NavMesh Data asset, which runs the risk that the same asset could inadvertently be edited in multiple scenes, potentially losing data that one scene needs when re-baking for another scene.
  - Finish Line
    - This is the physical Finish Line banner that players should run beneath to end the race. This should be kept at the same location as the final checkpoint and final waypoint.
  - Plane
    - This is a Unity primitive used for the template scene in place of a terrain.
      - We don't include a terrain in the template scene for the same reason we don't include a baked NavMesh: the duplication process means that the same terrain asset could inadvertently end up being shared between multiple scenes, potentially breaking one scene while another is being worked on.

