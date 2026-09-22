# Unity Project Test
Unity Project Take Home Test

# Instructions
1. Clone the repository to your local machine OR download the project as a zip file and establish your own local git repo
2. Read each question below and work through them.
3. Please commit each answer with comments on which question the commit addresses.  It is OK to go back and change previous answers if you jump around in the test, just note it in your commit.
4. You will not need to push your results.  Instead, when you are done use this git command to create a bundle with your results: git bundle create UnityTestResults.bundle --all
5. Note that deleting the "Library" folder before you create the bundle will significantly reduce the size
6. Email the bundle back to your contact.  If it is too large, try uploading to an online file storage provider like Google Drive and provide a link
7. We ask you to take no more than 4 hours on the test and to self report how long you worked on it.  Do not count environment setup (e.g. installing IDE, Unity, setting up `git`) as part of that 4 hours.
8. AI use is not permitted – we want to see how you write code and solve problems, so please disable generative-AI-driven-autocomplete and do not use agentic or other LLM-based AI.
9. If you run out of time, leave short notes explaining: what is done, what is missing, what you would do next

# Other Tips
- The project is using Unity version `2022.3.73f1`
- There is light project documentation found in the Assets/Documentation folder that will help you understand the project better
- Please avoid adding 3rd party frameworks to the solution as much as you can. If you feel one is necessary indicate why in your commit. Please commit this as a separate commit from any of your code.
- Aim to complete the questions using the style or approach this solution uses
- Developer art is expected, do not waste time finding the perfect art
- You do not have to complete all the questions.
- If you partially complete a question it is OK submit that with a comment indicating that it is WIP (work in progress)


# Overview
You have been provided with a copy of the 2D Platformer Microgame project, a small project offered by Unity as part of their learning program. This will be the basis for the test, all exercises are to help the artists or designers in this project.


## Exercise 1
Token Group Manager: Create a designer tool to manage groups of tokens in the scene.

The tool should let a designer:
- create token groups
- add/remove selected tokens
- remove individual tokens from a group
- instantiate a new token from the tool
- delete token groups
- move token groups so member tokens move together
- frame the Scene view on the selected group

Notes
- The tool must work in edit mode.
- The tool window should be usable by a designer without code changes.
- Use the provided TokenGroup and editor window scaffolding as a starting point.

## Exercise 2
Texture Importer tool: Create a tool that imports textures for a new environment and configures them as sprites.

The tool should let an artist:
- choose a source folder on disk
- enter an environment name
- import textures into: Assets/Environment/<EnvironmentName>/Sprites
- automatically configure imported textures as sprites

Notes
- Focus on a clean and reliable workflow.
- Use the provided editor window scaffold as a starting point.
- You may assume common image formats such as .png, .jpg, and .jpeg.
- You can use the textures from NewEnvironmentTextures folder for the exercise.

## Exercise 3: Bonus
Texture Importer Post-Processor: Create a Unity AssetPostprocessor that applies sprite import settings to environment textures based on their asset path.

The project already includes an editor tool that imports textures for a new environment. That tool depends on the user following the intended workflow.

This exercise focuses on enforcing import consistency automatically, even when textures are added manually into the project.





