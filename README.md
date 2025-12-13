# 3902: Mario Game
This project is a semester-long work for OSU CSE 3902. We were tasked to recreate the Super Mario Bros first level while focusing on code quality:
> **S** imple <br>
> **R** eadable <br>
> **M** aintainable <br>
> **R** eusable <br>
 
Our team worked with an OOP oriented style, focusin on **Readability** so that we can understand each others' code well. We worked on abstracting our code. We have learned many different design patterns to implement to our code. As we have learned throughout the semester, an important part of creating video game code, is how complicated it is to add a new element to a game. Our team focused on making sure code had high cohesion, so that elements were grouped together. As well as low coupling, no more than 3 "." to call an element.

## Team 2 - Members
  <img width="17" height="17" alt="image" src="https://github.com/user-attachments/assets/28aad848-e8cd-4ee6-a3be-7b8c62d7c653" /> Anika Talyan<br>
  <img width="63" height="91" alt="anika_3902" src="https://github.com/user-attachments/assets/e4495a1d-8d0e-4d3b-811c-8798340f54ae" />

  <img width="17" height="17" alt="image" src="https://github.com/user-attachments/assets/28aad848-e8cd-4ee6-a3be-7b8c62d7c653" /> Donte Beck-King<br>
  <img width="88" height="106" alt="donte_3902" src="https://github.com/user-attachments/assets/fafc302d-adaa-408d-b07c-4d9e3631a6ca" />

  <img width="17" height="17" alt="image" src="https://github.com/user-attachments/assets/28aad848-e8cd-4ee6-a3be-7b8c62d7c653" /> Jack Olson<br>
  <img width="80" height="67" alt="jack_3902_single" src="https://github.com/user-attachments/assets/8e2c8df2-856e-45e7-b2df-5c9a7fee9513" />

  <img width="17" height="17" alt="image" src="https://github.com/user-attachments/assets/28aad848-e8cd-4ee6-a3be-7b8c62d7c653" /> Jahnavi Acharya<br>
  <img width="76" height="96" alt="jhanavi_3902_single" src="https://github.com/user-attachments/assets/eb04366a-e80b-4b46-a83b-b0989e90b5f8" />

  <img width="17" height="17" alt="image" src="https://github.com/user-attachments/assets/28aad848-e8cd-4ee6-a3be-7b8c62d7c653" /> Krista Bair<br> <img width="53" height="60" alt="krista_3902_single" src="https://github.com/user-attachments/assets/4ce99e02-26c7-46af-8a3a-c09447549240" />


## Program Controls
<img width="1920" height="1080" alt="Mario Controls (3)" src="https://github.com/user-attachments/assets/841c8e37-1b25-4ef5-84ca-bf5263806cde" />

### Basic Mario Controls
  * **Left** and **Right**: Move Mario side to side
  * **Up**: Jump
  * **Down** Crouch
  * **Shift**: Mario sprints
    
### Different Mario States
  * **D** *(Hold)*: Activates Invincible Mario
  * **Space**: Activate Teleport Dash Mario
  * **Z**: Shoot fireballs

### Game Controls
  * **R**: Resets Mario to the beginning
  * **X**: Activate Christmas mode
  * **M**: Mute game
  * **P**: Pause game

 
## Level 1 - *Super Mario Bros Replica*

### Layout
<img width="1104" height="165" alt="Screenshot_2025-12-08_130329" src="https://github.com/user-attachments/assets/02172ade-4a9e-444c-b5c4-bccd0b9cacf8" />
Above is an entire layout of Level 1. One of our group members was able to find a tool to create a layout of the level, and automatically convert it into a JSON file. The group is then able to use the JSON file to store the level data and use it to load and construct the level. The code in **TiledMapLoader.cs** shows how we utilize this information.


<br>

### Sprint 2 - Focus on Functionality & Familiarizing Ourselves
 <img width="400" height="400" alt="image" src="https://github.com/user-attachments/assets/9bfb1cde-90e6-4e4c-ae38-c1e132d455f3" />
 
During Sprint 2, we worked on figuring out basic keypress commands, making sure that Mario moves, and placing all of our sprites on the screen. We started by using one member's Sprint 0 and ensured that every team member was familiar with the code. The team's most simple assignment, where we started on getting accostomed to GitHub and starting to implement the Command Design Patter with Command Managers.

<br>
 
 ### Sprint 3 - Focus on Collision and Level Loading
 <img width="350" height="350" alt="Screenshot 2025-12-09 160352" src="https://github.com/user-attachments/assets/5cb14c23-dd40-4530-a67f-b2126a80c148" />

During Sprint 3, our team focus on getting the level created and loaded into the game. We also worked on getting collision and physics to work. The collision used the rectangle logic that was covered in class. We focused on seeing how the two rectangles that represented the sprites were to overlap and use that information to determine the type of collision. We then passed that information to a HandleCollision class that used the information to correctly move Mario around so that he experienced collision.

At the end of the Sprint, our team ended up with our infamous two Marios. One had the correct collision working with the ground and pipes. The other had the correct physics and physics animation for when Mario jumps and moves. Our group started to have trouble with GitHub and figuring out how to manage all of our branches in an effective way. Our confusion led to last minute merges and trouble getting both the physics and the collision to work well together. However, our team was able to start communicating better about how our code was breaking so that we could effictively work together.

<br>
 
 ### Sprint 4 - Focus on Finishing Level 1

Sprint 4 was the big assignment, when our game should look similar to the Super Mario Bros Level 1. We had backlogged a lot throughout the year, and ended up having to backlog a lot of details. We did not have correct collision for enemies or powerups, as well as not having the pipe logic coded for the game. Our merging issues persisted in this sprint as well, with us having multiple aspects of the game finished in different branches. Our game was functional for the most part, our team just had issues getting all of the small details done. Throughout all of our sprints, our **Game1.cs** class had gotten bloated and a lot of the code had been left in Game1 that should have been abstracted. Our focus for the next sprint was to make our Game1 at least marginally smaller. One of our team members, Jack, took on a majority of the merging which we all really appreciate. He worked really hard to merge code together and doing a lot of the work on GitHub when we were confused.

## Level 2 - *Christmas Chaos Level*

### Inspiration
With the hoilday season, our team thought there would be no better way to end off the semester with our own christmas-themed chaos Mario level. Each team member added their own feature into the game, granted that it fit into the Christmas theme. Not only is the level visually inspired by Christmas, the sounds are bright and Christmas themed.
<br>

### Features
  * **Immortal Snail** <br> <img width="100" height="100" alt="snail2" src="https://github.com/user-attachments/assets/4d189507-04d7-44f2-9836-87dfdb9a43a3" />
    * Inspired by the Immortal Snail, our snail will (slowly) terrorize Mario as he runs through the game. Don't get caught!
    * As the snail gets closer to Mario, the heartbeat sound gets louder and louder
    * All Mario's lives will be lost and he will die no matter what Mario State he is in
   
  * **Mario Dash Feature**

  * **Christmas-Mode**
    * Added a Christmas-themed texture pack by adding Christmas elements to the background and sprites
    * Christmas Mario: <img width="585" height="52" alt="small-mario-final_Christmas(1)" src="https://github.com/user-attachments/assets/63d57c51-230c-4d9d-999c-9e48ef916cb4" />
    * **Christmas Music:** One of our team members, Krista, has a lot of experience with music. She was able to splice Christmas music from online and change it to a specific key so that it seamlessly overlays over the Mario music, making it more personalized to Mario!

  * **Team Member Sprites** - We created sprites that show each of our team members, however we didn't have time to place them into the game.

### Sprint 5 - Focus on Fixing and Fun Elements

Now that our team had the comments from Sprint 4, we were able to use that to focus on what to fix for our game. We focused on abstracting out the collision code into a **CollisionManager.cs** since it affected many different aspects of the game and we saw repeated code. By putting the collision code in a different place, we were able to trim down our Game1 and focus on cleaning up other parts of our code. We also got the functionality working to get Level 1 working correctly.

With the main part of Sprint 4, we all decided to split up and our own aspects of the code. I have explained what we added earlier in the **Features** section. We decided to focus on a Christmas theme, inspired by this time of the year.

## Program Structure

### Use of Managers
A big part of our code was using Managers to break up our code effectively. Throughout the semester, we learned that working together is much more effective when we can read each other's code easily. By grouping the logic together with Managers, we are able to focus on the different aspects of the game and understand the logic quickly. It is important that all aspects of the game work and are coordinated with each other. With different people working on different aspects, it is important that they could understand how to correlate their code in an effective manner. Our **Game1.cs** class can use few lines to do something as complex as draw the entire background. 

We used Managers for big parts of the game like collision and dealing with enemies in files like **CollisionManager.cs** and **EnemyCollisionHandler.cs**. For collision, one system is able to use the logic and we can avoid redundant code because almost every object in the game needs to experience collision. We use the **CollisionManager.cs** to determine how an object might collide and use files like **EnemyCollisionHandler.cs** to determine what to do with that collision. By being able to use the CollisionManager in a general way, we are able to adapt it to different aspects of the game.

<br>

### State Machine
State machines are helpful for creating distinct traits/behaviors for a common object. We recognized that both Mario and Powerups are objects that have many different types with distinct traits that change the way the game interacts with the objects. However, lots of similar actions like physics or collision might be similar for both, so by implementing a State Machine, it helps keep the code from being tightly coupled. Adding a new state doesn't mean changing a bunch of lines of code, instead a new file can be created for that specific state and all that needs to be done is adding a few lines of code in other files.

**_Powerups:_** With powerups, there are many types of powerups that change Mario, but they change him in different ways. When updating Mario or the powerups based on the decisions happening in the game, it can be easily split into multiple different classes. This helps the readability, by making it easy to tell exactly what powerup is affecting the game and how it is. We have the shared data for powerups in **PowerupInstance.cs**, but use **PowerupState.cs** to differentiate the objects.

**_Mario:_** Donte, one of our group members, took it upon himself to create an extensive state machine for the Mario sprites. We did not realzie at first how many different states Mario had and found ourselves getting overwhelmed by how much code seemed to need to be put in one file. Instead, we took away the "if" and "switch" statements and created seperate files for Mario. Each State file is designed to play the animation needed for that state, as well as determine the behavior of Mario. This keeps **PlayerMario.cs**, while still not short, much shorter than it would be without the State Machine. 

<br>

### Factory Machine
While we did focus on integrating Managers and State Machines in a lot of aspects in our code, we focused on putting a Factory in for powerups. The **PowerupFactory.cs** allows the powerup creation to happen efficently in one place. It helps avoid duplication and confusion because it takes few lines of codes to create a new powerup.

<br>

### How Game1 Evolved

Throughout our time working on the sprints, we just kept adding and adding to the Game1 file. The **Game1.cs** is basically the hub of all the code, it should only Load, Update, and Draw our content. But our Game1 was handling collision, physics, and many other parts of the game that it should not. We recognized that and wanted to make it better thoughout the semester. We got grader comments calling it "bloated" and saying that it was doing way too much.

While we did get caught up in working with other parts of the game to make it work, we focused on abstracting our collision code into a manager, as I touched on when talking about our Sprint 5. We wanted code that affects many parts of the game to be in one place and have it focused on collision, instead of working collision into our enemies in our Game1 class. We recognized that that was inefficent and not following the code quality rules that we were striving for. Like I said earlier, our Game1 isn't as cut down as we would like it to be, but we have learned a lot about abstracting code and have made plans to cut it down if we move forward with the project in the future, when we have more time.


### Collision UML Diagram
<img width="2820" height="1865" alt="UML class (1)" src="https://github.com/user-attachments/assets/89638b76-d23a-4f22-a806-743a9cd1e776" />

> **Blue:** Manager being observed <br>
> **Red:** Concrete Classes <br>
> **Green:** Abstract Classes

The UML diagram above is a visual representation of one of our Command Managers: CollisionManager. It shows how CollisionManager interacts with the other aspects of the game. Collision is such a vital part of our game because Mario has to interact with almost every element, so seeing how it affects the different parts of the game is very helpful.

## Overall Reflection
Creating a video game level was something most of us haven’t done before. It was a very new experience working with such a large project and learning how important organization and a smart use of version control is for our team. Throughout the semester, we started communicating more effectively and learning how to use GitHub to our advantage. The design decisions we made: State Machine, Factory, our use of Managers, ect. By being able to learn and implement patterns that helped us create the code logically, we gained skills that will help us in future projects. The game was filled with details, and as we went through the semester, we learned how to effectively manage all those details in a big project.



<br>
**Credits for the personal sprites go to:** <br><br><img width="250" height="114" alt="image" src="https://github.com/user-attachments/assets/150fcd6b-5494-4771-9458-7318c67e398c" />

