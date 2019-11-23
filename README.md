# UnityDijkstraGridSystem
An example use of my QUnity and QuestryGameGeneral libraries making use of the Dijkstra-Map feature inside the QuestryGameGeneral library

You can drag these scripts into your Unity project and use them freely as long as you abide by the MIT license provided herein. Do not forget to add the GameProperties, LateExecutor and GameManager to their respective gameobjects and modify the script orders such that (from initial to latter): GameProperties -> GameManager-> your stuff to be initialized -> LateExecutor. 

Feel free to add the code of the scripts to your scripts and modify them however you would like. With the base code, however, I would recommned using Cubes for gameobjects containing the Grid.cs script

In addition, keep in mind that this grid system is not ID/coordinate based but rather just holds distance relations between grids. With minor changes,you can create a non-symmetrical Grid system with this code. Like some grids can be larger than others etc. Of course you'd also have to change the relations and the distances in the pathfinding system. Nonetheless some interesting behaviour may be created. Quite interestingly, if you change just the distances you provide to the Dijkstra mapper and adjust your gameplay accordingly, you can achieve a non-euclidean grid system, where for example travelling from one grid to another can be faster than others despite the distances seemingly being similar.

A downside of this flexibility is that the grid relations have to be explicitly defined.
