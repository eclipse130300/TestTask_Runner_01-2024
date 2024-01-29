TEST TASK

Your task is to create a simple platformer runner with an emphasis on code extensibility. Whether it's 2D or 3D doesn't matter. You can use any available assets. The main character of this runner is a character who automatically runs along the platform. The runner should be enjoyable to play.
During the run, the character may encounter various coins/objects, each of which has an effect on its behavior for a certain period of time. In the basic version of the game, at least three types of coins with the following effects are proposed: one coin slows down the character for 10 seconds, another accelerates it for 10 seconds, and the third allows the character to take off and fly for 10 seconds. After the effect of the coin ends, the character's behavior returns to its original state.
An important part of the task is to demonstrate your understanding of code extensibility principles. You should organize the code in such a way that it can be easily extended with new types of coins and effects without the need to edit the existing character class or classes. Please limit the use of Unity components such as MonoBehaviour, if possible, to demonstrate your design skills and knowledge of basic patterns. Also, refrain from using ECS.
The project should be well-documented, with each class accompanied by comments explaining its functionality and operation principle. Additionally, provide a brief description of the architectural decisions you made during the task and justify the choice of specific patterns and principles.
This task will assess your understanding of SOLID principles, ability to develop extensible and easily maintainable code, as well as your experience with C# and Unity3D.
The project code should be published in your GitHub repository. In the Build folder of the repository, there should be a compiled project intended for the Android platform.

SELF-REVIEW
I used such patterns during the development:

Finite State Machine (GameStateMachine):
The initialization stages of the application are divided into unique states:
bootstrap, levelLoad, playerProgressLoad, GameLoop.

Service Locator (AllServices):
The primary initialization of the application occurs during the bootstrap stage. 
Here, all necessary services are registered, and dependencies are resolved. 
While IoC&DI could have been used at this stage, a custom solution was implemented for simplicity.

Component (IInteractable, Obstacle, PowerUpFlyer):
Game interactions (e.g., collecting PowerUps) are implemented using the component pattern and MonoBehaviour.
Composition of components on GameObjects allowed for an ideally extensible architecture.

EventBus (EventBus):
The project utilizes an EventBus as a global event bus (e.g., GameplayStarted, GameplayFinished). 
Despite the potential for globally altering application states, when used correctly (following the publisher-subscriber model, send the event and forget),
it provides a flexible architectural solution with minimal boilerplate code due to its implementation features.

Factory (GameFactory):
A common factory is employed in the project to create and initialize all objects in the scene.

Procedural Content Generation (LevelGeneratorService):
As an enhancement beyond the basic task (showing off), level creation is implemented through procedural generation. 
Techniques such as PerlinNoise and Breadth-First search are utilized to create plausible procedural levels.
