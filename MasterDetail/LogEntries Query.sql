DELETE FROM
	LogEntries
FROM
	LogEntries INNER JOIN WorkOrders
	ON LogEntries.EntityKeyValue = WorkOrders.WorkOrderId
	AND LogEntries.EntityFormalNamePlural = 'WorkOrders'
	AND (WorkOrders.WorkOrderStatus = 40 OR WorkOrders.WorkOrderStatus = -20)
	AND LogEntries.LogDate < DateAdd(d, -10, GetDate())

DELETE FROM
	LogEntries
FROM
	LogEntries le INNER JOIN Widgets w
	ON le.EntityKeyValue = w.WidgetId
	AND le.EntityFormalNamePlural = 'Widgets'
	AND (w.WidgetStatus = 30 OR w.WidgetStatus = -10)
	AND le.LogDate < DateAdd(d, -10, GetDate())