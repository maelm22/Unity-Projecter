Leaderboard exercise

I made a few small changes to the old game:
-PlayerController:
--Timer functionality
--Pickups give a little more time
-PickUp
--Coroutine to respawn pickups

Now your task is:
-Create a database to hold the info for a leaderboard (you can probably use what we already made)
-You need to add that new function to the server (something like /leaderboard, and the associated query)
--Hint: look into ORDER BY and LIMIT
-Integrate that with the game
-Show the leaderboard when the game is over 
-Extra: Upload the player score. This should be easy, since we already have example code we saw in class, but there will be duplicate entries unless we do some changes :) 
