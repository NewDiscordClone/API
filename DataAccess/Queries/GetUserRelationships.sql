DECLARE @userId AS UNIQUEIDENTIFIER = 'c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34'

SELECT
    U1.UserName AS ActiveUserName,
    U2.UserName AS PassiveUserName,
    CASE
        WHEN R.RelationshipType = 0 THEN 'Acquaintance'
        WHEN R.RelationshipType = 1 THEN 'Friend'
        WHEN R.RelationshipType = 2 THEN 'Pending'
        WHEN R.RelationshipType = 3 THEN 'Waiting'
        WHEN R.RelationshipType = 4 THEN 'Blocked'
    END AS RelationshipType
FROM
    Relationships R
INNER JOIN
    AspNetUsers U1 ON R.UserActive = U1.Id
INNER JOIN
    AspNetUsers U2 ON R.UserPassive = U2.Id
WHERE
    R.UserActive = @userId OR R.UserPassive = @userId
