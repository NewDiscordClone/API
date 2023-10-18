DECLARE @userId AS UNIQUEIDENTIFIER = '7AEF2538-E1B3-42D7-A3DB-A2809A81AC91'

SELECT
    U1.UserName AS ActiveName,   
    U1.Id AS ActiveId,
    U2.UserName AS PassiveName,
    U2.Id AS PassiveId,
    CASE
        WHEN R.RelationshipType = 0 THEN 'Acquaintance'
        WHEN R.RelationshipType = 1 THEN 'Friend'
        WHEN R.RelationshipType = 2 THEN 'Pending'
        WHEN R.RelationshipType = 3 THEN 'Blocked'
    END AS RelationshipType,
    R.PersonalChatId AS PersonalChat
FROM
    Relationships R
INNER JOIN
    AspNetUsers U1 ON R.Active = U1.Id
INNER JOIN
    AspNetUsers U2 ON R.Passive = U2.Id
WHERE
    R.Active = @userId OR R.Passive = @userId
