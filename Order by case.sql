select * from OBroInfo o
ORDER BY CASE
         WHEN o.Department = 'CSE' then 1
         WHEN o.Department = 'Management' then 2 
         WHEN o.Department = 'EEE' then 3
         WHEN o.Department = 'Math' then 4
         WHEN o.Department = 'Civil' then 5
		 WHEN o.Department = 'Mechanic' then 6
         END ASC