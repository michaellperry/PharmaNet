use [PharmaNet.Sales.SQL.SalesDB]

select
	SUM(Sales.Units),
	Products.ProductNumber,
	Members.Name,
	MeasurementPeriods.StartDate
from
	Sales
	join SalesHistories on Sales.SalesHistory_Id = SalesHistories.Id
	join Products on Sales.Product_Id = Products.Id
	join Members on SalesHistories.Member_Id = Members.Id
	join MeasurementPeriods on SalesHistories.MeasurementPeriod_Id = MeasurementPeriods.Id
group by
	Products.ProductNumber,
	Members.Name,
	MeasurementPeriods.StartDate