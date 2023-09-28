DECLARE @chatId NVARCHAR(24) = '6515b996f54e44488e99d803'

SELECT 
    CASE 
        WHEN us.DisplayName IS NULL THEN us.UserName
        ELSE us.DisplayName
    END AS UserName,
    r.Name AS RoleName,
    u.Id AS ProfileId
FROM 
    RoleUserProfile AS ru
JOIN 
    AspNetRoles AS r ON ru.RolesId = r.Id
LEFT JOIN 
    UserProfiles AS u ON ru.UserProfileId = u.Id
LEFT JOIN 
    AspNetUsers AS us ON u.UserId = us.Id
WHERE 
    u.ChatId = @chatId
ORDER BY r.Name DESC;
