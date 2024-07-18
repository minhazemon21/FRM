declare @OutputDate date
 
  set @outputdate = cast(year('07 Jul 2024') as varchar(4)) + '/' + cast(month('07 Jul 2024') as varchar(2)) + '/01'
  set @outputdate = dateadd(dd, -1, dateadd(m, 1, @outputdate))
  
  select @OutputDate