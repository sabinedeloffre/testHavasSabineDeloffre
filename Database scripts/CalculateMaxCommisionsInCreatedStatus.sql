
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE CalculateMaxCommisionsInCreatedStatus
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @i int
	DECLARE @numrows int
	DECLARE @numberOfCommissions int
	DECLARE @numberMaxOfCommissions int

	SET @i = 1
	SET @numberOfCommissions = 1
	SET @numberMaxOfCommissions = 0

	SET @numrows = (SELECT COUNT(*) from CommissionHistory
					where ExistingPaymentStatusId = 1 and NewPaymentStatusId = 1)

	IF OBJECT_ID('tempdb..#listOfReferenceDates') IS NOT NULL
		DROP TABLE #listOfReferenceDates
	SELECT ActionDate 
	INTO #listOfReferenceDates
	from CommissionHistory 
	where ExistingPaymentStatusId = 1 and NewPaymentStatusId = 1

	--select * from #listOfReferenceDates
	select @numrows
	
	IF @numrows > 0
		WHILE (@i <= @numrows)
		BEGIN
			select @numberOfCommissions = count(*)
			(
			select distinct 
				CoHi.CommissionId,
				(select ActionDate AS MinDate
						from CommissionHistory CH
						where ExistingPaymentStatusId = 1
							and NewPaymentStatusId = 1
							and CH.CommissionId = CoHi.CommissionId
							and ActionDate <= (SELECT ActionDate FROM(SELECT ROW_NUMBER() OVER (ORDER BY ActionDate) AS RowNum, * FROM #listOfReferenceDates) sub
												WHERE RowNum = @i )
				),
				(select ActionDate AS MaxDate
						from CommissionHistory CCH
						where ExistingPaymentStatusId = 1
							and NewPaymentStatusId = 2
							and CCH.CommissionId = CoHi.CommissionId
							and ActionDate >= (SELECT ActionDate FROM(SELECT ROW_NUMBER() OVER (ORDER BY ActionDate) AS RowNum, * FROM #listOfReferenceDates) sub
										WHERE RowNum = @i )
				) 
			from CommissionHistory AS CoHi
			)
			
			if (@numberMaxOfCommissions<@numberOfCommissions)
			BEGIN
				SET @numberMaxOfCommissions = @numberOfCommissions
			END

			-- increment counter
			SET @i = @i + 1
		END
	select @numberMaxOfCommissions AS NumMaxOfCommissions
	return @numberMaxOfCommissions
END
GO
