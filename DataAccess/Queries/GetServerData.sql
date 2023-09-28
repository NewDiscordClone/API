declare  @serverId nvarChar(24) = '65144d73d911379586fd6db1'


select * from UserProfiles where ServerId = @serverId;

select * from AspNetRoles where ServerId = @serverId;

SELECT 
    CASE 
        WHEN u.DisplayName IS NULL THEN  CAST(u.Id AS NVARCHAR(36))
        ELSE u.DisplayName
    END AS UserName,
    r.Name AS RoleName
FROM 
    RoleUserProfile AS ru
JOIN 
    AspNetRoles AS r ON ru.RolesId = r.Id
LEFT JOIN 
    UserProfiles AS u ON ru.UserProfileId = u.Id
WHERE 
    u.ServerId = @serverId
ORDER BY RoleName DESC; 
