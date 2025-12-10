# 3902: Mario Game
This project is a semester-long work for OSU CSE 3902. We were tasked to recreate the Super Mario Bros first level while focusing on code quality:
> **S** imple <br>
> **R** eadable <br>
> **M** aintainable <br>
> **R** eusable <br>

We have learned many different design patterns to implement to our code. As we have learned throughout the semester, an important part of creating video game code, is how complicated it is to add a new element to a game. Our team focused on making sure code had high cohesion, so that elements were grouped together. As well as low coupling, no more than 3 "." to call an element.

## Team 2 - Members
  <img width="17" height="17" alt="image" src="https://github.com/user-attachments/assets/28aad848-e8cd-4ee6-a3be-7b8c62d7c653" /> Anika <br>
  <img width="17" height="17" alt="image" src="https://github.com/user-attachments/assets/0a041384-c04f-46a9-ae2b-26a327774635" /> Donte <br>
  <img width="17" height="17" alt="image" src="https://github.com/user-attachments/assets/8b1cd855-0869-42fe-b28d-0f7b6d94a4b8" /> Jack <br>
  <img width="17" height="17" alt="image" src="https://github.com/user-attachments/assets/e2e426f4-b72e-42a1-ba57-e86a51ba4980" /> Jhanavi <br>
  <img width="17" height="17" alt="image" src="https://github.com/user-attachments/assets/21ba2075-0edf-4eed-b778-049db8690e23" /> Krista <br>

## Program Controls
<img width="1920" height="1080" alt="Mario Controls (3)" src="https://github.com/user-attachments/assets/841c8e37-1b25-4ef5-84ca-bf5263806cde" />

### Basic Mario Controls
  * **Left** and **Right**: Move Mario side to side
  * **Up**: Jump
  * **Down** Crouch
  * **Shift**: Mario sprints
    
### Activating Different Marios
  * **D** *(Hold)*: Activates Invincible Mario
  * **Space**: Activate Teleport Dash Mario

### Game Controls
  * **R**: Resets Mario to the beginning
  * **X**: Activate Christmas mode
  * **M**: Mute game
  * **P**: Pause game
 
## Level 1 - *Super Mario Bros Replica*

### Layout
<img width="1104" height="165" alt="Screenshot_2025-12-08_130329" src="https://github.com/user-attachments/assets/02172ade-4a9e-444c-b5c4-bccd0b9cacf8" />
Above is an entire layout of Level 1. One of our group members was able to find a tool to create a layout of the level, and automatically convert it into a JSON file. The group is then able to use the JSON file to store the level data and use it to load and construct the level. The code in **TiledMapLoader.cs** shows how we utilize this information.


### Sprint 2 - Focus on Functionality & Familiarizing Ourselves
 <img width="400" height="400" alt="image" src="https://github.com/user-attachments/assets/9bfb1cde-90e6-4e4c-ae38-c1e132d455f3" />
 
During Sprint 2, we worked on figuring out basic keypress commands, making sure that Mario moves, and placing all of our sprites on the screen. We started by using one member's Sprint 0 and ensured that every team member was familiar with the code. The team's most simple assignment, where we started on getting accostomed to GitHub and starting to implement the Command Design Patter with Command Managers.

 
 ### Sprint 3 - Focus on Collision and Level Loading
 <img width="350" height="350" alt="Screenshot 2025-12-09 160352" src="https://github.com/user-attachments/assets/5cb14c23-dd40-4530-a67f-b2126a80c148" />

During Sprint 3, our team focus on getting the level created and loaded into the game. We also worked on getting collision and physics to work. The collision used the rectangle logic that was covered in class. We focused on seeing how the two rectangles that represented the sprites were to overlap and use that information to determine the type of collision. We then passed that information to a HandleCollision class that used the information to correctly move Mario around so that he experienced collision.

At the end of the Sprint, our team ended up with our infamous two Marios. One had the correct collision working with the ground and pipes. The other had the correct physics and physics animation for when Mario jumps and moves. Our group started to have trouble with GitHub and figuring out how to manage all of our branches in an effective way. Our confusion led to last minute merges and trouble getting both the physics and the collision to work well together. However, our team was able to start communicating better about how our code was breaking so that we could effictively work together.

 
 ### Sprint 4 - Focus on Finishing Level 1

Sprint 4 was the big assignment, when our game should look similar to the Super Mario Bros Level 1. We had backlogged a lot throughout the year, and ended up having to backlog a lot of details. We did not have correct collision for enemies or powerups, as well as not having the pipe logic coded for the game. Our merging issues persisted in this sprint as well, with us having multiple aspects of the game finished in different branches. Our game was functional for the most part, our team just had issues getting all of the small details done. Throughout all of our sprints, our **Game1.cs** class had gotten bloated and a lot of the code had been left in Game1 that should have been abstracted. Our focus for the next sprint was to make our Game1 at least marginally smaller.

## Level 2 - *Christmas Chaos Level*

### Inspiration
With the hoilday season, our team thought there would be no better way to end off the semester with our own christmas-themed chaos Mario level. Each team member added their own feature into the game, granted that it fit into the Christmas theme. Not only is the level visually inspired by Christmas, the sounds are bright and Christmas themed.

### Features
  * **Christmas Snail** <br> <img width="100" height="100" alt="snail2" src="https://github.com/user-attachments/assets/4d189507-04d7-44f2-9836-87dfdb9a43a3" />
    * Inspired by the Immortal Christmas Snail, our snail will (slowly) terrorize Mario as he runs through the game. Don't get caught!
    * As the snail gets closer to Mario, the heartbeat sound gets louder and louder
  * **Mario Dash Feature**
  * **Christmas-Mode**

### Sprint 5 - Focus on Fixing and Fun Elements


Now that our team had the comments from Sprint 4, we were able to use that to focus on what to fix for our game. 

## Program Structure

### Command Design Pattern

### State Machine

### Factory Machine

### How Game1 Evolved

### Collision UML Diagram
<img width="2820" height="1865" alt="UML class (1)" src="https://github.com/user-attachments/assets/89638b76-d23a-4f22-a806-743a9cd1e776" />

> **Blue:** Manager being observed <br>
> **Red:** Concrete Classes <br>
> **Green:** Abstract Classes

The UML diagram above is a visual representation of one of our Command Managers: CollisionManager. It shows how CollisionManager interacts with the other aspects of the game. Collision is such a vital part of our game because Mario has to interact with almost every element, so seeing how it affects the different parts of the game is very helpful.



