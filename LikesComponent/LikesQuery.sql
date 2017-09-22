/* Select first non null value using Coalesce, casting as bit for true and false whether or not
   current logged on user has liked the content that is showing on the page*/
select b.ContentId, CAST(COALESCE(a.LikedByLoggedInUser, 0) as bit) LikedByLoggedInUser, b.TotalLikes
from (
	/* 1 as LikedByLoggedInUser to set all values to 1, since by the time it gets here there will be no null records*/
	SELECT ContentId, 1 as LikedByLoggedInUser
	FROM dbo.Likes as likes
	JOIN @ids as contentIds
	ON likes.ContentId = contentIds.Data
	WHERE likes.UserId = @UserId
) as a
full join 
/* Joining on second query from same table to display total likes */
(
	SELECT ContentId, Count(ContentId) as TotalLikes
	FROM dbo.Likes as likes
	JOIN @ids as contentIds
	ON likes.ContentId = contentIds.Data
	GROUP BY ContentId
) as b
on a.contentid = b.ContentId