select a.UniqueName as [PlatformName], b.Id, b.PlatformId, b.UniqueName, 
b.Latitude, b.Longitude, b.CreatedAt, b.UpdatedAt
from [Platform] a join Well b on b.PlatformId = a.Id
where b.UpdatedAt in (select max(UpdatedAt) from Well Group by PlatformId)
order by a.UniqueName, b.UpdatedAt desc