+ Keeping things simple is good, initially. I added the random food at the start and it kept hitting the walls
+ having more sensors seems useful. When I added the angle of the facing of slideman to ball, I got better results
+ Negative rewards can give the wrong incentives. When I added a negative incentive to staying still and made it so hitting the wall ended the episode, the agent would suicide as fast as possible, ramming the wall to end the episode and avoid negative rewards.
+ you can call on collision from the code.
+ for small values to discrete actions, when I set the threshold for on trigger discrete action to a low number, like 0.2 from 0, it seemed to work better. Using any value above 0 meant the AI would always spin out of control, while 0.5 meant the AI started by staying still.
+You must reload the nn model by deleting and adding it back again
+ Adding more steps to the model by editing the yaml file allowed me to do "curriculum training"
+ using discrete model with smaller move parameters works decently, but there is a major big with the avatar not seeming to be able to turn both direction.


5th feb, 
- Learned that the agent could be trained more effectively by setting the objects in a circle and subtly moving them around the space, so they know how to deal with a piece of food that is at a different orientation to them