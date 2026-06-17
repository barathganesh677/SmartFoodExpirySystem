DELETE FROM ShoppingPlanner
WHERE PlannerId NOT IN (
    SELECT MIN(PlannerId)
    FROM ShoppingPlanner
    GROUP BY ItemName, UserId, Status
);