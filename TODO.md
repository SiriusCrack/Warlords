TODO:
* Conquest Scene
  * add selectable posts that start battles
    * add selectable posts with camps & unit pools etc
    * move battle scene loader from start menu to posts
    * ending battle returns to conquest map and changes post
  * add characters that can move around map
    * make dude selectable
    * implement moving dude to post
    * iniate battle scene loading from dude moving to post
  * Deck Management
    * add UI scene accessable from conquest map
    * make UI scene methods to change a character's camps / unit pools / etc
    * polish UI
* Battle Scene
  * improve camp AI
  * Units
    * implement archer
      * add arrow projectile

      thoughts: implement same as sword. all weapons are just area2Ds that do special stuff on collision. bow is the weapon, target is picked by bow, aimed towards, and arrow is instantiated. arrow is another weapon, just does dammage on area collision and QueueFrees after. make ranged unit subclass to handle setting range according to lane spacing.
    * implement medic
    * implement engineer
    * implement general(?)
    * polish units
    * switch spawn select to pick from pool
  * Defending / Attacking stages
    * Destructable buildings
      * implement building
      * replace goal / score with building health
  * something something dialogue
* Art
  * come up w art style
  * Backgrounds
    * battle scene backgrounds
      * attack scenes vs defense scenes
      * different province settings
    * conquest map
    * Menus
      * Start menu
      * Deck management menu
  * units + weapons + animations
      * Kingdom A
      * Kingdom B
      * peasants / Kingdom C
      * portraits?
