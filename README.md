# Lab-Submission-10-Week-11-


"How does A* pathfinding calculate and prioritize paths?"

A* finds its paths by performing calculations to try to find the lowest distance between nodes in order to reach its target location.


"What challenges arise when dynamically updating obstacles in real-time?"

When obstacles are added/removed during runtime, an A* algorithm needs to be continuously running, and as such must find paths on update.
In order to make this operation less expensive, the path finding method should return a boolean when there is no available path, 
and only execute new pathfinding once the previous path becomes unusable.


"How could you adapt this code for larger grids or open-world settings?"

In an open world setting, where the playspace is perhaps too large for one shared grid, the nagivating NPC would need to generate a new local grid in real time, each time they began navigation. This would allow them to determine an end position without having to rely on a pre-generated, shared npc grid.


"What would your approach be if you were to add weighted cells (e.g., "difficult terrain" areas)?"

If the navigational algorithm were to included weighted cells, the individual cells would not have a binary value of 0 or 1 as they do now, but rather a float between 0 and 1. This float would represent how difficult the cell is to traverse. The algorithm would determine its path by choosing the cells that have the lowest total "traversal difficulty" value.
