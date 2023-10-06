DECLARE @userId AS UNIQUEIDENTIFIER = 'C1CE2C28-9CF4-4AB1-BDDB-B99C5408EE34'

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
