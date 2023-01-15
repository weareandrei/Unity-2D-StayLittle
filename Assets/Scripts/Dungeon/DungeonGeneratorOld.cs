using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using Array2DEditor;
using Unity.VisualScripting;

public static class DungeonGeneratorOld
{
    // private static List<RoomInstance> roomsInstances = GetRoomInstances();
    //
    // public static List<RoomInstance> GenerateBySeed(string seed) {
    //     // Firstly generate the room[0] - initial room with ScoreCounter
    //
    //     // Iteratively
    //     //    Look at end of each Tree branch. 
    //     //      Find an array of available rooms to attach (availableRooms)
    //     //          availableRooms will be of length X.
    //     //          seed[i] is a symbol Y.
    //     //          We have a map. For each symbol in seed we will have a mapped number (like 1 to 100).
    //     //      find corresponding Y to seed[i] (lets call that number N)
    //     //      then we take availableRooms[N] and attach that room
    //     //          add cosmetics to that room if we want. But that will require to make the seed more complex.
    //     //           for example 1 in seed indicated start and end of data chunk in that seed
    //
    //     List<RoomInstance> rooms = new List<RoomInstance>();
    //
    //     rooms.Add(selectEntranceRoom(seed[0]));
    //
    //     // Keep adding rooms until reach limit
    //     //  seed.Length is the maxRoomsForDungeon
    //     while(rooms.Count < seed.Length) {
    //         foreach (RoomInstance thisRoom in rooms) {
    //             // RoomRequest[] requests = Get Requests to find rooms for each exit
    //             // foreach (RoomInstance request in requests) :
    //             //    var newRoom = findSuitableRoom(request)
    //             //    rooms.Add(newRoom)
    //
    //             RoomRequest[] requests = getRoomRequests(thisRoom);
    //             foreach (RoomRequest request in requests) {
    //                 Debug.Log(request);
    //             }
    //
    //             // int[] attachedRoomIds = attachRoomsTo(thisRoom);
    //         }
    //     }
    //
    //     return rooms;
    // }
    //
    // private static RoomRequest[] getRoomRequests(RoomInstance thisRoom) { 
    //     List<RoomRequest> requests = new List<RoomRequest>();
    //     
    //     // Firstly find sides that require attachments.
    //     RoomSideStatus sidesStatus = find
    //
    //     return requests.ToArray();
    // }
    //
    // private static List<RoomInstance> GetRoomInstances() {
    //     string instancesPath = "Dungeon/Room/Prefabs";
    //     GameObject[] instancesPrefabs = Resources.LoadAll<GameObject>(instancesPath);
    //     RoomInstance[] instancesRooms = instancesPrefabs.Select(
    //             instanceObj => instanceObj.GetComponent<RoomInstance>()
    //         ).ToArray();
    //     return new List<RoomInstance>(instancesRooms);
    // }
    //
    // // For each exit find the room that can be attached
    // private static int[] attachRoomsTo(RoomInstance room) {
    //     Vector2[] exitsCordinates = findAllExits(room);
    //     return new[] {2,3,4};
    // }
    //
    // private static Vector2[] findAllExits(RoomInstance room) {
    //     List<Vector2> exitsFound = new List<Vector2>();
    //     
    //     var cells = room.exits.GetCells();
    //     var piece = new GameObject("Piece");
    //
    //     for (var y = 0; y < room.exits.GridSize.y; y++) {
    //         for (var x = 0; x < room.exits.GridSize.x; x++) {
    //             if (cells[y, x]) {
    //                 exitsFound.Add(new Vector2(y,x));
    //             }
    //         }
    //     }
    //
    //     return exitsFound.ToArray();
    // }
    //
    // private static RoomInstance selectEntranceRoom(char seedChar)
    // {
    //     return roomsInstances[0];
    // }
    //
    // private struct RoomRequest {
    //     public Array2DBool requiredSpace;
    //     public Array2DBool requiredExits;
    //     public Array2DBool forbiddenExits;
    // }
    //
    // private struct RoomSideStatus {
    //     public sideStatus leftSide;
    //     public sideStatus rightSide;
    //     public sideStatus topSide;
    //     public sideStatus bottomtSide;
    // }
    //
    // private enum sideStatus {
    //     noStatus,
    //     forbidden,
    //     required
    // }
    
}