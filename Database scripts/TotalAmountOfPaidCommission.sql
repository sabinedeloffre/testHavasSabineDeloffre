SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE TotalAmountOfPaidCommission 
	 @DealerId AS int,
	 @EndPeriodeDate datetime,
	 @StartPeriodeDate datetime
AS
BEGIN
	
	SET NOCOUNT ON;

	select count(*) AS numberOfPaidCommissions
	from CommissionHistory
	where ExistingPaymentStatusId = 4
		  and NewPaymentStatusId = 5
		  and NewDealerId = @DealerId
		  and ActionDate > @StartPeriodeDate
		  and ActionDate < @EndPeriodeDate
END
GO
